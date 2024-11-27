using AdventOfCodeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp._2023;
internal class Day01 : AdventBase
{
    protected override object InternalPart1()
    {
        long sum = 0;
        foreach (var line in this.Input.Lines)
        {
            sum += int.Parse($"{line.Where(c => char.IsDigit(c)).First()}{line.Where(c => char.IsDigit(c)).Last()}");
        }

        return sum;
    }

    protected override object InternalPart2()
    {
        var regex = new Regex(@"(?<v1>(?=(?:one)|(?:1)))?(?<v2>(?=(?:two)|(?:2)))?(?<v3>(?=(?:three)|(?:3)))?(?<v4>(?=(?:four)|(?:4)))?(?<v5>(?=(?:five)|(?:5)))?(?<v6>(?=(?:six)|(?:6)))?(?<v7>(?=(?:seven)|(?:7)))?(?<v8>(?=(?:eight)|(?:8)))?(?<v9>(?=(?:nine)|(?:9)))?", RegexOptions.Compiled);

        long sum = 0;
        foreach (var line in this.Input.Lines)
        {
            var matches = regex.Matches(line);
            var orderedMatchingGroups = matches
                .SelectMany(m => m.Groups.Values)
                .Where(g => g.Success && g.Name != "0")
                .OrderBy(g => g.Index);

            sum += int.Parse($"{orderedMatchingGroups.First().Name[1]}{orderedMatchingGroups.Last().Name[1]}");
        }

        return sum;
    }
}
