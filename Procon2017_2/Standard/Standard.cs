using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2017_2.Standard
{
    public class Standard
    {
        public static Node[,] Boad;
        public static List<Coor> CanOutList;

        public static void CreateBoad()
        {
            Boad = new Node[Field.Size, Field.Size];
            CanOutList= new List<Coor>();

            //初期化
            for (int x = 0; x < Field.Size; x++)
            {
                for (int y = 0; y < Field.Size; y++)
                {
                    Boad[x, y] = new Node(new Coor(x, y), Field.OriginalBoad[x, y]);
                }
            }

            //対応付け(node初期化)
            for (int x = 0; x < Field.Size; x++)
            {
                for (int y = 0; y < Field.Size; y++)
                {
                    var coor = new Coor(x, y);
                    var targetTile = Boad[coor.X, coor.Y];
                    for (int direction = 0; direction < 4; direction++)
                    {
                        var nextTile = MoveToStop(coor, direction);
                        if (nextTile != null)
                        {
                            targetTile.Next[direction] = Boad[nextTile.Value.X, nextTile.Value.Y];
                        }
                    }
                }
            }

            //外に出られるノードを探す
            var count = 1;
            while (count != 0)
            {
                count = 0;
                for (int x = 0; x < Field.Size; x++)
                {
                    for (int y = 0; y < Field.Size; y++)
                    {
                        // まだCanOutになっていなくて、CanOutに移動できるものはTrue
                        if (Field.OriginalBoad[x, y] != TileState.Wall && !Boad[x, y].CanOut && (Boad[x, y].Next.Contains(null) || Boad[x, y].Next.Any(n => n.CanOut)))
                        {
                            Boad[x, y].CanOut = true;
                            count++;
                        }
                    }
                }
#if DEBUG
                Console.WriteLine("count=" + count);
#endif
            }

            // 外に出られるノードのリスト
            for (int x = 0; x < Field.Size; x++)
            {
                for (int y = 0; y < Field.Size; y++)
                {
                    if (Field.OriginalBoad[x, y] != TileState.Wall &&
                        Boad[x, y].CanOut
                        )
                    {
                        CanOutList.Add(new Coor(x, y));
                    }
                }
            }
        }

        private static Coor? MoveToStop(Coor start, int direction)
        {
            var next = start;
            Coor last;
            while (true)
            {
                last = next;
                next = OneStep(last, direction);
                var canMove = (CanMove(next));
                if (canMove == null)
                {
                    return null;
                }
                else if (canMove == false)
                {
                    return last;
                }
            }
        }
        public static Coor OneStep(Coor start, int direction)
        {
            switch (direction)
            {
                case 0:
                    return new Coor(start.X - 1, start.Y);
                case 1:
                    return new Coor(start.X, start.Y - 1);
                case 2:
                    return new Coor(start.X + 1, start.Y);
                case 3:
                    return new Coor(start.X, start.Y + 1);
                default:
                    throw new Exception("存在しないベクトルです");
            }
        }
        private static bool? CanMove(Coor coor)
        {
            return (coor.X == -1 || coor.Y == -1 || coor.X == Field.Size || coor.Y == Field.Size)
                ? (bool?)null
                : Field.OriginalBoad[coor.X, coor.Y] != TileState.Wall;
        }
    }
}
