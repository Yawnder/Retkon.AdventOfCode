using AdventOfCodeSupport;
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

public partial class Day23 : AdventBase
{
    private int width;
    private readonly ConsoleHelper consoleHelper = ConsoleHelper.Instance;

    protected override object InternalPart1()
    {
        //var connections = new Dictionary<string, HashSet<string>>();

        var nodes = new Dictionary<string, Node>();
        for (int y = 0; y < this.Input.Lines.Length; y++)
        {
            var nodePair = this.Input.Lines[y].Split("-", StringSplitOptions.RemoveEmptyEntries);

            if (!nodes.TryGetValue(nodePair[0], out var mainNode))
            {
                mainNode = new Node(nodePair[0]);
                nodes.Add(nodePair[0], mainNode);
            }

            if (!nodes.TryGetValue(nodePair[1], out var childNode))
            {
                childNode = new Node(nodePair[1]);
                nodes.Add(nodePair[1], childNode);
            }

            mainNode.Tos.Add(childNode);
            childNode.Tos.Add(mainNode);
        }

        var matching = new List<string>();

        foreach (var node in nodes.Values)
        {
            foreach (var childNode in node.Tos)
            {
                var commonNodes = childNode.Tos.Select(n => n.From)
                    .Intersect(node.Tos.Select(n => n.From))
                    .Select(n => new string[]
                    {
                        node.From,
                        childNode.From,
                        n
                    }.OrderBy(v => v))
                    .Select(vs => string.Join("", vs))
                    .Where(n => n[0] == 't' || n[2] == 't' || n[4] == 't')
                    .Distinct();

                foreach (var commonNode in commonNodes)
                {
                    matching.Add(commonNode);
                }

            }
        }

        return matching.Distinct().Count();
    }

    protected override object InternalPart2()
    {
        var nodes = new Dictionary<string, Node>();
        for (int y = 0; y < this.Input.Lines.Length; y++)
        {
            var nodePair = this.Input.Lines[y].Split("-", StringSplitOptions.RemoveEmptyEntries);

            if (!nodes.TryGetValue(nodePair[0], out var mainNode))
            {
                mainNode = new Node(nodePair[0]);
                mainNode.Tos.Add(mainNode);
                nodes.Add(nodePair[0], mainNode);
            }

            if (!nodes.TryGetValue(nodePair[1], out var childNode))
            {
                childNode = new Node(nodePair[1]);
                childNode.Tos.Add(childNode);
                nodes.Add(nodePair[1], childNode);
            }

            mainNode.Tos.Add(childNode);
            childNode.Tos.Add(mainNode);
        }

        foreach (var node in nodes)
        {
            node.Value.Tos = node.Value.Tos.Distinct().OrderBy(n => n.From).ToList();
        }

        var networks = new List<HashSet<string>>();
        foreach (var node in nodes.Values.OrderByDescending(n => n.Tos.Count))
        {
            var networkQueue = new Queue<List<Node>>();

            var nodeNetworks = new List<List<string>>();

            var nodeTos = new List<Node>(node.Tos);

            networkQueue.Enqueue(nodeTos);

            while (networkQueue.TryDequeue(out var currentNetwork))
            {
                var cummulativeNetwork = new List<Node>();
                foreach (var evaluatedNode in currentNetwork.ToArray())
                {
                    foreach (var evaluatedNodeTo in evaluatedNode.Tos)
                    {
                        var notFriendlyTos = currentNetwork.Except(evaluatedNodeTo.Tos).ToList();

                        if (notFriendlyTos.Any())
                        {
                            var newList = new List<Node>(currentNetwork);

                            newList = newList.Except(notFriendlyTos).ToList();
                            networkQueue.Enqueue(newList);

                            currentNetwork.Remove(evaluatedNodeTo);
                        }
                    }
                }

                if (currentNetwork.Any())
                {
                    networks.Add(new HashSet<string>(currentNetwork.Select(n => n.From)));
                }
            }


        }

        var largestNetwork = networks.OrderByDescending(n => n.Count).First();
        var pw = string.Join(",", largestNetwork.OrderBy(v => v));


        return pw;
    }

    [DebuggerDisplay("{From,nq}")]
    private class Node
    {
        public string From { get; set; }
        public List<Node> Tos { get; set; } = new List<Node>();

        public Node(string from)
        {
            this.From = from;
        }

        public static bool operator ==(Node nodeA, Node nodeB)
        {
            return nodeA.From == nodeB.From;
        }

        public static bool operator !=(Node nodeA, Node nodeB)
        {
            return nodeA.From != nodeB.From;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (obj is not Node node)
            {
                return false;
            }

            return this.From == node.From;

        }

        public override int GetHashCode()
        {
            return this.From.GetHashCode();
        }
    }

}

