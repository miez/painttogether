
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
using PaintTogetherServer.Core;
using PaintTogetherServer.Messages.Core.PaintFieldManager;
using PaintTogetherServer.Messages.Adapter;

namespace PaintTogetherServer.Test.Core.PtPaintFieldManagerCS
{
    [TestFixture]
    public class ProcessInitMessageTest
    {
        [Test]
        public void Feld_mit_groesse_initialisieren()
        {
            var fieldMananger = new PtPaintFieldManager();
            fieldMananger.ProcessInitMessage(new InitMessage { Height = 324, Width = 123 });

            var request = new GetCurrentPaintContentRequest();
            fieldMananger.ProcessGetCurrentPaintContentRequest(request);

            Assert.That(request.Result.Height, Is.EqualTo(324));
            Assert.That(request.Result.Width, Is.EqualTo(123));
        }
    }
}
