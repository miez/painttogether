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

namespace PaintTogetherCommunicater.Test.PtMessageReceiverCS
{
    [TestFixture]
    public class ProcessStopReceivingTest
    {
        private Socket _senderSocket;
        private Socket _receiverSocket;
        private PtMessageReceiver _ptReceiver;

        [SetUp]
        public void SetUp()
        {
            var sockets = TestUtils.CreateLocalSocketConnection(20005);
            _senderSocket = sockets.Key;
            _receiverSocket = sockets.Value;

            _ptReceiver = new PtMessageReceiver();
        }

        [Test]
        public void Nicht_empfangen_einer_nachricht()
        {
            // Alles fürs Empfangen vorbereiten
            var receivedSomething = false;
            _ptReceiver.OnRequestDecode += request => receivedSomething = true;
            _ptReceiver.OnNewMessageReceived += message => receivedSomething = true;

            // Überwachung starten und wieder stoppen
            _ptReceiver.ProcessStartReceiving(new StartReceivingMessage { ToWatchSoketConnection = _receiverSocket });
            _ptReceiver.ProcessStopReceiving(new StopReceivingMessage { SoketConnection = _receiverSocket });

            // Dummybytes senden
            var sendBytes = new byte[] { 32, 12, 42, 234, 1, 47 };
            _senderSocket.Send(PtMessageReceiver.StartBlock);
            _senderSocket.Send(sendBytes);
            _senderSocket.Send(PtMessageReceiver.EndBlock);

            // Verarbeitung dauert einen kurzen Moment
            Thread.Sleep(1000);

            Assert.False(receivedSomething);
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
