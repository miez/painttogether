
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
using System.Collections.Generic;
using PaintTogetherCommunicater.Messages.ClientServerCommunication.Client;
using System.Drawing;
using PaintTogetherCommunicater.Messages.ClientServerCommunication.Server;

namespace PaintTogetherCommunicater.Test.PtMessageReceiverCS
{
    [TestFixture]
    public class CreateXmlTest
    {
        [Test]
        public void ClientConnectScm()
        {
            var toConvert = new ConnectScm();
            toConvert.Alias = "mein Name";
            toConvert.Color = Color.Red;

            var xml = PtMessageXmlSerializer.CreateXml(toConvert);
            var reConverted = PtMessageXmlSerializer.CreateMessage(xml) as ConnectScm;

            Assert.That(reConverted.Alias, Is.EqualTo(toConvert.Alias));
            CompareColors(reConverted.Color, toConvert.Color);
        }

        [Test]
        public void ClientPaintScm()
        {
            var toConvert = new PaintScm();
            toConvert.Point = new Point(4, 5);
            toConvert.Color = Color.Red;

            var xml = PtMessageXmlSerializer.CreateXml(toConvert);
            var reConverted = PtMessageXmlSerializer.CreateMessage(xml) as PaintScm;

            Assert.That(reConverted.Point, Is.EqualTo(toConvert.Point));
            CompareColors(reConverted.Color, toConvert.Color);
        }

        [Test]
        public void ServerAllConnectionsScm()
        {
            var toConvert = new AllConnectionsScm();
            toConvert.Painter = new[]
                                    {
                                        new KeyValuePair<string, Color>("Tim", Color.Blue),
                                        new KeyValuePair<string, Color>("Bernd", Color.Yellow)
                                    };

            var xml = PtMessageXmlSerializer.CreateXml(toConvert);
            var reConverted = PtMessageXmlSerializer.CreateMessage(xml) as AllConnectionsScm;

            Assert.That(reConverted.Painter.Length, Is.EqualTo(2));

            Assert.That(reConverted.Painter[0].Key, Is.EqualTo("Tim"));
            CompareColors(reConverted.Painter[0].Value, Color.Blue);

            Assert.That(reConverted.Painter[1].Key, Is.EqualTo("Bernd"));
            CompareColors(reConverted.Painter[1].Value, Color.Yellow);
        }

        [Test]
        public void ServerConnectedScm()
        {
            var toConvert = new ConnectedScm();
            toConvert.Alias = "mein Name";

            var xml = PtMessageXmlSerializer.CreateXml(toConvert);
            var reConverted = PtMessageXmlSerializer.CreateMessage(xml) as ConnectedScm;

            Assert.That(reConverted.Alias, Is.EqualTo(toConvert.Alias));
        }

        [Test]
        public void ServerConnectionLost()
        {
            var toConvert = new ConnectionLostScm();
            toConvert.Alias = "mein Name";
            toConvert.Color = Color.Red;

            var xml = PtMessageXmlSerializer.CreateXml(toConvert);
            var reConverted = PtMessageXmlSerializer.CreateMessage(xml) as ConnectionLostScm;

            Assert.That(reConverted.Alias, Is.EqualTo(toConvert.Alias));
            CompareColors(reConverted.Color, toConvert.Color);
        }

        [Test]
        public void ServerNewConnection()
        {
            var toConvert = new NewConnectionScm();
            toConvert.Alias = "mein Name";
            toConvert.Color = Color.Red;

            var xml = PtMessageXmlSerializer.CreateXml(toConvert);
            var reConverted = PtMessageXmlSerializer.CreateMessage(xml) as NewConnectionScm;

            Assert.That(reConverted.Alias, Is.EqualTo(toConvert.Alias));
            CompareColors(reConverted.Color, toConvert.Color);
        }

        [Test]
        public void ServerPaintContent()
        {
            var toConvert = new PaintContentScm();
            toConvert.PaintContent = new Bitmap(100, 200);
            toConvert.PaintContent.SetPixel(30, 40, Color.RoyalBlue);

            var xml = PtMessageXmlSerializer.CreateXml(toConvert);
            var reConverted = PtMessageXmlSerializer.CreateMessage(xml) as PaintContentScm;

            Assert.That(reConverted.PaintContent.Size, Is.EqualTo(toConvert.PaintContent.Size));
            CompareColors(reConverted.PaintContent.GetPixel(30, 40), toConvert.PaintContent.GetPixel(30, 40));
        }

        [Test]
        public void ServerPaintedScm()
        {
            var toConvert = new PaintedScm();
            toConvert.Point = new Point(4, 5);
            toConvert.Color = Color.Red;

            var xml = PtMessageXmlSerializer.CreateXml(toConvert);
            var reConverted = PtMessageXmlSerializer.CreateMessage(xml) as PaintedScm;

            Assert.That(reConverted.Point, Is.EqualTo(toConvert.Point));
            CompareColors(reConverted.Color, toConvert.Color);
        }

        private void CompareColors(Color originial, Color toCompare)
        {
            Assert.That(originial.R, Is.EqualTo(toCompare.R));
            Assert.That(originial.G, Is.EqualTo(toCompare.G));
            Assert.That(originial.B, Is.EqualTo(toCompare.B));
            Assert.That(originial.A, Is.EqualTo(toCompare.A));
        }
    }
}
