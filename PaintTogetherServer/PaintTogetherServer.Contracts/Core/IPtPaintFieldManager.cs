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
using PaintTogetherServer.Messages.Core.PaintFieldManager;

namespace PaintTogetherServer.Contracts.Core
{
    /// <summary>
    /// Verwaltet den Malbereich in Form einer BitMap
    /// </summary>
    internal interface IPtPaintFieldManager
    {
        /// <summary>
        /// Beauftragt die Benachrichtigung aller Clients über
        /// einen neu bemalten Punkt
        /// </summary>
        event Action<NotifyPaintToClientsMessage> OnNotifyPaint;

        /// <summary>
        /// Verarbeitet die Initialisierungsaufforderung
        /// </summary>
        /// <param name="message"></param>
        void ProcessInitMessage(InitMessage message);

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
    }
}
