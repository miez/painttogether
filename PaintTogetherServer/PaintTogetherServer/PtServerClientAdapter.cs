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

using System;
using PaintTogetherServer.Contracts;
using PaintTogetherServer.Messages.Adapter;
using PaintTogetherServer.Contracts.Adapter;
using PaintTogetherServer.Adapter;
using PaintTogetherCommunicater.Contracts;
using PaintTogetherServer.Messages.Adapter.ConnectionManager;

namespace PaintTogetherServer
{
    /// <summary>
    /// ServerClientAdapter des PaintTogetherServers.
    /// Hier werden die Socket-Verbindungen verwaltet,
    /// Nachrichten gesendet und Empfangen
    /// </summary>
    public class PtServerClientAdapter : IPtServerClientAdapter
    {
        #region Outputpins - Kommentare am Interface zu finden
        public event Action<NewClientConnectedMessage> OnNewClient;

        public event Action<ClientDisconnectedMessage> OnClientDisconnected;

        public event Action<GetCurrentPaintContentRequest> OnRequestCurPaintContent;

        public event Action<GetCurrentPainterRequest> OnRequestCurPainter;

        public event Action<ClientPaintedMessage> OnClientPainted;
        #endregion

        #region Die 3 internen EBCs, aus denen sich die EBC PtServerClientAdapter zusammensetzt
        /// <summary>
        /// Die PortListenerEBC überwacht den Serverport und liefert alle eingehenden
        /// Verbindungen
        /// </summary>
        private readonly IPtPortListener _portListener = new PtPortListener();

        /// <summary>
        /// Die ConnectionMananger-EBC verwaltet alle Socket-Connections und
        /// veranlasst das Versenden und Überwachen von Sockets.
        /// </summary>
        private readonly IPtConnectionMananger _connectionMananger = new PtConnectionMananger();

        /// <summary>
        /// Die Communicater-EBC schickt Nachrichten an die gewünschten Socket-Verbindungen
        /// und informiert über neu eingegangenen Nachrichten bei überwachten
        /// Socket-Verbindungen. Intern wandelt sie die zu verschickenden und zu empfangenden
        /// Nachrichten in XML und dieses in Bytes für die Byte-Übertragung zwischen
        /// 2 Sockets um.
        /// </summary>
        private readonly IPaintTogetherCommunicater _communicater = new PaintTogetherCommunicater.PaintTogetherCommunicater();
        #endregion

        /// <summary>
        /// Erstellt die EBC mit den internen EBCs, welche dann verdrahted werden
        /// </summary>
        public PtServerClientAdapter()
        {
            // Die Inputpins werden wie bei jeder Platine direkt in der Inputpin-Methode
            // auf die inneren EBCs weitergeleitet (siehe Input-Pin-Methoden)
            // --
            // Outputpins der PtServerClientAdapter-Platine mit den entsprechenden
            // Outputpins der internen EBCs verbinden
            _connectionMananger.OnClientDisconnected += message => OnClientDisconnected(message);
            _connectionMananger.OnClientPainted += message => OnClientPainted(message);
            _connectionMananger.OnNewClient += message => OnNewClient(message);
            _connectionMananger.OnRequestCurPaintContent += request => OnRequestCurPaintContent(request);
            _connectionMananger.OnRequestCurPainter += request => OnRequestCurPainter(request);
            // --
            // Jetzt müssen noch alle nicht verbundenen Input und Outputpins
            // der internen EBCs miteinander verdrahtet werden
            _portListener.OnNewConnection += message => _connectionMananger.ProcessNewConnectionMessage(message);
            _connectionMananger.OnSendMessage += message => _communicater.ProcessSendMessage(message);
            _connectionMananger.OnStartReceiving += message => _communicater.ProcessStartReceiving(message);
            _connectionMananger.OnStopReceiving += message => _communicater.ProcessStopReceiving(message);
            _communicater.OnConLost += message => _connectionMananger.ProcessConLostMessage(message);
            _communicater.OnNewMessageReceived += message => _connectionMananger.ProcessNewMessageMessage(message);
            // --
            // Die Verdrahtung ist jetzt abgeschlossen
        }

        #region Inputpins - Kommentare am Interface zu finden
        public void ProcessDisconnectAllClientsMessage(DisconnectAllClientsMessage message)
        {
            _connectionMananger.ProcessDisconnectAllClientsMessage(message);
        }

        public void ProcessNotifyNewClientMessage(NotifyNewClientMessage message)
        {
            _connectionMananger.ProcessNotifyNewClientMessage(message);
        }

        public void ProcessNotifyClientDisconnectedMessage(NotifyClientDisconnectedMessage message)
        {
            _connectionMananger.ProcessNotifyClientDisconnectedMessage(message);
        }

        public void ProcessNotifyPaintMessage(NotifyPaintToClientsMessage message)
        {
            _connectionMananger.ProcessNotifyPaintMessage(message);
        }

        public void ProcessInitAdapterMessage(InitAdapterMessage message)
        {
            // Hier muss der Inputpin die Verarbeitung auf zwei interne EBCs verteilen
            var initConManagerMessage = new InitConnectionManagerMessage();
            initConManagerMessage.Alias = message.Alias;
            _connectionMananger.ProcessInitConnectionManagerMessage(initConManagerMessage);

            var portStartMessage = new StartPortListingMessage();
            portStartMessage.Port = message.Port;
            _portListener.ProcessStartPortListingMessage(portStartMessage);
        }
        #endregion
    }
}
