using Emulator.settings;

namespace Emulator.Elements
{
    class Rvv
    {
// ReSharper disable InconsistentNaming
        private static readonly int _rvv = SettingsManager.GetValueFromConfigOrDefaulr("RVV");
// ReSharper restore InconsistentNaming
        public static int RVV { get { return _rvv; }}
    }
}
