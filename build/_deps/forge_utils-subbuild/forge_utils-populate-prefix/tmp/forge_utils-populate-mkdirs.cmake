# Distributed under the OSI-approved BSD 3-Clause License.  See accompanying
# file LICENSE.rst or https://cmake.org/licensing for details.

cmake_minimum_required(VERSION ${CMAKE_VERSION}) # this file comes with cmake

# If CMAKE_DISABLE_SOURCE_CHANGES is set to true and the source directory is an
# existing directory in our source tree, calling file(MAKE_DIRECTORY) on it
# would cause a fatal error, even though it would be a no-op.
if(NOT EXISTS "/root/forge/forge-cli/build/_deps/forge_utils-src")
  file(MAKE_DIRECTORY "/root/forge/forge-cli/build/_deps/forge_utils-src")
endif()
file(MAKE_DIRECTORY
  "/root/forge/forge-cli/build/_deps/forge_utils-build"
  "/root/forge/forge-cli/build/_deps/forge_utils-subbuild/forge_utils-populate-prefix"
  "/root/forge/forge-cli/build/_deps/forge_utils-subbuild/forge_utils-populate-prefix/tmp"
  "/root/forge/forge-cli/build/_deps/forge_utils-subbuild/forge_utils-populate-prefix/src/forge_utils-populate-stamp"
  "/root/forge/forge-cli/build/_deps/forge_utils-subbuild/forge_utils-populate-prefix/src"
  "/root/forge/forge-cli/build/_deps/forge_utils-subbuild/forge_utils-populate-prefix/src/forge_utils-populate-stamp"
)

set(configSubDirs )
foreach(subDir IN LISTS configSubDirs)
    file(MAKE_DIRECTORY "/root/forge/forge-cli/build/_deps/forge_utils-subbuild/forge_utils-populate-prefix/src/forge_utils-populate-stamp/${subDir}")
endforeach()
if(cfgdir)
  file(MAKE_DIRECTORY "/root/forge/forge-cli/build/_deps/forge_utils-subbuild/forge_utils-populate-prefix/src/forge_utils-populate-stamp${cfgdir}") # cfgdir has leading slash
endif()
