using CommunityToolkit.HighPerformance;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp.Helpers;
internal class ConsoleHelper
{

    private static readonly ConsoleColor[] consoleColors = [
        ConsoleColor.DarkBlue,
        ConsoleColor.DarkGreen,
        ConsoleColor.DarkCyan,
        ConsoleColor.DarkRed,
        ConsoleColor.DarkMagenta,
        ConsoleColor.DarkYellow,
        ConsoleColor.Gray,
        ConsoleColor.DarkGray,
        ConsoleColor.Blue,
        ConsoleColor.Green,
        ConsoleColor.Cyan,
        ConsoleColor.Red,
        ConsoleColor.Magenta,
        ConsoleColor.Yellow,
        ];

    //private static readonly string[] consoleColors = [
    //    "\x1b[91m",
    //    "\x1b[92m",
    //    "\x1b[93m",
    //    "\x1b[94m",
    //    "\x1b[95m",
    //    "\x1b[96m",
    //    ];

    public static ConsoleHelper Instance { get; } = new ConsoleHelper();

    public bool IsEnabled { get; set; } = true;

    public Dictionary<Type, Func<object, ConsoleColor>> ColorFunctions { get; set; } = [];
    //public Dictionary<Type, Func<object, int>> ColorFunctions { get; set; } = [];
    public Dictionary<Type, Func<object, string>> StringFunctions { get; set; } = [];
    public Func<object, ConsoleColor> DefaultColorFunction { get; set; } = new Func<object, ConsoleColor>(v => consoleColors[v?.GetHashCode() % consoleColors.Length ?? 0]);
    //public Func<object, int> DefaultColorFunction { get; set; } = new Func<object, int>(v => v?.GetHashCode() % consoleColors.Length ?? 0);
    public Func<object, string> DefaultStringFunction { get; set; } = new Func<object, string>(v => v?.ToString() ?? " ");
    private ConsoleHelper() { }

    public ConsoleHelper Clear()
    {
        if (this.IsEnabled)
        {
            Console.ResetColor();
            Console.Clear();
        }

        return this;
    }

    public ConsoleHelper Write<T>(T[][] map)
    {
        if (this.IsEnabled && map != null)
        {
            if (!this.ColorFunctions.TryGetValue(typeof(T), out var colorFunction))
            {
                colorFunction = this.DefaultColorFunction;
            }

            if (!this.StringFunctions.TryGetValue(typeof(T), out var stringFunction))
            {
                stringFunction = this.DefaultStringFunction;
            }

            for (int y = 0; y < map[0].Length; y++)
            {
                for (int x = 0; x < map.Length; x++)
                {
                    T value = map[x][y];
                    if (value == null)
                    {
                        Console.Write(' ');
                    }
                    else
                    {
                        Console.ForegroundColor = colorFunction(value);
                        Console.Write(stringFunction(value));
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        return this;
    }

}
