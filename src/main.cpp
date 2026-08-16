#include "CLI/CLI.hpp"
#include "commands/command_registry.hpp"
#include "commands/temp.hpp"
#include <memory>
#include <string>
#define SOL_ALL_SAFETIES_ON 1
#include <sol/sol.hpp>
#include "utils/project_config_manager.hpp"

int main(int argc, char** argv) {
  // Initialise the application
  CLI::App app {"Forge CLI"};

  sol::state lua;
  lua.open_libraries(sol::lib::base);

  ProjectConfigManager manager(lua);
  auto config_opt = manager.load_config();

  // Working
  if (config_opt) {
    sol::table config = *config_opt;

    sol::table project = config["project"];
    std::string name = project["name"];
  }

  // // Initialise my arguments 
  // // TODO: Abstract to more generic function
  // CreateCommandArgs command_args {};
  CommandRegistry registry;

  registry.add(std::make_unique<TempCommand>(app));

  registry.run_all();

  // Add commands
  // create_command(app, command_args);
  // temp_command(app, temp_args);

  // Require at least one command
  app.require_subcommand(1);

  CLI11_PARSE(app, argc, argv);

  return 0;
}
