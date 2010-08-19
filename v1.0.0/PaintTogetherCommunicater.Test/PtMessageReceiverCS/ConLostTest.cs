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
    public class ConLostTest
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
        public void Verbindungsverlust_durch_schliessen_des_sockets()
        {
            _ptReceiver.ProcessStartReceiving(new StartReceivingMessage { ToWatchSoketConnection = _receiverSocket });
            Socket disconnectedSocket = null;
            _ptReceiver.OnConLost += message =>
                disconnectedSocket = message.DisconnectedSoketConnection;

            Thread.Sleep(500);
            _receiverSocket.Disconnect(false);
            _receiverSocket.Close();
            Thread.Sleep(500);

            Assert.That(disconnectedSocket, Is.EqualTo(_receiverSocket));
        }

        [TearDown]
        public void TearDown()
        {
            _senderSocket.Disconnect(false);

            if (_receiverSocket.Connected)
            {
                _receiverSocket.Disconnect(false);
                _receiverSocket.Close();
            }

            _senderSocket.Close();
        }
    }
}
