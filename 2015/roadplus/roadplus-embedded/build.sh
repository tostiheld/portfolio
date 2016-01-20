#! /bin/bash

# Build script for roadplus-embedded on a travis-ci environment.
# Add new projects to this script to keep the ci environment
# running.

# REQUIREMENTS
# - arduino ide version 1.6.1
# - arturo

# Example build command:
# cd <project-name> && ano build -s . -d ../arduino-1.6.1 && cd ..

echo "This script is meant to be run on travis-ci"
echo "Building with ano version:"
ano version

# project build commands be below here

# test project
printf "\033[1;31m#### Building blink ####\033[0m\n"
cd blink && ano build -d ../arduino-1.6.1 && cd ..
