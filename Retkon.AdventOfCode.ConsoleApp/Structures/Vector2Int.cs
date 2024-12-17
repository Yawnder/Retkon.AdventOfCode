using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp.Structures;
[DebuggerDisplay("({X},{Y})")]
public readonly struct Vector2Int(int x, int y)
{
    public readonly int X { get; } = x;
    public readonly int Y { get; } = y;

    public static implicit operator Vector2Int((int X, int Y) tuple) => new(tuple.X, tuple.Y);

    public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2Int operator *(Vector2Int a, int value) => new(a.X * value, a.Y * value);
    public static Vector2Int operator *(int value, Vector2Int a) => new(a.X * value, a.Y * value);
    public static Vector2Int operator %(Vector2Int a, Vector2Int b) => new(a.X % b.X, a.Y % b.Y);
    public static bool operator ==(Vector2Int a, Vector2Int b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Vector2Int a, Vector2Int b) => a.X != b.X || a.Y != b.Y;

    public static readonly ReadOnlyCollection<Vector2Int> Plus = new([(0, -1), (1, 0), (0, 1), (-1, 0)]);
    public static readonly ReadOnlyCollection<Vector2Int> Diagonal = new([(1, -1), (1, 1), (-1, 1), (-1, -1)]);
    public static readonly ReadOnlyCollection<Vector2Int> Surrounding = new([(0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1)]);

    public List<Vector2Int> GetPlus()
    {
        var source = this;
        return Plus.Select(v => v + source).ToList();
    }

    public List<Vector2Int> GetDiagonal()
    {
        var source = this;
        return Diagonal.Select(v => v + source).ToList();
    }

    public List<Vector2Int> GetSurrounding()
    {
        var source = this;
        return Surrounding.Select(v => v + source).ToList();
    }

}