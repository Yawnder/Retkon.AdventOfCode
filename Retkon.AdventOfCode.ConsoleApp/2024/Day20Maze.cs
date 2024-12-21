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

public class Day20Maze : AdventBase
{

    private static ConsoleHelper consoleHelper = ConsoleHelper.Instance;

    private int height;
    private int width;
    private bool[][] map = null!;

    private Vector2Int end;
    private Vector2Int start;

    private const int maxJump = 20;
    private const int minimumSave = 100;

    //private const int maxJump = 6;
    //private const int minimumSave = 76;

    protected override object InternalPart1()
    {
        this.height = this.Input.Lines.Length;
        this.width = this.Input.Lines[0].Length;

        this.map = ArrayHelper.GetEmptyMap<bool>(this.width, this.height);
        var mapCostFromStart = ArrayHelper.GetEmptyMap<int>(this.width, this.height);
        var mapCostFromEnd = ArrayHelper.GetEmptyMap<int>(this.width, this.height);

        for (int y = 0; y < this.Input.Lines.Length; y++)
        {
            var line = this.Input.Lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                var symbol = line[x];
                mapCostFromStart[x][y] = int.MaxValue;
                mapCostFromEnd[x][y] = int.MaxValue;

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

        this.Advance(ref mapCostFromStart, this.start);
        this.Advance(ref mapCostFromEnd, this.end);

        var referenceCost = mapCostFromEnd[this.start.X][this.start.Y];

        var result = 0L;
        for (int x = 1; x < this.width - 1; x++)
        {
            for (int y = 1; y < this.height - 1; y++)
            {
                if (this.map[x][y])
                {
                    if (!this.map[x - 1][y] && !this.map[x + 1][y])
                    {
                        var saved1 = referenceCost - (mapCostFromEnd[x - 1][y] + mapCostFromStart[x + 1][y] + 2);
                        if (saved1 >= minimumSave)
                            result++;

                        var saved2 = referenceCost - (mapCostFromEnd[x + 1][y] + mapCostFromStart[x - 1][y] + 2);
                        if (saved2 >= minimumSave)
                            result++;
                    }
                    else if (!this.map[x][y - 1] && !this.map[x][y + 1])
                    {
                        var saved1 = referenceCost - (mapCostFromEnd[x][y - 1] + mapCostFromStart[x][y + 1] + 2);
                        if (saved1 >= minimumSave)
                            result++;

                        var saved2 = referenceCost - (mapCostFromEnd[x][y + 1] + mapCostFromStart[x][y - 1] + 2);
                        if (saved2 >= minimumSave)
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

        this.map = ArrayHelper.GetEmptyMap<bool>(this.width, this.height);
        var mapCostFromStart = ArrayHelper.GetEmptyMap<int>(this.width, this.height);
        var mapCostFromEnd = ArrayHelper.GetEmptyMap<int>(this.width, this.height);

        for (int y = 0; y < this.Input.Lines.Length; y++)
        {
            var line = this.Input.Lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                var symbol = line[x];
                mapCostFromStart[x][y] = int.MaxValue;
                mapCostFromEnd[x][y] = int.MaxValue;

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

        this.Advance(ref mapCostFromStart, this.start);
        this.Advance(ref mapCostFromEnd, this.end);

        Console.WriteLine($"Graph drawn: {sw.Elapsed}");

        var costListsFromStart = new List<Vector3Int>(10000);
        var costListsFromEnd = new List<Vector3Int>(10000);

        for (int x = 1; x < this.width - 1; x++)
        {
            for (int y = 1; y < this.height - 1; y++)
            {
                if (!this.map[x][y])
                {
                    costListsFromStart.Add(new Vector3Int(x, y, mapCostFromStart[x][y]));
                    costListsFromEnd.Add(new Vector3Int(x, y, mapCostFromEnd[x][y]));
                }
            }
        }

        Console.WriteLine($"Crushified: {sw.Elapsed}");
        var referenceCost = mapCostFromEnd[this.start.X][this.start.Y];

        var result = 0L;
        for (int i = 0; i < costListsFromStart.Count; i++)
        {
            for (int j = 0; j < costListsFromEnd.Count; j++)
            {
                var v1 = costListsFromStart[i];
                var v2 = costListsFromEnd[j];

                var distanceJumped = Math.Abs(v1.X - v2.X) + Math.Abs(v1.Y - v2.Y);
                if (distanceJumped > maxJump)
                    continue;

                var distanceSaved = referenceCost - (v1.Z + v2.Z + distanceJumped);
                if (distanceSaved >= minimumSave)
                    result++;
            }
        }

        consoleHelper.DefaultColorFunction = new Func<object, ConsoleColor>(v => (char)v == '#' ? ConsoleColor.White : ConsoleColor.Red);
        var consoleMap = ArrayHelper.GetEmptyMap<char>(this.width, this.height);

        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                if (this.map[x][y])
                    consoleMap[x][y] = '#';

                else if (mapCostFromStart[x][y] < int.MaxValue && mapCostFromEnd[x][y] < int.MaxValue && mapCostFromStart[x][y] + mapCostFromEnd[x][y] == referenceCost)
                    consoleMap[x][y] = 'X';
                else
                    consoleMap[x][y] = ' ';
            }
        }

        consoleHelper.Write(consoleMap);

        sw.Stop();
        Console.WriteLine($"End: {sw.Elapsed}");
        return result;
    }

    private void Advance(ref int[][] costMap, Vector2Int initialPosition)
    {
        var stackStep = new Stack<Vector2Int>(100000);
        var stackRunningCost = new Stack<int>(100000);

        stackStep.Push(initialPosition);
        stackRunningCost.Push(0);

        while (true)
        {
            if (!stackStep.TryPop(out var step) || !stackRunningCost.TryPop(out var runningCost))
                break;

            if (!this.map[step.X][step.Y] && costMap[step.X][step.Y] > runningCost)
            {
                costMap[step.X][step.Y] = runningCost;

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

