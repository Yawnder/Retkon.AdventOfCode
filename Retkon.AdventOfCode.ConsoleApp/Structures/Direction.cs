using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp.Structures;
internal enum Direction
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}

internal static class DirectionExtension
{

    public static Direction RotateLeft(this Direction direction, int amount = 1)
    {
        return (Direction)(((int)direction + (3 * amount)) % 4);
    }

    public static Direction RotateRight(this Direction direction, int amount = 1)
    {
        return (Direction)(((int)direction + (1 * amount)) % 4);
    }

    public static Vector2Int GetOffset(this Direction direction)
    {
        return Vector2Int.Plus[(int)direction];
    }

}