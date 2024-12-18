using AdventOfCodeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp.Helpers;
internal static class ArrayHelper
{

    public static T[][] GetEmptyMap<T>(int width, int height)
    {
        var result = new T[width][];
        for (int i = 0; i < width; i++)
        {
            result[i] = new T[height];
        }

        return result;
    }

    public static void Fill<T>(this T[][] array, T value)
    {
        for (int x = 0; x < array.Length; x++)
        {
            for (int y = 0; y < array[0].Length; y++)
            {
                array[x][y] = value;
            }
        }
    }

    public static T[][] DeepClone<T>(T[][] map)
    {
        var newMap = new T[map.Length][];
        for (int i = 0; i < map.Length; i++)
        {
            newMap[i] = (T[])map[i].Clone();
        }

        return newMap;
    }

    public static int[][] To2DInt(this InputBlock input, int fromRow = 0, int toRow = -1)
    {
        return To2DT(input, v => v - '0', fromRow, toRow);
    }

    public static char[][] To2DChar(this InputBlock input, int fromRow = 0, int toRow = -1)
    {
        return To2DT(input, v => v, fromRow, toRow);
    }

    public static T[][] To2DT<T>(this InputBlock input, Func<char, T> converter, int fromRow = 0, int toRow = -1)
    {
        var width = input.Lines[fromRow].Length;

        toRow = toRow != -1 ? toRow : input.Lines.Length;
        var height = toRow - fromRow;

        var map = GetEmptyMap<T>(width, height);
        for (int y = fromRow; y < toRow; y++)
        {
            var line = input.Lines[y];
            for (int x = 0; x < width; x++)
            {
                map[x][y - fromRow] = converter(line[x]);
            }
        }

        return map;
    }

}
