namespace Emulator.Elements
{
    class ControlSignals
    {
        public ControlSignals()
        {
            Zapp = false;
            Zam2 = false;
            Vib = false;
            Pusk = false;
            Zam1 = false;
            Vzap1 = false;
            Chist = false;
        }

        public bool Zapp { get; set; }
        public bool Zam2 { get; set; }
        public bool Vib { get; set; }
        public bool Pusk { get; set; }
        public bool Zam1 { get; set; }
        public bool Vzap1 { get; set; }
        public bool Chist { get; set; }
    }
}
