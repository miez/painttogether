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
using PaintTogetherCommunicater.Messages.PTMessageDecoder;
using PaintTogetherCommunicater.Messages.PTMessageXmlSerializer;

namespace PaintTogetherCommunicater.Contracts
{
    /// <summary>
    /// Kodiert Nachrichten in Bytes und Dekodiert Bytes zu Nachrichten<para/>
    /// Die Nachrichten sind dabei vom Typ IClientServerMessage
    /// </summary>
    internal interface IPtMessageDecoder
    {
        /// <summary>
        /// Verlangt die Umwandlung einer Nachricht in ein XML
        /// </summary>
        event Action<ToXmlRequest> OnRequestToXml;

        /// <summary>
        /// Verlangt die Umwandlung einer XML in eine Nachricht
        /// </summary>
        event Action<ToMessageRequest> OnRequestToMessage;

        /// <summary>
        /// Verarbeitet eine Dekodierungsaufforderung, wo Bytes in ein Nachrichtenobjekt umgewandelt werden
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="Exception">Bei Fehlern während der Dekodierung</exception>
        void ProcessDecodeRequest(DecodeRequest request);

        /// <summary>
        /// Verarbeitet eine Kodierungsaufforderung, wo eine Nachricht in Bytes umgewandelt wird
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="Exception">Wenn ein unbekannter Nachrichteninhalt geschickt wird</exception>
        void ProcessEncodeRequest(EncodeRequest request);
    }
}
