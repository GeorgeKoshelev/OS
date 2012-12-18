using Emulator.settings;

namespace Emulator.Elements
{
    class Deccom
    {
        public static void GenerateControlSignals(int operationCode)
        {
            var command = Commands.GetCommandByOperationCode(operationCode);
            ControlSignals.Zapp = (command.P == 0 && !SettingsManager.BrokenSignals["zapp"]);
            ControlSignals.Zam2 = (command.P != 3 && !SettingsManager.BrokenSignals["zam2"]);
            ControlSignals.Pusk = (operationCode != 255 && !SettingsManager.BrokenSignals["pusk"]);
            ControlSignals.Zam1 = (command.P == 1 && !SettingsManager.BrokenSignals["zam1"]);
            ControlSignals.Vzap1 = (command.P == 3 && !SettingsManager.BrokenSignals["vzap1"]);
            ControlSignals.Chist = (!(command.P == 2 || command.P == 3) && !SettingsManager.BrokenSignals["chist"]);
            ControlSignals.Vib = (SettingsManager.BrokenSignals["vib"]) ? 0 : command.I;
        }
    }
}
