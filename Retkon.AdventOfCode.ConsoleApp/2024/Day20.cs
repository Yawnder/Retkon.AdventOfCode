﻿using AdventOfCodeSupport;
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

    private const int maxJump = 20;
    private const int minimumSave = 100;

    protected override object InternalPart1()
    {
        this.height = this.Input.Lines.Length;
        this.width = this.Input.Lines[0].Length;

        this.map = ArrayHelper.GetEmptyMap<bool>(this.width, this.height);
        this.mapCost = ArrayHelper.GetEmptyMap<int>(this.width, this.height);

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
            for (int y = 1; y < this.height - 1; y++)
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
        this.height = this.Input.Lines.Length;
        this.width = this.Input.Lines[0].Length;

        this.map = ArrayHelper.GetEmptyMap<bool>(this.width, this.height);
        this.mapCost = ArrayHelper.GetEmptyMap<int>(this.width, this.height);

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

        var costLists = new List<Vector3Int>(10000);

        for (int x = 1; x < this.width - 1; x++)
        {
            for (int y = 1; y < this.height - 1; y++)
            {
                if (!this.map[x][y])
                {
                    costLists.Add(new Vector3Int(x, y, this.mapCost[x][y]));
                }
            }
        }

        var result = 0L;
        for (int i = 0; i < costLists.Count; i++)
        {
            for (int j = i + 1; j < costLists.Count; j++)
            {
                var v1 = costLists[i];
                var v2 = costLists[j];

                var distanceJumped = Math.Abs(v1.X - v2.X) + Math.Abs(v1.Y - v2.Y);
                if (distanceJumped > maxJump)
                    continue;

                var distanceSaved = Math.Abs(v1.Z - v2.Z) - distanceJumped;
                if (distanceSaved >= minimumSave)
                    result++;
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

