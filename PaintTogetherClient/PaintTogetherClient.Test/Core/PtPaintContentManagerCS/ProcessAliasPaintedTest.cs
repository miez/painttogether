
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
    public class ProcessAliasPaintedTest
    {
        [Test]
        public void Malsingalisierung_bei_neu_bemaltem_Punkt_testen()
        {
            PaintedMessage receivedPaintedMessage = null;

            var manager = new PtPaintContentManager();
            manager.OnPainted += message => receivedPaintedMessage = message;

            // Erst initialisieren!
            manager.ProcessInitPaintMananger(new InitPaintManagerMessage { PaintContent = new Bitmap(12, 13) });
            manager.ProcessAliasPaintedMessage(new AliasPaintedMessage()
                    {
                        Color = Color.FromArgb(1, 2, 3),
                        StartPoint = new Point(3, 4),
                        EndPoint = new Point(2, 1)
                    });

            // Manager muss Malnbestätigung gesendet haben
            Assert.That(receivedPaintedMessage.Color, Is.EqualTo(Color.FromArgb(1, 2, 3)));
            Assert.That(receivedPaintedMessage.StartPoint, Is.EqualTo(new Point(3, 4)));
            Assert.That(receivedPaintedMessage.EndPoint, Is.EqualTo(new Point(2, 1)));
        }

        [Test]
        public void Malinhalt_nach_bemalen_eines_Punkts_testen()
        {
            var manager = new PtPaintContentManager();
            manager.OnPainted += message => Assert.True(true)/*Dummyverdrahtung*/;

            // Erst initialisieren!
            manager.ProcessInitPaintMananger(new InitPaintManagerMessage { PaintContent = new Bitmap(12, 13) });
            manager.ProcessAliasPaintedMessage(new AliasPaintedMessage()
            {
                Color = Color.FromArgb(1, 2, 3),
                StartPoint = new Point(3, 4),
                EndPoint = new Point(2, 1)
            });

            var request = new GetPaintContentRequest();
            manager.ProcessGetPaintContentRequest(request);

            // Punkt muss jetzt bemalt sein
            Assert.That(request.Result.GetPixel(3, 4), Is.EqualTo(Color.FromArgb(1, 2, 3)));
        }
    }
}
