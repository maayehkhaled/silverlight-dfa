using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SilverlightApplication6
{
    class Node
    {
        public String nodeLabel;
        public List<Tuple> adjacent = new List<Tuple>();
        double x;
        double y;
        bool isEnd;
    }
}
