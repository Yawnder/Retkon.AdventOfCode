using AdventOfCodeSupport;

namespace Retkon.AdventOfCode.ConsoleApp;

internal class Program
{
#pragma warning disable IDE0060 // Remove unused parameter
    static async Task Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        Console.WriteLine("Hello, Adventer!");

        var solutions = new AdventSolutions();
        //var day = solutions.GetDay(2024, 17);

        //day.Part1().Part2().Benchmark();

        var day = solutions.GetMostRecentDay();

        await day.DownloadInputAsync();

        day.Part1();
        await day.SubmitPart1Async();
        day.Part2();
        await day.SubmitPart2Async();

        await Task.CompletedTask;
    }
}
