using Emulator.settings;

namespace Emulator.Elements
{
    class Ron
    {
        
        private static int _sum = SettingsManager.GetRONsumValueFromConfigOrDefault();
        public static int Sum { get { return _sum; }}


        public static bool Prznk { get; private set; }

        public static void Set(int res1 , bool pr)
        {
            if (!ControlSignals.Zam1) return;
            _sum = res1;
            Prznk = pr;
        }
    }
}
