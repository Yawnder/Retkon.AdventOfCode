using AdventOfCodeSupport;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp._2024;

public class Day03 : AdventBase
{
    protected override object InternalPart1()
    {
        var reg = new Regex(@"(?:mul\((\d{1,3}),(\d{1,3})\))");

        var matches = reg.Matches(this.Input.Text);

        var result = 0;
        foreach (var match in matches.AsEnumerable())
        {
            result += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        }

        return result;
    }

    protected override object InternalPart2()
    {
        var reg = new Regex(@"(mul\((\d{1,3}),(\d{1,3})\))|(?<do>do\(\))|(?<no>don\'t\(\))");

        var matches = reg.Matches(this.Input.Text);

        var result = 0;
        var multiply = true;
        foreach (var match in matches.AsEnumerable())
        {
            if (match.Groups["do"].Success)
            {
                multiply = true;
            }
            else if (match.Groups["no"].Success)
            {
                multiply = false;
            }
            else if (multiply)
            {
                result += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
            }
        }

        return result;
    }
}

