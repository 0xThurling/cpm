#include "temp.hpp"
#include "CLI/CLI.hpp"
#include "command.hpp"
#include <iostream>

TempCommand::TempCommand(CLI::App &app) : Command(app) {}

void TempCommand::run() {
  auto *temp = _app.add_subcommand("temp", "Just to trigger the application");

  temp->add_option("name", _args.name, "Just garbage");

  temp->callback([this]() { std::cout << _args.name << '\n'; });
}
