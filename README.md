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

### Using the install script (macOS and Linux)

You can install `cpm` by running the following command in your terminal:

```bash
curl -sSL https://raw.githubusercontent.com/0xThurling/cpm/main/install.sh | bash
```

This will download and run the `install.sh` script, which will install the `cpm` binary in `/usr/local/bin`.

### Manual Installation

1.  Download the latest binary for your platform from the [releases page](https://github.com/0xThurling/cpm/releases).
2.  Make the binary executable:
    ```bash
    chmod +x cpm
    ```
3.  Move the binary to a directory in your `PATH`. For example:
    ```bash
    # For macOS and Linux
    sudo mv cpm /usr/local/bin/
    ```

### Supported Platforms

*   macOS (x64, arm64)
*   Linux (x64)

## CLI Commands

### `cpm create <project_name>`

Creates a new C++ project with a standard directory structure.

### `cpm build`

Generates `CMakeLists.txt` and builds the project.

### `cpm run [program_args...]`

Builds and runs the executable. Arguments are passed to your program.

### `cpm test`

Builds and runs tests. It will automatically set up Google Test if not present.

### `cpm clean`

Removes the `build/` directory.

### `cpm embed <file_path>`

Embeds a resource file into the executable.

### `cpm new <entity> <name>`

Generates boilerplate code.
*   `class <ClassName>`: Creates a class.
*   `struct <StructName>`: Creates a struct.
*   `header <FileName>`: Creates a header file.
*   `source <FileName>`: Creates a header and source file.

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