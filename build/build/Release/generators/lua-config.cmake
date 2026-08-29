########## MACROS ###########################################################################
#############################################################################################

# Requires CMake > 3.15
if(${CMAKE_VERSION} VERSION_LESS "3.15")
    message(FATAL_ERROR "The 'CMakeDeps' generator only works with CMake >= 3.15")
endif()

if(lua_FIND_QUIETLY)
    set(lua_MESSAGE_MODE VERBOSE)
else()
    set(lua_MESSAGE_MODE STATUS)
endif()

include(${CMAKE_CURRENT_LIST_DIR}/cmakedeps_macros.cmake)
include(${CMAKE_CURRENT_LIST_DIR}/luaTargets.cmake)
include(CMakeFindDependencyMacro)

check_build_type_defined()

foreach(_DEPENDENCY ${lua_FIND_DEPENDENCY_NAMES} )
    # Check that we have not already called a find_package with the transitive dependency
    if(NOT ${_DEPENDENCY}_FOUND)
        find_dependency(${_DEPENDENCY} REQUIRED ${${_DEPENDENCY}_FIND_MODE})
    endif()
endforeach()

set(lua_VERSION_STRING "5.4.6")
set(lua_INCLUDE_DIRS ${lua_INCLUDE_DIRS_RELEASE} )
set(lua_INCLUDE_DIR ${lua_INCLUDE_DIRS_RELEASE} )
set(lua_LIBRARIES ${lua_LIBRARIES_RELEASE} )
set(lua_DEFINITIONS ${lua_DEFINITIONS_RELEASE} )


# Definition of extra CMake variables from cmake_extra_variables


# Only the last installed configuration BUILD_MODULES are included to avoid the collision
foreach(_BUILD_MODULE ${lua_BUILD_MODULES_PATHS_RELEASE} )
    message(${lua_MESSAGE_MODE} "Conan: Including build module from '${_BUILD_MODULE}'")
    include(${_BUILD_MODULE})
endforeach()


