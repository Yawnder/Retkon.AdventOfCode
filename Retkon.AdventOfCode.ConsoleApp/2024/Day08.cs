using AdventOfCodeSupport;
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

public class Day08 : AdventBase
{
    protected override object InternalPart1()
    {
        var result = 0;

        var height = this.Input.Lines.Length;
        var width = this.Input.Lines[0].Length;

        var antenaMap = GetEmptyMap<char>(width, height);
        var antenodeMap = GetEmptyMap<bool>(width, height);
        var antenas = new Dictionary<char, List<Vector2Int>>();

        for (int y = 0; y < height; y++)
        {
            var line = this.Input.Lines[y];
            for (int x = 0; x < width; x++)
            {
                var antenaType = line[x];
                if (antenaType != '.')
                {
                    if (!antenas.TryGetValue(antenaType, out var antenaList))
                    {
                        antenaList = [];
                        antenas[antenaType] = antenaList;
                    }

                    antenaList.Add(new Vector2Int(x, y));
                    antenaMap[x][y] = antenaType;
                }
            }
        }

        foreach (var antenaKvp in antenas)
        {
            var type = antenaKvp.Key;
            var antenaList = antenaKvp.Value;

            for (int i = 0; i < antenaList.Count - 1; i++)
            {
                for (int j = i + 1; j < antenaList.Count; j++)
                {
                    var antena1 = antenaList[i];
                    var antena2 = antenaList[j];

                    var x1 = antena1.X - (antena2.X - antena1.X);
                    var y1 = antena1.Y - (antena2.Y - antena1.Y);
                    var x2 = antena2.X + (antena2.X - antena1.X);
                    var y2 = antena2.Y + (antena2.Y - antena1.Y);

                    if (x1 >= 0 && x1 < width && y1 >= 0 && y1 < height)
                    {
                        antenodeMap[x1][y1] = true;
                    }

                    if (x2 >= 0 && x2 < width && y2 >= 0 && y2 < height)
                    {
                        antenodeMap[x2][y2] = true;
                    }
                }
            }
        }

        result = antenodeMap.Select(a => a.Count(v => v)).Sum();

        return result;
    }

    protected override object InternalPart2()
    {
        var result = 0;

        var height = this.Input.Lines.Length;
        var width = this.Input.Lines[0].Length;

        var antenaMap = GetEmptyMap<char>(width, height);
        var antenodeMap = GetEmptyMap<bool>(width, height);
        var antenas = new Dictionary<char, List<Vector2Int>>();

        for (int y = 0; y < height; y++)
        {
            var line = this.Input.Lines[y];
            for (int x = 0; x < width; x++)
            {
                var antenaType = line[x];
                if (antenaType != '.')
                {
                    if (!antenas.TryGetValue(antenaType, out var antenaList))
                    {
                        antenaList = [];
                        antenas[antenaType] = antenaList;
                    }

                    antenaList.Add(new Vector2Int(x, y));
                    antenaMap[x][y] = antenaType;
                }
            }
        }

        foreach (var antenaKvp in antenas)
        {
            var type = antenaKvp.Key;
            var antenaList = antenaKvp.Value;

            var possibleAntenodeMap = GetEmptyMap<int>(width, height);

            for (int i = 0; i < antenaList.Count - 1; i++)
            {
                for (int j = i + 1; j < antenaList.Count; j++)
                {
                    var antena1 = antenaList[i];
                    var antena2 = antenaList[j];

                    var stepX = antena2.X - antena1.X;
                    var stepY = antena2.Y - antena1.Y;

                    //for (int k = 2; k < 25; k++)
                    //{
                    //    if (stepX % k == 0 && stepY % k == 0)
                    //    {
                    //        stepX = stepX / k;
                    //        stepY = stepY / k;
                    //        k = 2;
                    //    }
                    //}

                    for (int k = -(width - 2); k <= (width - 2); k++)
                    {
                        var x = antena1.X - stepX * k;
                        var y = antena1.Y - stepY * k;

                        if (x >= 0 && x < width && y >= 0 && y < height)
                        {
                            possibleAntenodeMap[x][y]++;
                        }
                    }
                }
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (possibleAntenodeMap[x][y] > 0)
                    {
                        antenodeMap[x][y] = true;
                    }
                }
            }

        }

        result = antenodeMap.Select(a => a.Count(v => v)).Sum();

        return result;
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

}

