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

using System.Drawing;
using System.Windows.Forms;

namespace PaintTogetherClient.Portal
{
    public sealed class PaintContentPanel : Panel
    {
        /// <summary>
        /// Der aktuelle Malbereich
        /// </summary>
        private Bitmap _paintContent;

        /// <summary>
        /// Die Grafik, die den Malbereich darstellt
        /// </summary>
        private Graphics _paintGraph;

        public PaintContentPanel()
        {
            DoubleBuffered = true;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);

            if (_paintContent == null)
            {
                return;
            }

            pevent.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pevent.Graphics.DrawImage(_paintContent, 0, 0);
        }

        internal void InitPaintContent(Bitmap bitmap)
        {
            Size = bitmap.Size;
            MinimumSize = bitmap.Size;
            MaximumSize = bitmap.Size;

            // Kopieren, verhindert das Bild von außen geändert wird
            _paintContent = new Bitmap(bitmap);
            _paintGraph = Graphics.FromImage(_paintContent);

            Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startP"></param>
        /// <param name="endP"></param>
        /// <param name="color"></param>
        internal void PaintLine(Point startP, Point endP, Color color)
        {
            using (var pen = new Pen(color, 1f))
            {
                _paintGraph.DrawLine(pen, startP, endP);
            }

            Invalidate(); // Fordert zum Neumalen des Malbereichs auf
        }
    }
}
