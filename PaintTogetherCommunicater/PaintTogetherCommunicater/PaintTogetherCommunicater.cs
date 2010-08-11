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
using PaintTogetherCommunicater.Messages;

namespace PaintTogetherCommunicater
{
    /// <summary>
    /// Schnittstellenbeschreibung der HauptEBC dieses Projektes. <para/>
    /// Diese EBC ist für das Senden von Nachrichten des Typs IServerClientMessage <para/>
    /// über eine Soketverbindung sowie für den Empfang von Nachrichten des  Typs IServerClientMessage <para/>
    /// über eine beliebige Anzahl von Soketverbindungen zuständig.
    /// </summary>
    public class PaintTogetherCommunicater : IPaintTogetherCommunicater
    {
        // Zuerst werden die vier internen EBCs erzeugt
        private readonly IPtMessageDecoder _decoder = new PtMessageDecoder();
        private readonly IPtMessageReceiver _receiver = new PtMessageReceiver();
        private readonly IPtMessageSender _sender = new PtMessageSender();
        private readonly IPtMessageXmlSerializer _xmlSerializer = new PtMessageXmlSerializer();

        /// <summary>
        /// Erstellt die EBC
        /// </summary>
        public PaintTogetherCommunicater()
        {
            // Die Inputpins der PaintTogetherCommunicaterEBC werden
            // direkt bei den Inputpins (siehe z. Bsp. ProcessStartReceiving)
            // auf die internen Bausteine weiter geleitet (also verdrahtet)
            // --
            // Die Outputpins der PaintTogetherCommunicaterEBC müssen jetzt
            // mit den passenden Outputpins der internen EBC verlinkt werden
            // - Als Outputpins gibts hier OnConLost und OnNewMessageReceived
            _receiver.OnConLost += message => OnConLost(message);
            _receiver.OnNewMessageReceived += message => OnNewMessageReceived(message);
            // die von dem Receiver ausgelösten Nachrichten leiten wir also
            // einfach nach außen weiter.
            // --
            // Jetzt müssen noch alle noch nicht verdrahteten Pins der internen
            // Bausteine miteinander verdrahtet werden (es darf kein Pin unverdrahtet bleiben)
            _receiver.OnRequestDecode += request => _decoder.ProcessDecodeRequest(request);
            _sender.OnRequestEncode += request => _decoder.ProcessEncodeRequest(request);
            _decoder.OnRequestToXml += request => _xmlSerializer.ProcessToXmlRequest(request);
            _decoder.OnRequestToMessage += request => _xmlSerializer.ProcessToMessageRequest(request);
            // Das wars. Alle In- und Outputpins der inneren EBCs wurden verdrahtet.
        }

        /// <summary>
        /// Signalisiert einen Verbindungsverlust zu einer
        /// für den Empfang von Nachrichten überwachten SoketVerbindung
        /// </summary>
        public event Action<ConLostMessage> OnConLost;

        /// <summary>
        /// Signalisiert eine neue Nachricht, die über eine
        /// für den Empfang von Nachrichten überwachte SoketVerbindung
        /// eingetroffen ist
        /// </summary>
        public event Action<NewMessageReceivedMessage> OnNewMessageReceived;

        /// <summary>
        /// Verarbeitet die Nachricht und sendet an die
        /// enthaltene SoketVerbindung den angegeben Nachrichteninhalt <para/>
        /// Die Verbindung zu der SoketVerbindung muss aufgebaut sein
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="Exception">Wenn ein unbekannter Nachrichteninhalt geschickt wird</exception>
        /// <exception cref="Exception">Wenn der Zustand der SoketVerbidung ungültig ist</exception>
        /// <exception cref="Exception">Bei Fehlern beim Versenden</exception>
        public void ProcessSendMessage(SendMessageMessage message)
        {
            _sender.ProcessSendMessage(message);
        }

        /// <summary>
        /// Startet die Empfangsüberwachung für die angegebene SoketVerbinung<para/>
        /// Die Verbindung zu der SoketVerbindung muss aufgebaut sein
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="Exception">Wenn der Zustand der SoketVerbidung ungültig ist</exception>
        public void ProcessStartReceiving(StartReceivingMessage message)
        {
            _receiver.ProcessStartReceiving(message);
        }

        /// <summary>
        /// Stoppt die Empfangsüberwachung für die angegebene SoketVerbinung
        /// </summary>
        /// <param name="message"></param>
        public void ProcessStopReceiving(StopReceivingMessage message)
        {
            _receiver.ProcessStopReceiving(message);
        }
    }
}
