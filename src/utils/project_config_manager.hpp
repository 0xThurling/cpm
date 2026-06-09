#pragma once

#include <filesystem>
#include <sol/sol.hpp>

class ProjectConfigManager {
public:
  // Cannot instantiate
  ProjectConfigManager(sol::state &state) : _state(&state) {}
                                           
  // Get root directory path
  std::optional<std::filesystem::path> get_root_directory();

  // Loads forge.lua config
  sol::optional<sol::table> load_config();
private:
  // Lua Global state
  sol::state *_state;
};
