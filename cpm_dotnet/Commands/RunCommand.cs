using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;

namespace cpm_dotnet.Commands
{
    public class RunCommandSettings : CommandSettings
    {
        [CommandOption("-v|--verbose")]
        [Description("Show verbose output from CMake during the build.")]
        public bool Verbose { get; set; }

        [CommandOption("--std")]
        [Description("C++ standard to use (e.g., 11, 14, 17, 20). Defaults to 20.")]
        [DefaultValue("20")]
        public string Standard { get; set; } = "20";

        [CommandArgument(0, "[PROGRAM_ARGS]")]
        [Description("Arguments to pass to the program.")]
        public string[] ProgramArgs { get; set; } = Array.Empty<string>();
    }

    public class RunCommand : Command<RunCommandSettings>
    {
        public override int Execute(CommandContext context, RunCommandSettings settings)
        {
            // First, build the project
            var buildCommand = new BuildCommand();
            var buildSettings = new BuildCommandSettings
            {
                Verbose = settings.Verbose,
                Standard = settings.Standard
            };
            var buildResult = buildCommand.Execute(context, buildSettings);
            if (buildResult != 0)
            {
                return buildResult;
            }

            var projectName = ProjectConfigManager.GetProjectName();
            if (string.IsNullOrEmpty(projectName))
            {
                AnsiConsole.MarkupLine("[bold red]Error:[/bold red] Could not find project name to run.");
                return 1;
            }

            var executablePath = Path.Combine("build", projectName);
            if (!File.Exists(executablePath))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/bold red] Executable not found at '[bold]{executablePath}[/]'.");
                return 1;
            }

            AnsiConsole.MarkupLine($"[bold cyan]--- Running {projectName} ---");
            try
            {
                var processStartInfo = new ProcessStartInfo(executablePath)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = true,
                };

                foreach (var arg in settings.ProgramArgs)
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
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenPaths | ExceptionFormats.ShowLinks);
                return 1;
            }
        }
    }
}