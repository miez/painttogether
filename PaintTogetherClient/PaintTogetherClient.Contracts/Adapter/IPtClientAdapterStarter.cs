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

namespace PaintTogetherClient.Contracts.Adapter
{
    /// <summary>
    /// EBC für den initialen Verbindungsaufbau mit dem
    /// angegeben PaintTogetherServer
    /// </summary>
    internal interface IPtClientAdapterStarter
    {
        /// <summary>
        /// Informiert über den erfolgreichen Aufbau einer Socket-Verbindung
        /// zu einem PaintTogetherServer
        /// </summary>
        event Action<ConEstablishedMessage> OnConEstablished;

        /// <summary>
        /// Startet den Adapter und baut eine Verbindung zum angegeben Server auf
        /// </summary>
        /// <param name="request"></param>
        void ProcessConnectToServerRequest(ConnectToServerRequest request);
    }
}
