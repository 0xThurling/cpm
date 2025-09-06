using DotMake.CommandLine;

namespace cpm_dotnet.Commands
{
    [CliCommand(Description = "Create a new entity.", Parent = typeof(RootCommand))]
    public class NewCommand
    {
    }
}
