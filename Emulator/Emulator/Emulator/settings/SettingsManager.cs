using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Emulator.settings
{
    public class SettingsManager
    {
        private static readonly string[] SignalsNames = new[] { "pusk", "vzap1", "zam1", "zam2", "chist", "op", "vib", "zapp", "pereh" };

        public static bool [] BrokenSignals = new bool[9];

        public static string MemoryPath()
        {
            try
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory , ConfigurationManager.AppSettings["MemoryFile"]);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return String.Empty;
            }
        } 

        public static bool TryInitializeSignals()
        {
            foreach (var signalName in SignalsNames)
            {
                if (!ConfigurationManager.AppSettings.AllKeys.Contains(signalName))
                    return false;
                int value;
                if (!Int32.TryParse(ConfigurationManager.AppSettings[signalName], out value))
                {
                    Console.WriteLine("Not a integer value for {0}", signalName);
                    return false;
                }
                BrokenSignals[signalName.IndexOf(signalName, StringComparison.Ordinal)] = value != 1;
            }
            return true;
        }

        public static int GetValueFromConfigOrDefaulr(string name)
        {
            return (ConfigurationManager.AppSettings.AllKeys.Contains(name))
                       ? Int32.Parse(ConfigurationManager.AppSettings[name]): 0;
        }
    }
}
