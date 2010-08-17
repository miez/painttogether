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
using System.Diagnostics;
using PaintTogetherStartSelector.Contracts;
using PaintTogetherStartSelector.Messages;
using System.Text;
using PaintTogetherClient.Run;

namespace PaintTogetherStartSelector
{
    /// <summary>
    /// EBC zum Start einen PT-Clients
    /// </summary>
    public class PtStartClientAdapter : IPtStartClientAdapter
    {
        /// <summary>
        /// Verarbeitet die Aufforderung einen Client zu starten
        /// </summary>
        /// <param name="message"></param>
        public void ProcessStartClientMessage(StartClientMessage message)
        {
            var clientProcess = new Process();

            clientProcess.StartInfo.FileName = "PaintTogetherClient.Run.exe";
            clientProcess.StartInfo.Arguments = CreateStartParamText(message);

            clientProcess.Start();
        }

        /// <summary>
        /// Erstellt zu den Clientstartangaben die Parameter für den Clientstart
        /// </summary>
        /// <param name="messge"></param>
        /// <returns></returns>
        internal static string CreateStartParamText(StartClientMessage messge)
        {
            var sb = new StringBuilder();
            AppendParam(sb, StartClientParams.AliasParamName, messge.Alias);
            AppendParam(sb, StartClientParams.PortParamName, messge.Port.ToString());
            AppendParam(sb, StartClientParams.ServerParamName, messge.ServernameOrIp);

            var colorText = string.Format("{0}-{1}-{2}", messge.Color.R, messge.Color.G, messge.Color.B);
            AppendParam(sb, StartClientParams.ColorParamName, colorText);

            return sb.ToString();
        }

        /// <summary>
        /// Hängt an den aktuellen Stringbuilder eine Parameterzusweisung in der Form
        /// '"paramname=value" ' an.
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="paramname"></param>
        /// <param name="value"></param>
        internal static void AppendParam(StringBuilder sb, string paramname, string value)
        {
            sb.Append('"');
            sb.Append(paramname);
            sb.Append("=");
            sb.Append(value);
            sb.Append('"');
            sb.Append(' ');
        }
    }
}
