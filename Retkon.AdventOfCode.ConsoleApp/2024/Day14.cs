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

public class Day14 : AdventBase
{
    private int height;
    private ConsoleHelper consoleHelper;
    private int width;
    private Drone[] drones = null!;

    protected override object InternalPart1()
    {
        this.width = 101;
        this.height = 103;

        //this.width = 11;
        //this.height = 7;

        var parseRegex = new Regex(@"p=(?<posX>\d*),(?<posY>\d*) v=(?<velX>-?\d*),(?<velY>-?\d*)", RegexOptions.Compiled);

        this.drones = new Drone[this.Input.Lines.Length];

        for (int y = 0; y < this.Input.Lines.Length; y++)
        {
            var match = parseRegex.Match(this.Input.Lines[y]);
            this.drones[y] = new Drone
            {
                Position = new Vector2Int(int.Parse(match.Groups["posX"].Value), int.Parse(match.Groups["posY"].Value)),
                Velocity = new Vector2Int(int.Parse(match.Groups["velX"].Value), int.Parse(match.Groups["velY"].Value)),
            };
        }

        int steps = 100;
        var mapSize = new Vector2Int(this.width, this.height);
        foreach (var drone in this.drones)
        {
            drone.Position = (drone.Position + drone.Velocity * steps + (mapSize * 1000)) % mapSize;
        }

        var result = this.drones.GroupBy(d => this.GetQuadrant(d.Position)).Where(g => g.Key > -1).Select(g => g.LongCount()).Aggregate(1L, (v0, v) => v0 * v);

        return result;
    }

    protected override object InternalPart2()
    {
        this.consoleHelper = ConsoleHelper.Instance;
        this.consoleHelper.IsEnabled = true;
        this.consoleHelper.DefaultColorFunction = new Func<object, ConsoleColor>(o => ConsoleColor.Red);
        this.consoleHelper.DefaultStringFunction = new Func<object, string>(o => o != null ? "X" : " ");
        this.width = 101;
        this.height = 103;

        //this.width = 11;
        //this.height = 7;

        var parseRegex = new Regex(@"p=(?<posX>\d*),(?<posY>\d*) v=(?<velX>-?\d*),(?<velY>-?\d*)", RegexOptions.Compiled);

        this.drones = new Drone[this.Input.Lines.Length];

        for (int y = 0; y < this.Input.Lines.Length; y++)
        {
            var match = parseRegex.Match(this.Input.Lines[y]);
            this.drones[y] = new Drone
            {
                Position = new Vector2Int(int.Parse(match.Groups["posX"].Value), int.Parse(match.Groups["posY"].Value)),
                Velocity = new Vector2Int(int.Parse(match.Groups["velX"].Value), int.Parse(match.Groups["velY"].Value)),
            };
        }

        int steps = 100;
        var mapSize = new Vector2Int(this.width, this.height);
        for (int i = 0; i < 1000; i++)
        {
            this.consoleHelper.Clear();
            var map = ArrayHelper.GetEmptyMap<string>(this.width, this.height);
            foreach (var drone in this.drones)
            {
                drone.Position2 = (drone.Position + drone.Velocity * i + (mapSize * 1000)) % mapSize;
                map[drone.Position2.X][drone.Position2.Y] = "X";
            }

            this.consoleHelper.Write(map);
        }

        throw new NotImplementedException();
    }

    private int GetQuadrant(Vector2Int position)
    {
        if (position.X < (this.width - 1) / 2 && position.Y < (this.height - 1) / 2)
        {
            return 0;
        }
        else if (position.X > (this.width - 1) / 2 && position.Y < (this.height - 1) / 2)
        {
            return 1;
        }
        else if (position.X < (this.width - 1) / 2 && position.Y > (this.height - 1) / 2)
        {
            return 2;
        }
        else if (position.X > (this.width - 1) / 2 && position.Y > (this.height - 1) / 2)
        {
            return 3;
        }
        else
        {
            return -1;
        }
    }

    [DebuggerDisplay("{Position.X},{Position.Y}")]
    private class Drone
    {
        public Vector2Int Position { get; set; }
        public Vector2Int Velocity { get; set; }
        public Vector2Int Position2 { get; set; }

    }

}

