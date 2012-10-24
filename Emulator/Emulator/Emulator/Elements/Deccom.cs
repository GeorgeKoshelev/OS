namespace Emulator.Elements
{
    class Deccom
    {
        public static void GenerateControlSignals(int operationCode)
        {
            var command = Commands.GetCommandByOperationCode(operationCode);
            ControlSignals.Zapp = (command.P == 0);
            ControlSignals.Zam2 = (command.P != 3);
            ControlSignals.Pusk = (operationCode != 255);
            ControlSignals.Zam1 = (command.P == 1);
            ControlSignals.Vzap1 = (command.P == 3);
            ControlSignals.Chist = !(command.P == 2 || command.P == 3);
            ControlSignals.Vib = command.I;
        }
    }
}
