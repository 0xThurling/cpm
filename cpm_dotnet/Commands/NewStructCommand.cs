using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.IO;

namespace cpm_dotnet.Commands
{
    public class NewStructCommandSettings : CommandSettings
    {
        [CommandArgument(0, "<NAME>")]
        [Description("The name of the struct.")]
        public string Name { get; set; } = string.Empty;
    }

    public class NewStructCommand : Command<NewStructCommandSettings>
    {
        public override int Execute(CommandContext context, NewStructCommandSettings settings)
        {
            var structName = settings.Name;
            if (!Directory.Exists("src"))
            {
                AnsiConsole.MarkupLine("[bold red]Error:[/bold red] `src` directory not found. Please run this command from the project root.");
                return 1;
            }

            var headerContent = $"#ifndef {structName.ToUpper()}_H\n#define {structName.ToUpper()}_H\n\nstruct {structName} {{\n    // struct members\n}};\n\n#endif // {structName.ToUpper()}_H";
            var cppContent = $"#include \"{structName}.h\"\n\n// Implementation for struct methods if any";

            var headerPath = Path.Combine("src", $"{structName}.h");
            var cppPath = Path.Combine("src", $"{structName}.cpp");

            if (File.Exists(headerPath) || File.Exists(cppPath))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/bold red] Struct `[bold]{structName}[/]` already exists.");
                return 1;
            }

            try
            {
                File.WriteAllText(headerPath, headerContent);
                File.WriteAllText(cppPath, cppContent);
                AnsiConsole.MarkupLine($"[bold green]Created struct `[bold]{structName}[/]` in `src/`.[/bold green]");
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