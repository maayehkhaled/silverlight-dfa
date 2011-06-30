using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilverlightApplication6
{
    /** 
    * doc: Real programmers don't comment their code.  It was hard to write, it
      should be hard to understand and even harder to modify.
    */
    public class Node
    {
        public string nodeLabel;
        public List<Tuple> adjacent ;
        public double x;
        public double y;
        public bool isEnd;
        
        public Node(string label, double x, double y, bool isEnd)
        {
            this.nodeLabel = label;
            this.x = x;
            this.y = y;
            this.isEnd = isEnd;
            this.adjacent = new List<Tuple> ();
        }
    }
}
