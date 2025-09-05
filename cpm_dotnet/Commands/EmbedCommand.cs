using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.IO;

namespace cpm_dotnet.Commands
{
    public class EmbedCommandSettings : CommandSettings
    {
        [CommandArgument(0, "<FILE_PATH>")]
        [Description("The path to the resource file to embed.")]
        public string FilePath { get; set; } = string.Empty;
    }

    public class EmbedCommand : Command<EmbedCommandSettings>
    {
        public override int Execute(CommandContext context, EmbedCommandSettings settings)
        {
            if (!File.Exists(settings.FilePath))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/bold red] File not found at '[bold]{settings.FilePath}[/]'.");
                return 1;
            }

            AnsiConsole.MarkupLine($"[bold cyan]--- Registering resource: {settings.FilePath} ---");

            var config = ProjectConfigManager.LoadConfig();
            if (config == null)
            {
                AnsiConsole.MarkupLine("[bold red]Error:[/bold red] `package.toml` not found.");
                return 1;
            }

            // Use relative path for portability
            var relativePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), settings.FilePath);

            if (config.Resources == null)
            {
                config.Resources = new ProjectConfigManager.ResourcesSection();
            }

            if (!config.Resources.Files.Contains(relativePath))
            {
                config.Resources.Files.Add(relativePath);
                try
                {
                    ProjectConfigManager.SaveConfig(config);
                    AnsiConsole.MarkupLine($"[bold green]Successfully registered `[bold]{relativePath}[/]` in package.toml.[/bold green]");
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine($"[bold red]Error:[/bold red] Could not write to package.toml: {ex.Message}");
                    return 1;
                }
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]Resource `[bold]{relativePath}[/]` is already registered in package.toml.[/yellow]");
            }

            return 0;
        }
    }
}