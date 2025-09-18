using DotMake.CommandLine;

namespace cpm.Commands
{
    [CliCommand(Description = "Create a new entity.", Parent = typeof(RootCommand))]
    public class NewCommand
    {
    }
}
