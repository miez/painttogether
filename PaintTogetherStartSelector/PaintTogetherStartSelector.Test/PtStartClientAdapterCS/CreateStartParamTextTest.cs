
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
using NUnit.Framework;
using System.IO;
using PaintTogetherStartSelector.Messages;
using PaintTogetherClient.Run;

namespace PaintTogetherStartSelector.Test.PtStartClientAdapterCS
{
    [TestFixture]
    public class CreateStartParamTextTest
    {
        /// <summary>
        /// Bei Leerzeichen muss der Parameter mit "" umrahmt werden
        /// </summary>
        [Test]
        public void Alias_mit_Leerzeichen()
        {
            var message = new StartClientMessage();
            message.Alias = "Ich und du";

            var result = PtStartClientAdapter.CreateStartParamText(message);
            Assert.That(result, Contains.Substring(string.Format("\"{0}=Ich und du\"", StartClientParams.AliasParamName)));
        }

        [Test]
        public void Einfacher_Alias()
        {
            var message = new StartClientMessage();
            message.Alias = "Wir";

            var result = PtStartClientAdapter.CreateStartParamText(message);
            Assert.That(result, Contains.Substring(string.Format("{0}=Wir", StartClientParams.AliasParamName)));
        }

        [Test]
        public void Einfacher_Port()
        {
            var message = new StartClientMessage();
            message.Port = 28374;

            var result = PtStartClientAdapter.CreateStartParamText(message);
            Assert.That(result, Contains.Substring(string.Format("{0}=28374", StartClientParams.PortParamName)));
        }

        [Test]
        public void Einfacher_Servername()
        {
            var message = new StartClientMessage();
            message.ServernameOrIp = "servername";

            var result = PtStartClientAdapter.CreateStartParamText(message);
            Assert.That(result, Contains.Substring(string.Format("{0}=servername", StartClientParams.ServerParamName)));
        }

        /// <summary>
        /// Testet ob die RGB-Werte in der richtigen Reihenfolge übertragen
        /// werden
        /// </summary>
        [Test]
        public void Farbe_mit_Namen()
        {
            var message = new StartClientMessage();
            message.Color = Color.Red;

            var result = PtStartClientAdapter.CreateStartParamText(message);
            Assert.That(result, Contains.Substring(string.Format("{0}={1}-{2}-{3}", 
                StartClientParams.ColorParamName,
                message.Color.R,
                message.Color.G,
                message.Color.B)));
        }
    }
}
