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
using PaintTogetherClient.Messages.Adapter.ServerConnectionManager;
using PaintTogetherCommunicater.Messages;

namespace PaintTogetherClient.Contracts.Adapter
{
    /// <summary>
    /// EBC für die Verwaltung der Server-Socket-Verbindung und 
    /// den Nachrichtenaustausch zwischen diesem Server und dem Client
    /// </summary>
    internal interface IPtServerConnectionManager
    {
        /// <summary>
        /// Liefert den initialen Malbereich
        /// </summary>
        event Action<CurrentPaintContentMessage> OnCurrentPaintContent;

        /// <summary>
        /// Informiert über die Bemalung eines Punktes
        /// </summary>
        event Action<AliasPaintedMessage> OnAliasPainted;

        /// <summary>
        /// Information über Alias des Nutzers, der den Server gestartet hat
        /// </summary>
        event Action<ConnectedMessage> OnConnected;

        /// <summary>
        /// Informiert über den Verlust der Serververbindung
        /// </summary>
        event Action<ServerConnectionLostMessage> OnServerConnectionLost;

        /// <summary>
        /// Informiert über neuen Beteiligten
        /// </summary>
        event Action<NewAliasMessage> OnNewAlias;

        /// <summary>
        /// Informiert über das Verlassen eines bisher aktiven Beteiligten
        /// </summary>
        event Action<AliasDisconnectedMessage> OnAliasDisconnected;

        /// <summary>
        /// Verarbeitet den erfolgreichen Verbindungsaufbau zu einem Server
        /// </summary>
        /// <param name="message"></param>
        void ProcessConEstablishedMessage(ConEstablishedMessage message);

        /// <summary>
        /// Beantragt die Bemalung eines Punktes auf dem Server
        /// </summary>
        /// <param name="message"></param>
        void ProcessNewPaintMessage(NewPaintMessage message);

        /// <summary>
        /// Beantragt die Beendigung der Serververbindung
        /// </summary>
        /// <param name="message"></param>
        void ProcessCloseConnectionMessage(CloseConnectionMessage message);

        #region Pins die später mit dem PaintTogetherCommunicater verbunden werden
        /// <summary>
        /// Beauftragt das Versenden einen Nachricht über einen Socket
        /// </summary>
        event Action<SendMessageMessage> OnSendMessage;

        /// <summary>
        /// Beauftragt die Überwachung auf eingehende Nachrichten eines Sockets
        /// </summary>
        event Action<StartReceivingMessage> OnStartReceiving;

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
