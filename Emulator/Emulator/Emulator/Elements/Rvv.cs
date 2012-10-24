using Emulator.settings;

namespace Emulator.Elements
{
    class Rvv
    {
        private static readonly int _rvv = SettingsManager.GetRVVValueFromConfigOrDefault();
        public static int RVV { get { return _rvv; }}
        
        //TODO: ADD READ AND WRITE METHODS
    }
}
