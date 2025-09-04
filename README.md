# cpm: A Simple C++ Project Manager

`cpm` (C++ Project Manager) is a command-line tool designed to simplify the creation, building, and management of C++ projects using CMake. It provides a streamlined workflow for common development tasks, including project scaffolding, dependency management, and automated code generation.

## Features

*   **Project Creation:** Quickly scaffold new C++ projects with a standard directory structure, including `src/` and `assets/` directories.
*   **Dependency Management:** Declare Git-based dependencies in a `package.toml` file, which `cpm` automatically fetches and integrates into your CMake build.
*   **Automated Builds:** Generates `CMakeLists.txt` based on your `package.toml` and handles the entire CMake build process.
*   **Resource Management:** Embed assets (images, shaders, etc.) directly into your executable and access them through a simple, generated API.
*   **Code Generation:** Generate boilerplate for new C++ classes, structs, blank headers, or source file pairs.
*   **All-in-One Testing:** A single command to create the test harness (if needed), build, and run your tests using Google Test.
*   **LSP Support:** Automatically generates `compile_commands.json` for improved Language Server Protocol (LSP) support in editors.

## Installation

### Prerequisites

*   **Python 3.6+:** `cpm` is a Python script.
*   **CMake 3.20+:** Required for building C++ projects.
*   **Git:** Necessary for fetching Git-based dependencies.

### Easy Installation (Cross-Platform)

You can install `cpm` with a single command by running the `install.py` script:

```bash
git clone https://github.com/0xThurling/cpm.git
cd cpm
python3 install.py
```

The script will:
1.  Check for all the prerequisites.
2.  Install all needed dependencies in a virtual environment.
3.  Add `cpm` to your system's PATH.

### Updating

To update `cpm` to the latest version, simply run the `install.py` script again from within the `cpm` directory.

## Usage

### `cpm create <project_name>`

Creates a new C++ project.

```bash
cpm create my_new_project
```

This will create a directory `my_new_project` with the following structure:

```
my_new_project/
├── src/
│   └── main.cpp
├── assets/
├── package.toml
└── .gitignore
```

### `cpm build`

Generates the `CMakeLists.txt` file and builds the project.

### `cpm run [program_args...]`

Builds and runs the executable. Arguments after `run` are passed to your program.

### `cpm test`

Builds and runs the tests. If the test directory does not exist, `cpm` will automatically create it, add a sample test file, and configure `googletest` for you.

### `cpm clean`

Removes the `build/` directory.

### `cpm embed <file_path>`

Registers a resource file (e.g., `assets/icon.png`) in your `package.toml`. The `cpm build` command will then generate C++ code to make the asset's data available at runtime.

```bash
cpm embed assets/icon.png
```

In your code, you can then access the asset:
```cpp
#include "embedded_resources.h"

const Embedded::Resource& icon = Embedded::get("icon.png");
// Use icon.data and icon.size
```

### `cpm new <entity> <name>`

Generates boilerplate code in the `src/` directory.

*   **`cpm new class <ClassName>`**: Creates `ClassName.h` and `ClassName.cpp`.
*   **`cpm new struct <StructName>`**: Creates `StructName.h` and `StructName.cpp`.
*   **`cpm new header <FileName>`**: Creates a blank header file `FileName.h`.
*   **`cpm new source <FileName>`**: Creates `FileName.h` and `FileName.cpp`.

## `package.toml` Configuration

### Example `package.toml`

```toml
[project]
name = "my_project"
type = "executable"

[dependencies]
# Example:
# sdl = { git = "https://github.com/libsdl-org/SDL.git", tag = "release-2.30.3", target="SDL2::SDL2" }

[resources]
files = [
    # "assets/icon.png",
    # "assets/shader.glsl"
]
```

### `[resources]` Section

*   `files`: A list of file paths for assets you want to embed in your project via the `cpm embed` command.

## Contributing

Contributions are welcome! Please feel free to open issues or submit pull requests.

## License

This project is licensed under the MIT License.