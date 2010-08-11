
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
using System.Collections.Generic;

namespace PaintTogetherCommunicater.Test.PtMessageReceiverCS
{
    [TestFixture]
    public class FindBlockTest
    {
        [Test]
        public void Block_am_Anfang()
        {
            var content = new List<byte>(new byte[] { 32, 12, 34, 23, 24, 52 });
            var toFindBlock = new byte[] { 32, 12, 34 };
            var result = PtMessageReceiver.FindBlock(content, toFindBlock);

            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Block_in_der_Mitte()
        {
            var content = new List<byte>(new byte[] { 32, 12, 34, 23, 24, 52 });
            var toFindBlock = new byte[] { 34, 23, 24 };
            var result = PtMessageReceiver.FindBlock(content, toFindBlock);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void Block_am_Ende()
        {
            var content = new List<byte>(new byte[] { 32, 12, 34, 23, 24, 52 });
            var toFindBlock = new byte[] { 24, 52 };
            var result = PtMessageReceiver.FindBlock(content, toFindBlock);

            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        public void Ohne_Block()
        {
            var content = new List<byte>(new byte[] { 32, 12, 34, 23, 24, 52 });
            var toFindBlock = new byte[] { 24, 52, 23 };
            var result = PtMessageReceiver.FindBlock(content, toFindBlock);

            Assert.That(result, Is.EqualTo(-1));
        }
    }
}
