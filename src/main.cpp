#include "CLI/CLI.hpp"
#include "commands/create.hpp"
#define SOL_ALL_SAFETIES_ON 1
#include <sol/sol.hpp>
#include "utils/project_config_manager.hpp"

int main(int argc, char** argv) {
  // Initialise the application
  CLI::App app {"Forge CLI"};

  sol::state lua;
  lua.open_libraries(sol::lib::base);

  // ProjectConfigManager manager(lua);
  //
  // std::cout << manager.get_root_directory() << '\n';

  // Initialise my arguments 
  // TODO: Abstract to more generic function
  CreateCommandArgs command_args {};

  // Add commands
  create_command(app, command_args);

  // Require at least one command
  app.require_subcommand(1);

  CLI11_PARSE(app, argc, argv);

  return 0;
}
