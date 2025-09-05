using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.IO;

namespace cpm_dotnet.Commands
{
  public class NewClassCommandSettings : NewCommandSettings 
  {
    [CommandArgument(0, "<NAME>")]
    [Description("The name of the class.")]
    public string Name { get; set; } = string.Empty;
  }

  public class NewClassCommand : Command<NewClassCommandSettings>
  {
    public override int Execute(CommandContext context, NewClassCommandSettings settings)
    {
      var className = settings.Name;
      if (!Directory.Exists("src"))
      {
        AnsiConsole.MarkupLine("[bold red]Error:[/bold red] `src` directory not found. Please run this command from the project root.");
        return 1;
      }

      var headerContent = $$"""
#ifndef {{className.ToUpper()}}_H
#define {{className.ToUpper()}}_H

          class {{className}} { 
          public:
              {{className}}();
              ~{{className}}();
          };

#endif // {{className.ToUpper()}}_H
""";
      var cppContent = $$"""
#include "{{className}}.h"

{{className}}::{{className}}() {
    // Constructor implementation
}

{{className}}::~{{className}}() {
    // Destructor implementation
}
""";

      var headerPath = Path.Combine("src", $"{className}.h");
      var cppPath = Path.Combine("src", $"{className}.cpp");

      if (File.Exists(headerPath) || File.Exists(cppPath))
      {
        AnsiConsole.MarkupLine($"[bold red]Error:[/bold red] Class `[bold]{className}[/]` already exists.");
        return 1;
      }

      try
      {
        File.WriteAllText(headerPath, headerContent);
        File.WriteAllText(cppPath, cppContent);
        AnsiConsole.MarkupLine($"[bold green]Created class `[bold]{className}[/]` in `src/`.[/bold green]");
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
