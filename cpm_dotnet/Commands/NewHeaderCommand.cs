using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;

namespace cpm_dotnet.Commands
{
    public class NewHeaderCommandSettings : NewCommandSettings 
    {
        [CommandArgument(0, "<NAME>")]
        [Description("The name of the header file (without extension).")]
        public string Name { get; set; } = string.Empty;
    }

    public class NewHeaderCommand : Command<NewHeaderCommandSettings>
    {
        public override int Execute(CommandContext context, NewHeaderCommandSettings settings)
        {
            var fileName = settings.Name;
            if (!Directory.Exists("src"))
            {
                AnsiConsole.MarkupLine("[bold red]Error:[/bold red] `src` directory not found. Please run this command from the project root.");
                return 1;
            }

            var headerContent = $"""
#ifndef {fileName.ToUpper()}_H
#define {fileName.ToUpper()}_H

// Your code here

#endif // {fileName.ToUpper()}_H
""";

            var headerPath = Path.Combine("src", $"{fileName}.h");

            if (File.Exists(headerPath))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/bold red] Header file `[bold]{fileName}.h[/]` already exists.");
                return 1;
            }

            try
            {
                File.WriteAllText(headerPath, headerContent);
                AnsiConsole.MarkupLine($"[bold green]Created header file `[bold]{fileName}.h[/]` in `src/`.[/bold green]");
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
