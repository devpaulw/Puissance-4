using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using Power4Lib;

namespace Puissance4_Online_Console
{
    class Puissance4OnlineConsole
    {
        public const int Port = 23106;
        private string _connectionAddress;
        private string _nickname;
        private NetworkStream _nstm;
        private IFormatter _formatter;

        public Puissance4OnlineConsole()
        {
            _formatter = new BinaryFormatter();
        }

        //int OnPlay()
        //{
        //    int result;
        //    bool parsed = false;
        //    do
        //    {
        //        ConsoleGridDrawer.DrawGrid(_grid);
        //        Console.Write("A votre tour");
        //        Console.ForegroundColor = _myPlayer.Pawn.Color;
        //        Console.Write(" (pion {0})", _myPlayer.Pawn.Char);
        //        Console.ForegroundColor = ConsoleColor.White;
        //        Console.Write(" [1 - {0}] : ", Grid.Width);
        //        parsed = int.TryParse(Console.ReadLine(), out result);
        //        Console.Clear();
        //        //if (!parsed)!
        //        //    OnPlayerInputException(new Exception("Champ invalide."));
        //    }
        //    while (!parsed);
        //    return result - 1;
        //}

        //int OnConnectedPlayerPlay()
        //{
        //    _formatter.Serialize(_nstm, _grid);

        //    Grid opposingPlayer = (Player)_formatter.Deserialize(_nstm);
        //}

        private void PlayGame(NetworkOpponentClient nOpponent) // TODO Put in another class like Gameplay
        {

        }

        private void CreateGame()
        {
            Player myPlayer = new Player(1, _nickname, new Pawn('O') { Color = ConsoleColor.Red });

            Console.Write("Adresse locale de partage : ");
#if RELEASE
            string ipAddrStr = Console.ReadLine();
#else
            string ipAddrStr = "127.0.0.1";
            Console.WriteLine();
#endif

            NetworkOpponentClient nc = NetworkOpponentClient.HostCreate(ipAddrStr);

            string opposingNickname = nc.ExchangeInitInfos(_nickname);

            //try
            //{
            //    IPAddress ipAddr = IPAddress.Parse(ipAddrStr);
            //    TcpListener listener = new TcpListener(ipAddr, Port);
            //    listener.Start();

            //    Debug.WriteLine("Server running...");
            //    Debug.WriteLine("Local End point: " + listener.LocalEndpoint);

            //    Console.WriteLine("En attente du joueur adverse...");

            //    Socket socket = listener.AcceptSocket();
            //    Debug.WriteLine("Une connexion a été accepté depuis " + socket.RemoteEndPoint);

            //    listener.Stop();

            //    _nstm = new NetworkStream(socket);

            //    // Getting opposing player object
            //    Player opposingPlayer = (Player)_formatter.Deserialize(_nstm);
            //    Console.WriteLine($"Vous êtes contre le joueur {opposingPlayer.Name}");

            //    // Sending my player object
            //    _formatter.Serialize(_nstm, myPlayer);

            //    Console.WriteLine("La partie va pouvoir commencer");

            //    PlayGame(myPlayer, opposingPlayer);

            //    socket.Close();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Error: " + e.StackTrace);
            //}
        }

        private void JoinGame()
        {
            IFormatter formatter = new BinaryFormatter();
            Player myPlayer = new Player(0, _nickname, new Pawn('O') { Color = ConsoleColor.Yellow });

            Console.Write("Adresse du joueur hôte : ");
#if RELEASE
            string ipAddr = Console.ReadLine();
#else
            string ipAddr = "127.0.0.1";
            Console.WriteLine();
#endif

            try
            {
                //TcpClient client = new TcpClient();
                //Console.WriteLine("Connexion en cours...");

                //client.Connect(ipAddr, OnlineServer.Port);
                //Console.WriteLine("Connecté à l'adresse " + ipAddr);

                ////Grid p4GridTest = new Grid();
                ////p4GridTest.LayCircle(new Pawn('X') { Color = ConsoleColor.Red }, 2);

                
                //NetworkStream stm = client.GetStream();
                //formatter.Serialize(stm, myPlayer);

                //Player opposingPlayer = (Player)formatter.Deserialize(stm);

                //Console.WriteLine($"Vous êtes contre le joueur {opposingPlayer.Name}");

                //Console.WriteLine("La partie va pouvoir commencer");

                //PlayGame(myPlayer, opposingPlayer);

                ////byte[] ack = new byte[3];
                ////int k = stm.Read(ack, 0, 100);

                ////if (Encoding.ASCII.GetString(ack) == "RES")
                ////    Debug.WriteLine("Le message a bien été envoyé.");

                //client.Close();
                //stm.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur :" + e.StackTrace);
            }
        }

        public void LaunchGame()
        {
            Console.WriteLine("Puissance 4 - Version Online (console). Réalisé par Paul Wacquet en janvier 2021.\n");

            // Getting player infos
            Console.Write("Votre pseudonyme : ");
            string nickname = Console.ReadLine();
            _nickname = nickname;

            // Choosing mode
            bool choosing = true;
            while (choosing)
            {
                Console.WriteLine("Voulez-vous...\n1. Rejoindre une partie\n2. Créer une partie\n3. Quitter");
                Console.Write("Choix : ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        choosing = false;
                        JoinGame();
                        break;
                    case "2":
                        choosing = false;
                        CreateGame();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Choix invalide.\n");
                        break;
                }
            }
        }

        //void StartServer(string ipAddress)
        //{
        //    var serv = new OnlineServer();
        //    Thread thread = new Thread(() => serv.Start(ipAddress));
        //    thread.Start();

        //}
    }
}
