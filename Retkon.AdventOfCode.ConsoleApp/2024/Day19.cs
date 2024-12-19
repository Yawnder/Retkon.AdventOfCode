using AdventOfCodeSupport;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
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

public class Day19 : AdventBase
{

    private readonly ConsoleHelper consoleHelper = ConsoleHelper.Instance;
    private readonly Chop chop = new Chop([]);
    private readonly Dictionary<int, long> cache = new Dictionary<int, long>();

    protected override object InternalPart1()
    {
        var result = 0;

        var patterns = this.Input.Lines[0].Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        for (int i = 0; i < patterns.Length; i++)
        {
            var patternToCopy = patterns[i];
            var letters = patternToCopy.Distinct();
            var otherPatterns = patterns.Where((v, p) => i != p && v.Length <= patternToCopy.Length && !v.Distinct().Except(letters).Any()).ToArray();

            var canBeDecomposed = Decompose(patternToCopy, otherPatterns);
            if (canBeDecomposed)
            {
                patterns = patterns.Where((v, p) => i != p).ToArray();
                i--;
            }
        }

        for (int i = 2; i < this.Input.Lines.Length; i++)
        {
            var patternToCopy = this.Input.Lines[i];
            var letters = patternToCopy.Distinct();
            var otherPatterns = patterns.Where((v, p) => v.Length <= patternToCopy.Length && !v.Distinct().Except(letters).Any()).ToArray();

            var canBeDecomposed = Decompose(patternToCopy, otherPatterns);
            if (canBeDecomposed)
            {
                result++;
            }

        }

        return result;
    }

    private static bool Decompose(string patternToCopy, string[] availablePatterns)
    {
        for (int i = 0; i < availablePatterns.Length; i++)
        {
            var composingPattern = availablePatterns[i];

            if (!patternToCopy.StartsWith(composingPattern))
            {
                continue;
            }
            else if (patternToCopy == composingPattern)
            {
                return true;
            }

            var newValue = patternToCopy[composingPattern.Length..];

            var letters = newValue.Distinct();
            var otherPatterns = availablePatterns.Where((v, p) => v.Length <= newValue.Length && !v.Distinct().Except(letters).Any()).ToArray();

            if (otherPatterns.Length == 0)
                continue;

            var canBeDecomposed = Decompose(newValue, otherPatterns);

            if (canBeDecomposed)
                return true;
        }

        return false;
    }

    protected override object InternalPart2()
    {
        var sw = Stopwatch.StartNew();
        var patterns = this.Input.Lines[0].Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Order();

        foreach (var pattern in patterns)
        {
            this.chop.Add(pattern.ToArray());
        }

        long result = 0;
        for (int i = 2; i < this.Input.Lines.Length; i++)
        {
            var patternToCopy = this.Input.Lines[i];
            var letters = patternToCopy.Distinct();
            var otherPatterns = patterns.Where((v, p) => v.Length <= patternToCopy.Length && !v.Distinct().Except(letters).Any()).ToArray();

            this.cache.Clear();
            var waysToDecompose = this.Decompose2(patternToCopy.ToArray());
            result += waysToDecompose;
        }
        sw.Stop();
        return result;
    }

    private long Decompose2(Span<char> patternToCopy)
    {
        if (patternToCopy.Length == 0)
            return 1;

        if (this.cache.TryGetValue(patternToCopy.Length, out var knownCount))
            return knownCount;

        var waysToDecompose = new List<char[]>(100);
        this.chop.Chup(ref waysToDecompose, patternToCopy);

        var waysCount = 0L;
        foreach (var way in waysToDecompose)
        {
            waysCount += this.Decompose2(patternToCopy[way.Length..]);
        }

        this.cache[patternToCopy.Length] = waysCount;

        return waysCount;
    }

    private class Chop
    {
        public char[] Parent { get; init; }

        private Dictionary<char, Chop> chops = new Dictionary<char, Chop>();

        public bool IsAnEnd { get; set; }

        public Chop(char[] parent)
        {
            this.Parent = parent;
        }

        public void Add(char[] value)
        {
            if (!this.chops.TryGetValue(value[0], out var chop))
            {
                char[] nextParents = [.. this.Parent, .. value[0..1]];
                chop = new Chop(nextParents);
                this.chops.Add(value[0], chop);
            }

            if (value.Length == 1)
            {
                chop.IsAnEnd = true;
            }
            else
            {
                chop.Add(value[1..]);
            }
        }

        public void Chup(ref List<char[]> results, Span<char> value)
        {
            if (this.IsAnEnd)
                results.Add(this.Parent);

            if (value.Length == 0)
                return;

            var firstLetter = value[0];
            if (this.chops.TryGetValue(firstLetter, out var chop))
            {
                chop.Chup(ref results, value[1..]);
            }
            else
            {
                return;
            }
        }

    }


}

