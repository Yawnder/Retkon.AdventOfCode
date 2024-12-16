using AdventOfCodeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp._2024;

public class Day05 : AdventBase
{
    protected override object InternalPart1()
    {
        var result = 0;

        var orders = new Dictionary<int, List<int>>();

        var partTwo = false;
        foreach (var line in this.Input.Lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                partTwo = true;
                continue;
            }

            if (!partTwo)
            {
                var pageInfo = line.Split('|');
                var page = int.Parse(pageInfo[0]);
                var beforePage = int.Parse(pageInfo[1]);

                if (!orders.TryGetValue(page, out var beforePages))
                {
                    beforePages = new List<int>();
                    orders.Add(page, beforePages);
                }
                beforePages.Add(beforePage);
            }
            else
            {
                var isRightOrder = true;
                var pageOrder = line.Split(",").Select(v => int.Parse(v)).ToList();
                int middlePageIndex = (pageOrder.Count - 1) / 2;
                for (int i = pageOrder.Count - 1; i > 0; i--)
                {
                    var currentPageNumber = pageOrder[i];

                    if (!orders.TryGetValue(currentPageNumber, out var currentPageBeforePages))
                    {
                        continue;
                    }

                    var previousPages = pageOrder[..i];

                    if (previousPages.Union(currentPageBeforePages).Count() < previousPages.Count + currentPageBeforePages.Count)
                    {
                        isRightOrder = false;
                        break;
                    }
                }

                if (isRightOrder)
                {
                    result += pageOrder[middlePageIndex];
                }
            }
        }

        return result;
    }

    protected override object InternalPart2()
    {
        var result = 0;

        var orders = new Dictionary<int, List<int>>();

        var partTwo = false;
        foreach (var line in this.Input.Lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                partTwo = true;
                continue;
            }

            if (!partTwo)
            {
                var pageInfo = line.Split('|');
                var page = int.Parse(pageInfo[0]);
                var beforePage = int.Parse(pageInfo[1]);

                if (!orders.TryGetValue(page, out var beforePages))
                {
                    beforePages = new List<int>();
                    orders.Add(page, beforePages);
                }
                beforePages.Add(beforePage);
            }
            else
            {
                var isRightOrder = true;
                var pageOrder = line.Split(",").Select(v => int.Parse(v)).ToList();
                int middlePageIndex = (pageOrder.Count - 1) / 2;
                for (int i = pageOrder.Count - 1; i > 0; i--)
                {
                    var currentPageNumber = pageOrder[i];

                    if (!orders.TryGetValue(currentPageNumber, out var currentPageBeforePages))
                    {
                        continue;
                    }

                    var previousPages = pageOrder[..i];

                    if (previousPages.Union(currentPageBeforePages).Count() < previousPages.Count + currentPageBeforePages.Count)
                    {
                        isRightOrder = false;
                        break;
                    }
                }

                if (!isRightOrder)
                {
                    while (!isRightOrder)
                    {
                        for (int i = pageOrder.Count - 1; i > 0; i--)
                        {
                            var currentPageNumber = pageOrder[i];

                            if (!orders.TryGetValue(currentPageNumber, out var currentPageBeforePages))
                            {
                                continue;
                            }

                            var previousPages = pageOrder[..i];

                            var misorderedPage = previousPages.Intersect(currentPageBeforePages);
                            if (misorderedPage.Any())
                            {
                                var firstMisorderedPage = misorderedPage.First();
                                var firstMisorderedPageIndex = pageOrder.IndexOf(firstMisorderedPage);
                                pageOrder.Remove(currentPageNumber);
                                pageOrder.Remove(firstMisorderedPage);
                                pageOrder.Insert(firstMisorderedPageIndex, currentPageNumber);
                                pageOrder.Insert(i, firstMisorderedPage);
                                i = pageOrder.Count;
                            }
                        }

                        isRightOrder = true;
                    }

                    Console.WriteLine(string.Join(",", pageOrder));
                    result += pageOrder[middlePageIndex];
                }
            }
        }

        return result;
    }
}
