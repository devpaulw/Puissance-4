using Power4Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Puissance4_Online_Console
{
    class NetworkOpponentClient : IDisposable
    {
        public const int Port = 23106;
        const string _NicknameHeader = "NCK";
        const char _Separator = (char)3;

        NetworkStream _nstm;
        StreamWriter _sw;
        StreamReader _sr;
        Socket _socket;

        public string Nickname { get; private set; }

        protected NetworkOpponentClient(Socket socket)
        {
            _socket = socket;
            _nstm = new NetworkStream(socket);
            _sw = new StreamWriter(_nstm);
            _sr = new StreamReader(_nstm);
        }

        public void ExchangeInitInfos(string myNickname)
        {
            // Sending my nickname
            Send(_NicknameHeader, myNickname);

            // Getting opponent nickname
            string nickname = Receive(_NicknameHeader).First();
            Nickname = nickname;

            Console.WriteLine("La partie va pouvoir commencer");
        }

        public void Dispose()
        {
            _nstm.Dispose();
            _socket.Close();
            _socket.Dispose();
        }

        private string[] Receive(string headerToCheck) =>
            GetValuesFromPacket(_sr.ReadLine(), headerToCheck);

        private void Send(string header, params string[] values) => 
            _sw.WriteLine(GetPacketFromValues(header, values));

        private static string GetPacketFromValues(string header, string[] values)
        {
            return header + _Separator + string.Join(_Separator, values);
        }

        private static string[] GetValuesFromPacket(string packet, string headerToCheck)
        {
            string[] splittedPacket = packet.Split(_Separator);

            string header = splittedPacket[0];
            if (header != headerToCheck)
                throw new Exception("The expected header was wrong.");

            return splittedPacket.Skip(1).ToArray();
        }

        /// <summary>
        /// Creates a NetworkClient by being a host
        /// </summary>
        /// <returns></returns>
        public static NetworkOpponentClient HostCreate(string localAddress, string myNickname)
        {
            IPAddress ipAddr = IPAddress.Parse(localAddress);

            TcpListener listener = new TcpListener(ipAddr, Port);
            listener.Start();

            Debug.WriteLine("Server running...");
            Debug.WriteLine("Local End point: " + listener.LocalEndpoint);

            Console.WriteLine("En attente du joueur adverse...");

            Socket socket = listener.AcceptSocket();
            Debug.WriteLine("Une connexion a été accepté depuis " + socket.RemoteEndPoint);

            listener.Stop(); // No needing to listen for others players anymore since we got it

            var nc = new NetworkOpponentClient(socket);
            nc.ExchangeInitInfos(myNickname);

            return nc;
        }

        public static NetworkOpponentClient JoinHost(string hostAddress, string myNickname)
        {
            TcpClient client = new TcpClient();
            Console.WriteLine("Connexion en cours...");

            client.Connect(hostAddress, Port);
            Console.WriteLine("Connecté à l'adresse " + hostAddress);

            var nc = new NetworkOpponentClient(client.Client);
            nc.ExchangeInitInfos(myNickname);

            return nc;
        }
    }
}
