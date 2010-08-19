
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
    public class HasEndBlockTest
    {
        [Test]
        public void Endblock_am_Anfang()
        {
            var bytes = new List<byte>();
            bytes.AddRange(PtMessageReceiver.EndBlock);
            bytes.AddRange(new byte[] { 3, 2, 125, 2, 12, 3, 123, 54 });

            Assert.True(PtMessageReceiver.HasEndBlock(bytes));
        }

        [Test]
        public void Nur_der_Endblock()
        {
            var bytes = new List<byte>();
            bytes.AddRange(PtMessageReceiver.EndBlock);

            Assert.True(PtMessageReceiver.HasEndBlock(bytes));
        }

        [Test]
        public void Endblock_am_Ende()
        {
            var bytes = new List<byte>();
            bytes.AddRange(new byte[] { 3, 2, 125, 2, 12, 3, 123, 54 });
            bytes.AddRange(PtMessageReceiver.EndBlock);

            Assert.True(PtMessageReceiver.HasEndBlock(bytes));
        }

        [Test]
        public void Endblock_zwischendrin()
        {
            var bytes = new List<byte>();
            bytes.AddRange(new byte[] { 3, 2, 125, 2, 12, 3, 123, 54 });
            bytes.AddRange(PtMessageReceiver.EndBlock);
            bytes.AddRange(new byte[] { 3, 2, 125, 2, 12, 3, 123, 54 });

            Assert.True(PtMessageReceiver.HasEndBlock(bytes));
        }

        [Test]
        public void Endblock_geteielt()
        {
            var blockBytes = new List<byte>(PtMessageReceiver.EndBlock);
            var part1 = blockBytes.GetRange(0, 7).ToArray();
            var part2 = blockBytes.GetRange(7, PtMessageReceiver.EndBlock.Length - 7).ToArray();

            var bytes = new List<byte>();
            bytes.AddRange(new byte[] { 3, 2, 125, 2, 12, 3, 123, 54 });
            bytes.AddRange(part1);
            bytes.AddRange(part2);
            bytes.AddRange(new byte[] { 3, 2, 125, 2, 12, 3, 123, 54 });

            Assert.True(PtMessageReceiver.HasEndBlock(bytes));
        }

        [Test]
        public void Ohne_Endblock()
        {
            var bytes = new List<byte>();
            bytes.AddRange(new byte[] { 3, 2, 125, 2, 12, 3, 1, 5, 54 });
            bytes.AddRange(new byte[] { 23, 5, 41, 12, 3, 123, 54 });

            Assert.False(PtMessageReceiver.HasEndBlock(bytes));
        }
    }
}
