using AdventOfCodeSupport;
using Microsoft.Diagnostics.Tracing.Parsers.FrameworkEventSource;
using Retkon.AdventOfCode.ConsoleApp.Helpers;
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

public partial class Day22 : AdventBase
{

    private readonly ConsoleHelper consoleHelper = ConsoleHelper.Instance;

    protected override object InternalPart1()
    {
        var result = 0L;

        for (var i = 0; i < this.Input.Lines.Length; i++)
        {
            var secret = long.Parse(this.Input.Lines[i]);
            for (int j = 0; j < 2000; j++)
            {
                secret = NextNumber(secret);
            }

            result += secret;
        }

        return result;
    }

    protected override object InternalPart2()
    {
        var result = 0L;

        var allValues = new List<List<int>>();
        var allChanges = new List<List<int>>();

        for (var i = 0; i < this.Input.Lines.Length; i++)
        {
            var secret = long.Parse(this.Input.Lines[i]);

            byte previousValue = (byte)(secret % 10L);

            var values = new List<int>();
            allValues.Add(values);
            var changes = new List<int>();
            allChanges.Add(changes);

            for (int j = 0; j < 2000; j++)
            {
                secret = NextNumber(secret);
                var value = (byte)(secret % 10L);

                values.Add(value);
                changes.Add(value - previousValue);
                previousValue = value;
            }
        }

        var allSequenceValues = new List<Dictionary<string, int>>();
        var allPossibleSequences = new List<string>(160000);

        for (int i = 0; i < allChanges.Count; i++)
        {
            var values = allValues[i];
            var changes = allChanges[i];
            var sequenceValues = new Dictionary<string, int>();
            allSequenceValues.Add(sequenceValues);
            for (int j = 3; j < allChanges[i].Count; j++)
            {
                var sequence = $"{changes[j - 3]}{changes[j - 2]}{changes[j - 1]}{changes[j]}";
                allPossibleSequences.Add(sequence);
                if (!sequenceValues.ContainsKey(sequence))
                    sequenceValues.Add(sequence, values[j]);
            }
        }

        allPossibleSequences = allPossibleSequences.Distinct().ToList();

        var mostBananas = 0;

        for (int i = 0; i < allPossibleSequences.Count; i++)
        {
            var sequence = allPossibleSequences[i];
            var bananas = 0;
            for (int j = 0; j < allSequenceValues.Count; j++)
            {
                if (allSequenceValues[j].TryGetValue(sequence, out var banana))
                    bananas += banana;
            }

            if (mostBananas < bananas)
                mostBananas = bananas;

        }

        return mostBananas;
    }

    private static long NextNumber(long secret)
    {
        var m64 = secret << 6;
        secret = m64 ^ secret;
        secret %= 16777216;
        var d32 = secret >> 5;
        secret = d32 ^ secret;
        secret %= 16777216;
        var m2048 = secret << 11;
        secret = m2048 ^ secret;
        secret %= 16777216;

        return secret;
    }

}

