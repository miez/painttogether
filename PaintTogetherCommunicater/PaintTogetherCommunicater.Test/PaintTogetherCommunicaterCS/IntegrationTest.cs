
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
using System.Collections.Generic;
using PaintTogetherCommunicater.Contracts;
using PaintTogetherCommunicater.Messages;
using PaintTogetherCommunicater.Messages.ClientServerCommunication.Server;
using System.Threading;

namespace PaintTogetherCommunicater.Test.PaintTogetherCommunicaterCS
{
    /// <summary>
    /// Ein Integrationstest testet eine gesamte EBC mit ein
    /// paar kleinen Testfällen, die viele Funktionen gleichzeitig abdecken.
    /// </summary>
    [TestFixture]
    public class IntegrationTest
    {
        private IPaintTogetherCommunicater _communicater;
        private Socket _senderSocket;
        private Socket _receiverSocket;

        [SetUp]
        public void SetUp()
        {
            var sockets = TestUtils.CreateLocalSocketConnection(55555);
            _senderSocket = sockets.Key;
            _receiverSocket = sockets.Value;

            _communicater = new PaintTogetherCommunicater();
            _communicater.ProcessStartReceiving(new StartReceivingMessage { ToWatchSoketConnection = _receiverSocket });
        }

        [Test]
        public void Client_Nachricht_senden()
        {
            var toSendMessage = new PaintedScm();
            toSendMessage.Color = Color.FromArgb(32, 23, 14);
            toSendMessage.Point = new Point(3, 4);
            PaintedScm receivedMessage = null;

            _communicater.OnNewMessageReceived += 
                message => 
                    receivedMessage = message.Message as PaintedScm;
            _communicater.ProcessSendMessage(new SendMessageMessage { SoketConnection = _senderSocket, Message = toSendMessage });

            Thread.Sleep(1000); // Senden und Empfangen dauert einen kurzen Moment

            Assert.That(receivedMessage.Point, Is.EqualTo(toSendMessage.Point));
            Assert.That(receivedMessage.Color, Is.EqualTo(toSendMessage.Color));
        }

        [TearDown]
        public void TearDown()
        {
            _communicater.ProcessStopReceiving(new StopReceivingMessage { SoketConnection = _receiverSocket });

            _senderSocket.Disconnect(false);
            _receiverSocket.Disconnect(false);

            _senderSocket.Close();
            _receiverSocket.Close();
        }
    }
}
