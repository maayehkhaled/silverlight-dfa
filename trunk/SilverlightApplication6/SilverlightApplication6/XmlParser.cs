using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System;
using System.Windows.Controls;
using System.Diagnostics;

namespace SilverlightApplication6
{
    public class XmlParser
    {
        // schema validation is not supported in silverlight! what an evidence of incapacity!!!

        public static List<Node> parse(FileInfo file)
        {
            XElement root = XElement.Load(file.OpenRead());
            IDictionary<string, Node> nodes = new Dictionary<string, Node>();

            foreach (XElement state in root.Elements("States").Elements("State"))
            {
                Node node = new Node(state.Value);

                if (state.Attribute("accept") != null
                    && state.Attribute("accept").Value.ToUpper().Equals("Y"))
                {
                    node.isEnd = true;
                }

                nodes.Add(state.Value, node);
            }

            foreach (XElement transition in root.Elements("Transitions").Elements("Transition"))
            {
                string from = transition.Attribute("from").Value;
                string symbol = transition.Attribute("symbol").Value;
                string to = transition.Attribute("to").Value;

                Node fromNode;
                nodes.TryGetValue(from, out fromNode);
                Node toNode;
                nodes.TryGetValue(from, out toNode);

                fromNode.addAdjcent(toNode, symbol);
            }

            return new List<Node>(nodes.Values);
        }
    }
}
