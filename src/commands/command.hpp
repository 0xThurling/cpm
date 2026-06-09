#pragma once

#include "CLI/CLI.hpp"

class Command {
public:
  explicit Command(CLI::App& app) : _app(app) {}

  virtual ~Command() = default;
  virtual void run() = 0;
protected:
  CLI::App& _app;
};
