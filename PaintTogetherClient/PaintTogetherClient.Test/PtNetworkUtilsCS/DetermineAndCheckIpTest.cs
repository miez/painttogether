
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
using NUnit.Framework;

namespace PaintTogetherClient.Test.PtNetworkUtilsCS
{
    [TestFixture]
    public class DetermineAndCheckIpTest
    {
        [Test]
        public void lokale_IP()
        {
            var localIp = Dns.GetHostEntry("127.0.0.1").AddressList[1].ToString();
            Assert.That(PtNetworkUtils.DetermineAndCheckIp(localIp), Is.EqualTo(localIp));
        }

        [Test]
        public void lokaler_Servername()
        {
            Assert.That(PtNetworkUtils.DetermineAndCheckIp("localhost"), Is.EqualTo("127.0.0.1"));
        }

        [Test]
        public void nicht_erreichbare_IP()
        {
            Assert.That(string.IsNullOrEmpty(PtNetworkUtils.DetermineAndCheckIp("168.192.111.233")), Is.True);
        }

        [Test]
        public void unbekannter_Servername()
        {
            Assert.That(string.IsNullOrEmpty(PtNetworkUtils.DetermineAndCheckIp("gibtsnicht")), Is.True);
        }
    }
}
