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
        public static TileState[][] Ends;
        public static TileState[,] OriginalBoad;

        public static void InitializeField()
        {
            Size = int.Parse(Console.In.ReadLine());
            BallNum = int.Parse(Console.In.ReadLine());

            OriginalBoad = new TileState[Size, Size];
            Ends = new TileState[4][];
            for (int i = 0; i < Size; i++)
            {
                var line = (Console.In.ReadLine()).Split(' ')
                    .ToArray();
                for (int j = 0; j < Size; j++)
                {
                    OriginalBoad[j, i] = ConvertCharToState(line[j]);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                Ends[i] = (Console.In.ReadLine()).Split(' ').Select(c => ConvertCharToState(c)).ToArray();
            }
        }

        //public static int CalculatePoints(Coor[] startBalls, int[] directions)
        //{
        //    var boad = new BoadState[Field.Size, Field.Size];
        //    var points = new int[3];
        //    var balls = new Ball[Field.BallNum];
        //    var Route = new List<int>();

        //    //初期位置設定
        //    for (int i = 0; i < Field.BallNum; i++)
        //    {
        //        balls[i] = new Ball() { Coor = startBalls[i] };

        //        var startTile = ConvertCharToState(Field.OriginalBoad[startBallPosition[i].X, startBallPosition[i].Y]);
        //        balls[i].Points[(int)startTile] = 1;
        //        boad[startBallPosition[i].X, startBallPosition[i].Y].IsPassed = true;
        //    }
        //}

        static private TileState ConvertCharToState(string c)
        {
            switch (c)
            {
                case "r":
                    return TileState.Red;
                case "b":
                    return TileState.Blue;
                case "g":
                    return TileState.Green;
                case "w":
                    return TileState.Wall;
                default:
                    throw new Exception("[" + c + "]" + "は存在しないカラーです");
            }
        }

    }
}
