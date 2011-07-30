using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Resolvers;
using System;
using System.Windows.Controls;
using System.Diagnostics;

namespace SilverlightApplication6
{
    public class XmlParser
    {
        // schema validation is not supported in silverlight! what an evidence of incapacity!!!
        //private static XmlPreloadedResolver resolver = new XmlPreloadedResolver();
        //private static XmlReaderSettings settings = new XmlReaderSettings();
        //private static bool initialized = false;
        //private static Uri dtd = new Uri("Input.dtd", UriKind.Relative);

        public static List<VisualNode> parse(Stream stream)
        {
            // TODO static initalization block
            //if (!initialized)
            //{
            //    resolver.Add(dtd,
            //        App.GetResourceStream(dtd).Stream);
            //    settings.XmlResolver = resolver;
            //    settings.DtdProcessing = DtdProcessing.Parse;
            //    initialized = true;
            //    Debug.WriteLine("init done");
            //}
            //XElement root = XElement.Load(XmlReader.Create(file.OpenRead(), settings));

            XElement root = XElement.Load(stream);
            IDictionary<string, VisualNode> nodes = new Dictionary<string, VisualNode>();

            foreach (XElement state in root.Elements("States").Elements("State"))
            {
                double x = double.Parse(state.Attribute("x").Value);
                double y = double.Parse(state.Attribute("y").Value);
                bool isStart = false;
                bool isEnd = false;

                if (state.Attribute("start") != null
                    && state.Attribute("start").Value.ToUpper().Equals("Y"))
                {
                    isStart = true;
                }

                if (state.Attribute("accept") != null
                    && state.Attribute("accept").Value.ToUpper().Equals("Y"))
                {
                    isEnd = true;
                }

                VisualNode node = new VisualNode(state.Value, x, y, isStart, isEnd);

                Debug.WriteLine("*** node read: " + node.getLabelText() + ", isStart: " + node.isStartNode + ", isEnd: " + node.isEndNode + ", x: " + node.x + ", y: " + node.y);

                nodes.Add(state.Value, node);
            }

            foreach (XElement transition in root.Elements("Transitions").Elements("Transition"))
            {
                string from = transition.Attribute("from").Value;
                string symbol = transition.Attribute("symbol").Value;
                string to = transition.Attribute("to").Value;

                Debug.WriteLine("*** transition read: from: " + from + ", symbol: " + symbol + ", to: " + to);

                VisualNode fromNode;
                nodes.TryGetValue(from, out fromNode);

                VisualNode toNode;
                nodes.TryGetValue(to, out toNode);

                fromNode.addAdjacenceList(toNode, symbol);
                fromNode.addDstNode(symbol, toNode);

                foreach (Tuple<VisualNode, string> t in fromNode.adjacenceList)
                {
                    Debug.WriteLine("*** added: src node: " + fromNode.getLabelText() + ", dst node: " + ((VisualNode)t.Item1).getLabelText() + ", with: " + t.Item2);
                }
            }

            return new List<VisualNode>(nodes.Values);
        }

        public static List<VisualNode> parse(FileInfo file)
        {
            return parse(file.OpenRead());
        }
    }
}
