namespace Emulator.Elements
{
    class Deccom
    {
        public static ControlSignals GenerateControlSignals(int operationCode)
        {
            var command = Commands.GetCommandByOperationCode(operationCode);
            int i;
            return new ControlSignals
                                     {
                                        Zapp = (command.P == 0), Zam2 = (command.P != 3),
                                        Vib = Commands.TryGetIByOperationCode(operationCode, out i),
                                        Pusk = (operationCode != 255),
                                        Zam1 = (command.P == 1),
                                        Vzap1 = (command.P == 3),
                                        Chist = !(command.P == 2 || command.P == 3),
                                     };
        }
    }
}
