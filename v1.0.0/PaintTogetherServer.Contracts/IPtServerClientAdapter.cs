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

namespace PaintTogetherServer.Contracts
{
    /// <summary>
    /// ServerClientAdapter des PaintTogetherServers.
    /// Hier werden die Socket-Verbindungen verwaltet,
    /// Nachrichten gesendet und Empfangen
    /// </summary>
    public interface IPtServerClientAdapter
    {
        #region Outputpins - siehe Entwurf
        /// <summary>
        /// Signalisiert einen neuen Beteiligten
        /// </summary>
        event Action<NewClientConnectedMessage> OnNewClient;

        /// <summary>
        /// Signalisiert über den Verlust eines Beteiligten
        /// </summary>
        event Action<ClientDisconnectedMessage> OnClientDisconnected;

        /// <summary>
        /// Erfragt den aktuellen Zustand des Malbereichs
        /// </summary>
        event Action<GetCurrentPaintContentRequest> OnRequestCurPaintContent;

        /// <summary>
        /// Erfragt Informationen über die aktuell beteiligten Personen
        /// </summary>
        event Action<GetCurrentPainterRequest> OnRequestCurPainter;

        /// <summary>
        /// Informiert über eine Malanfrage einer Clientverbindung
        /// </summary>
        event Action<ClientPaintedMessage> OnClientPainted;
        #endregion

        #region Inputpins - siehe Entwurf
        /// <summary>
        /// Bearbeitet die Auffroderung alle Verbindungen zu beenden
        /// </summary>
        /// <param name="message"></param>
        void ProcessDisconnectAllClientsMessage(DisconnectAllClientsMessage message);

        /// <summary>
        /// Verarbeitet die Aufforderung alle aktiven Clients über
        /// einen neuen Beteiligten zu informieren
        /// </summary>
        /// <param name="message"></param>
        void ProcessNotifyNewClientMessage(NotifyNewClientMessage message);

        /// <summary>
        /// Verarbeitet die Aufforderung alle aktiven Clients über
        /// das Verlassen eines bisherigen Beteiligten zu informieren
        /// </summary>
        /// <param name="message"></param>
        void ProcessNotifyClientDisconnectedMessage(NotifyClientDisconnectedMessage message);

        /// <summary>
        /// Verarbeitet die Aufforderung alle aktiven Clients über
        /// einen neu bemalten Punkt zu informieren
        /// </summary>
        /// <param name="message"></param>
        void ProcessNotifyPaintMessage(NotifyPaintToClientsMessage message);

        /// <summary>
        /// Verarbeitet die initiale Aufforderung den Apdater zu starten und
        /// einen Port zu überwachen
        /// </summary>
        /// <param name="message"></param>
        void ProcessInitAdapterMessage(InitAdapterMessage message);
        #endregion
    }
}
