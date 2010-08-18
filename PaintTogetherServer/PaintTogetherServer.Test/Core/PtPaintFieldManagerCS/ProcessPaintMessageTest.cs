
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
using PaintTogetherServer.Core;
using PaintTogetherServer.Messages.Core.PaintFieldManager;
using PaintTogetherServer.Messages.Adapter;

namespace PaintTogetherServer.Test.Core.PtPaintFieldManagerCS
{
    [TestFixture]
    public class ProcessPaintMessageTest
    {
        private PtPaintFieldManager _fieldManager;

        [SetUp]
        public void SetUp()
        {
            _fieldManager = new PtPaintFieldManager();
            _fieldManager.ProcessInitMessage(new InitMessage { Height = 600, Width = 600 });

            var request = new GetCurrentPaintContentRequest();
            _fieldManager.ProcessGetCurrentPaintContentRequest(request);
        }

        [Test]
        public void Einen_Strich_bemalen_Benachrichtigung_pruefen()
        {
            NotifyPaintToClientsMessage paintedMessage = null;
            _fieldManager.OnNotifyPaint += message => paintedMessage = message;

            var sPoint = new Point(3, 12);
            var ePoint = new Point(6, 12);
            var color = Color.DodgerBlue;

            _fieldManager.ProcessClientPainted(new ClientPaintedMessage { Color = color, StartPoint = sPoint, EndPoint = ePoint });

            Assert.That(paintedMessage.StartPoint, Is.EqualTo(sPoint));
            Assert.That(paintedMessage.EndPoint, Is.EqualTo(ePoint));
            Assert.That(paintedMessage.Color, Is.EqualTo(color));
        }

        [Test]
        public void Einen_Strich_bemalen_Malinhalt_pruefen()
        {
            _fieldManager.OnNotifyPaint += message => Assert.True(true); /* Dummyverdrahtung damit keine NRE auftritt*/

            var sPoint = new Point(3, 12);
            var ePoint = new Point(6, 12);
            var color = Color.FromArgb(42, 143, 123);

            _fieldManager.ProcessClientPainted(new ClientPaintedMessage { Color = color, StartPoint = sPoint,EndPoint = ePoint});

            var request = new GetCurrentPaintContentRequest();
            _fieldManager.ProcessGetCurrentPaintContentRequest(request);

            // Nur den Start und Endpunkt prüfen
            Assert.That(request.Result.GetPixel(sPoint.X, sPoint.Y), Is.EqualTo(color));
            Assert.That(request.Result.GetPixel(ePoint.X, ePoint.Y), Is.EqualTo(color));
        }

        [Test]
        public void Einen_Strich_mit_Punkten_ausserhalb_des_Malbereichs()
        {
            _fieldManager.OnNotifyPaint += message => Assert.True(true); /* Dummyverdrahtung damit keine NRE auftritt*/

            var sPoint = new Point(1000, 0);
            var ePoint = new Point(0, 1000);
            var color = Color.FromArgb(42, 143, 123);

            _fieldManager.ProcessClientPainted(new ClientPaintedMessage { Color = color, StartPoint = sPoint, EndPoint = ePoint });

            var request = new GetCurrentPaintContentRequest();
            _fieldManager.ProcessGetCurrentPaintContentRequest(request);

            Assert.That(request.Result.GetPixel(500, 500), Is.EqualTo(color));
        }
    }
}
