using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power4Lib
{
    public class Grid
    {
        private readonly Pawn[,] pawns = new Pawn[Width, Height];

        public static int Width = 7;
        public static int Height = 6;

        public Pawn this[int indexX, int indexY] {
            get {
                if (!IsOutside(indexX, indexY))
                    return pawns[indexX, indexY];
                else
                    throw new LocationOutsideGridException("The location of the pawn was outside the grid.");
            }
        }

        public bool IsOutside(int indexX, int indexY)
            => !(indexX < Width && indexX >= 0 && indexY < Height && indexY >= 0);

        public bool IsFull()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (pawns[x, y] == null)
                        return false;
                }
            }

            return true;
        }

        internal void LayCircle(Pawn pawn, int location)
        {
            for (int i = 0; i <= Height; i++)
            {
                if (i == Height)
                    throw new FullColumnException(string.Format("Can't place the pawn at location {0}, it's full.", location)) { ColumnLocation = location };

                if (this[location, i] == null)
                {
                    pawns[location, i] = pawn;
                    break;
                }
            }
        }
    }
}
