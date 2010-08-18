
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

using System.Net;
using System.Net.Sockets;
using NUnit.Framework;
using System.Collections.Generic;
using PaintTogetherServer.Adapter;
using PaintTogetherServer.Messages.Adapter;
using System.Threading;
using PaintTogetherServer.Messages.Adapter.ConnectionManager;

namespace PaintTogetherServer.Test.Adapter.PtPortListenerCS
{
    [TestFixture]
    public class ProcessStartPortListingMessageTest
    {
        [Test]
        public void mehrere_Verbindungen_aufbauen()
        {
            var serverPort = 34523;
            var newConCount = 0;

            var listener = new PtPortListener();
            listener.ProcessStartPortListingMessage(new StartPortListingMessage { Port = serverPort });
            listener.OnNewConnection += message => newConCount++;

            for (var i = 0; i < 20; i++)
            {
                // Verbindung zu dem überwachten Port aufbauen
                var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(Dns.GetHostAddresses("127.0.0.1")[0], serverPort);
            }

            Thread.Sleep(500);

            Assert.That(newConCount, Is.EqualTo(20));
        }
    }
}
