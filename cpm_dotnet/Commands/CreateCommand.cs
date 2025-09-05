using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.IO;

namespace cpm_dotnet.Commands
{
    public class CreateCommandSettings : CommandSettings
    {
        [CommandArgument(0, "<NAME>")]
        [Description("The name of the project.")]
        public string Name { get; set; } = string.Empty;

        [CommandOption("--type")]
        [Description("Type of project to create (executable or library). Defaults to executable.")]
        [DefaultValue("executable")]
        public string Type { get; set; } = "executable";
    }

    public class CreateCommand : Command<CreateCommandSettings>
    {
        public override int Execute(CommandContext context, CreateCommandSettings settings)
        {
            var project_name = settings.Name;
            AnsiConsole.MarkupLine($"[bold cyan]--- Creating project: {project_name} ---[/]");

            try
            {
                // Create directories
                Directory.CreateDirectory(project_name);
                Directory.CreateDirectory(Path.Combine(project_name, "src"));
                Directory.CreateDirectory(Path.Combine(project_name, "assets"));

                // Create src/main.cpp for executable
                if (settings.Type == "executable")
                {
                    var mainCppContent = """
#include <iostream>

int main() {
    std::cout << "Hello, C++ World!" << std::endl;
    return 0;
}
""";
                    File.WriteAllText(Path.Combine(project_name, "src", "main.cpp"), mainCppContent);
                }

                // Create package.toml
                var projectConfig = new ProjectConfigManager.ProjectConfig
                {
                    Project = new ProjectConfigManager.ProjectSection
                    {
                        Name = project_name,
                        Type = settings.Type
                    }
                };

                if (settings.Type == "library")
                {
                    projectConfig.Project.InstallHeaders = true;
                }

                ProjectConfigManager.SaveConfig(projectConfig, project_name);

                // Create a placeholder .gitignore
                var gitignoreContent = "build/\nlib/\ncompile_commands.json\n";
                File.WriteAllText(Path.Combine(project_name, ".gitignore"), gitignoreContent);

                AnsiConsole.MarkupLine($"[bold green]Successfully created project `[bold yellow]{project_name}[/]`.[/]");
                AnsiConsole.MarkupLine($"To get started, `cd [bold yellow]{project_name}[/]`.");
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
