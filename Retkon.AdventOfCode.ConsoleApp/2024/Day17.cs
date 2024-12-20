using AdventOfCodeSupport;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Diagnostics.Tracing.Parsers.IIS_Trace;
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

public class Day17 : AdventBase
{

    private long? overrideA = null;
    private long[] regs = null!;

    protected override object InternalPart1()
    {
        this.regs =
        [
            long.Parse(this.Input.Lines[0][12..]),
            long.Parse(this.Input.Lines[1][12..]),
            long.Parse(this.Input.Lines[2][12..]),
        ];

        if (this.overrideA != null)
        {
            this.regs[0] = this.overrideA.Value;
        }

        var outputs = new List<long>();

        var program = this.Input.Lines[4][9..].Split(',');
        var ops = program.Where((v, i) => i % 2 == 0).Select(v => byte.Parse(v)).ToArray();
        var operands = program.Where((v, i) => i % 2 == 1).Select(v => byte.Parse(v)).ToArray();

        for (long i = 0; i < ops.Length; i++)
        {
            var op = ops[i];
            var literalOperand = operands[i];
            var comboOperand = this.GetComboValue(literalOperand);

            switch (op)
            {
                case 0:
                    this.regs[0] = (long)(this.regs[0] / Math.Pow(2, comboOperand));
                    break;
                case 1:
                    this.regs[1] = this.regs[1] ^ literalOperand;
                    break;
                case 2:
                    this.regs[1] = comboOperand % 8;
                    break;
                case 3:
                    var v3 = this.regs[0];
                    if (v3 != 0)
                    {
                        i = operands[i] - 1;
                    }
                    break;
                case 4:
                    this.regs[1] = this.regs[1] ^ this.regs[2];
                    break;
                case 5:
                    outputs.Add(comboOperand % 8);
                    break;
                case 7:
                    this.regs[2] = (long)(this.regs[0] / Math.Pow(2, comboOperand));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        return string.Join(",", outputs);
    }

    protected override object InternalPart2()
    {
        var previousCycleEndAStack = new Stack<long>([0]);
        var currentCycleEndAStack = new Stack<long>([0]);
        var cycleEndOutput = this.Input.Lines[4][9..].Split(',').Select(v => byte.Parse(v)).Reverse().ToList();

        var currentCycle = 0;

        while (currentCycle < cycleEndOutput.Count)
        {
            long a;
            long b;
            long c;

            while (currentCycleEndAStack.TryPop(out long cycleEndA))
            {
                long cycleOutput = cycleEndOutput[currentCycle];

                for (long deltaA = 0; deltaA < 8; deltaA++)
                {
                    a = cycleEndA * 8 + deltaA;

                    b = a % 8;
                    b = b ^ 2;
                    c = a / (long)Math.Pow(2, b);
                    a = a / 8;
                    b = b ^ 7 ^ c;

                    if (b % 8 == cycleOutput && a == cycleEndA)
                    {
                        previousCycleEndAStack.Push(cycleEndA * 8 + deltaA);
                    }
                }
            }

            if (previousCycleEndAStack.Count == 0)
                throw new NotImplementedException();

            currentCycleEndAStack = previousCycleEndAStack;
            previousCycleEndAStack = new Stack<long>();
            currentCycle++;
        }

        foreach (long overrideA in currentCycleEndAStack.Order())
        {
            this.overrideA = overrideA;
            var output = (string)this.InternalPart1();

            if (output == this.Input.Lines[4][9..])
            {
                break;
            }
            else
            {
                this.overrideA = null;
            }
        }

        if (this.overrideA == null)
            throw new NotImplementedException();

        else
            return this.overrideA.Value;
    }

    private long GetComboValue(byte combo)
    {
        switch (combo)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                return combo;
            case 4:
                return this.regs[0];
            case 5:
                return this.regs[1];
            case 6:
                return this.regs[2];
            default:
                return -1;
        }

    }
}

