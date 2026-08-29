########## MACROS ###########################################################################
#############################################################################################

# Requires CMake > 3.15
if(${CMAKE_VERSION} VERSION_LESS "3.15")
    message(FATAL_ERROR "The 'CMakeDeps' generator only works with CMake >= 3.15")
endif()

if(sol2_FIND_QUIETLY)
    set(sol2_MESSAGE_MODE VERBOSE)
else()
    set(sol2_MESSAGE_MODE STATUS)
endif()

include(${CMAKE_CURRENT_LIST_DIR}/cmakedeps_macros.cmake)
include(${CMAKE_CURRENT_LIST_DIR}/sol2Targets.cmake)
include(CMakeFindDependencyMacro)

check_build_type_defined()

foreach(_DEPENDENCY ${sol2_FIND_DEPENDENCY_NAMES} )
    # Check that we have not already called a find_package with the transitive dependency
    if(NOT ${_DEPENDENCY}_FOUND)
        find_dependency(${_DEPENDENCY} REQUIRED ${${_DEPENDENCY}_FIND_MODE})
    endif()
endforeach()

set(sol2_VERSION_STRING "3.5.0")
set(sol2_INCLUDE_DIRS ${sol2_INCLUDE_DIRS_RELEASE} )
set(sol2_INCLUDE_DIR ${sol2_INCLUDE_DIRS_RELEASE} )
set(sol2_LIBRARIES ${sol2_LIBRARIES_RELEASE} )
set(sol2_DEFINITIONS ${sol2_DEFINITIONS_RELEASE} )


# Definition of extra CMake variables from cmake_extra_variables


# Only the last installed configuration BUILD_MODULES are included to avoid the collision
foreach(_BUILD_MODULE ${sol2_BUILD_MODULES_PATHS_RELEASE} )
    message(${sol2_MESSAGE_MODE} "Conan: Including build module from '${_BUILD_MODULE}'")
    include(${_BUILD_MODULE})
endforeach()


