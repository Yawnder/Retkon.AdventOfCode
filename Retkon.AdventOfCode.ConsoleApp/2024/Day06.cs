using AdventOfCodeSupport;
using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp._2024;

public class Day06 : AdventBase
{
    protected override object InternalPart1()
    {
        var result = 0;

        var width = this.Input.Lines[0].Length;
        var height = this.Input.Lines.Length;

        var map = new bool[this.Input.Lines[0].Length][];
        var mapPath = new bool[this.Input.Lines[0].Length][];

        int currentPosX = 0;
        int currentPosY = 0;
        var direction = 0;

        for (int x = 0; x < width; x++)
        {
            var mapColumn = new bool[height];
            map[x] = mapColumn;
            mapPath[x] = new bool[height];
            for (int y = 0; y < height; y++)
            {
                var val = this.Input.Lines[y][x];
                if (val == '#')
                    mapColumn[y] = true;
                else if (val == '^')
                {
                    currentPosX = x;
                    currentPosY = y;
                }
            }
        }

        var movementX = 0;
        var movementY = 0;
        int nextPosX = 0;
        int nextPosY = 0;

        while (true)
        {
            mapPath[currentPosX][currentPosY] = true;
            switch (direction)
            {
                case 0:
                    movementX = 0;
                    movementY = -1;
                    break;
                case 1:
                    movementX = 1;
                    movementY = 0;
                    break;
                case 2:
                    movementX = 0;
                    movementY = 1;
                    break;
                case 3:
                    movementX = -1;
                    movementY = 0;
                    break;
            }

            nextPosX = currentPosX + movementX;
            nextPosY = currentPosY + movementY;

            if (nextPosX < 0 || nextPosY == width || nextPosY < 0 || nextPosY == height)
            {
                break;
            }

            var hasObstacle = map[nextPosX][nextPosY];
            if (hasObstacle)
            {
                direction++;
                direction %= 4;
            }
            else
            {
                currentPosX = nextPosX;
                currentPosY = nextPosY;
            }
        }

        result = mapPath.Select(v1 => v1.Where(v2 => v2).Count()).Sum();
        return result;
    }

    protected override object InternalPart2()
    {
        //Stopwatch sw = new Stopwatch();
        //sw.Start();
        var result = 0;

        var width = this.Input.Lines[0].Length;
        var height = this.Input.Lines.Length;

        var map = GetEmptyMap<bool>(width, height);

        int currentPosX = 0;
        int currentPosY = 0;
        var currentDirection = Direction.Up;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var val = this.Input.Lines[y][x];
                if (val == '#')
                    map[x][y] = true;
                else if (val == '^')
                {
                    currentPosX = x;
                    currentPosY = y;
                }
            }
        }

        int referencePosX;
        int referencePosY;
        Direction referenceDirection;
        int nextPosX;
        int nextPosY;
        var triedObstacleMap = GetEmptyMap<bool>(width, height);

        bool[][] referenceMap = map;
        map = DeepClone(referenceMap);

        while (true)
        {
            GetNextPosition(currentPosX, currentPosY, currentDirection, out nextPosX, out nextPosY);

            if (nextPosX < 0 || nextPosX == width || nextPosY < 0 || nextPosY == height)
            {
                break;
            }

            var hasObstacle = map[nextPosX][nextPosY];
            if (hasObstacle)
            {
                currentDirection = RotateRight(currentDirection);
            }
            else
            {
                if (!triedObstacleMap[nextPosX][nextPosY])
                {
                    // Save current state
                    referencePosX = currentPosX;
                    referencePosY = currentPosY;
                    referenceDirection = currentDirection;

                    // Flag you tried this
                    triedObstacleMap[nextPosX][nextPosY] = true;

                    // Try put obstacle, check the result;
                    map[nextPosX][nextPosY] = true;
                    var walkingDirectionMap = GetEmptyMap<Direction>(width, height);

                    var stayedInside = true;
                    while (true)
                    {
                        GetNextPosition(currentPosX, currentPosY, currentDirection, out nextPosX, out nextPosY);

                        if (nextPosX < 0 || nextPosX == width || nextPosY < 0 || nextPosY == height)
                        {
                            // Walked Outside
                            stayedInside = false;
                            break;
                        }
                        else if ((walkingDirectionMap[currentPosX][currentPosY] & currentDirection) == currentDirection)
                        {
                            // Looped
                            break;
                        }
                        else
                        {
                            walkingDirectionMap[currentPosX][currentPosY] |= currentDirection;
                        }

                        hasObstacle = map[nextPosX][nextPosY];
                        if (hasObstacle)
                        {
                            currentDirection = RotateRight(currentDirection);
                        }
                        else
                        {
                            currentPosX = nextPosX;
                            currentPosY = nextPosY;
                        }
                    }

                    if (stayedInside)
                    {
                        result++;
                    }

                    // Restore state
                    currentPosX = referencePosX;
                    currentPosY = referencePosY;
                    currentDirection = referenceDirection;

                    map = DeepClone(referenceMap);
                    // Back to walking
                }
                else
                {
                    currentPosX = nextPosX;
                    currentPosY = nextPosY;
                }
            }
        }

        //sw.Stop();
        return result;
    }

    private static void GetNextPosition(int currentPosX, int currentPosY, Direction direction, out int nextPosX, out int nextPosY)
    {
        var movementX = 0;
        var movementY = 0;

        switch (direction)
        {
            case Direction.Up:
                movementX = 0;
                movementY = -1;
                break;
            case Direction.Right:
                movementX = 1;
                movementY = 0;
                break;
            case Direction.Down:
                movementX = 0;
                movementY = 1;
                break;
            case Direction.Left:
                movementX = -1;
                movementY = 0;
                break;
        }

        nextPosX = currentPosX + movementX;
        nextPosY = currentPosY + movementY;
    }

    private static T[][] GetEmptyMap<T>(int width, int height)
    {
        var result = new T[width][];
        for (int i = 0; i < width; i++)
        {
            result[i] = new T[height];
        }

        return result;
    }

    private static T[][] DeepClone<T>(T[][] map)
    {
        var newMap = new T[map.Length][];
        for (int i = 0; i < map.Length; i++)
        {
            newMap[i] = (T[])map[i].Clone();
        }

        return newMap;
    }

    [Flags]
    private enum Direction
    {
        Up = 1,
        Right = 2,
        Down = 4,
        Left = 8
    }

    private static Direction RotateRight(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return Direction.Right;
            case Direction.Right:
                return Direction.Down;
            case Direction.Down:
                return Direction.Left;
            case Direction.Left:
                return Direction.Up;
            default:
                throw new NotImplementedException();
        }
    }
}
