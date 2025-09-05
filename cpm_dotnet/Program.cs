using Spectre.Console.Cli;
using cpm_dotnet.Commands;

var app = new CommandApp();

app.AddCommand<BuildCommand>("build")
    .WithDescription("Generate CMakeLists and build the project.");

app.AddCommand<CreateCommand>("create")
    .WithDescription("Create a new C++ project.");

app.AddCommand<RunCommand>("run")
    .WithDescription("Build and run the project.");

app.AddCommand<CleanCommand>("clean")
    .WithDescription("Remove the build directory.");

app.AddCommand<TestCommand>("test")
    .WithDescription("Build and run tests.");

app.AddBranch<NewCommandSettings>("new", config =>
{
    config.SetDescription("Create a new entity.");
    config.AddCommand<NewClassCommand>("class")
          .WithDescription("Create a new class.");
    config.AddCommand<NewStructCommand>("struct")
          .WithDescription("Create a new struct.");
    config.AddCommand<NewHeaderCommand>("header")
          .WithDescription("Create a new header file.");
    config.AddCommand<NewSourceCommand>("source")
          .WithDescription("Create a new source file pair (.h/.cpp).");
});

app.AddCommand<EmbedCommand>("embed")
    .WithDescription("Embed a resource file into a C++ header.");

return app.Run(args);