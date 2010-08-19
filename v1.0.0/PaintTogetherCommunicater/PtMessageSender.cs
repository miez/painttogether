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

namespace PaintTogetherCommunicater
{
    /// <summary>
    /// Sendet Nachrichten über Sokteverbindungen
    /// </summary>
    internal class PtMessageSender : IPtMessageSender
    {
        /// <summary>
        /// Logger fürs Logging
        /// </summary>
        private static ILog Log
        {
            get
            {
                return LogManager.GetLogger("PtMessageSender");
            }
        }

        /// <summary>
        /// Verlangt die Kodierung der angegebenen Nachricht
        /// </summary>
        public event Action<EncodeRequest> OnRequestEncode;

        /// <summary>
        /// Verarbeitet die Nachricht und sendet an die
        /// enthaltene SoketVerbindung den angegeben Nachrichteninhalt <para/>
        /// Die Verbindung zu der SoketVerbindung muss aufgebaut sein
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="Exception">Wenn der Zustand der SoketVerbidung ungültig ist</exception>
        /// <exception cref="Exception">Bei Fehlern beim Versenden</exception>
        public void ProcessSendMessage(SendMessageMessage message)
        {
            if (!message.SoketConnection.Connected)
            {
                Log.Error("Nachricht wird nicht versendet, da der Socket nicht verbunden ist.");
                throw new Exception("Socket ist nicht verbunden");
            }

            try
            {
                // Kodierungsauftrag erstellen und "abschicken"
                var encodeRequest = new EncodeRequest { Message = message.Message };
                OnRequestEncode(encodeRequest);

                // Nun müsste die Resulteigenschaft am request mit der
                // kodiertern Nachricht gefüllt sein. Dieses Ergebnis jetzt
                // über den Socket verschicken
                SendContent(message.SoketConnection, encodeRequest.Result);

                Log.Debug("Nachricht erfolgreich über Socket versendet");
            }
            catch (Exception e)
            {
                Log.Error("Fehler beim Versenden/Kodieren einer Nachricht", e);
                throw; // die aufgetretene Exception nach oben weiter geben
            }
        }

        /// <summary>
        /// Sendet an den Socket nicht den Content, dabei
        /// wird vor dem Content noch ein StartBlock und nach dem
        /// Content ein EndBlock gesendet
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="content"></param>
        private static void SendContent(Socket socket, byte[] content)
        {
            var bytes = new List<byte>();
            bytes.AddRange(PtMessageReceiver.StartBlock);
            bytes.AddRange(content);
            bytes.AddRange(PtMessageReceiver.EndBlock);
            socket.Send(bytes.ToArray());
        }
    }
}
