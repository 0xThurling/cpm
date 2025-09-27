using DotMake.CommandLine;

namespace cpm.Commands
{
    [CliCommand(Name = "project", Description = "Commands for managing the project.", Parent = typeof(RootCommand))]
    public class ProjectCommand
    {
    }
}
