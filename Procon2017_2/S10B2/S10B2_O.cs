using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Procon2017_2.Standard;

namespace Procon2017_2.S10B2
{
    public class S10B2_O
    {
        public int[] MaxRoute { get; set; }

        public int MaxPoint { get; set; }

        public DateTime CalculateStartTime { get; set; }

        public S10B2_O(DateTime calculateStaetTIme)
        {
            MaxRoute = new int[0];
            CalculateStartTime = calculateStaetTIme;
        }

        public void CalculateAllRoute(Coor[] startBallPosition)
        {
            var originalBoad = new Tile[10][];
            var boad = new BoadState[Field.Size, Field.Size];
            var points = new int[3];
            var balls = new Ball[Field.BallNum];
            var Route = new List<int>();
            //初期位置設定
            for (int i = 0; i < Field.BallNum; i++)
            {
                balls[i] = new Ball() { Coor = startBallPosition[i] };

                var startTile = Field.OriginalBoad[startBallPosition[i].X, startBallPosition[i].Y];
                balls[i].Points[(int)startTile] = 1;
                boad[startBallPosition[i].X, startBallPosition[i].Y].IsPassed = true;
            }
            CoverAllIncline(-1, boad, points, balls, Route);
        }

        public void CoverAllIncline(int lastIncline, BoadState[,] lastBoad, int[] lastPoints, Ball[] lastBalls, List<int> lastRoute)
        {
            if (DateTime.UtcNow.Subtract(CalculateStartTime).TotalMilliseconds > 9000)
            {
                return;
            }
            //全ての玉が外に出たら最大値を比較して、終了
            if (lastBalls.FirstOrDefault(b => b.IsOut == false) == null)
            {
                var tmpPointSum = lastPoints.Sum();
                if (tmpPointSum > MaxPoint)
                {
                    MaxPoint = tmpPointSum;
                    MaxRoute = lastRoute.ToArray();
                }
                return;
            }

            var currentBoad = new BoadState[Field.Size, Field.Size];

            var currentPoints = new int[3];

            var currentBalls = new Ball[Field.BallNum];

            List<int> currentRoute = null;

            CopyDataToCurrent(lastBoad, lastPoints, lastBalls, lastRoute, currentBoad, currentPoints, currentBalls, ref currentRoute);
            //前回移動した方向を抜いた3方向
            var dirList = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                if (i != lastIncline)
                {
                    dirList.Add(i);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                var randomIndex = Field.Rnd.Next(3-i);
                var direction = dirList[randomIndex];
                dirList.RemoveAt(randomIndex);
                //前回と同じ操作はしない
                //if (direction == lastIncline)
                //{
                //    continue;
                //}
                //1回傾ける ポイントがある場合
                var onekatamuke = InclineField(direction, currentBoad, currentPoints, currentBalls, currentRoute);
                var fukurokojinashi = true;
                foreach (var ball in currentBalls)
                {
                    if (!Standard.Standard.Boad[ball.Coor.X, ball.Coor.Y].CanOut)
                    {
                        break;
                    }
                }
                //かつ、袋小路に移動しない場合
                if (onekatamuke &&
                    fukurokojinashi
                    )
                {

                    //次へ
                    CoverAllIncline(direction, currentBoad, currentPoints, currentBalls, currentRoute);
                    //終わったら元に戻して別の方向へ
                    CopyDataToCurrent(lastBoad, lastPoints, lastBalls, lastRoute, currentBoad, currentPoints, currentBalls, ref currentRoute);

                }
                //ポイントが無い場合、元に戻して別方向へ
                else
                {
                    //全方向動かした場合は何もせず終了(重複ループ)
                    if (i == 3 )
                    {
                        return;
                    }
                    //元に戻す
                    CopyDataToCurrent(lastBoad, lastPoints, lastBalls, lastRoute, currentBoad, currentPoints, currentBalls, ref currentRoute);
                    continue;
                }

            }
            //全パターン回ったら終了
            //無限ループ？起きないはず
            return;
        }

