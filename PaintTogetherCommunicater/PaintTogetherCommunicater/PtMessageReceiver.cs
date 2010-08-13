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
using System.Collections.Generic;
using System.Net.Sockets;
using log4net;
using PaintTogetherCommunicater.Contracts;
using PaintTogetherCommunicater.Messages;
using PaintTogetherCommunicater.Messages.PTMessageDecoder;
using System.Threading;

namespace PaintTogetherCommunicater
{
    /// <summary>
    /// Dient der Überwachung von SoketVerindungen und signalisiert auf
    /// ihnen eingegangene Nachrichten.
    /// </summary>
    internal class PtMessageReceiver : IPtMessageReceiver
    {
        /// <summary>
        /// Die aktuelle Liste alle überwachten Socketverbindungen
        /// </summary>
        private readonly List<Socket> _watchedSockets = new List<Socket>();

        /// <summary>
        /// Diese Bytefolge muss am Beginn einer vollständigen, übertragenen Nachricht stehen
        /// </summary>
        internal readonly static byte[] StartBlock = new byte[] { 1, 10, 100, 111, 222, 111, 100, 10, 1 };

        /// <summary>
        /// Diese Bytefolge muss am Ende einer vollständigen, übertragenen Nachricht stehten
        /// </summary>
        internal readonly static byte[] EndBlock = new byte[] { 2, 11, 101, 112, 223, 112, 101, 11, 2 };

