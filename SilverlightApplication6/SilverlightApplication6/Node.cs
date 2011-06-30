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
    class Node
    {
        public String nodeLabel;
        public List<Tuple> adjacent ;
        double x;
        double y;
        bool isEnd;
        
        public Node(String label, double x, double y, bool isEnd)
        {
            this.nodeLabel = label;
            this.x = x;
            this.y = y;
            this.isEnd = isEnd;
            this.adjacent = new List<Tuple> ();
        }
    }
}
