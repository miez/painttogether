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
using PaintTogetherClient.Messages.Adapter;
using PaintTogetherClient.Contracts;
using PaintTogetherCommunicater.Contracts;
using PaintTogetherClient.Contracts.Adapter;
using PaintTogetherClient.Adapter;

namespace PaintTogetherClient
{
    /// <summary>
    /// ClientServerAdapter. Verwaltet die Socket-Verbindung
    /// zu einem PaintTogetherServer und tauscht über
    /// diese Nachrichten mit dem Server aus
    /// </summary>
    public class PtClientServerAdapter : IPtClientServerAdapter
    {
        #region Outputpins - Kommentare sieh Interface
        public event Action<CurrentPaintContentMessage> OnCurrentPaintContent;

        public event Action<AliasPaintedMessage> OnAliasPainted;

        public event Action<ServerConnectionLostMessage> OnServerConnectionLost;

        public event Action<NewAliasMessage> OnNewAlias;

        public event Action<ConnectedMessage> OnConnected;

        public event Action<AliasDisconnectedMessage> OnAliasDisconnected;
        #endregion

        #region Die 3 internen EBCs, aus denen sich die EBC PtClientCore zusammensetzt
        /// <summary>
        /// Die Communicater-EBC zur Kommunikation mit zwischen Client und Server
        /// </summary>
        private readonly IPaintTogetherCommunicater _communicater = new PaintTogetherCommunicater.PaintTogetherCommunicater();

        /// <summary>
        /// EBC zur Initialisierung des Adapters und Aufbau der Verbindung zum Server
        /// </summary>
        private readonly IPtClientAdapterStarter _adapterStarter = new PtClientAdapterStarter();

        /// <summary>
        /// EBC zur Verwaltung der Serversocket-Verbindung
        /// </summary>
        private readonly IPtServerConnectionManager _conMananger = new PtServerConnectionManager();
        #endregion

        /// <summary>
        /// Erstellt die EBC mit den internen EBCs, welche dann verdrahted werden
        /// </summary>
        public PtClientServerAdapter()
        {
            // Die Inputpins werden wie bei jeder Platine direkt in der Inputpin-Methode
            // auf die inneren EBCs weitergeleitet (siehe Input-Pin-Methoden)
            // --
            // Outputpins der PtClientServerAdapter-Platine mit den entsprechenden
            // Outputpins der internen EBCs verbinden
            _conMananger.OnAliasDisconnected += message => OnAliasDisconnected(message);
            _conMananger.OnAliasPainted += message => OnAliasPainted(message);
            _conMananger.OnConnected += message => OnConnected(message);
            _conMananger.OnCurrentPaintContent += message => OnCurrentPaintContent(message);
            _conMananger.OnNewAlias += message => OnNewAlias(message);
            _conMananger.OnServerConnectionLost += message => OnServerConnectionLost(message);
            // --
            // Jetzt müssen noch alle nicht verbundenen Input und Outputpins
            // der internen EBCs miteinander verdrahtet werden
            _conMananger.OnSendMessage += message => _communicater.ProcessSendMessage(message);
            _conMananger.OnStartReceiving += message => _communicater.ProcessStartReceiving(message);
            _conMananger.OnStopReceiving += message => _communicater.ProcessStopReceiving(message);
            _communicater.OnConLost += message => _conMananger.ProcessConLostMessage(message);
            _communicater.OnNewMessageReceived += message => _conMananger.ProcessNewMessageMessage(message);
            _adapterStarter.OnConEstablished += message => _conMananger.ProcessConEstablishedMessage(message);
            // --
            // Die Verdrahtung ist jetzt abgeschlossen
        }

        #region Inpupins - Kommentare siehe Interface
        public void ProcessConnectToServerRequest(ConnectToServerRequest request)
        {
            _adapterStarter.ProcessConnectToServerRequest(request);
        }

        public void ProcessNewPaintMessage(NewPaintMessage message)
        {
            _conMananger.ProcessNewPaintMessage(message);
        }

        public void ProcessCloseConnectionMessage(CloseConnectionMessage message)
        {
            _conMananger.ProcessCloseConnectionMessage(message);
        }
        #endregion
    }
}
