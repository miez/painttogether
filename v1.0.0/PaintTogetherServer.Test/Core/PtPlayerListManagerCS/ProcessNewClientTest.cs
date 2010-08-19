
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
using PaintTogetherServer.Messages.Portal;
using System.Collections.Generic;
using PaintTogetherServer.Messages.Adapter;
using System.Drawing;

namespace PaintTogetherServer.Test.Core.PtPlayerListManagerCS
{
    [TestFixture]
    public class ProcessNewClientTest
    {
        [Test]
        public void eine_neue_client_verbindung()
        {
            NotifyNewClientMessage newClientMessage = null;
            var ptPlayerManager = new PtPlayerListManager();
            ptPlayerManager.OnNotifyNewClient += message => newClientMessage = message;

            ptPlayerManager.ProcessNewClientMessage(new NewClientConnectedMessage { Alias = "Pier", Color = Color.PaleGreen });

            Assert.That(newClientMessage.Alias, Is.EqualTo("Pier"));
            Assert.That(newClientMessage.Color, Is.EqualTo(Color.PaleGreen));
        }

        [Test]
        public void drei_neue_Beteiligte()
        {
            var newClientMessages = new List<NotifyNewClientMessage>();
            var ptPlayerManager = new PtPlayerListManager();
            ptPlayerManager.OnNotifyNewClient += message => newClientMessages.Add(message);

            ptPlayerManager.ProcessNewClientMessage(new NewClientConnectedMessage { Alias = "Pier", Color = Color.PaleGreen });
            ptPlayerManager.ProcessNewClientMessage(new NewClientConnectedMessage { Alias = "Lorena", Color = Color.DimGray });
            ptPlayerManager.ProcessNewClientMessage(new NewClientConnectedMessage { Alias = "Pier", Color = Color.PaleGreen });

            Assert.That(newClientMessages[0].Alias, Is.EqualTo("Pier"));
            Assert.That(newClientMessages[0].Color, Is.EqualTo(Color.PaleGreen));

            Assert.That(newClientMessages[1].Alias, Is.EqualTo("Lorena"));
            Assert.That(newClientMessages[1].Color, Is.EqualTo(Color.DimGray));

            // Gucken ob das doppelte Hinzufügen von Pier auch möglich ist
            Assert.That(newClientMessages[2].Alias, Is.EqualTo("Pier"));
            Assert.That(newClientMessages[2].Color, Is.EqualTo(Color.PaleGreen));
        }
    }
}
