using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Power4Lib;

namespace Puissance_4
{
    class Program
    {
        static void Main()
        {
            int playCount = 0;

            bool playAgain = false;

            Power4Game game = null;
            Player player1 = null,
                player2 = null;

            do
            {
                Console.WriteLine("Puissance 4 version console par Paul Wacquet (27 février 2020)\nMode 2 joueurs");

                playCount++;
                Console.WriteLine(playCount + (playCount != 1 ? "ème" : "ère") + " partie\n");

                if (!playAgain) // When first play
                {
                    Console.Write("Pseudo du joueur 1 : ");
                    string player1Name = Console.ReadLine();
                    Console.Write("Pseudo du joueur 2 : ");
                    string player2Name = Console.ReadLine();

                    player1 = new Player(1, !string.IsNullOrEmpty(player1Name) ? player1Name : "1", new Pawn('O') { Color = ConsoleColor.Red });
                    player2 = new Player(2, !string.IsNullOrEmpty(player2Name) ? player2Name : "2", new Pawn('O') { Color = ConsoleColor.Yellow });

                    game = new Power4Game(
                        player1, player2,
                        () => OnPlayerPlay(player1),
                        () => OnPlayerPlay(player2),
                        OnPlayerInputException,
                        OnPlayerWin);
                }
                else
                {
                    Console.WriteLine("Joueur {0} prêt\nJoueur {1} prêt", player1.Name, player2.Name);
                    Console.WriteLine("\nAppuyez sur une touche pour commencer...");
                    if (Console.ReadKey().Key == ConsoleKey.D)
                    { // easter egg :p
                        Console.ForegroundColor = ConsoleColor.Green;
                        Random rnd = new Random();
                        for (int i = 0; i < 1000; i++)
                        {
                            Console.Write((char)rnd.Next(32, 127));
                            System.Threading.Thread.Sleep(1);
                        }
                        Console.WriteLine("\nCertes.");
                        string readLine = Console.ReadLine();
                        Grid.Width = Math.Abs(int.Parse(readLine.Split(';')[0]));
                        Grid.Height = Math.Abs(int.Parse(readLine.Split(';')[1]));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                Console.Clear();

                game.StartPlaying();
            }
            while (playAgain);

            int OnPlayerPlay(Player player)
            {
                int result;
                bool parsed = false;
                do
                {
                    ConsoleGridDrawer.DrawGrid(game.Grid);
                    Console.Write("Tour du joueur {0}", player.Name);
                    Console.ForegroundColor = player.Pawn.Color;
                    Console.Write(" (pion {0})", player.Pawn.Char);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" [1 - {0}] : ", Grid.Width);
                    parsed = int.TryParse(Console.ReadLine(), out result);
                    Console.Clear();
                    if (!parsed)
                        OnPlayerInputException(new Exception("Champ invalide."));
                }
                while (!parsed);
                return result - 1;
            }

            void OnPlayerInputException(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Impossible de placer le pion:");
                switch (ex)
                {
                    case LocationOutsideGridException _:
                        Console.WriteLine("Il doit être compris entre l'emplacement 1 et {0}.", Grid.Width);
                        break;
                    case FullColumnException fcEx:
                        Console.WriteLine("La colonne de l'emplacement {0} est pleine.", fcEx.ColumnLocation);
                        break;
                    default:
                        Console.WriteLine(ex.Message);
                        break;
                }
                Console.ForegroundColor = ConsoleColor.White;
            }

            void OnPlayerWin(Player player)
            {
                Console.WriteLine("Partie terminé");

                if (player == null)
                    Console.WriteLine("Personne n'a gagné.");
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Le joueur {0} a gagné ! (pion: ", player.Name);
                    Console.ForegroundColor = player.Pawn.Color;
                    Console.Write(player.Pawn.Char);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(")");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                ConsoleGridDrawer.DrawGrid(game.Grid);
                Console.WriteLine("\n...");
                Console.ReadKey();
                string answer;
                do
                {
                    Console.Write("Voulez-vous rejouer ? (O/N) : ");
                    answer = Console.ReadLine();
                }
                while (answer.ToUpper() != "O" && answer.ToUpper() != "N" && !string.IsNullOrEmpty(answer));
                playAgain = answer.ToUpper() != "N";
                Console.Clear();
            }
        }
    }
}
