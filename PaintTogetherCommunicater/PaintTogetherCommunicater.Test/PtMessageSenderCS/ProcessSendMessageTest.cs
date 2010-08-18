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

using System.Drawing;
using System.Net.Sockets;
using NUnit.Framework;
using PaintTogetherCommunicater.Messages;
using PaintTogetherCommunicater.Messages.ClientServerCommunication.Server;
using System.Collections.Generic;

namespace PaintTogetherCommunicater.Test.PtMessageSenderCS
{
    [TestFixture]
    public class ProcessSendMessageTest
    {
        private Socket _senderSocket;
        private Socket _receiverSocket;
        private PtMessageSender _ptSender;

        /// <summary>
        /// Vor Beginn der Testdurchführung muss erst einmal eine
        /// Soketverbindung aufgebaut werden, über die eine Nachricht
        /// verschickt werden kann.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            var sockets = TestUtils.CreateLocalSocketConnection(20001);
            _senderSocket = sockets.Key;
            _receiverSocket = sockets.Value;

            _ptSender = new PtMessageSender();
        }

        /// <summary>
        /// Hier wird das Versenden einer Nachricht über eine verbundene
        /// Socketconnection getestet. Dabei wird über den Empfangsocket geprüft
        /// ob das richtige Ergebnis geschickt wurde
        /// </summary>
        [Test]
        public void Einfache_Nachricht_senden()
        {
            #region Arrange - Testvereinbarungen/vorbereitungen treffen
            var sendBytes = new byte[] { 34, 123, 4 };
            var toSendMessage = new PaintedScm { Color = Color.Red, StartPoint = new Point(2, 2), EndPoint= new Point(3, 3) };

            // Wenn das Nachrichtenobjekt umgewandelt werden soll, dann ist das
            // Ergebnis das hier angegeben sendByte-Array. Wir brauchen für den Test
            // also kein funktionierenden PtMessageDecoder
            _ptSender.OnRequestEncode += request => request.Result = sendBytes;
            #endregion

            #region Act - Die zu testende Operation durchführen
            // Nachricht über den verbudenen SenderSocket schicken
            _ptSender.ProcessSendMessage(new SendMessageMessage { Message = toSendMessage, SoketConnection = _senderSocket });
            #endregion

            #region Assert - die Verarbeitung überprüfen, hier ob die zu sendenen Bytes auch gesendet wurden
            var receivedBytes = new byte[PtMessageReceiver.StartBlock.Length + 3 + PtMessageReceiver.EndBlock.Length];
            _receiverSocket.Receive(receivedBytes);

            // Anfangs und Startblock für Nachrichtenübermittlung werden auch gesesendet
            var toReceiveBytes = new List<byte>(PtMessageReceiver.StartBlock);
            toReceiveBytes.AddRange(sendBytes);
            toReceiveBytes.AddRange(PtMessageReceiver.EndBlock);

            Assert.That(receivedBytes, Is.EqualTo(toReceiveBytes.ToArray()));
            #endregion
        }

        [Test, ExpectedException]
        public void Unverbundener_Socket()
        {
            _ptSender.OnRequestEncode += request => request.Result = new byte[1];

            // Hier gibt es kein Assert, da eine Exception erwartet wird. Diese
            // Erwartung wird durch das Attribut 'ExpectedException' ausgedrueckt
            _ptSender.ProcessSendMessage(new SendMessageMessage { SoketConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp) });
        }

        /// <summary>
        /// Die für den Test aufgebaute Socketverbindung wieder beenden
        /// </summary>
        [TearDown]
        public void TearDown()
        {
            _senderSocket.Disconnect(false);
            _receiverSocket.Disconnect(false);

            _senderSocket.Close();
            _receiverSocket.Close();
        }
    }
}
