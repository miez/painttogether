
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
    public class ProcessTestLocalPortRequestTest
    {
        [Test]
        public void freier_Port()
        {
            var startSelector = new PtStartSelector();

            var request = new TestLocalPortRequest();
            request.Port = 6969; // Hoffentlich gerade frei
            startSelector.ProcessTestLocalPortRequest(request);

            Assert.That(request.Result, Is.True, "Möglicherweise läuft gerade ein PT-Server der den Test behindert");
        }

        [Test]
        public void belegter_Port()
        {
            var startSelector = new PtStartSelector();

            var request = new TestLocalPortRequest();
            request.Port = 6969;

            // Port belegen, damit er blockiert ist
            var listener = new TcpListener(IPAddress.Any, 6969);
            listener.Start();
            try
            {
                startSelector.ProcessTestLocalPortRequest(request);
            }
            finally
            {
                listener.Stop();
            }

            Assert.That(request.Result, Is.False);
        }
    }
}
