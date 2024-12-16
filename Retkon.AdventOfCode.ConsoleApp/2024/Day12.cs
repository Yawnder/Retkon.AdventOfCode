using AdventOfCodeSupport;
using BenchmarkDotNet.Validators;
using FastSerialization;
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

public class Day12 : AdventBase
{
    private ConsoleHelper consoleHelper = ConsoleHelper.Instance;
    private int height;
    private int width;
    private char[][] map = null!;
    private int?[][] zone = null!;
    private Dictionary<int, int> zoneArea = null!;
    private Dictionary<int, int> zoneFence = null!;
    private Dictionary<int, int> zoneFenceCount = null!;
    private DirectionFlag[][] knownFences = null!;

    protected override object InternalPart1()
    {
        this.InternalCore();

        var result = 0;
        for (int i = 0; i < this.zoneArea.Count; i++)
        {
            result += this.zoneArea[i] * this.zoneFence[i];
        }

        return result;
    }

    protected override object InternalPart2()
    {
        this.InternalCore();

        var result = 0;
        for (int i = 0; i < this.zoneArea.Count; i++)
        {
            result += this.zoneArea[i] * this.zoneFenceCount[i];
        }

        return result;
    }

    private void InternalCore()
    {
        this.consoleHelper.IsEnabled = true;
        this.consoleHelper.DefaultStringFunction = v => "■";

        this.height = this.Input.Lines.Length;
        this.width = this.Input.Lines[0].Length;

        this.map = this.Input.To2DChar();
        this.zone = ArrayHelper.GetEmptyMap<int?>(this.width, this.height);
        this.zoneArea = [];
        this.zoneFence = [];
        this.zoneFenceCount = [];

        var zoneId = 0;

        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.width; y++)
            {
                if (this.zone[x][y] != null)
                    continue;

                this.consoleHelper.Clear();

                this.knownFences = ArrayHelper.GetEmptyMap<DirectionFlag>(this.width, this.height);

                var crop = this.map[x][y];

                this.zoneArea[zoneId] = 1;
                this.zoneFence[zoneId] = 0;
                this.zoneFenceCount[zoneId] = 0;
                this.zone[x][y] = zoneId;

                this.FloodAll(crop, zoneId, x, y, Direction.Up);
                zoneId++;
            }
        }
    }

    private void FloodAll(char crop, int zoneId, int x, int y, Direction startAtDirection)
    {
        this.consoleHelper.Clear();
        this.consoleHelper.Write(this.zone);
        for (int i = 0; i < 4; i++)
        {
            var currentDirection = startAtDirection.RotateRight(i);
            var offSet = Vector2Int.Plus[(int)currentDirection];
            this.Flood(crop, zoneId, x, y, x + offSet.X, y + offSet.Y, currentDirection);
        }
    }

    private void Flood(char crop, int zoneId, int fromX, int fromY, int x, int y, Direction currentDirection)
    {
        if (x < 0 || x == this.width || y < 0 || y == this.height || this.map[x][y] != crop)
        {
            this.zoneFence[zoneId]++;

            DirectionFlag fenceSide;

            if (fromX < x)
            {
                fenceSide = DirectionFlag.Right;
            }
            else if (fromX > x)
            {
                fenceSide = DirectionFlag.Left;
            }
            else if (fromY < y)
            {
                fenceSide = DirectionFlag.Down;
            }
            else
            {
                fenceSide = DirectionFlag.Up;
            }

            if (!this.CheckFenceAdjacentAll(fenceSide, fromX, fromY))
            {
                this.zoneFenceCount[zoneId]++;
            }

            this.knownFences[fromX][fromY] |= fenceSide;
        }
        else
        {
            if (this.zone[x][y] == null)
            {
                this.zone[x][y] = zoneId;
                this.zoneArea[zoneId]++;
                this.FloodAll(crop, zoneId, x, y, currentDirection.RotateLeft());
            }
        }
    }

    private bool CheckFenceAdjacentAll(DirectionFlag fenceSide, int x, int y)
    {
        return this.CheckFenceAdjacent(fenceSide, x, y - 1)
            || this.CheckFenceAdjacent(fenceSide, x + 1, y)
            || this.CheckFenceAdjacent(fenceSide, x, y + 1)
            || this.CheckFenceAdjacent(fenceSide, x - 1, y);
    }

    private bool CheckFenceAdjacent(DirectionFlag fenceSide, int x, int y)
    {
        if (x < 0 || x == this.width || y < 0 || y == this.height)
        {
            return false;
        }
        else
        {
            return (this.knownFences[x][y] & fenceSide) == fenceSide;
        }
    }
}

