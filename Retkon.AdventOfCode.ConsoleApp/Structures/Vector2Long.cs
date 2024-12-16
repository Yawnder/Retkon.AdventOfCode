using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp.Structures;
[DebuggerDisplay("({X},{Y})")]
public readonly struct Vector2Long(long x, long y)
{
    public readonly long X { get; } = x;
    public readonly long Y { get; } = y;

    public static implicit operator Vector2Long((long X, long Y) tuple) => new(tuple.X, tuple.Y);

    public static Vector2Long operator +(Vector2Long a, Vector2Long b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2Long operator -(Vector2Long a, Vector2Long b) => new(a.X - b.X, a.Y - b.Y);

    public static readonly ReadOnlyCollection<Vector2Long> Plus = new([(0, -1), (1, 0), (0, 1), (-1, 0)]);
    public static readonly ReadOnlyCollection<Vector2Long> Diagonal = new([(1, -1), (1, 1), (-1, 1), (-1, -1)]);
    public static readonly ReadOnlyCollection<Vector2Long> Surrounding = new([(0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1)]);

    public List<Vector2Long> GetPlus()
    {
        var source = this;
        return Plus.Select(v => v + source).ToList();
    }

    public List<Vector2Long> GetDiagonal()
    {
        var source = this;
        return Diagonal.Select(v => v + source).ToList();
    }

    public List<Vector2Long> GetSurrounding()
    {
        var source = this;
        return Surrounding.Select(v => v + source).ToList();
    }

    public double Cross(Vector2Long v)
    {
        return this.X * v.Y - this.Y * v.X;
    }

}