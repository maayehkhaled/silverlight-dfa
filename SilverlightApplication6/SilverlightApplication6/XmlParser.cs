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

        public static List<Node> parse(FileInfo file)
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

            XElement root = XElement.Load(file.OpenRead());
            IDictionary<string, Node> nodes = new Dictionary<string, Node>();

            foreach (XElement state in root.Elements("States").Elements("State"))
            {
                double x = double.Parse(state.Attribute("x").Value);
                double y = double.Parse(state.Attribute("y").Value);
                bool isEnd = false;

                if (state.Attribute("accept") != null
                    && state.Attribute("accept").Value.ToUpper().Equals("Y"))
                {
                    isEnd = true;
                }

                Node node = new Node(state.Value, x, y, isEnd);

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

                fromNode.addAdjacent(toNode, symbol);
            }

            return new List<Node>(nodes.Values);
        }
    }
}
