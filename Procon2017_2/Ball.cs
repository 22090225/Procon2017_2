using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017_2
{
    public class Ball
    {
        public Coor Coor { get; set; }
        public int[] Points { get; set; }
        public bool IsOut { get; set; }
        public Ball()
        {
            this.Points = new int[3];
        }
    }
}
