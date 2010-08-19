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
using PaintTogetherServer.Messages.Adapter;
using PaintTogetherServer.Messages.Portal;
using PaintTogetherServer.Messages.Core;

namespace PaintTogetherServer.Contracts
{
    /// <summary>
    /// Funktionale Komponente des PaintTogetherServers.
    /// Verwaltet den Malbereich sowie die beteiligten Personen.
    /// </summary>
    public interface IPtServerCore
    {
        #region Outputpins - siehe Entwurf
        /// <summary>
        /// Informiert über eine neue Lognachricht
        /// </summary>
        event Action<SLogMessage> OnSLog;

        /// <summary>
        /// Beauftragt zum Schließen aller Client-Verbindungen
        /// </summary>
        event Action<DisconnectAllClientsMessage> OnDisconnectAllClients;

        /// <summary>
        /// Beauftragt die Benachrichtung aller Beteiligten über einen neuen
        /// Beteiligten
        /// </summary>
        event Action<NotifyNewClientMessage> OnNotifyNewClient;

        /// <summary>
        /// Beauftrag die noch verbundenen Clients über einen
        /// getrennten Beteiligten zu informieren
        /// </summary>
        event Action<NotifyClientDisconnectedMessage> OnNotifyClientDisconnected;

        /// <summary>
        /// Beauftragt die Initialisierung des Adapters und die Portüberwachung
        /// </summary>
        event Action<InitAdapterMessage> OnInitAdapter;

        /// <summary>
        /// Beauftragt die Benachrichtigung aller Clients über
        /// einen neu bemalten Punkt
        /// </summary>
        event Action<NotifyPaintToClientsMessage> OnNotifyPaint;
        #endregion

        #region Inputpins - siehe Entwurf
        /// <summary>
        /// Verarbeitet die Aufforderung den Server zu beenden
        /// </summary>
        /// <param name="message"></param>
        void ProcessCloseMessage(CloseMessage message);

        /// <summary>
        /// Verarbeitet die Startaufforderung für die Initialisierung des Servers
        /// </summary>
        /// <param name="message"></param>
        void ProcessStartServerMessage(StartServerMessage message);

        /// <summary>
        /// Verarbeitet einen neu verbundenen Beteiligten
        /// </summary>
        /// <param name="message"></param>
        void ProcessNewClientMessage(NewClientConnectedMessage message);

        /// <summary>
        /// Verarbeitet einen nicht mehr beteiligten Nutzer
        /// </summary>
        /// <param name="message"></param>
        void ProcessClientDisconnectedMessage(ClientDisconnectedMessage message);

        /// <summary>
        /// Setzt im Result der Anfrage die Informationen über die aktuellen
        /// Beteiligten 
        /// </summary>
        /// <param name="request"></param>
        void ProcessGetCurrentPainterRequest(GetCurrentPainterRequest request);

        /// <summary>
        /// Setzt den aktuellen Malbereich im aktuellen Zustand in
        /// das Result der Anfrage
        /// </summary>
        /// <param name="request"></param>
        void ProcessGetCurrentPaintContentRequest(GetCurrentPaintContentRequest request);

        /// <summary>
        /// Verarbeitet die Anfrage zum Malen eines Beteiligten
        /// </summary>
        /// <param name="message"></param>
        void ProcessClientPainted(ClientPaintedMessage message);
        #endregion
    }
}
