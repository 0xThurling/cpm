#include "project_config_manager.hpp"
#include <filesystem>

std::string ProjectConfigManager::get_root_directory() {
  std::filesystem::path path = std::filesystem::current_path();

  return path.string();
}
