
/* $HeadURL: http://ws201075/svn/deg/dotNet/trunk/Pegasus/src/HostApplicationForm.cs $
-----------------------------------------------------------------------------
        (c) by Martin Blankenstein

Dieses Dokument und die hierin enthaltenen Informationen unterliegen
dem Urheberrecht und duerfen ohne die schriftliche Genehmigung des
Herausgebers weder als ganzes noch in Teilen dupliziert oder reproduziert
noch manipuliert werden.

-----------------+------------------------------------------------------------
Version          : $Revision: 450 $
-----------------+------------------------------------------------------------
Last Change      : $LastChangedDate: 2009-02-23 18:26:54 +0100 (Mo, 23 Feb 2009) $
-----------------+------------------------------------------------------------
Last User        : $LastChangedBy: NLBERLIN\mblankenstein $
-----------------+------------------------------------------------------------
Beschreibung     :
-----------------+ 

-----------------+------------------------------------------------------------
Updates          :
-----------------+

$Id: HostApplicationForm.cs 450 2009-02-23 17:26:54Z NLBERLIN\mblankenstein $

*/

using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using NUnit.Framework;
using PaintTogetherCommunicater.Messages;
using PaintTogetherServer.Adapter;
using PaintTogetherServer.Messages.Adapter;
using System.Threading;
using PaintTogetherServer.Contracts;
using PaintTogetherCommunicater.Messages.ClientServerCommunication.Client;
using System;

namespace PaintTogetherServer.Test.PtServerClientAdapterCS
{
    [TestFixture]
    public class IntegrationsTest
    {
        private IPtServerClientAdapter _adapter;
        private int _serverPort;
        private Bitmap _requestedBitMap = new Bitmap(230, 120);
        private KeyValuePair<string, Color>[] _requestedPainter = new KeyValuePair<string, Color>[0];

        [SetUp]
        public void SetUp()
        {
            _serverPort = new Random().Next(20000, 30000);
            _adapter = new PtServerClientAdapter();
            _adapter.ProcessInitAdapterMessage(new InitAdapterMessage { Port = _serverPort });
            _adapter.OnRequestCurPaintContent += request => request.Result = _requestedBitMap;
            _adapter.OnRequestCurPainter += request => request.Result = _requestedPainter;
        }

        [TearDown]
        public void TearDown()
        {
            Thread.Sleep(500);

            _adapter.OnClientDisconnected += message => Assert.True(true) /*Dummyverdrahtung*/;

            // TODO Hier muss später auch der PortListener mit gestoppt werden
            _adapter.ProcessDisconnectAllClientsMessage(new DisconnectAllClientsMessage());
        }

        [Test]
        public void Neue_Clientverbindung_ohne_connectMessage()
        {
            var connectionReceived = false;
            _adapter.OnNewClient += message => connectionReceived = true;

            // Verbindung zu dem überwachten Port aufbauen
            var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(Dns.GetHostAddresses("127.0.0.1")[0], _serverPort);

            // Gesendete Nachrichten muessen auf Clientseite abgerufen, sonst blockiert der Sendevorgang
            var communicater = new PaintTogetherCommunicater.PaintTogetherCommunicater();
            communicater.OnNewMessageReceived += message => Assert.True(true);
            communicater.ProcessStartReceiving(new StartReceivingMessage { ToWatchSoketConnection = clientSocket });

            Thread.Sleep(500);

            // Der einfache Verbindungsaufbau darf noch nicht zur
            // Signalisierung eines neuen Clients führen
            Assert.False(connectionReceived);
        }

        [Test]
        public void Neue_Clientverbindung_mit_connectMessage()
        {
            NewClientConnectedMessage receivedConMessage = null;
            _adapter.OnNewClient += message => receivedConMessage = message;

            var alias = "Herbert";
            var color = Color.FromArgb(3, 1, 242);

            // Verbindung zu dem überwachten Port aufbauen
            var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(Dns.GetHostAddresses("127.0.0.1")[0], _serverPort);

            // Gesendete Nachrichten muessen auf Clientseite abgerufen, sonst blockiert der Sendevorgang
            var communicater = new PaintTogetherCommunicater.PaintTogetherCommunicater();
            communicater.OnNewMessageReceived += message => Assert.True(true);
            communicater.ProcessStartReceiving(new StartReceivingMessage { ToWatchSoketConnection = clientSocket });

            Thread.Sleep(1000);

            // Mit Hilfe des Communicater eine Nachricht über die Verbindung schicken
            var toSendMessage = new ConnectScm { Alias = alias, Color = color };
            communicater.ProcessSendMessage(new SendMessageMessage { SoketConnection = clientSocket, Message = toSendMessage });

            Thread.Sleep(1000);

            // Jetzt müssten wir eine neue Verbinund mit den angegebenen Daten erhalten haben
            Assert.That(receivedConMessage.Alias, Is.EqualTo(alias));
            Assert.That(receivedConMessage.Color, Is.EqualTo(color));
        }
    }
}