        /// <summary>
        /// ベクトルの方向に1回傾け
        /// ポイントが増える移動をした場合はtrue
        /// </summary>
        /// <param name="vector"></param>
        public bool InclineField(int vector, BoadState[,] boad, int[] points, Ball[] balls, List<int> route)
        {
            var includesPoints = false;
            switch (vector)
            {
                case 0:
                    balls = balls.OrderBy(b => b.Coor.X).ToArray();
                    break;
                case 1:
                    balls = balls.OrderBy(b => b.Coor.Y).ToArray();
                    break;
                case 2:
                    balls = balls.OrderByDescending(b => b.Coor.X).ToArray();
                    break;
                case 3:
                    balls = balls.OrderByDescending(b => b.Coor.Y).ToArray();
                    break;
            }

            for (int i = 0; i < Field.BallNum; i++)
            {
                if (balls[i].IsOut)
                {
                    continue;
                }
                boad[balls[i].Coor.X, balls[i].Coor.Y].IsBall = false;
                //壁にぶつかるかボールにぶつかるか外に出るまで移動
                while (true)
                {
                    var next = AddVector(balls[i].Coor, MoveOne(vector));

                    if (IsOut(next, vector))
                    {

                        //外に出た場合、ポイントを加算して終了
                        balls[i].IsOut = true;
                        var endStatus = Field.Ends[vector][vector % 2 == 0 ? next.Y : next.X];
                        points[(int)endStatus] += balls[i].Points[(int)endStatus];
                        includesPoints = true;

                        break;
                    }
                    else if (Field.OriginalBoad[next.X, next.Y] == TileState.Wall || boad[next.X, next.Y].IsBall)
                    {
                        boad[balls[i].Coor.X, balls[i].Coor.Y].IsBall = true;
                        break;
                    }
                    else
                    {
                        if (!boad[next.X, next.Y].IsPassed)
                        {
                            balls[i].Points[(int)Field.OriginalBoad[next.X, next.Y]]++;
                            boad[next.X, next.Y].IsPassed = true;
                            includesPoints = true;
                        }
                        balls[i].Coor = next;

                    }
                }
            }

            route.Add(vector);

            return includesPoints;

        }

        //lastの値をcurrentにコピーします
        private void CopyDataToCurrent(BoadState[,] lastBoad, int[] lastPoints, Ball[] lastBalls, List<int> lastRoute, BoadState[,] currentBoad, int[] currentPoints, Ball[] currentBalls, ref List<int> currentRoute)
        {
            for (int x = 0; x < Field.Size; x++)
            {
                for (int y = 0; y < Field.Size; y++)
                {
                    currentBoad[x, y] = lastBoad[x, y];
                }
            }
            for (int i = 0; i < 3; i++)
            {
                currentPoints[i] = lastPoints[i];
            }
            for (int i = 0; i < Field.BallNum; i++)
            {
                currentBalls[i] = new Ball()
                {
                    Coor = lastBalls[i].Coor,
                    IsOut = lastBalls[i].IsOut
                };
                for (int j = 0; j < 3; j++)
                {
                    currentBalls[i].Points[j] = lastBalls[i].Points[j];
                }
            }
            currentRoute = new List<int>(lastRoute);
        }

        private Coor AddVector(Coor coor1, Coor coor2)
        {
            return new Coor(coor1.X + coor2.X, coor1.Y + coor2.Y);
        }
        private Coor MoveOne(int vector)
        {
            switch (vector)
            {
                case 0:
                    return new Coor(-1, 0);
                case 1:
                    return new Coor(0, -1);
                case 2:
                    return new Coor(1, 0);
                case 3:
                    return new Coor(0, 1);
                default:
                    throw new Exception("存在しないベクトルです");
            }
        }

        private bool IsOut(Coor coor, int vector)
        {
            switch (vector)
            {
                case 0:
                    return coor.X == -1;
                case 1:
                    return coor.Y == -1;
                case 2:
                    return coor.X == Field.Size;
                case 3:
                    return coor.Y == Field.Size;
                default:
                    throw new Exception("存在しないベクトルです");
            }
        }

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

        public class Tile
        {
            public int X;
            public int Y;
            public TileState State;
            //public bool IsPassed;

            public enum TileState
            {
                Red,
                Blue,
                Green,
                Wall,
                Ball
            }
        }

    }
}
