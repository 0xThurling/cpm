using Spectre.Console.Cli;
using Spectre.Console;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;

namespace cpm_dotnet.Commands
{
    public class TestCommandSettings : CommandSettings
    {
        [CommandArgument(0, "[TEST_SUITE_NAME]")]
        [Description("Optional: Name of the test suite to run (e.g., MyTestSuite).")]
        public string? TestSuiteName { get; set; }

        [CommandOption("--filter")]
        [Description("Filter tests to run (e.g., MyTestSuite.TestName or MyTestSuite.*).")]
        public string? Filter { get; set; }

        [CommandOption("--std")]
        [Description("C++ standard to use (e.g., 11, 14, 17, 20). Defaults to 20.")]
        [DefaultValue("20")]
        public string Standard { get; set; } = "20";
    }

    public class TestCommand : Command<TestCommandSettings>
    {
        public override int Execute(CommandContext context, TestCommandSettings settings)
        {
            if (!Directory.Exists("test"))
            {
                Utils.CreateTests();
            }

            // Build the project (which includes tests if googletest is present)
            var buildCommand = new BuildCommand();
            var buildSettings = new BuildCommandSettings
            {
                Verbose = false, // Tests usually don't need verbose build output
                Standard = settings.Standard
            };
            var buildResult = buildCommand.Execute(context, buildSettings);
            if (buildResult != 0)
            {
                return buildResult;
            }

            AnsiConsole.MarkupLine("[bold cyan]--- Running Tests ---");

            var testExecutable = Path.Combine("build", "run_tests");
            if (!File.Exists(testExecutable))
            {
                AnsiConsole.MarkupLine("[bold red]Error:[/bold red] Test executable not found. Ensure googletest is a dependency and project builds correctly.");
                return 1;
            }

            try
            {
                var testCommandArgs = new List<string>();
                var gtestFilter = settings.Filter;
                if (string.IsNullOrEmpty(gtestFilter) && !string.IsNullOrEmpty(settings.TestSuiteName))
                {
                    gtestFilter = $"{settings.TestSuiteName}.*";
                }

                if (!string.IsNullOrEmpty(gtestFilter))
                {
                    testCommandArgs.Add($"--gtest_filter={gtestFilter}");
                }

                var processStartInfo = new ProcessStartInfo(testExecutable)
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    CreateNoWindow = true,
                };

                foreach (var arg in testCommandArgs)
                {
                    processStartInfo.ArgumentList.Add(arg);
                }

                using (var process = Process.Start(processStartInfo))
                {
                    if (process == null) throw new Exception("Failed to start test process.");
                    process.WaitForExit();
                    if (process.ExitCode != 0)
                    {
                        AnsiConsole.MarkupLine("[bold red]Tests failed.[/bold red]");
                        return 1;
                    }
                }
                AnsiConsole.MarkupLine("[bold green]All tests passed.[/bold green]");
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenPaths | ExceptionFormats.ShowLinks);
                return 1;
            }

            return 0;
        }
    }
}