
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
using PaintTogetherServer.Run;
using PaintTogetherStartSelector.Messages;

namespace PaintTogetherStartSelector.Test.PtStartServerAdapterCS
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
            var message = new StartServerMessage();
            message.Alias = "Ich und du";

            var result = PtStartServerAdapter.CreateStartParamText(message);
            Assert.That(result, Contains.Substring(string.Format("\"{0}=Ich und du\"", StartServerParams.AliasParamName)));
        }

        [Test]
        public void Einfacher_Alias()
        {
            var message = new StartServerMessage();
            message.Alias = "Wir";

            var result = PtStartServerAdapter.CreateStartParamText(message);
            Assert.That(result, Contains.Substring(string.Format("{0}=Wir", StartServerParams.AliasParamName)));
        }

        [Test]
        public void Einfacher_Port()
        {
            var message = new StartServerMessage();
            message.Port = 28374;

            var result = PtStartServerAdapter.CreateStartParamText(message);
            Assert.That(result, Contains.Substring(string.Format("{0}=28374", StartServerParams.PortParamName)));
        }

        [Test]
        public void Groessen_angabe()
        {
            var message = new StartServerMessage();
            message.Size = new Size(200, 400);

            var result = PtStartServerAdapter.CreateStartParamText(message);

            Assert.That(result, Contains.Substring(string.Format("{0}=200", StartServerParams.WidthParamName)));
            Assert.That(result, Contains.Substring(string.Format("{0}=400", StartServerParams.HeightParamName)));
        }
    }
}
