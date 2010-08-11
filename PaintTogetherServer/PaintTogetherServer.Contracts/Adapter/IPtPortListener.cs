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
using PaintTogetherServer.Messages.Adapter.ConnectionManager;

namespace PaintTogetherServer.Contracts.Adapter
{
    /// <summary>
    /// Startet die Überwachung eines Ports und meldet alle eingehenden Verbindungen
    /// </summary>
    internal interface IPtPortListener
    {
        /// <summary>
        /// Informiert über eine neu eingegangene Verbindung
        /// </summary>
        event Action<NewConnectionMessage> OnNewConnection;

        /// <summary>
        /// Verarbeitet die initiale Aufforderung den Apdater zu starten und
        /// einen Port zu überwachen
        /// </summary>
        /// <param name="message"></param>
        void ProcessStartPortListingMessage(StartPortListingMessage message);
    }
}
