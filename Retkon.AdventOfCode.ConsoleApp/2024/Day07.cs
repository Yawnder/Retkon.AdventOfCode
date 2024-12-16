using AdventOfCodeSupport;
using BenchmarkDotNet.Attributes;
using CommunityToolkit.HighPerformance;
using Microsoft.Diagnostics.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp._2024;

public class Day07 : AdventBase
{
    protected override object InternalPart1()
    {
        long result = 0;

        for (int i = 0; i < this.Input.Lines.Length; i++)
        {
            var line = this.Input.Lines[i];
            var splitter = line.IndexOf(':');
            var answer = long.Parse(line[..splitter]);
            var ops = line[(splitter + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(v => long.Parse(v)).ToList().AsSpan();

            var canBeSolved = this.Solve(answer, ops[0], ops[1..], new List<Operation> { Operation.Addition, Operation.Multiplication });
            if (canBeSolved)
                result += answer;
        }

        return result;
    }

    protected override object InternalPart2()
    {
        var sw = Stopwatch.StartNew();
        long result = 0;

        for (int i = 0; i < this.Input.Lines.Length; i++)
        {
            var line = this.Input.Lines[i];
            var splitter = line.IndexOf(':');
            var answer = long.Parse(line[..splitter]);
            var ops = line[(splitter + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(v => long.Parse(v)).ToList().AsSpan();

            var canBeSolved = this.Solve(answer, ops[0], ops[1..], new List<Operation> { Operation.Addition, Operation.Multiplication, Operation.Concatenation });
            if (canBeSolved)
                result += answer;
        }
        sw.Stop();
        return result;
    }

    private bool Solve(long answer, long firstOp, Span<long> otherOps, IEnumerable<Operation> validOperations)
    {
        if (otherOps.Length == 0)
            return answer == firstOp;
        else if (answer < firstOp)
            return false;

        foreach (Operation operation in validOperations)
        {
            var runningAnswer = this.Operate(operation, firstOp, otherOps[0]);
            var isSolved = this.Solve(answer, runningAnswer, otherOps[1..], validOperations);

            if (isSolved)
                return true;
        }

        return false;
    }

    private enum Operation
    {
        Addition,
        Multiplication,
        Concatenation
    }

    private long Operate(Operation operation, long op1, long op2)
    {
        switch (operation)
        {
            case Operation.Addition:
                return op1 + op2;
            case Operation.Multiplication:
                return op1 * op2;
            case Operation.Concatenation:
                return long.Parse(op1.ToString() + op2.ToString());
            default:
                throw new NotImplementedException();
        }

    }
}

//internal class Day07 : AdventBase
//{
//    protected override object InternalPart1()
//    {
//        long result = 0;

//        for (int i = 0; i < this.Input.Lines.Length; i++)
//        {
//            var line = this.Input.Lines[i];
//            var splitter = line.IndexOf(':');
//            var answer = long.Parse(line[..splitter]);
//            var ops = line[(splitter + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(v => long.Parse(v)).ToList();

//            var canBeSolved = this.Solve(answer, ops, new List<Operation> { Operation.Addition, Operation.Multiplication });
//            if (canBeSolved)
//                result += answer;
//        }

//        return result;
//    }

//    protected override object InternalPart2()
//    {
//        long result = 0;

//        for (int i = 0; i < this.Input.Lines.Length; i++)
//        {
//            var line = this.Input.Lines[i];
//            var splitter = line.IndexOf(':');
//            var answer = long.Parse(line[..splitter]);
//            var ops = line[(splitter + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(v => long.Parse(v)).ToList();

//            var canBeSolved = this.Solve(answer, ops, new List<Operation> { Operation.Addition, Operation.Multiplication, Operation.Concatenation });
//            if (canBeSolved)
//                result += answer;
//        }

//        return result;
//    }

//    private bool Solve(long answer, List<long> ops, IEnumerable<Operation> validOperations)
//    {
//        if (ops.Count == 1)
//            return answer == ops[0];
//        else if (answer < ops[0])
//            return false;

//        foreach (Operation operation in validOperations)
//        {
//            var newList = new List<long>(ops[2..]);
//            var runningAnswer = this.Operate(operation, ops[0], ops[1]);
//            newList.Insert(0, runningAnswer);
//            var isSolved = this.Solve(answer, newList, validOperations);

//            if (isSolved)
//                return true;
//        }

//        return false;
//    }

//    private enum Operation
//    {
//        Addition,
//        Multiplication,
//        Concatenation
//    }

//    private long Operate(Operation operation, long op1, long op2)
//    {
//        switch (operation)
//        {
//            case Operation.Addition:
//                return op1 + op2;
//            case Operation.Multiplication:
//                return op1 * op2;
//            case Operation.Concatenation:
//                return long.Parse(op1.ToString() + op2.ToString());
//            default:
//                throw new NotImplementedException();
//        }

//    }
//}