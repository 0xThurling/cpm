add_test([=[HelloTest.BasicAssertions]=]  /root/forge/forge-cli/build/forge_tests [==[--gtest_filter=HelloTest.BasicAssertions]==] --gtest_also_run_disabled_tests)
set_tests_properties([=[HelloTest.BasicAssertions]=]  PROPERTIES DEF_SOURCE_LINE /root/forge/forge-cli/test/main.cpp:4 WORKING_DIRECTORY /root/forge/forge-cli/build SKIP_REGULAR_EXPRESSION [==[\[  SKIPPED \]]==])
set(  forge_tests_TESTS HelloTest.BasicAssertions)
