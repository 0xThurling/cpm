using Spectre.Console.Cli;
using cpm_dotnet.Commands;

var app = new CommandApp();
app.Configure(config =>
{
  config.AddCommand<BuildCommand>("build")
      .WithDescription("Generate CMakeLists and build the project.");

  config.AddCommand<CreateCommand>("create")
      .WithDescription("Create a new C++ project.");

  config.AddCommand<RunCommand>("run")
      .WithDescription("Build and run the project.");

  config.AddCommand<CleanCommand>("clean")
      .WithDescription("Remove the build directory.");

  config.AddCommand<TestCommand>("test")
      .WithDescription("Build and run tests.");


  config.AddBranch<NewCommandSettings>("new", new_cmd =>
  {
    new_cmd.SetDescription("Create a new entity.");
    new_cmd.AddCommand<NewClassCommand>("class")
          .WithDescription("Create a new class.");
    new_cmd.AddCommand<NewStructCommand>("struct")
          .WithDescription("Create a new struct.");
    new_cmd.AddCommand<NewHeaderCommand>("header")
          .WithDescription("Create a new header file.");
    new_cmd.AddCommand<NewSourceCommand>("source")
          .WithDescription("Create a new source file pair (.h/.cpp).");
  });

  config.AddCommand<EmbedCommand>("embed")
     .WithDescription("Embed a resource file into a C++ header.");
});



return app.Run(args);
