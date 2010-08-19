
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
using System.Collections.Generic;
using PaintTogetherServer.Messages.Adapter;
using System.Drawing;

namespace PaintTogetherServer.Test.Core.PtPlayerListManagerCS
{
    [TestFixture]
    public class ProcessGetCurPainterTest
    {
        [Test]
        public void mehrere_unterschiedliche_Beteiligte()
        {
            var ptPlayerManager = new PtPlayerListManager();
            ptPlayerManager.OnNotifyNewClient += message => Assert.True(true)/*Sonst gibts ne NRE, wenn das Event nicht behandelt wird*/;
            ptPlayerManager.OnNotifyClientDisconnected += message => Assert.True(true)/*Sonst gibts ne NRE, wenn das Event nicht behandelt wird*/;

            ptPlayerManager.ProcessNewClientMessage(new NewClientConnectedMessage { Alias = "Eco1", Color = Color.Beige });
            ptPlayerManager.ProcessNewClientMessage(new NewClientConnectedMessage { Alias = "Eco2", Color = Color.Black });

            var request = new GetCurrentPainterRequest();
            ptPlayerManager.ProcessGetCurrentPainterRequest(request);

            Assert.That(request.Result[0].Key, Is.EqualTo("Eco1"));
            Assert.That(request.Result[0].Value, Is.EqualTo(Color.Beige));

            Assert.That(request.Result[1].Key, Is.EqualTo("Eco2"));
            Assert.That(request.Result[1].Value, Is.EqualTo(Color.Black));
        }
    }
}
