using Emulator.settings;

namespace Emulator.Elements
{
    class Deccom
    {
        public static void GenerateControlSignals(int operationCode)
        {
            var command = Commands.GetCommandByOperationCode(operationCode);
            ControlSignals.Zapp = (command.P == 0 && !SettingsManager.BrokenSignals.Contains("zapp"));
            ControlSignals.Zam2 = (command.P != 3 && !SettingsManager.BrokenSignals.Contains("zam2"));
            ControlSignals.Pusk = (operationCode != 255 && !SettingsManager.BrokenSignals.Contains("pusk"));
            ControlSignals.Zam1 = (command.P == 1 && !SettingsManager.BrokenSignals.Contains("zam1"));
            ControlSignals.Vzap1 = (command.P == 3 && !SettingsManager.BrokenSignals.Contains("vzap1"));
            ControlSignals.Chist = (!(command.P == 2 || command.P == 3) && !SettingsManager.BrokenSignals.Contains("chist"));
            ControlSignals.Vib = (SettingsManager.BrokenSignals.Contains("vib")) ? 0 : command.I;
        }
    }
}
