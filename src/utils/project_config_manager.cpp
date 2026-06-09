#include "project_config_manager.hpp"
#include <filesystem>
#include <optional>

std::optional<std::filesystem::path> ProjectConfigManager::get_root_directory() {
  std::filesystem::path current = std::filesystem::absolute(std::filesystem::current_path());

  while (true) {
    if (std::filesystem::exists(current / "forge.lua")) {
      return current;
    }

    std::filesystem::path parent = current.parent_path(); 

    if (parent == current) {
      return std::nullopt;
    }

    current = parent;
  }
}

sol::optional<sol::table> ProjectConfigManager::load_config() {
  auto root_directory = ProjectConfigManager::get_root_directory();

  if (root_directory) {
    return this->_state->script_file(*root_directory / "forge.lua");
  }
  
  return std::nullopt;
} 