        /// <summary>
        /// Logger fürs Logging
        /// </summary>
        private static ILog Log
        {
            get
            {
                return LogManager.GetLogger("PtMessageReceiver");
            }
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
        /// Verlangt die Dekodierung der angegebenen Daten in ein Nachrichtenobjekt
        /// </summary>
        public event Action<DecodeRequest> OnRequestDecode;

        /// <summary>
        /// Startet die Empfangsüberwachung für die angegebene SoketVerbinung<para/>
        /// Die Verbindung zu der SoketVerbindung muss aufgebaut sein
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="Exception">Wenn der Zustand der SoketVerbidung ungültig ist</exception>
        public void ProcessStartReceiving(StartReceivingMessage message)
        {
            var thread = new Thread(StartReceiving);
            thread.Start(message.ToWatchSoketConnection);
        }

        /// <summary>
        /// Startet die Überwachung und reagiert auf Fehler und Verbindungabbrüche
        /// </summary>
        /// <param name="obj"></param>
        private void StartReceiving(object obj)
        {
            var socket = obj as Socket;
            _watchedSockets.Add(socket);
            Log.Debug("Start der Socketüberwachung auf eingehende Nachrichten wird durchgeführt");

            try
            {
                ReceiveMessages(socket);
            }
            catch (Exception e)
            {
                if (!socket.Connected)
                {
                    Log.Info("Verbindung zu einer überwachten SocketVerbinung wurde getrennt");
                }
                else
                {
                    Log.Error("Fehler beim Empfangen von Nachrichten. Überwachung wird beendet", e);
                }
                ProcessStopReceiving(new StopReceivingMessage { SoketConnection = socket });
            }
        }

        /// <summary>
        /// Verarbeitet solange Nachrichten von der angegebenen
        /// SocketVerbindung, bis ihre Überwachung beendet wird
        /// </summary>
        /// <param name="socket"></param>
        private void ReceiveMessages(Socket socket)
        {
            // TODO Vereinfachen
            while (_watchedSockets.Contains(socket))
            {
                var allContent = new List<byte>();
                var hasStartBlock = false;
                var emptyMessageCount = 0;

                do
                {
                    var buffer = new byte[32000]; // 32KB-Blöcke

                    // Ganz wichtig, nur die Bytes verwenden, die bei Receive als Datenbytes angegeben wurden!
                    var receiveCount = socket.Receive(buffer);
                    var content = new byte[receiveCount];
                    Array.Copy(buffer, content, receiveCount);

                    if (content.Length == 0)
                    {
                        emptyMessageCount++;
                        if (emptyMessageCount >= 10)
                        {
                            Log.Info("Die überwachte Verbindung sendet nur noch leere Nachrichten (Verbindung wurde Clientseitig geschlossen). Überwachung wird beendet");
                            ProcessStopReceiving(new StopReceivingMessage { SoketConnection = socket });
                            return;
                        }
                        continue;
                    }
                    emptyMessageCount = 0;

                    if (!hasStartBlock)
                    {
                        if (!HasStartBlock(content))
                        {
                            allContent.Clear();
                            Log.WarnFormat("Es wurde eine Nachricht ohne Startcode empfangen: {0}", content);
                            continue;
                        }
                        hasStartBlock = true;
                    }
                    allContent.AddRange(content);

                } while (!HasEndBlock(allContent));

                if (!_watchedSockets.Contains(socket)) return;

                var leftBytes = ProcessAllContent(allContent, socket);
                allContent.Clear();
                allContent.AddRange(leftBytes); // Alle noch nicht verarbeiteten Bytes für die weitere Verarbeitung verwenden
            }
        }

        /// <summary>
        /// Sendet für jede in dem Content verborgene Nachricht eine Empfangssignalisierung mit
        /// den dekodierten Nachrichten
        /// </summary>
        /// <param name="allContent"></param>
        /// <returns></returns>
        private byte[] ProcessAllContent(List<byte> allContent, Socket socket)
        {
            // TODO Vereinfachen
            var toReadContent = new List<byte>(allContent.ToArray());

            while (HasEndBlock(toReadContent))
            {
                // Stück für Stück eine Nachricht aus dem Content herausfiltern
                var readingResult = ReadContent(toReadContent);

                if (readingResult.Value != -1)
                {
                    // Hat toReadContent noch eine Nachricht beinhaltet, so über
                    // den Empfang der Nachricht informieren (vorher dekodieren)
                    var request = new DecodeRequest { Bytes = readingResult.Key };
                    OnRequestDecode(request);

                    Log.Debug("Eine neue Nachricht wurde empfangen. Singalisierung wird durchgeführt.");
                    OnNewMessageReceived(new NewMessageReceivedMessage { SoketConnection = socket, Message = request.Result });
                }

                // +1 weil im Ergebnis der Index der noch nicht verarbeiteten
                // Bytes drin steht.
                if (readingResult.Value >= toReadContent.Count + 1)
                {
                    // Es wurden alle Bytes verarbeitet
                    toReadContent.Clear();
                }
                else
                {
                    // Nur mit dem noch nicht verarbeitetem Bereich weiter arbeiten
                    toReadContent = new List<byte>(
                            toReadContent.GetRange(readingResult.Value,
                            toReadContent.Count - readingResult.Value));
                }
            }
            return toReadContent.ToArray();
        }

        /// <summary>
        /// Überprüft die Bytes, ob sie mit dem Startcode für
        /// Nachrichten beginnen
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal static bool HasStartBlock(byte[] content)
        {
            return FindBlock(new List<byte>(content), StartBlock) != -1;
        }

        /// <summary>
        /// Überprüft die Bytes, ob ein EndBlock enthalten ist
        /// </summary>
        /// <param name="allContent"></param>
        /// <returns></returns>
        internal static bool HasEndBlock(List<byte> allContent)
        {
            return FindBlock(allContent, EndBlock) != -1;
        }

        /// <summary>
        /// Liefert einem zu der angegebenen Bytefolge den StartIndex,
        /// wo der angegeben Byteblock zu finden ist.
        /// Ist der Block nicht enthalten, liefert die Methode -1
        /// </summary>
        /// <param name="content"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        internal static int FindBlock(List<byte> content, byte[] block)
        {
            var currentEndBlockIdx = 0;
            for (var i = 0; i < content.Count; i++)
            {
                if (content[i] == block[currentEndBlockIdx])
                {
                    currentEndBlockIdx++;
                    if (currentEndBlockIdx == block.Length)
                    {
                        // Es gibt einen vollständigen Endblock
                        return i - currentEndBlockIdx + 1;
                    }
                    continue;
                }
                currentEndBlockIdx = 0;
            }
            return -1;
        }

        /// <summary>
        /// Liefert einem die Bytes die zwischen einem Start- und
        /// einem Endblock liegen oder ein leeres Bytearray
        /// </summary>
        /// <param name="allContent"></param>
        /// <returns>die Nachricht + Position bis zu der der Content verarbeitet wurde</returns>
        internal static KeyValuePair<byte[], int> ReadContent(List<byte> allContent)
        {
            var startPos = FindBlock(allContent, StartBlock);
            var endPos = FindBlock(allContent, EndBlock);

            if (startPos == -1 || endPos == -1) return new KeyValuePair<byte[], int>(new byte[0], -1);

            var result = new List<byte>();
            for (var i = startPos + StartBlock.Length; i < endPos; i++)
            {
                result.Add(allContent[i]);
            }
            return new KeyValuePair<byte[], int>(result.ToArray(), endPos + EndBlock.Length);
        }

        /// <summary>
        /// Stoppt die Empfangsüberwachung für die angegebene SoketVerbinung
        /// </summary>
        /// <param name="message"></param>
        public void ProcessStopReceiving(StopReceivingMessage message)
        {
            Log.Debug("Überwachung eines Sockets soll beendet werden");
            if (_watchedSockets.Contains(message.SoketConnection))
            {
                _watchedSockets.Remove(message.SoketConnection);
                OnConLost(new ConLostMessage { DisconnectedSoketConnection = message.SoketConnection });
            }
            Log.Debug("Überwachung eienes Sockets erfolgreich beendet");
        }
    }
}