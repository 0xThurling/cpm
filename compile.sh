#!/bin/bash

dotnet publish -c Release -r osx-arm64 -p:PublishAot=true --self-contained -o ~/.local/bin/
