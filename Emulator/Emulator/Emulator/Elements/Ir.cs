using Emulator.settings;

namespace Emulator.Elements
{
    internal class Ir
    {
        private static int _ir = SettingsManager.GetIRValueFromConfigOrDefault();
        public static int IR { get { return _ir; } set { _ir = (ControlSignals.Zam2) ? value : 0; } }
    }
}
