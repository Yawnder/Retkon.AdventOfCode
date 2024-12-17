using AdventOfCodeSupport;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

public class Day16 : AdventBase
{

    private int height;
    private int width;
    private bool[][] map = null!;
    private Dictionary<Direction, int>[][] mapCost = null!;
    private Vector2Int start;
    private Vector2Int end;
    private bool[][] mapBestPath = null!;

    protected override object InternalPart1()
    {
        var result = 0;

        this.height = this.Input.Lines.Length;
        this.width = this.Input.Lines[0].Length;

        this.map = ArrayHelper.GetEmptyMap<bool>(this.height, this.width);
        this.mapCost = ArrayHelper.GetEmptyMap<Dictionary<Direction, int>>(this.height, this.width);

        for (int y = 0; y < this.Input.Lines.Length; y++)
        {
            var line = this.Input.Lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                var symbol = line[x];
                this.mapCost[x][y] = new Dictionary<Direction, int> { { Direction.Up, int.MaxValue }, { Direction.Right, int.MaxValue }, { Direction.Down, int.MaxValue }, { Direction.Left, int.MaxValue } };

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

        this.Advance(this.start, Direction.Right, 0);
        result = this.mapCost[this.end.X][this.end.Y].Min(kvp => kvp.Value);

        return result;
    }

    protected override object InternalPart2()
    {
        var sw = Stopwatch.StartNew();
        this.InternalPart1();

        this.mapBestPath = ArrayHelper.GetEmptyMap<bool>(this.height, this.width);

        var stackStep = new Stack<Vector2Int>(100000);
        var stackDirection = new Stack<Direction>(100000);
        var stackRunningCost = new Stack<int>(100000);

        var endTileResults = this.mapCost[this.end.X][this.end.Y];
        var endTileCost = this.mapCost[this.end.X][this.end.Y].Min(kvp => kvp.Value);
        var endTileDirections = this.mapCost[this.end.X][this.end.Y].Where(kvp => kvp.Value == endTileCost);

        this.mapBestPath[this.end.X][this.end.Y] = true;

        foreach (var endTileDirection in endTileDirections)
        {
            stackStep.Push(this.end);
            stackDirection.Push(endTileDirection.Key);
            stackRunningCost.Push(endTileCost);
        }

        while (true)
        {
            if (!stackStep.TryPop(out var step) || !stackDirection.TryPop(out var direction) || !stackRunningCost.TryPop(out var runningCost))
                break;

            foreach (var heading in (Direction[])Enum.GetValues(typeof(Direction)))
            {
                int rotationCost;
                if (heading == direction)
                    rotationCost = 0;
                else if ((heading - direction) % 2 == 0)
                    continue;
                else
                    rotationCost = 1000;

                var offset = -1 * heading.GetOffset();
                var previousStep = step + offset;
                if (this.map[previousStep.X][previousStep.Y])
                    continue;

                var previousRunningCost = runningCost - rotationCost - 1;

                if (this.mapCost[previousStep.X][previousStep.Y][heading] == previousRunningCost)
                {
                    this.mapBestPath[previousStep.X][previousStep.Y] = true;
                    stackStep.Push(previousStep);
                    stackDirection.Push(heading);
                    stackRunningCost.Push(previousRunningCost);
                }
            }
        }

        sw.Stop();

        //ConsoleHelper.Instance.DefaultColorFunction = (v) => ConsoleColor.White;
        //ConsoleHelper.Instance.DefaultStringFunction = (v) => (bool)v ? "X" : ".";
        //ConsoleHelper.Instance.Write(this.mapBestPath);

        var result = this.mapBestPath.Select(va => va.Where(v => v).Count()).Sum();
        return result;
    }


    private void Advance(Vector2Int initialStep, Direction initialDirection, int initialRunningCost)
    {
        var stackStep = new Stack<Vector2Int>(100000);
        var stackDirection = new Stack<Direction>(100000);
        var stackRunningCost = new Stack<int>(100000);

        stackStep.Push(initialStep);
        stackDirection.Push(initialDirection);
        stackRunningCost.Push(initialRunningCost);

        while (true)
        {
            if (!stackStep.TryPop(out var step) || !stackDirection.TryPop(out var direction) || !stackRunningCost.TryPop(out var runningCost))
                break;

            foreach (var heading in (Direction[])Enum.GetValues(typeof(Direction)))
            {
                int rotationCost;
                if (heading == direction)
                    rotationCost = 0;
                else if ((heading - direction) % 2 == 0)
                    continue;
                else
                    rotationCost = 1000;

                if (this.mapCost[step.X][step.Y][heading] > runningCost + rotationCost)
                {
                    this.mapCost[step.X][step.Y][heading] = runningCost + rotationCost;

                    var offset = heading.GetOffset();
                    var nextStep = step + offset;

                    if (this.map[nextStep.X][nextStep.Y])
                        continue;

                    var nextRunningCost = runningCost + rotationCost + 1;

                    if (this.mapCost[nextStep.X][nextStep.Y][heading] > nextRunningCost)
                    {
                        stackStep.Push(nextStep);
                        stackDirection.Push(heading);
                        stackRunningCost.Push(nextRunningCost);
                    }
                }
            }
        }
    }

}

