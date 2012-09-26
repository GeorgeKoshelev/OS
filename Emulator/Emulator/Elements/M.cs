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

        public static int NextOffset (int AI , int ip , byte operByte)
        {
            switch (operByte)
            {
                case 240:
                    return (Ron.Sum == 0) ? AI : ip+2;                    
                case 241:
                    return (Ron.Sum > 0) ? AI : ip+2; //INFO: в табличке сказано что при sum>0 ip+2, как-то подозрительно
                case 254:
                    return AI;
                default:
                    return ip + 2;
            }
        }
    }
}
