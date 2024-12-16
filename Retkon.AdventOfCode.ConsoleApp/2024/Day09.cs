using AdventOfCodeSupport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Retkon.AdventOfCode.ConsoleApp._2024;

public class Day09 : AdventBase
{
    protected override object InternalPart1()
    {
        var result = 0L;

        var segments = new List<int?>(this.Input.Text.Length * 9);
        var queue = new Queue<int>();

        var isData = true;
        var fileId = 0;
        var position = 0;
        int segmentLength;

        foreach (var data in this.Input.Text)
        {
            segmentLength = int.Parse(data.ToString());

            if (isData)
            {
                for (int i = 0; i < segmentLength; i++)
                {
                    segments.Add(fileId);
                    position++;
                }
                fileId++;
            }
            else
            {
                for (int i = 0; i < segmentLength; i++)
                {
                    segments.Add(null);
                    queue.Enqueue(position++);
                }
            }

            isData = !isData;
        }

        for (int i = segments.Count - 1; i >= 0; i--)
        {
            if (i < queue.Peek())
                break;

            var value = segments[i];
            if (value != null)
            {
                var newPosition = queue.Dequeue();
                segments[newPosition] = value;
            }

            segments.RemoveAt(i);
        }

        for (int i = 0; i < segments.Count; i++)
        {
            result += i * segments[i]!.Value;
        }

        return result;
    }

    protected override object InternalPart2()
    {
        var result = 0L;

        var segments = new List<int?>(this.Input.Text.Length * 9);

        var fileSegments = new Stack<FileSegment>();
        var freeSegments = new List<FreeSegment>();

        var isData = true;
        var fileId = 0;
        var position = 0;
        int segmentLength;

        foreach (var data in this.Input.Text)
        {
            segmentLength = int.Parse(data.ToString());

            if (isData)
            {
                fileSegments.Push(new FileSegment
                {
                    FileId = fileId,
                    Start = position,
                    Length = segmentLength,
                });

                for (int i = 0; i < segmentLength; i++)
                {
                    segments.Add(fileId);
                    position++;
                }
                fileId++;
            }
            else
            {
                freeSegments.Add(new FreeSegment
                {
                    Start = position,
                    Length = segmentLength,
                });

                for (int i = 0; i < segmentLength; i++)
                {
                    segments.Add(null);
                }

                position += segmentLength;
            }

            isData = !isData;
        }

        while (fileSegments.TryPop(out var fileSegment))
        {
            for (int i = 0; i < freeSegments.Count; i++)
            {
                var freeSegment = freeSegments[i];
                if (fileSegment.Start < freeSegment.Start)
                    break;

                if (freeSegment.Length >= fileSegment.Length)
                {
                    for (int j = 0; j < fileSegment.Length; j++)
                    {
                        segments[freeSegment.Start + j] = fileSegment.FileId;
                        segments[fileSegment.Start + j] = null;
                    }

                    if (freeSegment.Length == fileSegment.Length)
                    {
                        freeSegments.RemoveAt(i);
                    }
                    else
                    {
                        freeSegment.Start += fileSegment.Length;
                        freeSegment.Length -= fileSegment.Length;
                    }
                    break;
                }
            }
        }


        for (int i = 0; i < segments.Count; i++)
        {
            result += i * segments[i] ?? 0;
        }

        return result;
    }

    [DebuggerDisplay("S:{Start},L:{Length}")]
    private class FreeSegment
    {
        public int Start { get; set; }
        public int Length { get; set; }
    }

    [DebuggerDisplay("S:{Start},L:{Length},F:{FileId}")]
    private class FileSegment
    {
        public int FileId { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
    }

}

