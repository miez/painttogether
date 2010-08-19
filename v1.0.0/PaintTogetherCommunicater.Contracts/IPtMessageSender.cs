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
using PaintTogetherCommunicater.Messages;
using PaintTogetherCommunicater.Messages.PTMessageDecoder;

namespace PaintTogetherCommunicater.Contracts
{
    /// <summary>
    /// Sendet Nachrichten über Sokteverbindungen
    /// </summary>
    internal interface IPtMessageSender
    {
        /// <summary>
        /// Verlangt die Kodierung der angegebenen Nachricht
        /// </summary>
        event Action<EncodeRequest> OnRequestEncode;
        
        /// <summary>
        /// Verarbeitet die Nachricht und sendet an die
        /// enthaltene SoketVerbindung den angegeben Nachrichteninhalt <para/>
        /// Die Verbindung zu der SoketVerbindung muss aufgebaut sein
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="Exception">Wenn ein unbekannter Nachrichteninhalt geschickt wird</exception>
        /// <exception cref="Exception">Wenn der Zustand der SoketVerbidung ungültig ist</exception>
        /// <exception cref="Exception">Bei Fehlern beim Versenden</exception>
        void ProcessSendMessage(SendMessageMessage message);
    }
}
