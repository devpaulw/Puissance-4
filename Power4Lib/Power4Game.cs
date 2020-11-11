using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Power4Lib
{
    public class Power4Game
    {

        Player Player1 { get; }
        Player Player2 { get; }
        Func<int> Player1Play { get; }
        Func<int> Player2Play { get; }
        Action<Exception> PlayerInputException { get; }
        Action<Player> PlayerWin { get; }

        public Grid Grid { get; private set; }

        public Power4Game(Player player1,
            Player player2,
            Func<int> player1Play,
            Func<int> player2Play,
            Action<Exception> playerInputException,
            Action<Player> playerWin)
        {
            Player1 = player1 ?? throw new ArgumentNullException(nameof(player1));
            Player2 = player2 ?? throw new ArgumentNullException(nameof(player2));
            Player1Play = player1Play ?? throw new ArgumentNullException(nameof(player1Play));
            Player2Play = player2Play ?? throw new ArgumentNullException(nameof(player2Play));
            PlayerInputException = playerInputException ?? throw new ArgumentNullException(nameof(playerInputException));
            PlayerWin = playerWin ?? throw new ArgumentNullException(nameof(playerWin));
        }

        public void StartPlaying()
        {
            Grid = new Grid();

            Tuple<Player, Func<int>>[] playingPlayers = new[]
            {
                new Tuple<Player, Func<int>>(Player1, Player1Play),
                new Tuple<Player, Func<int>>(Player2, Player2Play),
            };

            bool playing = true;

            while (playing)
            {
                foreach (var playingPlayer in playingPlayers)
                {
                    while (true)
                    {
                        try
                        {
                            Grid.LayCircle(playingPlayer.Item1.Pawn.Clone() as Pawn, playingPlayer.Item2.Invoke());
                        }
                        catch (Exception ex)
                        {
                            PlayerInputException(ex);
                            continue;
                        }

                        break;
                    }

                    Player potentialWinner = CheckForAWinner();
                    if (potentialWinner != null)
                    {
                        PlayerWin(potentialWinner);
                        playing = false;
                        break;
                    }

                    if (Grid.IsFull())
                    {
                        PlayerWin(null);
                        playing = false;
                        break;
                    }
                }
            }
        }

        public Player CheckForAWinner()
        {
            for (int indexX = 0; indexX < Grid.Width; indexX++)
            {
                for (int indexY = 0; indexY < Grid.Height; indexY++)
                {
                    if (Grid[indexX, indexY] == null)
                        continue;

                    int[][][] starMovementsCases = new int[][][]
                    {
                        // Staight
                        new int[][] { new int[] { 0, 1 }, new int[] { 0, 2 }, new int[] { 0, 3 } },
                        new int[][] { new int[] { 0, -1 }, new int[] { 0, -2 }, new int[] { 0, -3 }, },
                        new int[][] { new int[] { 1, 0 }, new int[] { 2, 0 }, new int[] { 3, 0 }, },
                        new int[][] { new int[] { -1, 0 }, new int[] { -2, 0 }, new int[] { -3, 0 }, },
                        // Diagonal
                        new int[][] { new int[] { 1, 1 }, new int[] { 2, 2 }, new int[] { 3, 3 }, },
                        new int[][] { new int[] { -1, -1 }, new int[] { -2, -2 }, new int[] { -3, -3 }, },
                        new int[][] { new int[] { 1, -1 }, new int[] { 2, -2 }, new int[] { 3, -3 }, },
                        new int[][] { new int[] { -1, 1 }, new int[] { -2, 2 }, new int[] { -3, 3 }, },
                    };

                    foreach (int[][] movementCases in starMovementsCases)
                    {
                        bool lined = true;

                        foreach (int[] movementCase in movementCases)
                        {
                            int checkX = indexX + movementCase[0],
                                checkY = indexY + movementCase[1];

                            if (!Grid.IsOutside(checkX, checkY))
                                lined &= Grid[checkX, checkY] == Grid[indexX, indexY];
                            else
                                lined = false;
                        }

                        if (lined)
                        {
                            Grid[indexX, indexY].Winning = true;
                            for (int i = 0; i < movementCases.Length; i++)
                                Grid[indexX + movementCases[i][0], 
                                    indexY + movementCases[i][1]]
                                    .Winning = true;

                            return Player1.Pawn == Grid[indexX, indexY] ? Player1 : Player2;
                        }
                    }
                }
            }

            return null;
        }
    }
}
