using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Emulator.settings
{
    public class SettingsManager
    {
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

        public static Dictionary<string , bool> confSignals = new Dictionary<string, bool>(); 

        private static readonly string[] signalsNames = new []{"pusk" , "vzap1" , "zam1" , "zam2" , "chist", "op" ,"vib" ,"zapp" ,"pereh"};

        public static bool HasAllSignals()
        {
            foreach (var signalsName in signalsNames)
            {
                if (!ConfigurationManager.AppSettings.AllKeys.Contains(signalsName))
                    return false;
                confSignals.Add(signalsName, (Int32.Parse(ConfigurationManager.AppSettings[signalsName]) == 1));
            }
            return true;
        }

        public static int GetIRValueFromConfigOrDefault()
        {
            return (ConfigurationManager.AppSettings.AllKeys.Contains("IR"))
                       ? Int32.Parse(ConfigurationManager.AppSettings["IR"])
                       : 0;
        }

        public static int GetRVVValueFromConfigOrDefault()
        {
            return (ConfigurationManager.AppSettings.AllKeys.Contains("RVV"))
                       ? Int32.Parse(ConfigurationManager.AppSettings["RVV"])
                       : 0;
        }

        public static int GetRONprznkValueFromConfigOrDefault()
        {
            return (ConfigurationManager.AppSettings.AllKeys.Contains("RON.prznk"))
                       ? Int32.Parse(ConfigurationManager.AppSettings["RON.prznk"])
                       : 0;
        }

        public static int GetRONsumValueFromConfigOrDefault()
        {
            return (ConfigurationManager.AppSettings.AllKeys.Contains("RON.sum"))
                       ? Int32.Parse(ConfigurationManager.AppSettings["RON.sum"])
                       : 0;
        }
    }
}
