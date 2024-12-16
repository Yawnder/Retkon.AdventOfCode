using AdventOfCodeSupport;
using FastSerialization;
using Retkon.AdventOfCode.ConsoleApp.Helpers;
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

public class Day10 : AdventBase
{
    private int[][] map = null!;
    private int height;
    private int width;

    protected override object InternalPart1()
    {
        var result = 0;

        this.height = this.Input.Lines.Length;
        this.width = this.Input.Lines[0].Length;

        this.map = this.Input.To2DInt();

        for (int y = 0; y < this.width; y++)
        {
            var line = this.Input.Lines[y];
            for (int x = 0; x < this.width; x++)
            {
                if (this.map[x][y] == 0)
                {
                    var achieved = ArrayHelper.GetEmptyMap<bool>(this.width, this.height);
                    this.NextPointCross(x, y, 1, ref achieved);
                    result += achieved.Select(c => c.Where(x => x).Count()).Sum();
                }
            }
        }

        return result;
    }

    protected override object InternalPart2()
    {
        var result = 0;

        this.height = this.Input.Lines.Length;
        this.width = this.Input.Lines[0].Length;

        this.map = this.Input.To2DInt();

        for (int y = 0; y < this.width; y++)
        {
            var line = this.Input.Lines[y];
            for (int x = 0; x < this.width; x++)
            {
                if (this.map[x][y] == 0)
                {
                    var achieved = ArrayHelper.GetEmptyMap<int>(this.width, this.height);
                    this.NextPointCross(x, y, 1, ref achieved);
                    result += achieved.Select(c => c.Sum()).Sum();
                }
            }
        }

        return result;
    }

    private void NextPointCross(int x, int y, int next, ref bool[][] achieved)
    {
        this.NextPoint(x, y - 1, next, ref achieved);
        this.NextPoint(x + 1, y, next, ref achieved);
        this.NextPoint(x, y + 1, next, ref achieved);
        this.NextPoint(x - 1, y, next, ref achieved);
    }

    private void NextPointCross(int x, int y, int next, ref int[][] achieved)
    {
        this.NextPoint(x, y - 1, next, ref achieved);
        this.NextPoint(x + 1, y, next, ref achieved);
        this.NextPoint(x, y + 1, next, ref achieved);
        this.NextPoint(x - 1, y, next, ref achieved);
    }

    private void NextPoint(int x, int y, int next, ref bool[][] achieved)
    {
        if (x < 0 || x == this.width || y < 0 || y == this.height || this.map[x][y] != next)
            return;

        if (next == 9)
        {
            achieved[x][y] = true;
        }
        else
        {
            this.NextPointCross(x, y, next + 1, ref achieved);
        }
    }

    private void NextPoint(int x, int y, int next, ref int[][] achieved)
    {
        if (x < 0 || x == this.width || y < 0 || y == this.height || this.map[x][y] != next)
            return;

        if (next == 9)
        {
            achieved[x][y]++;
        }
        else
        {
            this.NextPointCross(x, y, next + 1, ref achieved);
        }
    }
}

