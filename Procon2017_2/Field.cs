using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017_2
{
    public class Field
    {
        public static int Size;
        public static int BallNum;
        public static string[][] Ends;
        public static string[,] OriginalBoad;

        public static void InitializeField()
        {
            Size = int.Parse(Console.In.ReadLine());
            BallNum = int.Parse(Console.In.ReadLine());

            OriginalBoad = new string[Size, Size];
            Ends = new string[4][];
            for (int i = 0; i < Size; i++)
            {
                var line = (Console.In.ReadLine()).Split(' ')
                    .ToArray();
                for (int j = 0; j < Size; j++)
                {
                    OriginalBoad[j, i] = line[j];
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Ends[i] = (Console.In.ReadLine()).Split(' ');
            }
        }

    }
}
