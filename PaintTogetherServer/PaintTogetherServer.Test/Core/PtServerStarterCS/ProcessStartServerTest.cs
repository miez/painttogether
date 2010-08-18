
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

using System;
using NUnit.Framework;
using PaintTogetherServer.Core;
using log4net;
using PaintTogetherServer.Messages.Core;
using PaintTogetherServer.Messages.Portal;
using System.Collections.Generic;
using PaintTogetherServer.Messages.Core.PaintFieldManager;
using PaintTogetherServer.Messages.Adapter;

namespace PaintTogetherServer.Test.Core.PtServerStarterCS
{
    [TestFixture]
    public class ProcessStartServerTest
    {
        [Test]
        public void Startnachricht_senden()
        {
            var height = 233;
            var width = 412;
            var port = 12342;

            InitMessage initMessage = null;
            InitAdapterMessage initAdapterMessage = null;

            var starter = new PtServerStarter();
            starter.OnInit += message => initMessage = message;
            starter.OnInitAdapter += message => initAdapterMessage = message;

            starter.ProcessStartServerMessage(new StartServerMessage { Height = height, Width = width, Port = port });

            Assert.That(initMessage.Height, Is.EqualTo(height));
            Assert.That(initMessage.Width, Is.EqualTo(width));
            Assert.That(initAdapterMessage.Port, Is.EqualTo(port));
        }
    }
}
