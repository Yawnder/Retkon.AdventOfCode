using AdventOfCodeSupport;
using Retkon.AdventOfCode.ConsoleApp.Helpers;
using Retkon.AdventOfCode.ConsoleApp.Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Retkon.AdventOfCode.ConsoleApp._2024;

public class Day14 : AdventBase
{
    private int height;
    private readonly ConsoleHelper consoleHelper = ConsoleHelper.Instance;
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
        this.consoleHelper.DefaultColorFunction = new Func<object, ConsoleColor>(o => ConsoleColor.Red);
        this.consoleHelper.DefaultStringFunction = new Func<object, string>(o => o != null ? "X" : " ");
        this.width = 101;
        this.height = 103;

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

        var mapSize = new Vector2Int(this.width, this.height);
        var directoryInfo = new DirectoryInfo("Output");
        if (directoryInfo.Exists)
        {
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
        }
        else
        {
            directoryInfo.Create();
        }

        for (int i = 0; i < 10000; i++)
        {
            if ((i - 1) % 103 == 0 && ((i - 48) % 101 == 0))
            {
#pragma warning disable CA1416 // Validate platform compatibility
                var bitmap = new Bitmap(this.width, this.height);

                foreach (var drone in this.drones)
                {
                    var dronePosition = (drone.Position + drone.Velocity * i + (mapSize * 100000)) % mapSize;
                    bitmap.SetPixel(dronePosition.X, dronePosition.Y, Color.White);
                }

                bitmap.Save(Path.Combine(directoryInfo.FullName, $"D14P2_{i:000000}.jpg"), ImageFormat.Jpeg);
#pragma warning restore CA1416 // Validate platform compatibility
            }
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

    }

}

