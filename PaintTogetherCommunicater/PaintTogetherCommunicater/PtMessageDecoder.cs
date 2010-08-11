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
using PaintTogetherCommunicater.Contracts;
using PaintTogetherCommunicater.Messages.PTMessageDecoder;
using PaintTogetherCommunicater.Messages.PTMessageXmlSerializer;
using System.Text;

namespace PaintTogetherCommunicater
{
    /// <summary>
    /// Kodiert Nachrichten in Bytes und Dekodiert Bytes zu Nachrichten<para/>
    /// Die Nachrichten sind dabei vom Typ IClientServerMessage
    /// </summary>
    internal class PtMessageDecoder : IPtMessageDecoder
    {
        /// <summary>
        /// Das für die Kodierung und Dekodierung der XMLs verwendete
        /// Encoding
        /// </summary>
        internal readonly Encoding UsedEncoding = Encoding.UTF8;

        /// <summary>
        /// Verlangt die Umwandlung einer Nachricht in ein XML
        /// </summary>
        public event Action<ToXmlRequest> OnRequestToXml;

        /// <summary>
        /// Verlangt die Umwandlung einer XML in eine Nachricht
        /// </summary>
        public event Action<ToMessageRequest> OnRequestToMessage;

        /// <summary>
        /// Verarbeitet eine Dekodierungsaufforderung, wo Bytes in ein Nachrichtenobjekt umgewandelt werden
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="Exception">Bei Fehlern während der Dekodierung</exception>
        public void ProcessDecodeRequest(DecodeRequest request)
        {
            var toMessageRequest = new ToMessageRequest
                {
                    Xml = UsedEncoding.GetString(request.Bytes)
                };

            OnRequestToMessage(toMessageRequest);
            request.Result = toMessageRequest.Result;
        }

        /// <summary>
        /// Verarbeitet eine Kodierungsaufforderung, wo eine Nachricht in Bytes umgewandelt wird
        /// </summary>
        /// <param name="request"></param>
        /// <exception cref="Exception">Wenn ein unbekannter Nachrichteninhalt geschickt wird</exception>
        public void ProcessEncodeRequest(EncodeRequest request)
        {
            var toXmlRequest = new ToXmlRequest { Message = request.Message };
            OnRequestToXml(toXmlRequest);
            request.Result = UsedEncoding.GetBytes(toXmlRequest.Result);
        }
    }
}
