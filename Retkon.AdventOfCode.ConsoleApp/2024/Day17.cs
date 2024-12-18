using AdventOfCodeSupport;
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

    private long[] regs = null!;

    protected override object InternalPart1()
    {
        this.regs =
        [
            long.Parse(this.Input.Lines[0][12..]),
            long.Parse(this.Input.Lines[1][12..]),
            long.Parse(this.Input.Lines[2][12..]),
        ];

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
        var program = this.Input.Lines[4][9..].Split(',').Select(v => byte.Parse(v));
        var ops = program.Where((v, i) => i % 2 == 0).ToArray();
        var operands = program.Where((v, i) => i % 2 == 1).ToArray();
        var outputList = new List<byte>(program);

        //long initialA = 200_000_000_000_000L;
        //long initialA = 000L;

        var outputs = new List<long>();
        var loopCount = 0;
        for (long initialA = 4398046511103L; initialA <= 35184372088831L; initialA++)
        {

            var outputCount = 0;
            var isImpossible = false;
            outputs.Clear();

            this.regs =
            [
                initialA,
                0,
                0,
            ];

            for (int i = 0; i < ops.Length; i++)
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
                        var outputValue = comboOperand % 8;
                        if (outputList[outputCount] != outputValue)
                        {
                            isImpossible = true;
                        }
                        else
                        {
                            outputs.Add(comboOperand % 8);
                            outputCount++;
                        }
                        break;
                    case 7:
                        this.regs[2] = (long)(this.regs[0] / Math.Pow(2, comboOperand));
                        break;
                    default:
                        throw new NotImplementedException();
                }

                if (isImpossible)
                    break;
            }

            if (!isImpossible)
            {
            }

            if (outputs.Count > 14)
            {
                Console.WriteLine($"{initialA}: {string.Join(",", outputs)}");
            }

            if (++loopCount > 50_000_000)
            {
                loopCount = 0;
                Console.WriteLine($"{DateTime.Now:HH:mm:ss}: {(float)(initialA - 4398046511103L) / 35184372088831L:0.0000%}");
            }
            //initialA++;
        }

        throw new NotImplementedException();
    }

    //protected override object InternalPart2()
    //{
    //    var program = this.Input.Lines[4][9..].Split(',').Select(v => byte.Parse(v));
    //    var ops = program.Where((v, i) => i % 2 == 0).ToArray();
    //    var operands = program.Where((v, i) => i % 2 == 1).ToArray();

    //    var desiredOutput = string.Join(',', this.Input.Lines[4][9..]);
    //    var reverseOutputList = new List<byte>(program.Reverse());

    //    while (true)
    //    {
    //        this.regs = [0, 0, 0];

    //        for (long initialC = 0L; initialC <= long.MaxValue; initialC++)
    //        {
    //            this.regs[2] = initialC;
    //            var isLast = true;

    //            for (int outputId = 0; outputId < reverseOutputList.Count; outputId++)
    //            {
    //                var currentOutput = reverseOutputList[outputId];

    //                // I7
    //                for (long i7B = currentOutput; i7B < long.MaxValue; i7B += 8)
    //                {
    //                    this.regs[1] = i7B;

    //                    // I6
    //                    this.regs[1] ^= this.regs[2];

    //                    // I5
    //                    this.regs[1] ^= 7;

    //                    // I4
    //                    for (long i4DA = 0L; i4DA < 8; i4DA++)
    //                    {
    //                        if (isLast && i4DA > 0)
    //                            break;

    //                        this.regs[0] = this.regs[0] * 8 + i4DA;

    //                        // I3
    //                        for (long i3DC = 0L; i3DC < 8; i3DC++)
    //                        {
    //                            if (this.regs[0] != this.regs[2] * Math.Pow(2, this.regs[1]))
    //                            {
    //                                continue;
    //                            }

    //                        }

    //                    }

    //                }
    //                isLast = false;
    //            }
    //        }

    //        for (var g0 = 0; g0 < 3; g0++)
    //        {
    //        }







    //        this.regs[0] *= 8;

    //        for (byte k = 0; k < 8; k++)
    //        {
    //            this.regs[0] *= 8;



    //            var outputs = new List<long>();
    //            for (long i = 0; i < ops.Length; i++)
    //            {
    //                var op = ops[i];
    //                var literalOperand = operands[i];
    //                var comboOperand = this.GetComboValue(literalOperand);

    //                switch (op)
    //                {
    //                    case 0:
    //                        this.regs[0] = (long)(this.regs[0] / Math.Pow(2, comboOperand));
    //                        break;
    //                    case 1:
    //                        this.regs[1] = this.regs[1] ^ literalOperand;
    //                        break;
    //                    case 2:
    //                        this.regs[1] = comboOperand % 8;
    //                        break;
    //                    case 3:
    //                        var v3 = this.regs[0];
    //                        if (v3 != 0)
    //                        {
    //                            i = operands[i] - 1;
    //                        }
    //                        break;
    //                    case 4:
    //                        this.regs[1] = this.regs[1] ^ this.regs[2];
    //                        break;
    //                    case 5:
    //                        outputs.Add(comboOperand % 8);
    //                        break;
    //                    case 7:
    //                        this.regs[2] = (long)(this.regs[0] / Math.Pow(2, comboOperand));
    //                        break;
    //                    default:
    //                        throw new NotImplementedException();
    //                }
    //            }

    //            var currentOutput = string.Join(",", outputs);
    //            if (desiredOutput == currentOutput)
    //            {
    //                break;
    //            }
    //        }
    //    }

    //    throw new NotImplementedException();

    //    return this.regs[0];
    //}

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

