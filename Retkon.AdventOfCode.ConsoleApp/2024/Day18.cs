using AdventOfCodeSupport;
using Retkon.AdventOfCode.ConsoleApp.Helpers;
using Retkon.AdventOfCode.ConsoleApp.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp._2024;

public class Day18 : AdventBase
{

    private Vector2Int start = new Vector2Int(0, 0);
    //private Vector2Int end = new Vector2Int(6, 6);
    //private int height = 7;
    //private int width = 7;

    private Vector2Int end = new Vector2Int(70, 70);
    private int height = 71;
    private int width = 71;
    private int[][] map = null!;
    private int[][] mapCost = null!;
    private Regex regex = new Regex(@"(?<x>\d*),(?<y>\d*)", RegexOptions.Compiled);

    protected override object InternalPart1()
    {
        var result = 0;

        this.map = ArrayHelper.GetEmptyMap<int>(this.height, this.width);
        this.mapCost = ArrayHelper.GetEmptyMap<int>(this.height, this.width);

        this.map.Fill(int.MaxValue);
        this.mapCost.Fill(int.MaxValue);

        for (int y = 0; y < 1024; y++)
        {
            var match = this.regex.Match(this.Input.Lines[y]);
            this.map[int.Parse(match.Groups["x"].Value)][int.Parse(match.Groups["y"].Value)] = y;
        }

        this.Advance(this.start, 1);
        result = this.mapCost[this.end.X][this.end.Y] - 1;

        return result;
    }

    protected override object InternalPart2()
    {
        string result = "";

        this.map = ArrayHelper.GetEmptyMap<int>(this.height, this.width);
        this.mapCost = ArrayHelper.GetEmptyMap<int>(this.height, this.width);

        for (int i = this.Input.Lines.Length; i > 1024; i = i - 1)
        {
            this.map.Fill(int.MaxValue);
            this.mapCost.Fill(int.MaxValue);

            for (int y = 0; y < i; y++)
            {
                var match = this.regex.Match(this.Input.Lines[y]);
                this.map[int.Parse(match.Groups["x"].Value)][int.Parse(match.Groups["y"].Value)] = y;
            }

            this.Advance(this.start, 1);

            if (this.mapCost[this.end.X][this.end.Y] < int.MaxValue)
            {
                result = this.Input.Lines[i];
                break;
            }
        }

        return result;
    }

    private void Advance(Vector2Int initialStep, int initialRunningCost)
    {
        var stackStep = new Stack<Vector2Int>(100000);
        var stackRunningCost = new Stack<int>(100000);

        stackStep.Push(initialStep);
        stackRunningCost.Push(initialRunningCost);

        while (true)
        {
            if (!stackStep.TryPop(out var step) || !stackRunningCost.TryPop(out var runningCost))
                break;

            //if ((this.map[step.X][step.Y] == int.MaxValue || this.map[step.X][step.Y] > runningCost) && this.mapCost[step.X][step.Y] > runningCost)
            if (this.map[step.X][step.Y] == int.MaxValue && this.mapCost[step.X][step.Y] > runningCost)
            {
                this.mapCost[step.X][step.Y] = runningCost;

                foreach (var heading in (Direction[])Enum.GetValues(typeof(Direction)))
                {
                    var offset = heading.GetOffset();
                    var nextStep = step + offset;

                    if (nextStep.X < 0 || nextStep.X == this.width || nextStep.Y < 0 || nextStep.Y == this.height)
                        continue;

                    var nextRunningCost = runningCost + 1;

                    //if (this.map[nextStep.X][nextStep.Y] != int.MaxValue && this.map[nextStep.X][nextStep.Y] <= nextRunningCost)
                    if (this.map[nextStep.X][nextStep.Y] < int.MaxValue)
                        continue;

                    stackStep.Push(nextStep);
                    stackRunningCost.Push(nextRunningCost);
                }
            }
        }
    }


}

