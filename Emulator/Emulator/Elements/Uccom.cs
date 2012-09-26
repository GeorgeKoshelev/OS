namespace Emulator.Elements
{
    class Uccom
    {
        private static int _uccom = 0;
        public static int UCCOM { get { return _uccom; } set { if (ControlSignals.Pusk) _uccom = value; } }
    }
}
