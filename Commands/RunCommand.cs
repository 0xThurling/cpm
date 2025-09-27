using DotMake.CommandLine;
using Spectre.Console;
using System.Diagnostics;

namespace cpm.Commands
{
    [CliCommand(Name = "run", Description = "Run a custom script.", Parent = typeof(RootCommand))]
    public class RunCommand
    {
        [CliArgument(Description = "Name of the script to run.")]
        public string? ScriptName { get; set; } = null;

        public int Run()
        {
            var config = ProjectConfigManager.LoadConfig();

            if (string.IsNullOrEmpty(ScriptName))
            {
                var startCommand = new StartCommand();
                return startCommand.Run();
            }

            if (!config!.Scripts.TryGetValue(ScriptName, out var scriptCommand))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/] Script '[bold]{ScriptName}[/]' not found in package.toml.");
                return 1;
            }

            AnsiConsole.MarkupLine($"> {scriptCommand}");

            try
            {
                var processStartInfo = new ProcessStartInfo("bash", $"-c \"{scriptCommand}\"")
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = true,
                };

                using (var process = Process.Start(processStartInfo))
                {
                    if (process == null) throw new Exception("Failed to start script process.");
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
