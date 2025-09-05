using Spectre.Console.Cli;
using Spectre.Console;
using System.IO;

namespace cpm_dotnet.Commands
{
    public class CleanCommandSettings : CommandSettings
    {
    }

    public class CleanCommand : Command<CleanCommandSettings>
    {
        public override int Execute(CommandContext context, CleanCommandSettings settings)
        {
            var buildDir = "build";
            if (Directory.Exists(buildDir))
            {
                AnsiConsole.MarkupLine($"[bold cyan]--- Removing build directory: {buildDir} ---");
                try
                {
                    Directory.Delete(buildDir, true);
                    AnsiConsole.MarkupLine("[bold green]Project cleaned.[/bold green]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenPaths | ExceptionFormats.ShowLinks);
                    return 1;
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Build directory not found. Nothing to clean.[/yellow]");
            }
            return 0;
        }
    }
}