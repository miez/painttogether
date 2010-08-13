
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
using PaintTogetherClient.Core;
using PaintTogetherClient.Messages.Adapter;
using PaintTogetherClient.Messages.Core.PaintContentManager;
using PaintTogetherClient.Messages.Portal;

namespace PaintTogetherClient.Test.Core.PtPaintContentManagerCS
{
    [TestFixture]
    public class ProcessPaintSelfTest
    {
        [Test]
        public void Malnachfrage_testen_bei_einem_Punkt_mit_anderer_Farbe()
        {
            NewPaintMessage receivedPaintMessage = null;

            var manager = new PtPaintContentManager();
            manager.OnNewPaint += message => receivedPaintMessage = message;

            // Erst initialisieren!
            manager.ProcessInitPaintMananger(new InitPaintManagerMessage { PaintContent = new Bitmap(12, 13) });
            manager.ProcessPaintSelfMessage(new PaintSelfMessage
                    {
                        Color = Color.FromArgb(1, 2, 3),
                        Point = new Point(3, 4)
                    });

            // Manager muss Malnachfrage gesendet haben
            Assert.That(receivedPaintMessage.Color, Is.EqualTo(Color.FromArgb(1, 2, 3)));
            Assert.That(receivedPaintMessage.Point, Is.EqualTo(new Point(3, 4)));
        }

        [Test]
        public void Malinhalt_nach_bemlaen_eines_Punkts_testen()
        {
            var manager = new PtPaintContentManager();
            manager.OnNewPaint += message => Assert.True(true)/*Dummyverdrahtung*/;

            // Erst initialisieren!
            manager.ProcessInitPaintMananger(new InitPaintManagerMessage { PaintContent = new Bitmap(12, 13) });
            manager.ProcessPaintSelfMessage(new PaintSelfMessage
            {
                Color = Color.FromArgb(1, 2, 3),
                Point = new Point(3, 4)
            });

            var request = new GetPaintContentRequest();
            manager.ProcessGetPaintContentRequest(request);

            // Punkt muss jetzt bemalt sein
            Assert.That(request.Result.GetPixel(3, 4), Is.EqualTo(Color.FromArgb(1, 2, 3)));
        }

        [Test]
        public void Malnachfrage_testen_bei_einem_Punkt_mit_gleicher_Farbe()
        {
            NewPaintMessage receivedPaintMessage = null;

            var manager = new PtPaintContentManager();
            manager.OnNewPaint += message => receivedPaintMessage = message;

            // Erst initialisieren!
            var content = new Bitmap(12, 13);
            content.SetPixel(3, 3, Color.FromArgb(1, 2, 3));
            manager.ProcessInitPaintMananger(new InitPaintManagerMessage { PaintContent = content });
            manager.ProcessPaintSelfMessage(new PaintSelfMessage
            {
                Color = Color.FromArgb(1, 2, 3),
                Point = new Point(3, 3)
            });

            // Manager darf bei gleicher Farbe nicht keine Malnachfrage senden,
            // da überflüssig
            Assert.IsNull(receivedPaintMessage);
        }
    }
}
