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
using PaintTogetherCommunicater.Messages.PTMessageXmlSerializer;

namespace PaintTogetherCommunicater.Contracts
{
    /// <summary>
    /// Wandelt Xmls in Nachrichten und Nachrichten in Xmls um
    /// </summary>
    internal interface IPtMessageXmlSerializer
    {
        /// <summary>
        /// Verarbeitet die Aufforderung eine Nachricht in ein XML umzuwandeln
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="Exception">Wenn ein unbekannter Nachrichteninhalt geschickt wird</exception>
        void ProcessToXmlRequest(ToXmlRequest request);

        /// <summary>
        /// Verarbeitet die Aufforderung ein XMl in eine Nachricht umzuwandeln
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="Exception">Bei Fehlern im XML</exception>
        void ProcessToMessageRequest(ToMessageRequest request);
    }
}
