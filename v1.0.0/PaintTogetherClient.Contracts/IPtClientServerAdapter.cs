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

namespace PaintTogetherClient.Contracts
{
    /// <summary>
    /// ClientServerAdapter. Verwaltet die Socket-Verbindung
    /// zu einem PaintTogetherServer und tauscht über
    /// diese Nachrichten mit dem Server aus
    /// </summary>
    public interface IPtClientServerAdapter
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
        /// Informiert über den Verlust der Serververbindung
        /// </summary>
        event Action<ServerConnectionLostMessage> OnServerConnectionLost;

        /// <summary>
        /// Informiert über neuen Beteiligten
        /// </summary>
        event Action<NewAliasMessage> OnNewAlias;

        /// <summary>
        /// Information über Alias des Nutzers, der den Server gestartet hat
        /// </summary>
        event Action<ConnectedMessage> OnConnected;

        /// <summary>
        /// Informiert über das Verlassen eines bisher aktiven Beteiligten
        /// </summary>
        event Action<AliasDisconnectedMessage> OnAliasDisconnected;

        /// <summary>
        /// Startet den Adapter und baut eine Verbindung zum angegeben Server auf
        /// </summary>
        /// <param name="request"></param>
        void ProcessConnectToServerRequest(ConnectToServerRequest request);

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
    }
}
