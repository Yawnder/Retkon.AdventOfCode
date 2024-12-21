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

public class Day20 : AdventBase
{

    private int height;
    private int width;
    private bool[][] map = null!;
    private int[][] mapCost = null!;

    private Vector2Int end;
    private Vector2Int start;

    protected override object InternalPart1()
    {
        this.height = this.Input.Lines.Length;
        this.width = this.Input.Lines[0].Length;

        this.map = ArrayHelper.GetEmptyMap<bool>(this.height, this.width);
        this.mapCost = ArrayHelper.GetEmptyMap<int>(this.height, this.width);

        for (int y = 0; y < this.Input.Lines.Length; y++)
        {
            var line = this.Input.Lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                var symbol = line[x];
                this.mapCost[x][y] = int.MaxValue;

                switch (symbol)
                {
                    case '.':
                        break;
                    case '#':
                        this.map[x][y] = true;
                        break;
                    case 'E':
                        this.end = new Vector2Int(x, y);
                        break;
                    case 'S':
                        this.start = new Vector2Int(x, y);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        this.Advance(this.end, 0);
        int minimumSave = 100;
        var result = 0L;
        for (int x = 1; x < this.width - 1; x++)
        {
            for (int y = 1; y < this.width - 1; y++)
            {
                if (this.map[x][y])
                {
                    if (!this.map[x - 1][y] && !this.map[x + 1][y])
                    {
                        var saved = Math.Abs(this.mapCost[x - 1][y] - this.mapCost[x + 1][y]) - 2;
                        if (saved >= minimumSave)
                            result++;
                    }
                    else if (!this.map[x][y - 1] && !this.map[x][y + 1])
                    {
                        var saved = Math.Abs(this.mapCost[x][y - 1] - this.mapCost[x][y + 1]) - 2;
                        if (saved >= minimumSave)
                            result++;
                    }

                }
            }
        }

        return result;
    }

    protected override object InternalPart2()
    {
        var sw = Stopwatch.StartNew();

        this.height = this.Input.Lines.Length;
        this.width = this.Input.Lines[0].Length;

        this.map = ArrayHelper.GetEmptyMap<bool>(this.height, this.width);
        this.mapCost = ArrayHelper.GetEmptyMap<int>(this.height, this.width);

        for (int y = 0; y < this.Input.Lines.Length; y++)
        {
            var line = this.Input.Lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                var symbol = line[x];
                this.mapCost[x][y] = int.MaxValue;

                switch (symbol)
                {
                    case '.':
                        break;
                    case '#':
                        this.map[x][y] = true;
                        break;
                    case 'E':
                        this.end = new Vector2Int(x, y);
                        break;
                    case 'S':
                        this.start = new Vector2Int(x, y);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        Console.WriteLine($"Data Loaded: {sw.Elapsed}");

        this.Advance(this.end, 0);

        Console.WriteLine($"Graph drawn: {sw.Elapsed}");

        int minimumSave = 100;


        var costLists = new List<Vector3Int>(10000);

        for (int x = 1; x < this.width - 1; x++)
        {
            for (int y = 1; y < this.width - 1; y++)
            {
                if (!this.map[x][y])
                {
                    costLists.Add(new Vector3Int(x, y, this.mapCost[x][y]));
                }
            }
        }

        Console.WriteLine($"Crushified: {sw.Elapsed}");
        var maxCost = this.mapCost[this.end.X][this.end.Y];

        var wrongResult = 0L;
        var result = 0L;
        for (int i = 0; i < costLists.Count; i++)
        {
            for (int j = i + 1; j < costLists.Count; j++)
            {
                var v1 = costLists[i];
                var v2 = costLists[j];

                var distanceJumped = Math.Abs(v1.X - v2.X) + Math.Abs(v1.Y - v2.Y);
                if (distanceJumped > 20)
                    continue;

                var distanceSaved = Math.Abs(v1.Z - v2.Z) - distanceJumped;
                if (distanceSaved >= minimumSave)
                    result++;

                if (v1.Z > maxCost || v2.Z > maxCost)
                    wrongResult++;

                //var deltaX = v1.X - v2.X;
                //var deltaY = v1.Y - v2.Y;
                //var deltaZ = v1.Z - v2.Z;

                ////deltaX = deltaX > 0 ? deltaX : checked(-deltaX);
                ////deltaY = deltaY > 0 ? deltaY : checked(-deltaY);
                //deltaX = (deltaX + (deltaX >> 31)) ^ (deltaX >> 31);
                //deltaY = (deltaY + (deltaY >> 31)) ^ (deltaY >> 31);

                //var distanceJumped = deltaX + deltaY;
                //if (distanceJumped > 20)
                //    continue;

                ////deltaZ = deltaZ > 0 ? deltaZ : checked(-deltaZ);
                //deltaZ = (deltaZ + (deltaZ >> 31)) ^ (deltaZ >> 31);

                //if (deltaZ - distanceJumped >= minimumSave)
                //    result++;
            }
        }

        sw.Stop();
        Console.WriteLine($"End: {sw.Elapsed}");
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

            if (!this.map[step.X][step.Y] && this.mapCost[step.X][step.Y] > runningCost)
            {
                this.mapCost[step.X][step.Y] = runningCost;

                foreach (var heading in (Direction[])Enum.GetValues(typeof(Direction)))
                {
                    var offset = heading.GetOffset();
                    var nextStep = step + offset;

                    if (nextStep.X < 0 || nextStep.X == this.width || nextStep.Y < 0 || nextStep.Y == this.height)
                        continue;

                    var nextRunningCost = runningCost + 1;

                    if (this.map[nextStep.X][nextStep.Y])
                        continue;

                    stackStep.Push(nextStep);
                    stackRunningCost.Push(nextRunningCost);
                }
            }
        }
    }

}

