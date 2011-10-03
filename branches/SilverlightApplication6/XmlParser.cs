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
    public static class XmlParser
    {
        // schema validation is not supported in silverlight! what an evidence of incapacity!!!
        //private static XmlPreloadedResolver resolver = new XmlPreloadedResolver();
        //private static XmlReaderSettings settings = new XmlReaderSettings();
        //private static bool initialized = false;
        //private static Uri dtd = new Uri("Input.dtd", UriKind.Relative);

        public static List<VisualAnimationNode> parse(Stream stream, out List<string> inputAlphabet)
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
            IDictionary<string, VisualAnimationNode> nodes = new Dictionary<string, VisualAnimationNode>();
            inputAlphabet = new List<string>();

            foreach (XElement symbol in root.Elements("InputAlphabet").Elements("Symbol"))
            {
                inputAlphabet.Add(symbol.Value);
            }

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

                VisualAnimationNode node = new VisualAnimationNode(state.Value, new EPoint(x, y), isStart, isEnd);

                Debug.WriteLine("*** node read: " + node.getLabelText() + ", isStart: " + node.isStartNode + ", isEnd: " + node.isEndNode + ", x: " + node.location.x + ", y: " + node.location.y);

                nodes.Add(state.Value, node);
            }

            foreach (XElement transition in root.Elements("Transitions").Elements("Transition"))
            {
                string from = transition.Attribute("from").Value;
                string symbol = transition.Attribute("symbol").Value;
                string to = transition.Attribute("to").Value;

                Debug.WriteLine("*** transition read: from: " + from + ", symbol: " + symbol + ", to: " + to);

                VisualAnimationNode fromNode;
                nodes.TryGetValue(from, out fromNode);

                VisualAnimationNode toNode;
                nodes.TryGetValue(to, out toNode);

                if (!symbol.Contains("|"))
                {
                    fromNode.addAdjacenceList(toNode, symbol);
                }
                else
                {
                    string[] symbolS = symbol.Split('|');
                    for (int i = 0; i < symbolS.Length; i++) {
                        fromNode.addAdjacenceList(toNode, symbolS[i]);
                    }
                }

                
                //fromNode.addDstNode(symbol, toNode);

                //foreach (Tuple<VisualNode, string> t in fromNode.adjacenceList)
                //{
                //    Debug.WriteLine("*** added: src node: " + fromNode.getLabelText() + ", dst node: " + ((VisualNode)t.Item1).getLabelText() + ", with: " + t.Item2);
                //}
            }

            return new List<VisualAnimationNode>(nodes.Values);
        }

        public static List<VisualAnimationNode> parse(FileInfo file, out List<string> inputAlphabet)
        {
            return parse(file.OpenRead(), out inputAlphabet);
        }
    }
}
