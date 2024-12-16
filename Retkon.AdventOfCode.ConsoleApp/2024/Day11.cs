using AdventOfCodeSupport;
using CommunityToolkit.HighPerformance.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp._2024;

public class Day11 : AdventBase
{
    protected override object InternalPart1()
    {
        var stones = this.Input.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(v => long.Parse(v)).ToList();

        for (int step = 0; step < 25; step++)
        {
            for (int i = 0; i < stones.Count; i++)
            {
                var stone = stones[i];
                if (stone == 0)
                {
                    stones[i] = 1;
                    continue;
                }

                var stoneString = stone.ToString();

                if (stoneString.Length % 2 == 0)
                {
                    stones[i] = long.Parse(stoneString[..(stoneString.Length / 2)]);
                    stones.Insert(++i, long.Parse(stoneString[(stoneString.Length / 2)..]));
                }
                else
                {
                    stones[i] = stone * 2024;
                }
            }
        }

        return stones.Count;
    }

    protected override object InternalPart2()
    {
        var stones = this.Input.Text
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(v => long.Parse(v))
            .GroupBy(v => v)
            .ToDictionary(g => g.Key, v => (long)v.Count());

        for (int step = 0; step < 75; step++)
        {
            var newStones = new Dictionary<long, long>();

            foreach (var stoneKvp in stones)
            {
                var stone = stoneKvp.Key;
                var count = stoneKvp.Value;

                ApplyRule(stone, out var newStoneA, out var newStoneB);

                if (newStones.TryGetValue(newStoneA, out var currentStoneACount))
                {
                    newStones[newStoneA] += count;
                }
                else
                {
                    newStones[newStoneA] = count;
                }

                if (newStoneB != null)
                {
                    if (newStones.TryGetValue(newStoneB.Value, out var currentStoneBCount))
                    {
                        newStones[newStoneB.Value] += count;
                    }
                    else
                    {
                        newStones[newStoneB.Value] = count;
                    }
                }
            }

            stones = newStones;
        }

        var stoneCount = stones.Values.Sum();

        return stoneCount;
    }

    private static void ApplyRule(long stone, out long number1, out long? number2)
    {
        if (stone == 0)
        {
            number1 = 1;
            number2 = null;
            return;
        }

        var stoneString = stone.ToString();

        if (stoneString.Length % 2 == 0)
        {
            number1 = long.Parse(stoneString[..(stoneString.Length / 2)]);
            number2 = long.Parse(stoneString[(stoneString.Length / 2)..]);
            return;
        }
        else
        {
            number1 = stone * 2024;
            number2 = null;
            return;
        }
    }
}

