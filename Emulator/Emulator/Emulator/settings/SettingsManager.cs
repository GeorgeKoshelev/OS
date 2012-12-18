using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Emulator.settings
{
    public class SettingsManager
    {
        private static readonly string[] SignalsNames = new[] { "pusk", "vzap1", "zam1", "zam2", "chist", "op", "vib", "zapp", "pereh" };
        public static Dictionary<string, bool> BrokenSignals = new Dictionary<string, bool>();

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

        public static bool HasAllSignals()
        {
            foreach (var signalName in SignalsNames)
            {
                if (!ConfigurationManager.AppSettings.AllKeys.Contains(signalName))
                    return false;
                if (!BrokenSignals.ContainsKey(signalName))
                    BrokenSignals.Add(signalName , Int32.Parse(ConfigurationManager.AppSettings[signalName]) != 1);
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
