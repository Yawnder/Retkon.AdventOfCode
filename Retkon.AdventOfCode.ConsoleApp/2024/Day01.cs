using AdventOfCodeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp._2024;

public class Day01 : AdventBase
{
    protected override object InternalPart1()
    {
        var list1 = new List<int>();
        var list2 = new List<int>();
        foreach (var line in this.Input.Lines)
        {
            var lineValues = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            list1.Add(int.Parse(lineValues[0]));
            list2.Add(int.Parse(lineValues[1]));
        }

        var order = 0;
        var list1Prime = list1.OrderBy(v => v).Select(v => new { Order = order++, Value = v }).ToList();
        order = 0;
        var list2Prime = list2.OrderBy(v => v).Select(v => new { Order = order++, Value = v }).ToList();

        var result = list1Prime.Join(list2Prime, vp => vp.Order, vp => vp.Order, (l1, l2) => Math.Abs(l1.Value - l2.Value)).Sum();
        return result;
    }

    protected override object InternalPart2()
    {
        var list1 = new List<int>();
        var list2 = new List<int>();
        foreach (var line in this.Input.Lines)
        {
            var lineValues = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            list1.Add(int.Parse(lineValues[0]));
            list2.Add(int.Parse(lineValues[1]));
        }

        var grouped1 = list1.GroupBy(v => v).Select(g => new { Value = g.Key, Count = g.Count() }).ToList();
        var grouped2 = list2.GroupBy(v => v).Select(g => new { Value = g.Key, Count = g.Count() }).ToDictionary(v => v.Value, v => v.Count);

        var sum = 0;

        foreach (var group1Value in grouped1)
        {
            if (grouped2.TryGetValue(group1Value.Value, out var group2Count))
            {
                sum += group1Value.Value * group1Value.Count * group2Count;
            }
        }

        return sum;
    }
}
