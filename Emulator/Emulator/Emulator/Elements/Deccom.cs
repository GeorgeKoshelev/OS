using Emulator.settings;

namespace Emulator.Elements
{
    class Deccom
    {
        public static void GenerateControlSignals(int operationCode)
        {
            var command = Commands.GetCommandByOperationCode(operationCode);
            ControlSignals.Zapp = (command.P == 0 && !SettingsManager.BrokenSignals[7]);
            ControlSignals.Zam2 = (command.P != 3 && !SettingsManager.BrokenSignals[3]);
            ControlSignals.Pusk = (operationCode != 255 && !SettingsManager.BrokenSignals[0]);
            ControlSignals.Zam1 = (command.P == 1 && !SettingsManager.BrokenSignals[2]);
            ControlSignals.Vzap1 = (command.P == 3 && !SettingsManager.BrokenSignals[1]);
            ControlSignals.Chist = (!(command.P == 2 || command.P == 3) && !SettingsManager.BrokenSignals[4]);
            ControlSignals.Vib = (SettingsManager.BrokenSignals[6]) ? 0 : command.I;
        }
    }
}
