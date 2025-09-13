namespace cpm_dotnet.Models
{
    public partial class Dependency
    {
        public string Git { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty; // Optional, defaults to key name
    }
}
