using DotMake.CommandLine;
using Spectre.Console;
using System.Diagnostics;

namespace cpm_dotnet.Commands
{
    [CliCommand(Name = "run", Description = "Build and run the project.", Parent = typeof(RootCommand))]
    public class RunCommand
    {
        [CliOption(Description = "Show verbose output from CMake during the build.")]
        public bool Verbose { get; set; }

        [CliOption(Description = "C++ standard to use (e.g., 11, 14, 17, 20). Defaults to 20.")]
        public string Standard { get; set; } = "20";

        [CliArgument(Description = "Arguments to pass to the program.")]
        public string[] ProgramArgs { get; set; } = Array.Empty<string>();

        public int Run()
        {
            // First, build the project
            var buildCommand = new BuildCommand
            {
                Verbose = Verbose,
                Standard = Standard
            };
            if (!buildCommand.Run())
            {
                return 1; // Build failed
            }

            var projectName = ProjectConfigManager.GetProjectName();
            if (string.IsNullOrEmpty(projectName))
            {
                AnsiConsole.MarkupLine("[bold red]Error:[/] Could not find project name to run.");
                return 1;
            }

            var executablePath = Path.Combine("build", projectName);
            if (!File.Exists(executablePath))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/] Executable not found at '[bold]{executablePath}[/]'.");
                return 1;
            }

            AnsiConsole.MarkupLine($"[bold cyan]--- Running {projectName} ---[/]");
            try
            {
                var processStartInfo = new ProcessStartInfo(executablePath)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = true,
                };

                foreach (var arg in ProgramArgs)
                {
                    processStartInfo.ArgumentList.Add(arg);
                }

                using (var process = Process.Start(processStartInfo))
                {
                    if (process == null) throw new Exception("Failed to start program process.");
                    process.WaitForExit();
                    return process.ExitCode;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                return 1;
            }
        }
    }
}
