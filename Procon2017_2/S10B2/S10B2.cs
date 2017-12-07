using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017_2.S10B2
{
    public static class S10B2
    {
        //玉2個 10×10の場合
        public static void Calculate(ref Coor[] maxStartPosition, ref int[] maxRoute, DateTime calculateStartTime)
        {
            maxStartPosition = new Coor[Field.BallNum];
            int maxpoint = 0;
            int katamuke = 0;

            var startBallPosition = new Coor[Field.BallNum];

            for (int x0 = 0; x0 < Field.Size; x0++)
            {
                for (int y0 = 0; y0 < Field.Size; y0++)
                {
                    //袋小路は除外
                    if (Field.OriginalBoad[x0, y0] == TileState.Wall ||
                        !Standard.Standard.Boad[x0, y0].CanOut
                        )
                    {
                        continue;
                    }
                    for (int x1 = 0; x1 < Field.Size; x1++)
                    {
                        for (int y1 = 0; y1 < Field.Size; y1++)
                        {
                            //袋小路は除外
                            if (Field.OriginalBoad[x1, y1] == TileState.Wall ||
                                (x0 == x1 && y0 == y1) ||
                                !Standard.Standard.Boad[x1, y1].CanOut
                                )
                            {
                                continue;
                            }
                            startBallPosition[0] = new Coor(x0, y0);
                            startBallPosition[1] = new Coor(x1, y1);
                            var field = new S10B2_O(calculateStartTime);
                            field.CalculateAllRoute(startBallPosition);
                            if (field.MaxPoint > maxpoint || (field.MaxPoint == maxpoint && field.MaxRoute.Length > katamuke))
                            {
                                maxpoint = field.MaxPoint;
                                maxStartPosition[0] = startBallPosition[0];
                                maxStartPosition[1] = startBallPosition[1];
                                maxRoute = field.MaxRoute;
                                katamuke = field.MaxRoute.Length;
                            }
                        }
                    }
                }
            }
        }

        //それ以外は初期位置ランダム
        public static void CalculateRandom(ref Coor[] maxStartPosition, ref int[] maxRoute, DateTime calculateStartTime)
        {
            maxStartPosition = new Coor[Field.BallNum];
            int maxpoint = 0;
            int katamuke = 0;

            var startBallPosition = new Coor[Field.BallNum];

#if DEBUG
            var i = 0;
#endif
            while (DateTime.UtcNow.Subtract(calculateStartTime).TotalMilliseconds < 9000)
            {
#if DEBUG
                i++;
#endif
                //壁と袋小路を除外したランダムな被らない初期位置
                startBallPosition = GenerateRandomStartBalls();

                var field = new S10B2_O(calculateStartTime);
                field.CalculateAllRoute(startBallPosition);
                if (field.MaxPoint > maxpoint || (field.MaxPoint == maxpoint && field.MaxRoute.Length > katamuke))
                {
                    maxpoint = field.MaxPoint;
                    maxStartPosition = startBallPosition;
                    maxRoute = field.MaxRoute;
                    katamuke = field.MaxRoute.Length;
                }
            }
#if DEBUG
            Console.WriteLine(i + "回繰り返し");
#endif
        }

        //玉一個で適当に闊歩
        public static void CalculateRandomSingle(ref Coor[] maxStartPosition, ref int[] maxRoute, DateTime calculateStartTime)
        {
            //初期位置
            maxStartPosition = GenerateRandomStartBalls();
            var start = maxStartPosition[0];
            var passedNode = new bool[Field.Size, Field.Size];
            var route = new List<int>();
            passedNode[start.X, start.Y] = true;
            //9秒間は外に出ない
            var i = 0;
            while (i < 10000)
            //while (DateTime.UtcNow.Subtract(calculateStartTime).TotalMilliseconds < 9000)
            {
                i++;
                // 自分
                var startNode = Standard.Standard.Boad[start.X, start.Y];
                // 移動できる方向,移動したことのない方向を探す
                var can = new List<int>();
                var nonpassed = new List<int>();
                for (int dir = 0; dir < 4; dir++)
                {
                    var nextNode = startNode.Next[dir];
                    if (nextNode != startNode && nextNode != null && nextNode.CanOut)
                    {
                        can.Add(dir);

                        if (!passedNode[nextNode.Coor.X, nextNode.Coor.Y])
                        {
                            nonpassed.Add(dir);
                        }
                    }
                }

                //移動する方向を決める
                var direction = 0;
                // 移動したことのない方向がある場合
                if (nonpassed.Count() != 0)
                {
                    if (nonpassed.Count() == 1)
                    {
                        //一個なら次はそこ
                        direction = nonpassed.First();

                    }
                    //そうでないならランダムに順番を決める
                    direction = nonpassed[Field.Rnd.Next(nonpassed.Count())];
                    //次の準備して終了
                    route.Add(direction);
                    start = startNode.Next[direction].Coor;
                    passedNode[start.X, start.Y] = true;
                    continue;
                }
                else
                {
                    //ない場合
                    if (can.Count() == 1)
                    {
                        //一個なら次はそこ
                        direction = can.First();

                    }
                    //そうでないならランダムに順番を決める 無いはずはない
                    direction = can[Field.Rnd.Next(can.Count())];
                    //次の準備して終了
                    route.Add(direction);
                    start = startNode.Next[direction].Coor;
                    passedNode[start.X, start.Y] = true;
                    continue;
                }
            }
            //最後適当に外に出る
            while (true)
            {
                // 自分
                var startNode = Standard.Standard.Boad[start.X, start.Y];
                // 外に出る方向、移動できる方向,移動したことのない方向を探す
                var can = new List<int>();
                var nonpassed = new List<int>();
                for (int dir = 0; dir < 4; dir++)
                {
                    var nextNode = startNode.Next[dir];
                    // 外に出れたら終了
                    if (nextNode != startNode && nextNode == null)
                    {
                        route.Add(dir);
                        maxRoute = route.ToArray();
                        return;
                    }
                    if (nextNode != startNode && nextNode != null && nextNode.CanOut)
                    {
                        can.Add(dir);

                        if (!passedNode[nextNode.Coor.X, nextNode.Coor.Y])
                        {
                            nonpassed.Add(dir);
                        }
                    }
                }

                //移動する方向を決める
                var direction = 0;
                // 移動したことのない方向がある場合
                if (nonpassed.Count() != 0)
                {
                    if (nonpassed.Count() == 1)
                    {
                        //一個なら次はそこ
                        direction = nonpassed.First();

                    }
                    //そうでないならランダムに順番を決める
                    direction = nonpassed[Field.Rnd.Next(nonpassed.Count())];
                    //次の準備して終了
                    route.Add(direction);
                    start = startNode.Next[direction].Coor;
                    passedNode[start.X, start.Y] = true;
                    continue;
                }
                else
                {
                    //ない場合
                    if (can.Count() == 1)
                    {
                        //一個なら次はそこ
                        direction = can.First();

                    }
                    //そうでないならランダムに順番を決める 無いはずはない
                    direction = can[Field.Rnd.Next(can.Count())];
                    //次の準備して終了
                    route.Add(direction);
                    start = startNode.Next[direction].Coor;
                    passedNode[start.X, start.Y] = true;
                    continue;
                }
            }
        }

        private static Coor Next(Coor coor)
        {
            if (coor.X == Field.Size - 1)
            {
                if (coor.Y == Field.Size - 1)
                {
                    return new Coor(0, 0);
                }
                return new Coor(0, coor.Y + 1);
            }
            coor.X++;
            return coor;
        }

        private static Coor[] GenerateRandomStartBalls()
        {
            var result = new Coor[Field.BallNum];
            var nokoriList = new List<Coor>(Standard.Standard.CanOutList);
            for (int i = 0; i < Field.BallNum; i++)
            {
                var index = Field.Rnd.Next(Standard.Standard.CanOutList.Count() - i);
                result[i] = nokoriList[index];
                nokoriList.RemoveAt(index);
            }
            return result;
        }
    }
}
