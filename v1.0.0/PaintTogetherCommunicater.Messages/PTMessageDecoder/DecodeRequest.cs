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

using PaintTogetherCommunicater.Messages.ClientServerCommunication;

namespace PaintTogetherCommunicater.Messages.PTMessageDecoder
{
    /// <summary>
    /// Aufforderung das beinhaltete byte-Array in ein
    /// Nachrichtenobjekt umzuwandeln. Dieses soll dann in der Resulteigenschaft
    /// gespeichert werden.
    /// </summary>
    internal class DecodeRequest
    {
        /// <summary>
        /// Die in eine Nachricht umzuwandelnden Bytes
        /// </summary>
        public byte[] Bytes { get; set; }

        /// <summary>
        /// Das Verarbeitungserebnis:
        /// <para/>
        /// Nachricht, welche durch das byte-Array abgebildet wurde
        /// </summary>
        public IServerClientMessage Result { set; get; }
    }
}
