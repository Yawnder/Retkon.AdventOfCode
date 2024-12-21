using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp.Structures;
[DebuggerDisplay("({X},{Y},{Z})")]
public readonly struct Vector3Int(int x, int y, int z)
{
    public readonly int X { get; } = x;
    public readonly int Y { get; } = y;
    public readonly int Z { get; } = z;

    //public static implicit operator Vector3Int((int X, int Y) tuple) => new(tuple.X, tuple.Y);

    //public static Vector3Int operator +(Vector3Int a, Vector3Int b) => new(a.X + b.X, a.Y + b.Y);
    //public static Vector3Int operator -(Vector3Int a, Vector3Int b) => new(a.X - b.X, a.Y - b.Y);
    //public static Vector3Int operator *(Vector3Int a, int value) => new(a.X * value, a.Y * value);
    //public static Vector3Int operator *(int value, Vector3Int a) => new(a.X * value, a.Y * value);
    //public static Vector3Int operator %(Vector3Int a, Vector3Int b) => new(a.X % b.X, a.Y % b.Y);
    public static bool operator ==(Vector3Int a, Vector3Int b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    public static bool operator !=(Vector3Int a, Vector3Int b) => a.X != b.X || a.Y != b.Y || a.Z != b.Z;
    public override bool Equals(object? obj)
    {
        return obj is Vector3Int @int &&
               this.X == @int.X &&
               this.Y == @int.Y &&
               this.Z == @int.Z;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.X, this.Y, this.Z);
    }

    //public static readonly ReadOnlyCollection<Vector3Int> Plus = new([(0, -1), (1, 0), (0, 1), (-1, 0)]);
    //public static readonly ReadOnlyCollection<Vector3Int> Diagonal = new([(1, -1), (1, 1), (-1, 1), (-1, -1)]);
    //public static readonly ReadOnlyCollection<Vector3Int> Surrounding = new([(0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1)]);

    //public List<Vector3Int> GetPlus()
    //{
    //    var source = this;
    //    return Plus.Select(v => v + source).ToList();
    //}

    //public List<Vector3Int> GetDiagonal()
    //{
    //    var source = this;
    //    return Diagonal.Select(v => v + source).ToList();
    //}

    //public List<Vector3Int> GetSurrounding()
    //{
    //    var source = this;
    //    return Surrounding.Select(v => v + source).ToList();
    //}

}