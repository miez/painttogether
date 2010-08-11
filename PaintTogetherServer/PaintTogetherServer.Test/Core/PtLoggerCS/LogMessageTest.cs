
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
using System.Net;
using System.Net.Sockets;
using NUnit.Framework;
using PaintTogetherServer.Adapter;
using PaintTogetherServer.Messages.Adapter;
using System.Threading;
using PaintTogetherServer.Core;
using log4net;
using PaintTogetherServer.Messages.Portal;
using System.Collections.Generic;

namespace PaintTogetherServer.Test.Core.PtLoggerCS
{
    [TestFixture]
    public class LogMessageTest
    {
        [Test]
        public void eine_einfache_Lognachricht()
        {
            var receivedMessages = new List<SLogMessage>();

            var logger = new PtLogger();
            logger.OnSLog += message => receivedMessages.Add(message);

            LogManager.GetLogger("Keks").Debug("Nachricht");

            Assert.That(receivedMessages[0].Message, Contains.Substring("Nachricht"));
            Assert.That(receivedMessages[0].Message, Contains.Substring("Keks"));
        }

        [Test]
        public void eine_Nachricht_mit_Exception()
        {
            var receivedMessages = new List<SLogMessage>();

            var logger = new PtLogger();
            logger.OnSLog += message => receivedMessages.Add(message);

            LogManager.GetLogger("Keks").Debug("Nachricht", new Exception("Fehler beim Lachen"));

            Assert.That(receivedMessages[0].Message, Contains.Substring("Nachricht"));
            Assert.That(receivedMessages[0].Message, Contains.Substring("Keks"));
            Assert.That(receivedMessages[1].Message, Contains.Substring("Fehler beim Lachen"));
        }
    }
}
