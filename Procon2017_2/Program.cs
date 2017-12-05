using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Procon2017_2.S10B2;

namespace Procon2017_2
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            var calculateStartTime = DateTime.UtcNow;
            // 問題の
            // シード、サイズ、弾の数、壁割合
            // 問題作成
            Console.WriteLine("デバッグバージョンです。");
#endif
            Field.InitializeField();
            Coor[] maxStartPosition = null;
            int[] maxStartRoute = null;
            //場合分け
            // size10 玉2の場合 3/27 全パターン
            //if (Field.Size == 10 && Field.BallNum == 2)
            if (false)
            {
                S10B2.S10B2.Calculate(ref maxStartPosition, ref maxStartRoute);
            }
            else
            {
                Standard.Standard.CreateBoad();

                var startBalls = GenerateStartBalls();
                var model = new Standard.Model(startBalls);
                //初期位置
                for (int i=0; i < 10000; i++)
                {
                    model.CalculateUntilAllOut();
                }

                Write(startBalls, model.Route.ToArray());
#if DEBUG
                //WriteCanOutMap();
#endif
            }

            //Write(maxStartPosition, maxStartRoute);

#if DEBUG

            Console.WriteLine(DateTime.UtcNow.Subtract(calculateStartTime).TotalMilliseconds + "ミリ秒くらい時間がかりました！");
#endif
        }

        static private Coor[] GenerateStartBalls()
        {
            var result = new Coor[Field.BallNum];
            var rnd = new Random();
            var nokoriList = new List<Coor>(Standard.Standard.CanOutList);
            for (int i = 0; i < Field.BallNum; i++)
            {
                var index = rnd.Next(Standard.Standard.CanOutList.Count() - i);
                result[i] = nokoriList[index];
                nokoriList.RemoveAt(index);
            }
            return result;
        }

        static private void Write(Coor[] ballposi, int[] route)
        {
            foreach (var coor in ballposi)
            {
                Console.WriteLine(coor.X + " " + coor.Y);
            }
            foreach (var direction in route)
            {
                Console.WriteLine(direction);
            }
        }

#if DEBUG
        static private void WriteCanOutMap()
        {
            for (int y = 0; y < Field.Size; y++)
            {
                for (int x = 0; x < Field.Size; x++)
                {
                    var node = Standard.Standard.Boad[x, y];
                    if (node.Tile == TileState.Wall)
                    {
                        Console.Write("■");
                    }
                    else if (node.CanOut)
                    {
                        Console.Write("○");
                    }
                    else
                    {
                        Console.Write("☆");
                    }
                }
                Console.Write("\r\n");
            }
        }

        public static void CreateInputs(long seed, int size, int blackOrbCount, double wallRate)
        {

        }
#endif
    }
}
