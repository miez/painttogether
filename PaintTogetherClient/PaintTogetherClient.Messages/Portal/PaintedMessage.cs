﻿/* $HeadURL: http://ws201075/svn/deg/dotNet/trunk/Pegasus/src/HostApplicationForm.cs $
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

namespace PaintTogetherClient.Messages.Portal
{
    /// <summary>
    /// Signalisierung über einen neu bemalten Strich
    /// </summary>
    public class PaintedMessage
    {
        /// <summary>
        /// Startpunkt für Strich
        /// </summary>
        public Point StartPoint { get; set; }

        /// <summary>
        /// Endpunkt für Strich
        /// </summary>
        public Point EndPoint { get; set; }

        /// <summary>
        /// Malfarbe
        /// </summary>
        public Color Color { get; set; }
    }
}
