#pragma once

#include "command.hpp"
#include <memory>
#include <vector>

class CommandRegistry {
public:
  void add(std::unique_ptr<Command> cmd);
  void run_all();
private:
  std::vector<std::unique_ptr<Command>> _commands;
};
