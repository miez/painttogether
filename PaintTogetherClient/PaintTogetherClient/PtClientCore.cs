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
using PaintTogetherClient.Contracts;
using PaintTogetherClient.Messages.Adapter;
using PaintTogetherClient.Messages.Core.ClientStarter;
using PaintTogetherClient.Messages.Portal;
using PaintTogetherClient.Contracts.Core;
using PaintTogetherClient.Core;

namespace PaintTogetherClient
{
    /// <summary>
    /// Funktionale EBC der PaintTogether-Clientanwendung
    /// </summary>
    public class PtClientCore : IPtClientCore
    {
        #region Outputpins - Kommentare am Interface
        public event Action<InitPortalMessage> OnInitPortal;

        public event Action<PaintedMessage> OnPainted;

        public event Action<ConnectToServerRequest> OnRequestConnectToServer;

        public event Action<NewPaintMessage> OnNewPaint;

        // Bei den folgenden 4 Pins ist im Entwurf (ClientCore als Platine) zu sehen,
        // dass Sie einfach nur die Nachricht von einen eigenen
        // Inputpin entgegen nehmen - siehe die letzten 4 Inputpins
        #region Durchgeschlatete Outputpins
        public event Action<ServerClosedMessage> OnServerClosed;

        public event Action<AddAliasMessage> OnAddAlias;

        public event Action<RemoveAliasMessage> OnRemoveAlias;

        public event Action<CloseConnectionMessage> OnCloseConnection;
        #endregion
        #endregion

        #region Die 3 internen EBCs, aus denen sich die EBC PtClientCore zusammensetzt
        /// <summary>
        /// EBC zum initialen Start des Clients
        /// </summary>
        private readonly IPtClientStarter _clientStarter = new PtClientStarter();

        /// <summary>
        /// EBC zur Verwaltung des Malbereichs
        /// </summary>
        private readonly IPtPaintContentManager _paintContentManager = new PtPaintContentManager();

        /// <summary>
        /// EBC zum Speichern des aktuellen Malbereichs in einer Datei
        /// </summary>
        private readonly IPtPictureTaker _pictureTaker = new PtPictureTaker();
        #endregion

        /// <summary>
        /// Erstellt die EBC mit den internen EBCs, welche dann verdrahted werden
        /// </summary>
        public PtClientCore()
        {
            // Die Inputpins werden wie bei jeder Platine direkt in der Inputpin-Methode
            // auf die inneren EBCs weitergeleitet (siehe Input-Pin-Methoden)
            // --
            // Outputpins der PtClientCore-Platine mit den entsprechenden
            // Outputpins der internen EBCs verbinden
            _clientStarter.OnInitPortal += message => OnInitPortal(message);
            _clientStarter.OnRequestConnectToServer += request => OnRequestConnectToServer(request);
            _paintContentManager.OnNewPaint += message => OnNewPaint(message);
            _paintContentManager.OnPainted += message => OnPainted(message);
            // --
            // Jetzt müssen noch alle nicht verbundenen Input und Outputpins
            // der internen EBCs miteinander verdrahtet werden
            _clientStarter.OnInitPaintManager += message => _paintContentManager.ProcessInitPaintMananger(message);
            _pictureTaker.OnRequestPaintContent += request => _paintContentManager.ProcessGetPaintContentRequest(request);
            // --
            // Die Verdrahtung ist jetzt abgeschlossen
        }

        #region Inputpins - Kommentare am Interface
        public void ProcessPaintSelfMessage(PaintSelfMessage message)
        {
            _paintContentManager.ProcessPaintSelfMessage(message);
        }

        public void ProcessConnectedMessage(ConnectedMessage message)
        {
            _clientStarter.ProcessConnectedMessage(message);
        }

        public void ProcessTakePictureRequest(TakePictureRequest request)
        {
            _pictureTaker.ProcessTakePictureRequest(request);
        }

        public void ProcessCurrentPaintContentMessage(CurrentPaintContentMessage message)
        {
            _clientStarter.ProcessCurrentPaintContentMessage(message);
        }

        public void ProcessAliasPaintedMessage(AliasPaintedMessage message)
        {
            _paintContentManager.ProcessAliasPaintedMessage(message);
        }

        public void ProcessStartClientRequest(StartClientRequest request)
        {
            _clientStarter.ProcessStartClientRequest(request);
        }

        // Bei den folgenden 4 Pins ist im Entwurf (ClientCore als Platine) zu sehen,
        // dass Sie einfach nur die Nachricht von einen eigenen
        // Outputpin erhalten - siehe die letzten 4 Outputpins
        #region Durchgeschlatete Inputpins
        public void ProcessServerConLostMessage(ServerConnectionLostMessage message)
        {
            OnServerClosed(new ServerClosedMessage());
        }

        public void ProcessNewAliasMessage(NewAliasMessage message)
        {
            OnAddAlias(new AddAliasMessage { Alias = message.Alias, Color = message.Color });
        }

        public void ProcessAliasDisconnectedMessage(AliasDisconnectedMessage message)
        {
            OnRemoveAlias(new RemoveAliasMessage() { Alias = message.Alias, Color = message.Color });
        }

        public void ProcessCloseMessage(CloseMessage message)
        {
            OnCloseConnection(new CloseConnectionMessage());
        }
        #endregion
        #endregion
    }
}
