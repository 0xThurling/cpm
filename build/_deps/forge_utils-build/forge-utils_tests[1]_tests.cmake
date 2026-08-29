add_test([=[HelloTest.BasicAssertions]=]  /root/forge/forge-cli/build/_deps/forge_utils-build/forge-utils_tests [==[--gtest_filter=HelloTest.BasicAssertions]==] --gtest_also_run_disabled_tests)
set_tests_properties([=[HelloTest.BasicAssertions]=]  PROPERTIES DEF_SOURCE_LINE /root/forge/forge-cli/build/_deps/forge_utils-src/test/main.cpp:4 WORKING_DIRECTORY /root/forge/forge-cli/build/_deps/forge_utils-build SKIP_REGULAR_EXPRESSION [==[\[  SKIPPED \]]==])
set(  forge-utils_tests_TESTS HelloTest.BasicAssertions)
