#pragma once

#include "command.hpp"

struct TempCommandArgs {
  std::string name;
};

class TempCommand : public Command {
public:
  explicit TempCommand(CLI::App& app);

  void run() override;

private:
  TempCommandArgs _args;
};
