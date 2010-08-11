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

namespace PaintTogetherServer.Run
{
    /// <summary>
    /// Startparameter für den Serverstart
    /// </summary>
    public class StartServerParams
    {
        public const int MinWidth = 500;   // siehe Anforderung/Definition
        public const int MaxWidth = 1000;    // siehe Anforderung/Definition
        public const int MinHeight = 200;   // siehe Anforderung/Definition
        public const int MaxHeight = 500;   // siehe Anforderung/Definition
        public const int MinPort = 1001;   // die ersten 1000 sollte man nicht verwenden, da standardisiert
        public const int MaxPort = 99999;

        public const int DefaultHeigth = 350;   // siehe Entwurf
        public const int DefaultPort = 6969;    // siehe Entwurf
        public const int DefaultWidth = 750;    // siehe Entwurf
        public const string DefaultAlias = "Unbekannt";

        public const string PortParamName = "-port";
        public const string HeightParamName = "-height";
        public const string WidthParamName = "-width";
        public const string AliasParamName = "-alias";

        public int Port { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string Alias { get; set; }
        public bool Valid { get; set; }

        /// <summary>
        /// Erzeugt aus den Argumenten ein StartParameterobjekt
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static StartServerParams ParseArgs(List<string> args)
        {
            var result = new StartServerParams { Valid = false };

            if ((result.Width = ValidateIntValue(args, WidthParamName, MinWidth, MaxWidth, DefaultWidth)) == 0) return result;
            if ((result.Height = ValidateIntValue(args, HeightParamName, MinHeight, MaxHeight, DefaultHeigth)) == 0) return result;
            if ((result.Port = ValidateIntValue(args, PortParamName, MinPort, MaxPort, DefaultPort)) == 0) return result;
            if (string.IsNullOrEmpty(result.Alias = ValidateAlias(args))) return result;

            result.Valid = true;
            return result;
        }

        /// <summary>
        /// Ermittelt den in den Startargumenten angegebenen Alias
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static string ValidateAlias(List<string> args)
        {
            var result = GetParameterValue(args, AliasParamName);
            if (string.IsNullOrEmpty(result))
            {
                Console.WriteLine("Alias nicht angegeben, Standardalias wird verwendet");
                result = DefaultAlias;
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
