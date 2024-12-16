﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp.Structures;
[Flags]
internal enum DirectionFlag
{
    None = 0,
    Up = 1,
    Right = 2,
    Down = 4,
    Left = 8
}
