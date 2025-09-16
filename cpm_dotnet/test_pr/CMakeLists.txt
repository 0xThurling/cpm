cmake_minimum_required(VERSION 3.20)
project(test_pr LANGUAGES CXX)

set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

include(FetchContent)

# --- Dependencies ---
FetchContent_Declare(googletest GIT_REPOSITORY "https://github.com/google/googletest.git" GIT_TAG "v1.14.0")

FetchContent_MakeAvailable(googletest)

# --- Project Target ---
file(GLOB_RECURSE SOURCES ${PROJECT_SOURCE_DIR}/src/*.cpp)
add_executable(test_pr ${SOURCES})

# --- Linking ---

# --- Testing ---
enable_testing()

file(GLOB_RECURSE TEST_SOURCES "${PROJECT_SOURCE_DIR}/test/*.cpp")
message(STATUS "Found test sources: ${TEST_SOURCES}")

set(SOURCES_FOR_TESTS ${SOURCES})
if(EXISTS "${PROJECT_SOURCE_DIR}/src/main.cpp")
    list(REMOVE_ITEM SOURCES_FOR_TESTS "${PROJECT_SOURCE_DIR}/src/main.cpp")
endif()

add_executable(run_tests ${TEST_SOURCES} ${SOURCES_FOR_TESTS})

target_include_directories(run_tests PRIVATE ${CMAKE_CURRENT_SOURCE_DIR}/src)

target_link_libraries(run_tests PUBLIC GTest::gtest_main)

include(GoogleTest)
gtest_discover_tests(run_tests)
