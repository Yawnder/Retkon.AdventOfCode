using AdventOfCodeSupport;

namespace Retkon.AdventOfCode.ConsoleApp;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, Adventer!");

        var solutions = new AdventSolutions();
        var day = solutions.GetMostRecentDay();

        await day.DownloadInputAsync();

        // var day3 = solutions.GetDay(2024, 3);
        // var day4 = solutions.First(x => x.Year == 2024 && x.Day == 4);
        day.Part1();
        day.Part2();
        await day.SubmitPart1Async();
        await day.SubmitPart2Async();
    }
}
