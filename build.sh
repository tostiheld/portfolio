#! /bin/sh

# Build script for roadplus-embedded.
# Add new projects to this script to keep the ci environment
# running.

# REQUIREMENTS
# - arduino ide version 1.6.1
# - arturo

# Example build command:
# cd <project-name> && ano build -s . -d ../arduino-1.6.1 && cd ..

ano version

# project build commands be below here

# test project
cd blink && && ano build -d ../arduino-1.6.1 && cd ..
