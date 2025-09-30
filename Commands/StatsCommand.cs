using DotMake.CommandLine;
using Spectre.Console;

namespace cpm.Commands
{
    [CliCommand(Name = "stats", Description = "Display some statistics about the project.", Parent = typeof(ProjectCommand))]
    public class StatsCommand
    {
        public int Run()
        {
            var projectRoot = ProjectConfigManager.FindProjectRoot();
            if (projectRoot == null)
            {
                AnsiConsole.MarkupLine("[bold red]Error:[/] Not a cpm project. `package.toml` not found.");
                return 1;
            }

            var files = Directory.GetFiles(projectRoot, "*.*", SearchOption.AllDirectories);
            var totalFiles = files.Length;
            var totalLines = 0;
            long totalSize = 0;

            foreach (var file in files)
            {
                try
                {
                    totalLines += File.ReadLines(file).Count();
                    totalSize += new FileInfo(file).Length;
                }
                catch { }
            }

            AnsiConsole.MarkupLine($"[bold]Total Files:[/] {totalFiles}");
            AnsiConsole.MarkupLine($"[bold]Total Lines:[/] {totalLines}");
            AnsiConsole.MarkupLine($"[bold]Total Size:[/] {totalSize / 1024} KB");

            return 0;
        }
    }
}
