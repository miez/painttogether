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

using System.Net.Sockets;
using NUnit.Framework;
using PaintTogetherCommunicater.Messages;
using PaintTogetherCommunicater.Messages.ClientServerCommunication.Server;
using System.Threading;
using System.Collections.Generic;

namespace PaintTogetherCommunicater.Test.PtMessageReceiverCS
{
    [TestFixture]
    public class ProcessStartReceivingTest
    {
        private Socket _senderSocket;
        private Socket _receiverSocket;
        private PtMessageReceiver _ptReceiver;

        [SetUp]
        public void SetUp()
        {
            var sockets = TestUtils.CreateLocalSocketConnection(20003);
            _senderSocket = sockets.Key;
            _receiverSocket = sockets.Value;

            _ptReceiver = new PtMessageReceiver();
        }

        [Test]
        public void Empfangen_einer_nachricht()
        {
            // Alles fürs Empfangen vorbereiten
            _ptReceiver.ProcessStartReceiving(new StartReceivingMessage { ToWatchSoketConnection = _receiverSocket });
            var expectedMessageContent = new PaintedScm();
            NewMessageReceivedMessage rMessage = null;
            var receivedBytes = new byte[0];
            _ptReceiver.OnRequestDecode += request => receivedBytes = request.Bytes;
            _ptReceiver.OnRequestDecode += request => request.Result = expectedMessageContent;
            _ptReceiver.OnNewMessageReceived += message => rMessage = message;

            // Dummybytes senden
            var sendBytes = new byte[] { 32, 12, 42, 234, 1, 47 };
            var bytes = new List<byte>();
            bytes.AddRange(PtMessageReceiver.StartBlock);
            bytes.AddRange(sendBytes);
            bytes.AddRange(PtMessageReceiver.EndBlock);
            _senderSocket.Send(bytes.ToArray());

            // Verarbeitung dauert einen kurzen Moment
            Thread.Sleep(5000);

            // gucken ob die Dummybytes angekommen sind und ob 
            // das erwartete Nachrichtenobjekt empfangen wurde
            Assert.That(rMessage.Message, Is.EqualTo(expectedMessageContent));
            Assert.That(receivedBytes, Is.EqualTo(sendBytes));
        }

        /// <summary>
        /// Beim unmittelbaren Senden von mehreren Nachrichten hintereinander,
        /// werden deren Bytes alle aneinander gehangen. Der Receiver muss in 
        /// der Lage sein diese Nachrichten auseinander zu teilen und zu verwerten.
        /// </summary>
        [Test]
        public void Empfangen_mehrerer_nachrichten_in_direkter_folge()
        {
            // Alles fürs Empfangen vorbereiten
            _ptReceiver.ProcessStartReceiving(new StartReceivingMessage { ToWatchSoketConnection = _receiverSocket });

            var receivedMessageCount = 0;
            _ptReceiver.OnRequestDecode += request => request.Result = new PaintedScm();
            _ptReceiver.OnNewMessageReceived += message => receivedMessageCount++;

            // Dummybytes senden
            const int sendMessageCount = 1000;
            for (var i = 0; i < sendMessageCount; i++)
            {
                var bytes = new List<byte>();
                bytes.AddRange(PtMessageReceiver.StartBlock);
                bytes.AddRange(new byte[] { 32, 12, 42, 234, 1, 47 });
                bytes.AddRange(PtMessageReceiver.EndBlock);
                _senderSocket.Send(bytes.ToArray());
            }

            // Verarbeitung dauert einen kurzen Moment
            Thread.Sleep(5000);

            Assert.That(receivedMessageCount, Is.EqualTo(sendMessageCount));
        }

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
