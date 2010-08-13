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
using System.Drawing;

namespace PaintTogetherClient.Run
{
    /// <summary>
    /// Startparameter für den Clientstart
    /// </summary>
    public class StartClientParams
    {
        public const int MinPort = 1001;   // die ersten 1000 sollte man nicht verwenden, da standardisiert
        public const int MaxPort = 99999;

        public const int DefaultPort = 6969;    // siehe Entwurf
        public const string DefaultServer = "localhost";

        public const string PortParamName = "-port";
        public const string ColorParamName = "-color";
        public const string ServerParamName = "-server";
        public const string AliasParamName = "-alias";

        public int Port { get; set; }
        public Color Color { get; set; }
        public string Server { get; set; }
        public string Alias { get; set; }
        public bool Valid { get; set; }

        /// <summary>
        /// Erzeugt aus den Argumenten ein ClientParameterobjekt
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static StartClientParams ParseArgs(List<string> args)
        {
            var result = new StartClientParams { Valid = false };

            if (string.IsNullOrEmpty(result.Server = ValidateServer(args))) return result;
            if ((result.Color = ValidateColor(args)) == Color.Empty) return result;
            if ((result.Port = ValidateIntValue(args, PortParamName, MinPort, MaxPort, DefaultPort)) == 0) return result;
            if (string.IsNullOrEmpty(result.Alias = GetParameterValue(args, AliasParamName))) return result;

            result.Valid = true;
            return result;
        }

        private static Color ValidateColor(List<string> args)
        {
            var sValue = GetParameterValue(args, ColorParamName);
            if (string.IsNullOrEmpty(sValue))
            {
                Console.WriteLine("Keine Malfarbe angegeben");
                return Color.Empty;
            }
            return Color.FromName(sValue);
        }

        private static string ValidateServer(List<string> args)
        {
            var result = GetParameterValue(args, ServerParamName);
            if (string.IsNullOrEmpty(result))
            {
                Console.WriteLine("Server nicht angegeben, Standardserver (localhost) wird verwendet");
                result = DefaultServer;
            }
            return result;
        }

        private static int ValidateIntValue(List<string> args, string name, int min, int max, int defaultValue)
        {
            int result;
            var stringValue = GetParameterValue(args, name);
            if (string.IsNullOrEmpty(stringValue))
            {
                Console.WriteLine(string.Format("{0} nicht angegeben, Standard '{1}' wird verwendet", name, defaultValue));
                result = defaultValue;
            }
            else if (!Int32.TryParse(stringValue, out result))
            {
                Console.WriteLine(string.Format("Fehler!!! Keine Zahl für {0} angegeben", name));
                return 0;
            }

            if (result < min || result > max)
            {
                Console.WriteLine(string.Format("Fehler!!! {0} außerhalb des gültigen Bereichs", name));
                return 0;
            }

            return result;
        }

        /// <summary>
        /// Liefert einem den Wert der für einen Parameternamen in der
        /// Liste enthalten ist oder null.
        /// Beispiel: suche nach "-alias"
        /// liefert bei Listenwert "-alias=Bert" -> "Bert"
        /// </summary>
        /// <param name="args"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetParameterValue(List<string> args, string name)
        {
            string result = null;

            foreach (var curParam in args)
            {
                if (curParam.StartsWith(name))
                {
                    result = curParam;
                    break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return result;
            }

            return result.Substring(name.Length + 1);
        }
    }
}
