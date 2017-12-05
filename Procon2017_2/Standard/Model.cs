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
        public IEnumerable<Coor> Balls;
        public List<int> Route;
        public Model(IEnumerable<Coor> startBalls)
        {
            Balls = startBalls.ToList();
            Route = new List<int>();
        }

        public bool CalculateUntilAllOut()
        {
            var rnd = new Random();
            while (Balls.Count() > 0)
            {
                //さっきと違う方向
                int dir;
                if (Route.Count() == 0)
                {
                    dir = rnd.Next(4);
                }
                else
                {
                    dir = (Route.Last() + rnd.Next(3)) % 4;
                }

                var katamukeOKFrag = true;
                // 全部canoutになる方向を探す。
                for (int i = 0; i < 4; i++)
                {
                    var nextdir = (dir + i) % 4;
                    if (Route.Count() != 0 && nextdir == Route.Last())
                    {
                        continue;
                    }
                    var nextBalls = OneKatamuke(Balls, nextdir);
                    foreach (var coor in nextBalls)
                    {
                        if (!Standard.Boad[coor.X, coor.Y].CanOut)
                        {
                            katamukeOKFrag = false;
                            break;
                        }
                    }
                    if (katamukeOKFrag)
                    {
                        Route.Add(nextdir);
                        Balls = nextBalls;
                        break;
                    }
                }
                if (!katamukeOKFrag)
                {
                    throw new Exception("つみパターンです");
                }
            }
            return true;

        }

        public static IEnumerable<Coor> OneKatamuke(IEnumerable<Coor> balls, int direction)
        {
            return balls
                .Select(b => Standard.Boad[b.X, b.Y].Next[direction])
                .GroupBy(next => next)
                .SelectMany(g =>
                {
                    var count = g.Count(next => next != null);
                    var list = new List<Coor>();
                    for (int i = 0; i < count; i++)
                    {
                        if (i == 0)
                        {
                            list.Add(g.Key.Coor);
                        }
                        else
                        {
                            list.Add(Standard.OneStep(list[i - 1], (direction + 2) % 4));
                        }
                    }
                    return list;
                });
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
