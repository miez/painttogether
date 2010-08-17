
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

using NUnit.Framework;
using PaintTogetherStartSelector.Messages;
using System.Net.Sockets;
using System.Net;

namespace PaintTogetherStartSelector.Test.PtStartSelectorCS
{
    [TestFixture]
    public class ProcessTestServerRequestTest
    {
        [Test]
        public void verfuegbarer_Server()
        {
            var startSelector = new PtStartSelector();

            var request = new TestServerRequest();
            request.ServernameOrIp = "localhost";
            request.Port = 6969;

            // Port belegen, damit er verfügbar ist
            var listener = new TcpListener(IPAddress.Any, 6969);
            listener.Start();
            try
            {
                startSelector.ProcessTestServerRequest(request);
            }
            finally
            {
                listener.Stop();
            }

            Assert.That(request.Result, Is.True);
        }

        [Test]
        public void verfuegbarer_Server_port_nicht_belegt()
        {
            var startSelector = new PtStartSelector();

            var request = new TestServerRequest();
            request.ServernameOrIp = "localhost";
            request.Port = 6969;

            startSelector.ProcessTestServerRequest(request);

            Assert.That(request.Result, Is.False);
        }

        [Test]
        public void nicht_verfuegbarer_Server()
        {
            var startSelector = new PtStartSelector();

            var request = new TestServerRequest();
            request.ServernameOrIp = "localhost2010";
            request.Port = 6969;

            startSelector.ProcessTestServerRequest(request);

            Assert.That(request.Result, Is.False);
        }
    }
}
