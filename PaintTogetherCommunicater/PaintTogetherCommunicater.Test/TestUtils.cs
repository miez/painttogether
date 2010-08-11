using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PaintTogetherCommunicater.Test
{
    public class TestUtils
    {
        /// <returns>2 Socket die miteinander auf localHost verbunden sind</returns>
        public static KeyValuePair<Socket, Socket> CreateLocalSocketConnection(int serverPort)
        {
            // Auf clientverbindung lauschen
            var listener = new TcpListener(IPAddress.Any, serverPort);
            listener.Start();

            // Verbindung zu dem überwachten Port aufbauen
            var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(Dns.GetHostAddresses("192.168.2.44")[0], serverPort);

            // Den durch Connect gestarteten Verbindungsaufbau akzeptieren
            while (!listener.Pending()) { Thread.Sleep(100); }
            var serverSocket = listener.AcceptSocket();

            listener.Stop();

            return new KeyValuePair<Socket, Socket>(serverSocket, clientSocket);
        }
    }
}
