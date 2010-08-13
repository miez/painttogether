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
using PaintTogetherClient.Messages.Portal;

namespace PaintTogetherClient.Contracts
{
    /// <summary>
    /// Funktionale EBC der PaintTogether-Clientanwendung
    /// </summary>
    public interface IPtClientCore
    {
        #region Outputpins - siehe Entwurf
        /// <summary>
        /// Aufforderung das Portal zu initialisieren
        /// </summary>
        event Action<InitPortalMessage> OnInitPortal;

        /// <summary>
        /// Information das jemand einen Punkt bemalt hat
        /// </summary>
        event Action<PaintedMessage> OnPainted;

        /// <summary>
        /// Aufforderung eine Verbindung zum Server aufzubauen
        /// </summary>
        event Action<ConnectToServerRequest> OnRequestConnectToServer;

        /// <summary>
        /// Information das Anwender einen Punkt bemalen möchte
        /// </summary>
        event Action<NewPaintMessage> OnNewPaint;

        // Bei den folgenden 4 Pins ist im Entwurf (ClientCore als Platine) zu sehen,
        // dass Sie einfach nur die Nachricht von einen eigenen
        // Inputpin entgegen nehmen - siehe die letzten 4 Inputpins
        #region Durchgeschlatete Outputpins
        /// <summary>
        /// Informiert über den Verlust der Serververbindung
        /// </summary>
        event Action<ServerClosedMessage> OnServerClosed;

        /// <summary>
        /// Informiert über neuen Beteiligten
        /// </summary>
        event Action<AddAliasMessage> OnAddAlias;

        /// <summary>
        /// Informiert über das Verlassen eines bisher aktiven Beteiligten
        /// </summary>
        event Action<RemoveAliasMessage> OnRemoveAlias;

        /// <summary>
        /// Aufforderung das die Verbindung zum Server beendet werden soll
        /// </summary>
        event Action<CloseMessage> OnCloseConnection;
        #endregion
        #endregion

        #region Inputpins - siehe Entwurf
        /// <summary>
        /// Bearbeitet die Anfrage des Anwenders, einen Punkt zu bemalen
        /// </summary>
        /// <param name="message"></param>
        void ProcessPaintSelfMessage(PaintSelfMessage message);

        /// <summary>
        /// Verarbeitet die Information über den Alias des Serverstarters
        /// </summary>
        /// <param name="message"></param>
        void ProcessConnectedMessage(ConnectedMessage message);

        /// <summary>
        /// Verarbeitet den Auftrag den aktuellen Malstand in einer Datei zu speichern
        /// </summary>
        /// <param name="request"></param>
        void ProcessTakePictureRequest(TakePictureRequest request);

        /// <summary>
        /// Verarbeitet die Information mit dem initialen Malstand
        /// </summary>
        /// <param name="message"></param>
        void ProcessCurrentPaintContentMessage(CurrentPaintContentMessage message);

        /// <summary>
        /// Verarbeitet die Information, das ein Beteiligter einen Punkt bemalt hat
        /// </summary>
        /// <param name="message"></param>
        void ProcessAliasPaintedMessage(AliasPaintedMessage message);

        // Bei den folgenden 4 Pins ist im Entwurf (ClientCore als Platine) zu sehen,
        // dass Sie einfach nur die Nachricht von einen eigenen
        // Outputpin erhalten - siehe die letzten 4 Outputpins
        #region Durchgeschlatete Inputpins
        /// <summary>
        /// Informiert den Anwender über den Verlust der Serververbindung
        /// </summary>
        /// <param name="message"></param>
        void ProcessServerConLostMessage(ServerConnectionLostMessage message);

        /// <summary>
        /// Fügt einen neuen Beteiligten in der GUI-Liste hinzu
        /// </summary>
        /// <param name="message"></param>
        void ProcessNewAliasMessage(NewAliasMessage message);

        /// <summary>
        /// Entfernt einen Beteiligten aus der GUI-Liste
        /// </summary>
        /// <param name="message"></param>
        void ProcessAliasDisconnectedMessage(AliasDisconnectedMessage message);

        /// <summary>
        /// Beantragt die Beendigung der Serververbindung
        /// </summary>
        /// <param name="message"></param>
        void ProcessCloseMessage(CloseMessage message);
        #endregion
        #endregion
    }
}
