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
    /// Dient der Überwachung von SoketVerindungen und signalisiert auf
    /// ihnen eingegangene Nachrichten.
    /// </summary>
    internal interface IPtMessageReceiver
    {
        /// <summary>
        /// Signalisiert einen Verbindungsverlust zu einer
        /// für den Empfang von Nachrichten überwachten SoketVerbindung
        /// </summary>
        event Action<ConLostMessage> OnConLost;

        /// <summary>
        /// Signalisiert eine neue Nachricht, die über eine
        /// für den Empfang von Nachrichten überwachte SoketVerbindung
        /// eingetroffen ist
        /// </summary>
        event Action<NewMessageReceivedMessage> OnNewMessageReceived;

        /// <summary>
        /// Verlangt die Dekodierung der angegebenen Daten in ein Nachrichtenobjekt
        /// </summary>
        event Action<DecodeRequest> OnRequestDecode;

        /// <summary>
        /// Startet die Empfangsüberwachung für die angegebene SoketVerbinung<para/>
        /// Die Verbindung zu der SoketVerbindung muss aufgebaut sein
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="Exception">Wenn der Zustand der SoketVerbidung ungültig ist</exception>
        void ProcessStartReceiving(StartReceivingMessage message);

        /// <summary>
        /// Stoppt die Empfangsüberwachung für die angegebene SoketVerbinung
        /// </summary>
        /// <param name="message"></param>
        void ProcessStopReceiving(StopReceivingMessage message);
    }
}
