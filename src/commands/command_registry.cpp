#include "command_registry.hpp"
#include <memory>
#include <utility>

void CommandRegistry::add(std::unique_ptr<Command> cmd) {
  _commands.push_back(std::move(cmd));
}

void CommandRegistry::run_all() {
  for (auto& cmd : _commands) {
    cmd->run();
  }
}
