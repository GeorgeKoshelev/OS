using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Emulator.settings
{
    public class SettingsManager
    {
        public static List<string> BrokenSignals = new List<string>(); 
        private static readonly string[] SignalsNames = new[] { "pusk", "vzap1", "zam1", "zam2", "chist", "op", "vib", "zapp", "pereh" };

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
            foreach (var signalsName in SignalsNames)
            {
                if (!ConfigurationManager.AppSettings.AllKeys.Contains(signalsName))
                    return false;
                if (Int32.Parse(ConfigurationManager.AppSettings[signalsName]) != 1)
                    BrokenSignals.Add(signalsName);
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
