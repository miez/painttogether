
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
using System.IO;
using PaintTogetherClient.Messages.Portal;

namespace PaintTogetherClient.Test.Core.PtPictureTakerCS
{
    [TestFixture]
    public class ProcessTakePictureTest
    {
        [Test]
        public void Nicht_vorhandene_Datei()
        {
            var pTaker = new PtPictureTaker();

            var toSaveImage = new Bitmap(10, 10);
            toSaveImage.SetPixel(5, 5, Color.FromArgb(23, 123, 4));

            pTaker.OnRequestPaintContent += request => request.Result = toSaveImage;

            File.Delete("nicht_da.png");

            var tPRequest = new TakePictureRequest();
            tPRequest.Filename = "nicht_da.png";
            pTaker.ProcessTakePictureRequest(tPRequest);

            var readedImage = Image.FromFile("nicht_da.png") as Bitmap;

            Assert.That(readedImage.Height, Is.EqualTo(toSaveImage.Height));
            Assert.That(readedImage.Height, Is.EqualTo(toSaveImage.Width));
            Assert.That(readedImage.GetPixel(5, 5), Is.EqualTo(Color.FromArgb(23, 123, 4)));
        }
    }
}
