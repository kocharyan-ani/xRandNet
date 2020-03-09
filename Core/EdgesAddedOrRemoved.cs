using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public struct EdgesAddedOrRemoved
    {
        public EdgesAddedOrRemoved(int vertex1, int vertex2, bool added)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Added = added;
        }
        public int Vertex1 { get; set; }
        public int Vertex2 { get; set; }
        public bool Added { get; set; }
    }
}
