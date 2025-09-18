#!/bin/bash

# This script downloads and installs the latest release of cpm.

set -e

# Get the latest release tag from GitHub.
LATEST_RELEASE=$(curl -s "https://api.github.com/repos/0xThurling/cpm/releases/latest" | grep '"tag_name":' | sed -E 's/.*"([^"]+)".*/\1/')

if [ -z "$LATEST_RELEASE" ]; then
    echo "Error: Could not get the latest release tag from GitHub."
    exit 1
fi

# Determine the OS and architecture.
OS=$(uname -s | tr '[:upper:]' '[:lower:]')
ARCH=$(uname -m)

if [ "$OS" == "darwin" ]; then
    OS="osx"
fi

if [ "$ARCH" == "x86_64" ]; then
    ARCH="x64"
fi

# Construct the asset name.
ASSET_NAME="cpm-${OS}-${ARCH}"

# Construct the download URL.
DOWNLOAD_URL="https://github.com/0xThurling/cpm/releases/download/${LATEST_RELEASE}/${ASSET_NAME}"

# Download the asset.
echo "Downloading ${ASSET_NAME} from ${DOWNLOAD_URL}..."
curl -L -o cpm "${DOWNLOAD_URL}"

# Make the binary executable.
chmod +x cpm

# Move the binary to /usr/local/bin.
echo "Installing cpm to /usr/local/bin..."
sudo mv cpm /usr/local/bin/cpm

echo "cpm has been installed successfully!"
