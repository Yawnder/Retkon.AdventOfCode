using AdventOfCodeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp._2024;

public class Day02 : AdventBase
{
    protected override object InternalPart1()
    {
        var result = 0;

        var levels = new List<int>();
        foreach (var line in this.Input.Lines)
        {
            var increasing = true;
            var decreasing = true;
            var properlySpaced = true;

            var lineValues = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var lastValue = int.Parse(lineValues[0]);
            for (int i = 1; i < lineValues.Length; i++)
            {
                var lineValue = int.Parse(lineValues[i]);
                increasing &= lineValue > lastValue;
                decreasing &= lineValue < lastValue;
                properlySpaced &= Math.Abs(lineValue - lastValue) >= 1 && Math.Abs(lineValue - lastValue) <= 3;
                lastValue = lineValue;
            }
            if (properlySpaced && (increasing || decreasing))
            {
                result++;
            }
        }

        return result;
    }

    protected override object InternalPart2()
    {
        var result = 0;

        var levels = new List<int>();
        foreach (var line in this.Input.Lines)
        {
            var allLineValues = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            for (int skip = 0; skip < allLineValues.Length; skip++)
            {
                var increasing = true;
                var decreasing = true;
                var properlySpaced = true;

                var lineValues = allLineValues.ToArray();
                lineValues[skip] = "";
                lineValues = lineValues.Where(e => e != "").ToArray();

                var lastValue = int.Parse(lineValues[0]);
                for (int i = 1; i < lineValues.Length; i++)
                {
                    var lineValue = int.Parse(lineValues[i]);
                    increasing &= lineValue > lastValue;
                    decreasing &= lineValue < lastValue;
                    properlySpaced &= Math.Abs(lineValue - lastValue) >= 1 && Math.Abs(lineValue - lastValue) <= 3;
                    lastValue = lineValue;
                }
                if (properlySpaced && (increasing || decreasing))
                {
                    result++;
                    break;
                }
            }

        }

        return result;
    }
}
