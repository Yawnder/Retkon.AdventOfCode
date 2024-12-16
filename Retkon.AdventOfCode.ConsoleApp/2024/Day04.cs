using AdventOfCodeSupport;
using CommunityToolkit.HighPerformance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp._2024;

public class Day04 : AdventBase
{
    private Regex regex = new Regex(@"((?<=X)(?:MA)(?=S))|((?<=S)(?:AM)(?=X))", RegexOptions.Compiled);

    protected override object InternalPart1()
    {
        var result = 0;
        var count = 0;

        var all = new char[this.Input.Lines.Count()][];

        var i = 0;
        foreach (var line in this.Input.Lines)
        {
            all[i++] = line.ToCharArray();
        }

        var height = this.Input.Lines.Count();
        var width = this.Input.Lines[0].Count();

        //foreach (var line in this.Input.Lines)
        //{
        //    result += this.CountMatches(line);
        //    count++;
        //}

        var sb1 = new StringBuilder();
        var sb2 = new StringBuilder();
        //var sb3 = new StringBuilder();
        //var sb4 = new StringBuilder();

        for (int x = 0; x < width; x++)
        {
            sb1.Clear();
            sb2.Clear();
            for (int y = 0; y < height; y++)
            {
                sb1.Append(all[y][x]);  // Vertical
                sb2.Append(all[x][y]);  // Horizontal
            }

            result += this.CountMatches(sb1.ToString()) + this.CountMatches(sb2.ToString());
            count++;
            count++;
        }

        for (int x = 0; x < width; x++)
        {
            sb1.Clear();
            sb2.Clear();

            for (int y = x; y >= 0; y--)
            {
                sb1.Append(all[y][x - y]);                  // Diagonal /
                sb2.Append(all[y][width - 1 - (x - y)]);    // Diagonal \
            }

            result += this.CountMatches(sb1.ToString()) + this.CountMatches(sb2.ToString());
            count++;
            count++;

        }

        for (int x = 1; x < width; x++)
        {
            sb1.Clear();
            sb2.Clear();
            for (int y = 0; y < height - x; y++)
            {
                sb1.Append(all[height - y - 1][x + y]);                 // Diagonal / (en descendant)
                sb2.Append(all[height - y - 1][width - (x + y) - 1]);   // Diagonal \ (en descendant)
            }

            result += this.CountMatches(sb1.ToString()) + this.CountMatches(sb2.ToString());
            count++;
            count++;
        }

        return result;
    }

    private int CountMatches(string value)
    {
        var result = 0;
        var matches = this.regex.Matches(value).AsEnumerable();
        foreach (var match in matches)
        {
            result += (match.Groups[1].Success ? 1 : 0) + (match.Groups[2].Success ? 1 : 0);
        }

        return result;
    }

    protected override object InternalPart2()
    {
        var result = 0;
        var all = new char[this.Input.Lines.Length][];

        var i = 0;
        foreach (var line in this.Input.Lines)
        {
            all[i++] = line.ToCharArray();
        }

        var height = all.Length;
        var width = all[0].Length;

        var clockwiseCorners = new List<char>(5);

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (all[y][x] != 'A')
                    continue;

                clockwiseCorners.Clear();
                clockwiseCorners.Add(all[y - 1][x - 1]);
                clockwiseCorners.Add(all[y + 1][x - 1]);
                clockwiseCorners.Add(all[y + 1][x + 1]);
                clockwiseCorners.Add(all[y - 1][x + 1]);

                for (i = 0; i < 4; i++)
                {
                    clockwiseCorners.Add(clockwiseCorners[0]);
                    clockwiseCorners.RemoveAt(0);

                    if (clockwiseCorners[0] == 'M' && clockwiseCorners[1] == 'M' && clockwiseCorners[2] == 'S' && clockwiseCorners[3] == 'S')
                    {
                        result++;
                        break;
                    }
                }
            }
        }

        return result;
    }
}

