using System.IO;
using Tomlyn;
using Tomlyn.Model;
using Spectre.Console;

namespace cpm_dotnet
{
  public static class ProjectConfigManager
  {
    private const string PackageConfigFileName = "package.toml";

    public class ProjectConfig
    {
      public ProjectSection Project { get; set; } = new ProjectSection();
      public Dictionary<string, Dependency> Dependencies { get; set; } = new Dictionary<string, Dependency>();
      public ResourcesSection Resources { get; set; } = new ResourcesSection();
    }

    public class ProjectSection
    {
      public string Name { get; set; } = string.Empty;
      public string Type { get; set; } = "executable"; // Default to executable
      public bool InstallHeaders { get; set; } = false;
    }

    public class Dependency
    {
      public string Git { get; set; } = string.Empty;
      public string Tag { get; set; } = string.Empty;
      public string Target { get; set; } = string.Empty; // Optional, defaults to key name
    }

    public class ResourcesSection
    {
      public List<string> Files { get; set; } = new List<string>();
    }

    public static ProjectConfig? LoadConfig()
    {
      if (!File.Exists(PackageConfigFileName))
      {
        return null;
      }

      try
      {
        var tomlString = File.ReadAllText(Path.Combine(PackageConfigFileName));
        var model = Toml.ToModel<ProjectConfig>(tomlString);
        return model;
      }
      catch (TomlException ex)
      {
        AnsiConsole.MarkupLine($"[bold red]Error parsing {PackageConfigFileName}:[/] {ex.Message}");
        return null;
      }
      catch (Exception ex)
      {
        AnsiConsole.MarkupLine($"[bold red]Error reading {PackageConfigFileName}:[/] {ex.Message}");
        return null;
      }
    }

    public static void SaveConfig(ProjectConfig config, string project_name = "")
    {
      try
      {
        var tomlString = Toml.FromModel(config);
        File.WriteAllText(Path.Combine(project_name, PackageConfigFileName), tomlString);
      }
      catch (Exception ex)
      {
        AnsiConsole.MarkupLine($"[bold red]Error writing to {PackageConfigFileName}:[/] {ex.Message}");
        throw;
      }
    }

    public static string? GetProjectName()
    {
      var config = LoadConfig();
      return config?.Project?.Name;
    }
  }
}
