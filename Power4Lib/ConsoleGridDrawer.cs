using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power4Lib
{
    public static class ConsoleGridDrawer
    {
        public static void DrawGrid(Grid grid)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  ");
            for (int columnNumber = 0; columnNumber < Grid.Width; columnNumber++)
                Console.Write((columnNumber + 1).ToString().Length > 1 ? (columnNumber + 1) + " " : (columnNumber + 1) + "  ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("__");
            for (int i = 0; i < Grid.Width; i++)
                Console.Write(" __");

            Console.WriteLine();

            for (int y = Grid.Height - 1; y >= 0; y--)
            {
                Console.Write('|');
                for (int x = 0; x < Grid.Width; x++)
                {
                    if (grid[x, y] != null)
                    {
                        if (grid[x, y].Winning)
                            Console.ForegroundColor = ConsoleColor.Green;
                        else
                            Console.ForegroundColor = grid[x, y].Color;

                        Console.Write(" " + grid[x, y].ToString() + " ");
                    }
                    else
                        Console.Write(" . ");

                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                Console.Write('|');
                Console.WriteLine();
            }

            Console.Write('|');
            for (int i = 1; i < (Grid.Width * 3) + 2 - 1; i++)
                Console.Write(i % 2 == 0 ? "=" : "-");

            Console.Write("|\n| ");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int columnNumber = 0; columnNumber < Grid.Width; columnNumber++)
                Console.Write(columnNumber == Grid.Width - 1 ? ((columnNumber + 1).ToString().Length > 1 ? columnNumber + 1 + "" : (columnNumber + 1) + " ") : ((columnNumber + 1).ToString().Length > 1 ? (columnNumber + 1) + " " : (columnNumber + 1) + "  "));

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("|\n");

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
