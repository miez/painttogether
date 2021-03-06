﻿
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
            toSendMessage.StartPoint = new Point(3, 4);
            toSendMessage.EndPoint = new Point(30, 40);
            PaintedScm receivedMessage = null;

            _communicater.OnNewMessageReceived += 
                message => 
                    receivedMessage = message.Message as PaintedScm;
            _communicater.ProcessSendMessage(new SendMessageMessage { SoketConnection = _senderSocket, Message = toSendMessage });

            Thread.Sleep(1000); // Senden und Empfangen dauert einen kurzen Moment

            Assert.That(receivedMessage.StartPoint, Is.EqualTo(toSendMessage.StartPoint));
            Assert.That(receivedMessage.EndPoint, Is.EqualTo(toSendMessage.EndPoint));
            Assert.That(receivedMessage.Color, Is.EqualTo(toSendMessage.Color));
        }

        [Test]
        public void Malbereich_senden()
        {
            var toSendMessage = new PaintContentScm();
            toSendMessage.PaintContent = new Bitmap(750, 350);
            PaintContentScm receivedMessage = null;

            _communicater.OnNewMessageReceived +=
                message => receivedMessage = message.Message as PaintContentScm;

            _communicater.ProcessSendMessage(new SendMessageMessage { SoketConnection = _senderSocket, Message = toSendMessage });

            Thread.Sleep(5000); // Senden und Empfangen dauert einen kurzen Moment
            Assert.That(receivedMessage.PaintContent.Size, Is.EqualTo(toSendMessage.PaintContent.Size));
        }

        [TearDown]
        public void TearDown()
        {
            _communicater.OnConLost += message => Assert.True(true) /*Dummyverdrahtung*/;
            _communicater.ProcessStopReceiving(new StopReceivingMessage { SoketConnection = _receiverSocket });
         
            _senderSocket.Disconnect(false);
            _receiverSocket.Disconnect(false);

            _senderSocket.Close();
            _receiverSocket.Close();
        }
    }
}
