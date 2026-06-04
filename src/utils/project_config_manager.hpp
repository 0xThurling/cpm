#include <sol/sol.hpp>

class ProjectConfigManager {
public:
  // Cannot instantiate
  ProjectConfigManager(sol::state &state) : _state(&state) {}
                                           
  // Loads forge.lua config
  sol::table load_config();

  // Get root directory path
  std::string get_root_directory();
private:
  // Lua Global state
  sol::state *_state;
};
