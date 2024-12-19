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
        //var day = solutions.GetDay(2024, 14);
        //day.Part1().Part2().Benchmark();

        var day = solutions.GetMostRecentDay();
        // var day = solutions.First(x => x.Year == 2024 && x.Day == 4);

        await day.DownloadInputAsync();

        //day.Part1().Part2();
        //await day.SubmitPart1Async();
        day.Part2();

        await day.SubmitPart2Async();

        await Task.CompletedTask;
    }
}
