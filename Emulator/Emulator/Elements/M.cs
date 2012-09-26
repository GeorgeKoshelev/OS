namespace Emulator.Elements
{
    class M
    {
        public static int Calculate(int SP , int IA , int RVV)
        {
            return ControlSignals.Vib == 0 ? SP : IA;
        }

        public static void CalculateUCCOM(int IA , int ADDRCOM)
        {
            Uccom.UCCOM = (ControlSignals.Pereh) ? IA : ADDRCOM + 2;
        }
        public static int Calculate(int res)
        {
            return !ControlSignals.Chist ? res : 0;
        }
    }
}
