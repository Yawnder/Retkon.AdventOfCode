﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp.Helpers;
internal static class DoubleExtensions
{
    private const double Epsilon = 1e-10;

    public static bool IsZero(this double d)
    {
        return Math.Abs(d) < Epsilon;
    }

}
