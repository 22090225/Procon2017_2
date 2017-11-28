using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017_2.Standard
{
    public class Node
    {
        public string Tile;
        public Coor Coor;
        public Node[] Next;
        public bool CanOut;

        public Node(Coor coor,string tile) {
            this.Tile = tile;
            this.Coor = coor;
            this.Next = new Node[4];
        }
    }
}
