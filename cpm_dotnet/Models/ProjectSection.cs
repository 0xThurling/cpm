namespace cpm_dotnet.Models
{
    public partial class ProjectSection
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "executable"; // Default to executable
        public bool InstallHeaders { get; set; } = false;
    }
}
