using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.IO;

namespace cpm_dotnet.Commands
{
    public class NewSourceCommandSettings : NewCommandSettings 
    {
        [CommandArgument(0, "<NAME>")]
        [Description("The name of the source files (without extension).")]
        public string Name { get; set; } = string.Empty;
    }

    public class NewSourceCommand : Command<NewSourceCommandSettings>
    {
        public override int Execute(CommandContext context, NewSourceCommandSettings settings)
        {
            var fileName = settings.Name;
            if (!Directory.Exists("src"))
            {
                AnsiConsole.MarkupLine("[bold red]Error:[/bold red] `src` directory not found. Please run this command from the project root.");
                return 1;
            }

            var headerContent = $$"""
#ifndef {{fileName.ToUpper()}}_H
#define {{fileName.ToUpper()}}_H

// Your code here

#endif // {{fileName.ToUpper()}}_H
""";
            var cppContent = $$"""
#include "{{fileName}}.h"

// Your code here
""";

            var headerPath = Path.Combine("src", $"{fileName}.h");
            var cppPath = Path.Combine("src", $"{fileName}.cpp");

            if (File.Exists(headerPath) || File.Exists(cppPath))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/bold red] Source files `[bold]{fileName}[/]` already exist.");
                return 1;
            }

            try
            {
                File.WriteAllText(headerPath, headerContent);
                File.WriteAllText(cppPath, cppContent);
                AnsiConsole.MarkupLine($"[bold green]Created source files `[bold]{fileName}.h[/]` and `[bold]{fileName}.cpp[/]` in `src/`.[/bold green]");
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
