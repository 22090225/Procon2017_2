using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017_2.Standard
{
    public class Model
    {
        public Ball[,] BallPlace;
        public List<Ball> Balls;
        public Model(Coor[] startBalls)
        {

            Boad = new BoadState[Field.Size, Field.Size];
            //初期位置設定
            for (int i = 0; i < Field.BallNum; i++)
            {
                balls[i] = new Ball() { Coor = startBalls[i] };

                var startTile = Field.OriginalBoad[startBalls[i].X, startBalls[i].Y];
                balls[i].Points[(int)startTile] = 1;
                boad[startBalls[i].X, startBalls[i].Y].IsPassed = true;
            }
        }

        public static Coor[] OneStep(Coor[] balls, int direction)
        {
            var boad = new int?[Field.Size, Field.Size];
            //var result = new List<Coor?>();
            var nexts = balls
                .Select(b => Standard.Boad[b.X, b.Y])

            var kabutteru = nexts
                .GroupBy(b => b.Coor)
                .Where(g => g.Count() > 1);
                .SelectMany(group =>
                {
                    if (group.Count() == 1)
                    {
                        return group.Key;
                    }
                    return null;
                });

            for (int i = 0; i < balls.Length; i++)
            {
                var next = Standard.Boad[balls[i].X, balls[i].Y];
                //result.Add(next.Coor);
                //被っていた場合
                if (next != null && boad[next.Coor.X, next.Coor.Y] != null)
                {

                }
                boad[next.Coor.X, next.Coor.Y] = i;
            }
            return (Coor[])(result.Where(b => b.HasValue).ToArray());
        }

        private Coor Kabuttatoki(Coor coor,, int direction)
        {

        }

        private Coor OneBack(Coor coor, int direction)
        {
            switch (direction)
            {
                case 0:
                    return new Coor(coor.X + 1, coor.Y);
                case 1:
                    return new Coor(coor.X, coor.Y + 1);
                case 2:
                    return new Coor(coor.X - 1, coor.Y);
                case 3:
                    return new Coor(coor.X, coor.Y - 1);
                default:
                    throw new Exception("存在しないベクトルです");
            }
        }


    }
}
