using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace cpm_dotnet.Commands
{
    public class BuildCommandSettings : CommandSettings
    {
        [CommandOption("-v|--verbose")]
        [Description("Show verbose output from CMake.")]
        public bool Verbose { get; set; }

        [CommandOption("--std")]
        [Description("C++ standard to use (e.g., 11, 14, 17, 20). Defaults to 20.")]
        [DefaultValue("20")]
        public string Standard { get; set; } = "20";
    }

    public class BuildCommand : Command<BuildCommandSettings>
    {
        public override int Execute(CommandContext context, BuildCommandSettings settings)
        {
            var projectConfig = ProjectConfigManager.LoadConfig();
            if (projectConfig == null || string.IsNullOrEmpty(projectConfig.Project.Name))
            {
                AnsiConsole.MarkupLine("[bold red]Error:[/bold red] Not a cpm project. `package.toml` not found or is missing project name.");
                return 1;
            }

            var projectName = projectConfig.Project.Name;
            AnsiConsole.MarkupLine("[bold cyan]--- Configuring project and generating CMakeLists.txt ---");

            try
            {
                // Generate resource files if any
                if (projectConfig.Resources != null && projectConfig.Resources.Files.Any())
                {
                    Utils.GenerateResourceFiles(projectConfig.Resources.Files);
                }

                // Generate CMakeLists.txt content
                var cmakeContent = new StringBuilder();
                cmakeContent.AppendLine("cmake_minimum_required(VERSION 3.20)");
                cmakeContent.AppendLine($"project({projectName} LANGUAGES CXX)");
                cmakeContent.AppendLine("");
                cmakeContent.AppendLine($"set(CMAKE_CXX_STANDARD {settings.Standard})");
                cmakeContent.AppendLine("set(CMAKE_CXX_STANDARD_REQUIRED ON)");
                cmakeContent.AppendLine("set(CMAKE_CXX_EXTENSIONS OFF)");
                cmakeContent.AppendLine("");
                cmakeContent.AppendLine("include(FetchContent)");
                cmakeContent.AppendLine("");
                cmakeContent.AppendLine("# --- Dependencies ---");

                var linkTargets = new List<string>();
                foreach (var dep in projectConfig.Dependencies)
                {
                    var name = dep.Key;
                    var details = dep.Value;

                    if (string.IsNullOrEmpty(details.Git) || string.IsNullOrEmpty(details.Tag))
                    {
                        AnsiConsole.MarkupLine("[bold yellow]Warning:[/bold yellow] Skipping invalid dependency '[bold]" + name + "[/bold]'. 'git' and 'tag' are required.");
                        continue;
                    }

                    var target = string.IsNullOrEmpty(details.Target) ? name : details.Target;
                    cmakeContent.AppendLine($"FetchContent_Declare({name} GIT_REPOSITORY \"{details.Git}\" GIT_TAG \"{details.Tag}\")");
                    if (name != "googletest")
                    {
                        linkTargets.Add(target);
                    }
                }

                cmakeContent.AppendLine("");

                foreach (var dep in projectConfig.Dependencies)
                {
                    cmakeContent.AppendLine($"FetchContent_MakeAvailable({dep.Key})");
                }

                cmakeContent.AppendLine("");
                cmakeContent.AppendLine("# --- Project Target ---");
                cmakeContent.AppendLine($"file(GLOB_RECURSE SOURCES ${{PROJECT_SOURCE_DIR}}/src/*.cpp)");

                if (projectConfig.Project.Type == "executable")
                {
                    cmakeContent.AppendLine($"add_executable({projectName} ${{SOURCES}})");
                }
                else if (projectConfig.Project.Type == "library")
                {
                    cmakeContent.AppendLine($"add_library({projectName} STATIC ${{SOURCES}})");
                    if (projectConfig.Project.InstallHeaders)
                    {
                        cmakeContent.AppendLine($"target_include_directories({projectName} PUBLIC ${{PROJECT_SOURCE_DIR}}/src)");
                        cmakeContent.AppendLine($"install(TARGETS {projectName} EXPORT {projectName}Config DESTINATION lib)");
                        cmakeContent.AppendLine($"install(DIRECTORY ${{PROJECT_SOURCE_DIR}}/src/ DESTINATION include/{projectName})");
                    }
                }

                cmakeContent.AppendLine("");
                cmakeContent.AppendLine("# --- Linking ---");
                if (linkTargets.Any())
                {
                    cmakeContent.AppendLine($"target_link_libraries({projectName} PRIVATE {string.Join(" ", linkTargets)})");
                }

                if (Directory.Exists("test"))
                {
                    if (projectConfig.Dependencies.ContainsKey("googletest"))
                    {
                        cmakeContent.AppendLine("");
                        cmakeContent.AppendLine("# --- Testing ---");
                        cmakeContent.AppendLine("enable_testing()");
                        cmakeContent.AppendLine("");
                        cmakeContent.AppendLine($"file(GLOB_RECURSE TEST_SOURCES \"${{PROJECT_SOURCE_DIR}}/test/*.cpp\")");
                        cmakeContent.AppendLine("message(STATUS \"Found test sources: ${{TEST_SOURCES}}\")");
                        cmakeContent.AppendLine("");
                        cmakeContent.AppendLine("set(SOURCES_FOR_TESTS ${{SOURCES}})");
                        cmakeContent.AppendLine("if(EXISTS \"${{PROJECT_SOURCE_DIR}}/src/main.cpp\")");
                        cmakeContent.AppendLine("    list(REMOVE_ITEM SOURCES_FOR_TESTS \"${{PROJECT_SOURCE_DIR}}/src/main.cpp\")");
                        cmakeContent.AppendLine("endif()");
                        cmakeContent.AppendLine("");
                        cmakeContent.AppendLine($"add_executable(run_tests ${{TEST_SOURCES}} ${{SOURCES_FOR_TESTS}})");
                        cmakeContent.AppendLine("");
                        cmakeContent.AppendLine($"target_include_directories(run_tests PRIVATE ${{CMAKE_CURRENT_SOURCE_DIR}}/src)");
                        cmakeContent.AppendLine("");
                        cmakeContent.AppendLine("target_link_libraries(run_tests PUBLIC GTest::gtest_main)");
                        cmakeContent.AppendLine("");
                        cmakeContent.AppendLine("include(GoogleTest)");
                        cmakeContent.AppendLine("gtest_discover_tests(run_tests)");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold yellow]Warning:[/bold yellow] 'test' directory exists but 'googletest' is not a dependency in package.toml. Skipping test CMake generation.");
                    }
                }

                File.WriteAllText("CMakeLists.txt", cmakeContent.ToString());

                AnsiConsole.MarkupLine("[bold cyan]--- Running CMake to build project ---");

                // Configure step
                var cmakeConfigureCommand = new ProcessStartInfo("cmake", "-B build -S . -DCMAKE_EXPORT_COMPILE_COMMANDS=ON -DCMAKE_INSTALL_PREFIX=.")
                {
                    RedirectStandardOutput = !settings.Verbose,
                    RedirectStandardError = !settings.Verbose,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using (var process = Process.Start(cmakeConfigureCommand))
                {
                    if (process == null) throw new Exception("Failed to start CMake process.");
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        AnsiConsole.MarkupLine("[bold red]CMake configure failed.[/bold red]");
                        if (!settings.Verbose)
                        {
                            AnsiConsole.MarkupLine(process.StandardOutput.ReadToEnd());
                            AnsiConsole.MarkupLine(process.StandardError.ReadToEnd());
                        }
                        return 1;
                    }
                }

                // Create a symlink in the root for the LSP
                var compileCommandsPath = Path.Combine("build", "compile_commands.json");
                var symlinkPath = "compile_commands.json";
                if (File.Exists(compileCommandsPath))
                {
                    if (File.Exists(symlinkPath) || Directory.Exists(symlinkPath))
                    {
                        File.Delete(symlinkPath);
                    }
                    File.CreateSymbolicLink(symlinkPath, compileCommandsPath);
                    if (settings.Verbose)
                    {
                        AnsiConsole.MarkupLine("[bold green]--- Created compile_commands.json for LSP ---");
                    }
                }

                // Build step
                var buildCommandArgs = new StringBuilder("--build build");
                if (settings.Verbose)
                {
                    buildCommandArgs.Append(" --verbose");
                }

                var cmakeBuildCommand = new ProcessStartInfo("cmake", buildCommandArgs.ToString())
                {
                    RedirectStandardOutput = !settings.Verbose,
                    RedirectStandardError = !settings.Verbose,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using (var process = Process.Start(cmakeBuildCommand))
                {
                    if (process == null) throw new Exception("Failed to start CMake process.");
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        AnsiConsole.MarkupLine("[bold red]CMake build failed.[/bold red]");
                        if (!settings.Verbose)
                        {
                            AnsiConsole.MarkupLine(process.StandardOutput.ReadToEnd());
                            AnsiConsole.Console.MarkupLine(process.StandardError.ReadToEnd());
                        }
                        return 1;
                    }
                }

                AnsiConsole.MarkupLine("[bold green]Build finished successfully.[/bold green]");
            }
            catch (FileNotFoundException)
            {
                AnsiConsole.MarkupLine("[bold red]Error:[/bold red] `cmake` command not found. Please ensure CMake is installed and in your PATH.");
                return 1;
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenPaths | ExceptionFormats.ShowLinks);
                return 1;
            }

            return 0;
        }
    }
}
