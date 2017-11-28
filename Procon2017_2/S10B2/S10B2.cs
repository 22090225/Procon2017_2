using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017_2.S10B2
{
    public static class S10B2
    {
        public static void Calculate(ref Coor[] maxStartPosition, ref int[] maxRoute)
        {
            maxStartPosition = new Coor[Field.BallNum];
            int maxpoint = 0;
            int katamuke = 0;

            var startBallPosition = new Coor[Field.BallNum];

            for (int x0 = 0; x0 < Field.Size; x0++)
            {
                for (int y0 = 0; y0 < Field.Size; y0++)
                {
                    for (int x1 = 0; x1 < Field.Size; x1++)
                    {
                        for (int y1 = 0; y1 < Field.Size; y1++)
                        {
                            if (Field.OriginalBoad[x0, y0] == "w" ||
                                Field.OriginalBoad[x1, y1] == "w" ||
                                (x0 == x1 && y0 == y1)
                                )
                            {
                                continue;
                            }
                            startBallPosition[0] = new Coor(x0, y0);
                            startBallPosition[1] = new Coor(x1, y1);
                            var field = new S10B2_O();
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

    }
}
