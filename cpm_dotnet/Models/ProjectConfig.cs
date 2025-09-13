using System.Collections.Generic;

namespace cpm_dotnet.Models
{
    public partial class ProjectConfig
    {
        public ProjectSection Project { get; set; } = new ProjectSection();
        public Dictionary<string, Dependency> Dependencies { get; set; } = new Dictionary<string, Dependency>();
        public ResourcesSection Resources { get; set; } = new ResourcesSection();
    }
}
