cmake_minimum_required(VERSION 3.20)
project(test_pr LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

include(FetchContent)

# --- Dependencies ---


# --- Project Target ---
file(GLOB_RECURSE SOURCES ${PROJECT_SOURCE_DIR}/src/*.cpp)
add_executable(test_pr ${SOURCES})

# --- Linking ---
