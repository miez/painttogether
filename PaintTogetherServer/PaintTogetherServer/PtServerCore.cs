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
using PaintTogetherServer.Contracts.Core;
using PaintTogetherServer.Messages.Adapter;
using PaintTogetherServer.Messages.Portal;
using PaintTogetherServer.Messages.Core;
using PaintTogetherServer.Core;

namespace PaintTogetherServer.Contracts
{
    /// <summary>
    /// Funktionale Komponente des PaintTogetherServers.
    /// Verwaltet den Malbereich sowie die beteiligten Personen.
    /// </summary>
    public class PtServerCore : IPtServerCore
    {
        #region Outputpins - Kommentare am Interface zu finden
        public event Action<SLogMessage> OnSLog;

        public event Action<DisconnectAllClientsMessage> OnDisconnectAllClients;

        public event Action<NotifyNewClientMessage> OnNotifyNewClient;

        public event Action<NotifyClientDisconnectedMessage> OnNotifyClientDisconnected;

        public event Action<StartPortListingMessage> OnStartPortListing;

        public event Action<NotifyPaintToClientsMessage> OnNotifyPaint;
        #endregion

        #region Die 4 internen EBCs, aus denen sich die EBC PtServerCore zusammensetzt
        /// <summary>
        /// LoggerEBC für Erstellen von Lognachrichten aus allen log4net-Nachrichten
        /// </summary>
        private readonly IPtLogger _logger = new PtLogger();

        /// <summary>
        /// EBC zur Verwaltung aller an der Malerei beteiligten Personen
        /// </summary>
        private readonly IPtPlayerListManager _playerListManager = new PtPlayerListManager();

        /// <summary>
        /// EBC zur Verwaltung des Malbereichs
        /// </summary>
        private readonly IPtPaintFieldManager _fieldManager = new PtPaintFieldManager();

        /// <summary>
        /// EBC zum Initialisieren des Servers
        /// </summary>
        private readonly IPtServerStarter _serverStarter = new PtServerStarter();
        #endregion

        /// <summary>
        /// Erstellt die EBC mit den internen EBCs, welche dann verdrahted werden
        /// </summary>
        public PtServerCore()
        {
            // Die Inputpins werden wie bei jeder Platine direkt in der Inputpin-Methode
            // auf die inneren EBCs weitergeleitet (siehe Input-Pin-Methoden)
            // --
            // Outputpins der PtServerCore-Platine mit den entsprechenden
            // Outputpins der internen EBCs verbinden
            _playerListManager.OnNotifyClientDisconnected += message => OnNotifyClientDisconnected(message);
            _playerListManager.OnNotifyNewClient += message => OnNotifyNewClient(message);
            _logger.OnSLog += message => OnSLog(message);
            _fieldManager.OnNotifyPaint += message => OnNotifyPaint(message);
            _serverStarter.OnStartPortListing += message => OnStartPortListing(message);
            // --
            // Jetzt müssen noch alle nicht verbundenen Input und Outputpins
            // der internen EBCs miteinander verdrahtet werden
            _serverStarter.OnInit += message => _fieldManager.ProcessInitMessage(message);
            // --
            // Die Verdrahtung ist jetzt abgeschlossen
        }

        #region Inputpins - Kommentare am Interface zu finden
        public void ProcessCloseMessage(CloseMessage message)
        {
            OnDisconnectAllClients(new DisconnectAllClientsMessage());
        }

        public void ProcessStartServerMessage(StartServerMessage message)
        {
            _serverStarter.ProcessStartServerMessage(message);
        }

        public void ProcessNewClientMessage(NewClientConnectedMessage message)
        {
            _playerListManager.ProcessNewClientMessage(message);
        }

        public void ProcessClientDisconnectedMessage(ClientDisconnectedMessage message)
        {
            _playerListManager.ProcessClientDisconnectedMessage(message);
        }

        public void ProcessGetCurrentPainterRequest(GetCurrentPainterRequest request)
        {
            _playerListManager.ProcessGetCurrentPainterRequest(request);
        }

        public void ProcessGetCurrentPaintContentRequest(GetCurrentPaintContentRequest request)
        {
            _fieldManager.ProcessGetCurrentPaintContentRequest(request);
        }

        public void ProcessClientPainted(ClientPaintedMessage message)
        {
            _fieldManager.ProcessClientPainted(message);
        }
        #endregion
    }
}
