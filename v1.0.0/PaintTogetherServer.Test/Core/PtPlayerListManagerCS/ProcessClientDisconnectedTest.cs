
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
    public class ProcessClientDisconnectedTest
    {
        [Test]
        public void eine_vorhandene_Clientverbindung_entfernen()
        {
            NotifyClientDisconnectedMessage lostClientMessage = null;
            var ptPlayerManager = new PtPlayerListManager();
            ptPlayerManager.OnNotifyNewClient += message => Assert.True(true)/*Sonst gibts ne NRE, wenn das Event nicht behandelt wird*/;
            ptPlayerManager.OnNotifyClientDisconnected += message => lostClientMessage = message;

            ptPlayerManager.ProcessNewClientMessage(new NewClientConnectedMessage { Alias = "Pier", Color = Color.PaleGreen });
            ptPlayerManager.ProcessClientDisconnectedMessage(new ClientDisconnectedMessage { Alias = "Pier", Color = Color.PaleGreen });

            Assert.That(lostClientMessage.Alias, Is.EqualTo("Pier"));
            Assert.That(lostClientMessage.Color, Is.EqualTo(Color.PaleGreen));
        }

        [Test]
        public void eine_nicht_vorhandene_clientverbindung_entfernen()
        {
            NotifyClientDisconnectedMessage lostClientMessage = null;
            var ptPlayerManager = new PtPlayerListManager();
            ptPlayerManager.OnNotifyNewClient += message => Assert.True(true)/*Sonst gibts ne NRE, wenn das Event nicht behandelt wird*/;
            ptPlayerManager.OnNotifyClientDisconnected += message => lostClientMessage = message;

            ptPlayerManager.ProcessNewClientMessage(new NewClientConnectedMessage { Alias = "Pier", Color = Color.PaleGreen });
            ptPlayerManager.ProcessClientDisconnectedMessage(new ClientDisconnectedMessage { Alias = "Pier123", Color = Color.PaleGreen });

            Assert.That(lostClientMessage, Is.Null);
        }
    }
}
