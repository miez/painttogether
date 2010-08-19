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
    public class HasStartBlockTest
    {
        [Test]
        public void Startblock_am_Anfang_des_Bytearrays()
        {
            var bytes = new List<byte>();
            bytes.AddRange(PtMessageReceiver.StartBlock);
            bytes.AddRange(new byte[] { 3, 2, 1, 3, 51, 2, 31, 23, 125, 2, 12, 3, 123, 54 });

            Assert.True(PtMessageReceiver.HasStartBlock(bytes.ToArray()));
        }

        [Test]
        public void Ohne_Startblock()
        {
            var bytes = new byte[] { 3, 2, 1, 3, 51, 2, 31, 23, 125, 2, 12, 3, 123, 54 };

            Assert.False(PtMessageReceiver.HasStartBlock(bytes));
        }

        [Test]
        public void Startblock_mitten_drin()
        {
            var bytes = new List<byte> { 3, 2, 1, 3, 51, 2, 31, 23 };
            bytes.AddRange(PtMessageReceiver.StartBlock);
            bytes.AddRange(new byte[] { 125, 2, 12, 3, 123, 54 });

            Assert.True(PtMessageReceiver.HasStartBlock(bytes.ToArray()));
        }

        [Test]
        public void Nur_der_Startblock()
        {
            Assert.True(PtMessageReceiver.HasStartBlock(PtMessageReceiver.StartBlock));
        }
    }
}
