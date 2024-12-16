using AdventOfCodeSupport;
using Retkon.AdventOfCode.ConsoleApp.Helpers;
using Retkon.AdventOfCode.ConsoleApp.Structures;
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

public partial class Day13 : AdventBase
{
    private static Regex buttonARegex = GetButtonARegex();
    private static Regex buttonBRegex = GetButtonBRegex();
    private static Regex prizeRegex = GetPrizeRegex();

    protected override object InternalPart1()
    {
        var result = 0;

        var i = 0;
        do
        {
            var buttonAMatch = buttonARegex.Match(this.Input.Lines[4 * i + 0]);
            var buttonBMatch = buttonBRegex.Match(this.Input.Lines[4 * i + 1]);
            var prizeMatch = prizeRegex.Match(this.Input.Lines[4 * i + 2]);

            var buttonAX = int.Parse(buttonAMatch.Groups[1].Value);
            var buttonAY = int.Parse(buttonAMatch.Groups[2].Value);

            var buttonBX = int.Parse(buttonBMatch.Groups[1].Value);
            var buttonBY = int.Parse(buttonBMatch.Groups[2].Value);

            var prizeX = int.Parse(prizeMatch.Groups[1].Value);
            var prizeY = int.Parse(prizeMatch.Groups[2].Value);

            var buttonA = new Vector2Int(buttonAX, buttonAY);
            var buttonB = new Vector2Int(buttonBX, buttonBY);
            var prize = new Vector2Int(prizeX, prizeY);

            if (Solve(buttonA, buttonB, prize, out var buttonAPresses, out var buttonBPresses))
            {
                result += buttonAPresses * 3 + buttonBPresses * 1;
            }
        } while (this.Input.Lines.Length > ++i * 4);

        return result;
    }

    protected override object InternalPart2()
    {
        var result = 0L;

        var i = 0;
        do
        {
            var buttonAMatch = buttonARegex.Match(this.Input.Lines[4 * i + 0]);
            var buttonBMatch = buttonBRegex.Match(this.Input.Lines[4 * i + 1]);
            var prizeMatch = prizeRegex.Match(this.Input.Lines[4 * i + 2]);

            var buttonAX = long.Parse(buttonAMatch.Groups[1].Value);
            var buttonAY = long.Parse(buttonAMatch.Groups[2].Value);

            var buttonBX = long.Parse(buttonBMatch.Groups[1].Value);
            var buttonBY = long.Parse(buttonBMatch.Groups[2].Value);

            var prizeX = long.Parse(prizeMatch.Groups[1].Value);
            var prizeY = long.Parse(prizeMatch.Groups[2].Value);

            var buttonA = new Vector2Long(buttonAX, buttonAY);
            var buttonB = new Vector2Long(buttonBX, buttonBY);
            var prize = new Vector2Long(prizeX + 10000000000000, prizeY + 10000000000000);
            //var prize = new Vector2Long(prizeX, prizeY);

            if (Solve(buttonA, buttonB, prize, out var buttonAPresses, out var buttonBPresses))
            {
                result += buttonAPresses * 3 + buttonBPresses * 1;
            }
        } while (this.Input.Lines.Length > ++i * 4);

        return result;
    }

    private static bool Solve(Vector2Int v1, Vector2Int v2, Vector2Int target, out int v1Presses, out int v2Presses)
    {
        var solved = false;
        v1Presses = (int)Math.Ceiling((decimal)target.X / (decimal)v1.X);
        v2Presses = 0;

        do
        {
            var deltaX = (decimal)target.X - (decimal)v1Presses * (decimal)v1.X - (decimal)v2Presses * (decimal)v2.X;
            var deltaY = (decimal)target.Y - (decimal)v1Presses * (decimal)v1.Y - (decimal)v2Presses * (decimal)v2.Y;

            if (deltaX == 0 && deltaY == 0)
            {
                solved = true;
                break;
            }
            else if (deltaX < 0 || deltaY < 0)
            {
                v1Presses--;
            }
            else if (deltaX > 0 || deltaY > 0)
            {
                v2Presses++;
            }

        } while (v1Presses > 0);

        return solved;
    }

    //private bool Solve(Vector2Long v1, Vector2Long v2, Vector2Long target, out long v1Presses, out long v2Presses)
    //{
    //    var solved = false;
    //    v1Presses = (long)Math.Min(Math.Ceiling((decimal)target.X / (decimal)v1.X), Math.Ceiling((decimal)target.Y / (decimal)v1.Y));
    //    v2Presses = 0;

    //    do
    //    {
    //        var deltaX = (decimal)target.X - (decimal)v1Presses * (decimal)v1.X - (decimal)v2Presses * (decimal)v2.X;
    //        var deltaY = (decimal)target.Y - (decimal)v1Presses * (decimal)v1.Y - (decimal)v2Presses * (decimal)v2.Y;

    //        if (deltaX == 0M && deltaY == 0M)
    //        {
    //            solved = true;
    //            break;
    //        }
    //        else if (deltaX < 0 || deltaY < 0)
    //        {
    //            v1Presses--;
    //        }
    //        else if (deltaX > 0 || deltaY > 0)
    //        {
    //            v2Presses++;
    //        }

    //    } while (v1Presses > 0);

    //    return solved;
    //}

    private static bool Solve(Vector2Long v1, Vector2Long v2, Vector2Long target, out long v1Presses, out long v2Presses)
    {
        var equations = new decimal[2][];
        equations[0] = [v1.X, v2.X, target.X];
        equations[1] = [v1.Y, v2.Y, target.Y];

        var c1 = (decimal)v1.X;
        equations[0][0] = 1;
        for (int i = 1; i < 3; i++)
        {
            equations[0][i] /= c1;
        }

        var c2 = equations[1][0];
        equations[1][0] = 0;
        for (int i = 1; i < 3; i++)
        {
            equations[1][i] -= equations[0][i] * c2;
        }

        var c3 = equations[1][1];
        equations[1][1] = 1;
        equations[1][2] /= c3;

        var c4 = equations[0][1];
        equations[0][1] = 0;
        equations[0][2] -= equations[1][2] * c4;

        v1Presses = (long)Math.Round(equations[0][2], 8);
        v2Presses = (long)Math.Round(equations[1][2], 8);

        return v1Presses * v1.X + v2Presses * v2.X == target.X && v1Presses * v1.Y + v2Presses * v2.Y == target.Y;
    }

    [GeneratedRegex(@"(?:Button A: )X\+(?<x>\d*), Y\+(?<y>\d*)", RegexOptions.Compiled)]
    private static partial Regex GetButtonARegex();
    [GeneratedRegex(@"(?:Button B: )X\+(?<x>\d*), Y\+(?<y>\d*)", RegexOptions.Compiled)]
    private static partial Regex GetButtonBRegex();
    [GeneratedRegex(@"(?:Prize: )X=(?<x>\d*), Y=(?<y>\d*)", RegexOptions.Compiled)]
    private static partial Regex GetPrizeRegex();
}

