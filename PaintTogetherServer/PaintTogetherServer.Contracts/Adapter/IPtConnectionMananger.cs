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
using PaintTogetherCommunicater.Messages;
using PaintTogetherServer.Messages.Adapter;
using PaintTogetherServer.Messages.Adapter.ConnectionManager;

namespace PaintTogetherServer.Contracts.Adapter
{
    /// <summary>
    /// Verwaltet alle Socketverbindungen
    /// </summary>
    internal interface IPtConnectionMananger
    {
        #region Pins die später direkt mit dem IPtServerClienAdapter verbunden werden
        #region Output-Pins
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

        #region Inputpins
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
        #endregion
        #endregion

        #region Pins zur Kommunikation mit dem PortListener
        /// <summary>
        /// Verarbeitet eine neue Socket-Verbindung
        /// </summary>
        /// <param name="message"></param>
        void ProcessNewConnectionMessage(NewConnectionMessage message);
        #endregion

        #region Pins die später mit dem PaintTogetherCommunicater verbunden werden
        /// <summary>
        /// Beauftragt das Versenden einen Nachricht über einen Socket
        /// </summary>
        event Action<SendMessageMessage> OnSendMessage;

        /// <summary>
        /// Beauftragt die Überwachung auf eingehende Nachrichten eines Sockets
        /// </summary>
        event Action<StartPortListingMessage> OnStartReceiving;

        /// <summary>
        /// Beauftragt die Beendigung der Überwachung von eingehenden Nachrichten
        /// </summary>
        event Action<StopReceivingMessage> OnStopReceiving;

        /// <summary>
        /// Verarbeitet die Information über den Verlust einer Socketverbindung
        /// </summary>
        /// <param name="message"></param>
        void ProcessConLostMessage(ConLostMessage message);

        /// <summary>
        /// Verarbeitet eine neu eingegangene Nachricht
        /// </summary>
        /// <param name="message"></param>
        void ProcessNewMessageMessage(NewMessageReceivedMessage message);
        #endregion
    }
}
