using Emulator.settings;

namespace Emulator.Elements
{
    class ControlSignals
    {

        public static bool Zapp { get; set; }
        public static bool Zam2 { get; set; }
        public static int Vib { get; set; }
        public static bool Pusk { get; set; }
        public static bool Zam1 { get; set; }
        public static bool Vzap1 { get; set; }
        public static bool Chist { get; set; }
        public static int Op { get; set; }
        public static bool Pereh { get; set; }

        public static void SetBrokenSignals()
        {
            if (!SettingsManager.confSignals["pusk"])
                Pusk = false;
            if (!SettingsManager.confSignals["vzap1"])
                Vzap1 = false;
            if (!SettingsManager.confSignals["zam1"])
                Zam1 = false;
            if (!SettingsManager.confSignals["zam2"])
                Zam2 = false;
            if (!SettingsManager.confSignals["chist"])
                Chist = false;
            if (!SettingsManager.confSignals["op"])
                Op = 0;
            if (!SettingsManager.confSignals["vib"])
                Vib = 0;
            if (!SettingsManager.confSignals["zapp"])
                Zapp = false;
            if (!SettingsManager.confSignals["pereh"])
                Pereh = false;
        }

    }
}
