using AdventOfCodeSupport;
using BenchmarkDotNet.Disassemblers;
using Perfolizer.Mathematics.SignificanceTesting;
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

public class Day15 : AdventBase
{

    private int height;
    private int width;
    private Vector2Int position;
    private List<TileContent[]> map = null!;

    protected override object InternalPart1()
    {
        var result = 0;
        this.width = this.Input.Lines[0].Length;
        var inHeader = true;

        this.map = new List<TileContent[]>(1000);
        var moves = new List<Direction>(10000);
        for (int y = 0; y < this.Input.Lines.Length; y++)
        {
            var line = this.Input.Lines[y];

            if (inHeader)
            {
                if (line.Length == 0)
                {
                    this.height = y;
                    inHeader = false;
                    continue;
                }

                var mapRow = new TileContent[this.width];
                this.map.Add(mapRow);

                for (int x = 0; x < this.width; x++)
                {
                    switch (line[x])
                    {
                        case '.':
                            mapRow[x] = TileContent.Nothing;
                            break;
                        case 'O':
                            mapRow[x] = TileContent.Box;
                            break;
                        case '@':
                            mapRow[x] = TileContent.Nothing;
                            this.position = new Vector2Int(x, y);
                            break;
                        case '#':
                            mapRow[x] = TileContent.Wall;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            else
            {
                foreach (var move in line)
                {
                    switch (move)
                    {
                        case '^':
                            moves.Add(Direction.Up);
                            break;
                        case '>':
                            moves.Add(Direction.Right);
                            break;
                        case 'v':
                            moves.Add(Direction.Down);
                            break;
                        case '<':
                            moves.Add(Direction.Left);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        foreach (var moveDirection in moves)
        {
            var move = Vector2Int.Plus[(int)moveDirection];
            var newPosition = this.position + move;
            var canMove = this.TryMove(newPosition, moveDirection);
            if (canMove)
            {
                this.position = newPosition;
            }
            else
            {

            }
        }

        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                if (this.map[y][x] == TileContent.Box)
                {
                    result += x + y * 100;
                }
            }
        }

        return result;
    }

    protected override object InternalPart2()
    {
        var result = 0L;
        this.width = this.Input.Lines[0].Length;
        var inHeader = true;

        this.map = new List<TileContent[]>(1000);
        var moves = new List<Direction>(10000);
        for (int y = 0; y < this.Input.Lines.Length; y++)
        {
            var line = this.Input.Lines[y];

            if (inHeader)
            {
                if (line.Length == 0)
                {
                    this.height = y;
                    inHeader = false;
                    continue;
                }

                var mapRow = new TileContent[this.width * 2];
                this.map.Add(mapRow);

                for (int x = 0; x < this.width; x++)
                {
                    switch (line[x])
                    {
                        case '.':
                            mapRow[2 * x] = TileContent.Nothing;
                            mapRow[2 * x + 1] = TileContent.Nothing;
                            break;
                        case 'O':
                            mapRow[2 * x] = TileContent.Box1;
                            mapRow[2 * x + 1] = TileContent.Box2;
                            break;
                        case '@':
                            mapRow[2 * x] = TileContent.Nothing;
                            mapRow[2 * x + 1] = TileContent.Nothing;
                            this.position = new Vector2Int(2 * x, y);
                            break;
                        case '#':
                            mapRow[2 * x] = TileContent.Wall;
                            mapRow[2 * x + 1] = TileContent.Wall;
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            else
            {
                foreach (var move in line)
                {
                    switch (move)
                    {
                        case '^':
                            moves.Add(Direction.Up);
                            break;
                        case '>':
                            moves.Add(Direction.Right);
                            break;
                        case 'v':
                            moves.Add(Direction.Down);
                            break;
                        case '<':
                            moves.Add(Direction.Left);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        foreach (var moveDirection in moves)
        {
            var move = Vector2Int.Plus[(int)moveDirection];
            var newPosition = this.position + move;
            var canMove = this.TryMove2(newPosition, moveDirection);
            if (canMove)
            {
                this.position = newPosition;
            }
            else
            {

            }
        }

        for (int x = 0; x < this.width * 2; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                if (this.map[y][x] == TileContent.Box1)
                {
                    result += x + y * 100;
                }
            }
        }

        return result;
    }

    private bool TryMove(Vector2Int position, Direction direction)
    {
        switch (this.map[position.Y][position.X])
        {
            case TileContent.Nothing:
                return true;
            case TileContent.Box:
                var move = Vector2Int.Plus[(int)direction];
                var newPosition = position + move;
                var canMove = this.TryMove(newPosition, direction);
                if (canMove)
                {
                    this.map[newPosition.Y][newPosition.X] = TileContent.Box;
                    this.map[position.Y][position.X] = TileContent.Nothing;
                }
                return canMove;
            case TileContent.Wall:
                return false;
            default:
                throw new NotImplementedException();
        }

    }

    private bool TryMove2(Vector2Int position, Direction direction)
    {
        if (direction == Direction.Left || direction == Direction.Right)
        {
            return this.TryMoveHorizontal(position, direction);
        }
        else
        {
            return this.TryMoveVertical(position, direction, true);
        }
    }

    private bool TryMoveHorizontal(Vector2Int position, Direction direction)
    {
        switch (this.map[position.Y][position.X])
        {
            case TileContent.Nothing:
                return true;
            case TileContent.Box1:
            {
                var move = Vector2Int.Plus[(int)direction];
                var newPosition = position + move;
                var canMove = this.TryMoveHorizontal(newPosition, direction);
                if (canMove)
                {
                    this.map[newPosition.Y][newPosition.X] = TileContent.Box1;
                    this.map[position.Y][position.X] = TileContent.Nothing;
                }
                return canMove;
            }
            case TileContent.Box2:
            {
                var move = Vector2Int.Plus[(int)direction];
                var newPosition = position + move;
                var canMove = this.TryMoveHorizontal(newPosition, direction);
                if (canMove)
                {
                    this.map[newPosition.Y][newPosition.X] = TileContent.Box2;
                    this.map[position.Y][position.X] = TileContent.Nothing;
                }
                return canMove;
            }
            case TileContent.Wall:
                return false;
            default:
                throw new NotImplementedException();
        }

    }

    private bool TryMoveVertical(Vector2Int position, Direction direction, bool isFirstMove)
    {
        switch (this.map[position.Y][position.X])
        {
            case TileContent.Nothing:
                return true;
            case TileContent.Box1:
            {
                var move = Vector2Int.Plus[(int)direction];
                var offset = Vector2Int.Plus[(int)Direction.Right];
                var newPosition1 = position + move;
                var newPosition2 = position + move + offset;
                var canMove = this.TryMoveVertical(newPosition1, direction, false) && this.TryMoveVertical(newPosition2, direction, false);
                if (canMove && isFirstMove)
                {
                    this.DoMoveVertical(newPosition1, direction);
                    this.DoMoveVertical(newPosition2, direction);
                    this.DoMoveVertical(position, direction);
                }
                return canMove;
            }
            case TileContent.Box2:
            {
                var move = Vector2Int.Plus[(int)direction];
                var offset = Vector2Int.Plus[(int)Direction.Left];
                var newPosition1 = position + move;
                var newPosition2 = position + move + offset;
                var canMove = this.TryMoveVertical(newPosition1, direction, false) && this.TryMoveVertical(newPosition2, direction, false);
                if (canMove && isFirstMove)
                {
                    this.DoMoveVertical(newPosition1, direction);
                    this.DoMoveVertical(newPosition2, direction);
                    this.DoMoveVertical(position, direction);
                }
                return canMove;
            }
            case TileContent.Wall:
                return false;
            default:
                throw new NotImplementedException();
        }

    }

    private void DoMoveVertical(Vector2Int position, Direction direction)
    {
        switch (this.map[position.Y][position.X])
        {
            case TileContent.Nothing:
                return;
            case TileContent.Box1:
            {
                var move = Vector2Int.Plus[(int)direction];
                var offset = Vector2Int.Plus[(int)Direction.Right];
                var newPosition1 = position + move;
                var newPosition2 = position + move + offset;

                this.DoMoveVertical(newPosition1, direction);
                this.DoMoveVertical(newPosition2, direction);

                this.map[newPosition1.Y][newPosition1.X] = TileContent.Box1;
                this.map[position.Y][position.X] = TileContent.Nothing;
                this.map[newPosition2.Y][newPosition2.X] = TileContent.Box2;
                this.map[(position + offset).Y][(position + offset).X] = TileContent.Nothing;
                return;
            }
            case TileContent.Box2:
            {
                var move = Vector2Int.Plus[(int)direction];
                var offset = Vector2Int.Plus[(int)Direction.Left];
                var newPosition1 = position + move;
                var newPosition2 = position + move + offset;

                this.DoMoveVertical(newPosition1, direction);
                this.DoMoveVertical(newPosition2, direction);

                this.map[newPosition1.Y][newPosition1.X] = TileContent.Box2;
                this.map[position.Y][position.X] = TileContent.Nothing;
                this.map[newPosition2.Y][newPosition2.X] = TileContent.Box1;
                this.map[(position + offset).Y][(position + offset).X] = TileContent.Nothing;
                return;
            }
            case TileContent.Wall:
                throw new NotImplementedException();
        }

    }

    private enum TileContent
    {
        Nothing,
        Wall,
        Box,
        Box1,
        Box2,
    }

}

