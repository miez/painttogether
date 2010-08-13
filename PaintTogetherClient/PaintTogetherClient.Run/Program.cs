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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using log4net.Config;
using log4net.Appender;

namespace PaintTogetherClient.Run
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Client client = null;
            try
            {
                //var fileAppender = new FileAppender();
                //fileAppender.AppendToFile = false;
                //fileAppender.File = "client.log";
                //fileAppender.

                //BasicConfigurator.Configure(fileAppender);

                var argList = new List<string>();
                argList.AddRange(args);

                if (argList.Contains("-help") || argList.Contains("-h"))
                {
                    ShowHelp();
                    return;
                }

                var startParams = StartClientParams.ParseArgs(argList);

                if (!startParams.Valid)
                {
                    MessageBox.Show("Ungültige Startparameter");
                    ShowHelp();
                    return;
                }

                client = new Client();
                client.Start(startParams);
            }
            catch (Exception e)
            {
                MessageBox.Show("Fehler beim Start des Clients: " + e.Message);
                return;
            }

            Application.Run(client.Portal as PtClientPortal);
        }

        private static void ShowHelp()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Startparameterbeschreibung des PaintTogetherClient");
            sb.AppendLine(string.Concat("  ", StartClientParams.AliasParamName, " (pflicht) Ihr Alias für die anderen Beteiligten"));
            sb.AppendLine(string.Concat("  ", StartClientParams.ColorParamName, " (pflicht) Bekannter Farbenname, Ihre Malfarbe"));
            sb.AppendLine(string.Concat("  ", StartClientParams.PortParamName, " (optional) Port des Servers - Standard ist '", StartClientParams.DefaultPort, "'"));
            sb.AppendLine(string.Concat("  ", StartClientParams.ServerParamName, " (optional) Name/IP des Servers - Standard ist '", StartClientParams.DefaultServer, "'"));

            MessageBox.Show(sb.ToString(), "Hilfe", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
