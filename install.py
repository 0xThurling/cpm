import os
import subprocess
import sys
import venv

def check_command(command):
    """Check if a command exists."""
    try:
        subprocess.run([command, "--version"], check=True, capture_output=True)
    except (subprocess.CalledProcessError, FileNotFoundError):
        print(f"Error: {command} is not installed. Please install it first.")
        sys.exit(1)

def install_pip():
    """Install pip if it's not available."""
    try:
        import pip
    except ImportError:
        print("pip not found. Installing pip...")
        import ensurepip
        ensurepip.bootstrap()
        # Re-run the script with pip available
        subprocess.run([sys.executable, *sys.argv], check=True, capture_output=True)
        sys.exit(0)

def setup_repo(install_dir, repo_url):
    """Clone or update the repository."""
    if os.path.exists(install_dir):
        print("Updating cpm...")
        subprocess.run(["git", "pull", "origin", "main"], cwd=install_dir, check=True, capture_output=True)
    else:
        print("Cloning cpm...")
        subprocess.run(["git", "clone", repo_url, install_dir], check=True, capture_output=True)

def setup_venv(install_dir):
    """Create a virtual environment and install dependencies."""
    print("Setting up virtual environment...")
    venv_dir = os.path.join(install_dir, "venv")
    venv.create(venv_dir, with_pip=True)

    pip_executable = os.path.join(venv_dir, "bin", "pip") if sys.platform != "win32" else os.path.join(venv_dir, "Scripts", "pip.exe")
    script_dir = os.path.dirname(os.path.abspath(__file__))
    requirements_path = os.path.join(script_dir, "requirements.txt")

    subprocess.run([pip_executable, "install", "-r", requirements_path], check=True, capture_output=True)

def create_executable(install_dir, bin_dir):
    """Create the cpm executable."""
    print("Creating executable...")
    cpm_executable_path = os.path.join(bin_dir, "cpm")
    cpm_script_path = os.path.join(install_dir, "cpm")

    if sys.platform == "win32":
        cpm_executable_path += ".bat"
        wrapper_content = rf'''@echo off
"%~dp0..\..\..\..\..\..{os.path.join(install_dir, "venv", "Scripts", "python.exe")}" "{cpm_script_path}" %*'''
    else:
        wrapper_content = f'''#!/bin/bash
source "{os.path.join(install_dir, "venv", "bin", "activate")}"
exec "{cpm_script_path}" "$@"'''

    os.makedirs(bin_dir, exist_ok=True)
    with open(cpm_executable_path, "w") as f:
        f.write(wrapper_content)

    if sys.platform != "win32":
        os.chmod(cpm_executable_path, 0o755)

def add_to_path(bin_dir):
    """Add the bin directory to the user's PATH."""
    print(f"Adding {bin_dir} to PATH...")
    if sys.platform == "win32":
        print(f"Please add {bin_dir} to your system's PATH manually by running this command in your terminal:")
        print(f'setx PATH "%PATH%;{bin_dir}"')
    else:
        shell_configs = {
            "bash": os.path.expanduser("~/.bashrc"),
            "zsh": os.path.expanduser("~/.zshrc"),
        }
        shell = os.environ.get("SHELL", "").split("/")[-1]
        if shell in shell_configs:
            config_file = shell_configs[shell]
            with open(config_file, "a") as f:
                f.write(f'\nexport PATH="$PATH:{bin_dir}"\n')
            print(f"Added {bin_dir} to your {config_file}. Please run 'source {config_file}' or restart your shell.")
        else:
            print(f"Could not determine shell. Please add {bin_dir} to your PATH manually.")

def main():
    install_dir = os.path.expanduser("~/.cpm")
    bin_dir = os.path.expanduser("~/.local/bin")
    repo_url = "https://github.com/0xThurling/cpm"  # Replace with actual URL

    print("Installing cpm...")

    # 1. Check prerequisites
    check_command("git")
    check_command("cmake")

    # 2. Install pip if necessary
    install_pip()

    # 3. Set up the repository
    setup_repo(install_dir, repo_url)

    # 4. Set up the virtual environment
    setup_venv(install_dir)

    # 5. Create the executable
    create_executable(install_dir, bin_dir)

    # 6. Add to PATH
    add_to_path(bin_dir)

    print("cpm has been installed.")
    print("You can now use 'cpm' from your terminal.")
    print("To update, simply run this install script again.")

if __name__ == "__main__":
    main()
