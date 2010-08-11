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
using PaintTogetherServer.Messages.Core;
using PaintTogetherServer.Messages.Core.PaintFieldManager;

namespace PaintTogetherServer.Contracts.Core
{
    /// <summary>
    /// Sendet Nachrichten über Sokteverbindungen
    /// </summary>
    internal interface IPtServerStarter
    {
        /// <summary>
        /// Beauftragt zur Initialisierung des Malbereichs
        /// </summary>
        event Action<InitMessage> OnInit;

        /// <summary>
        /// Beauftragt den Start der Portüberwachung
        /// </summary>
        event Action<StartPortListingMessage> OnStartPortListing;

        /// <summary>
        /// Verarbeitet die Startaufforderung für die Initialisierung des Servers
        /// </summary>
        /// <param name="message"></param>
        void ProcessStartServerMessage(StartServerMessage message);
    }
}
