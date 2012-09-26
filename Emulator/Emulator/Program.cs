using System;
using System.Text;
using Emulator.Elements;
using Emulator.settings;

namespace Emulator
{
    class Program
    {
        static void Main()
        {
            if (!SettingsManager.HasAllSignals())
            {
                Console.WriteLine( "You should set all signals [pusk|vzap1|zam1|zam2|chist|op|vib|zapp|pereh] in app.config (1 is ok , 0 is broken)");
                return;
            }
            var pathToMemory = SettingsManager.MemoryPath();
            if (pathToMemory.Equals(String.Empty)) return;
            var memoryManager = new MemoryManager(pathToMemory);
            if (!memoryManager.TryInitFromFile())
            {
                Console.WriteLine("Can't init from memory file , exit");
                return;
            }
            var ip = 0;
            while(true)
            {
                byte addrByte;
                byte operationByte;
                if (!(memoryManager.TryGetByte(ip, out operationByte) && memoryManager.TryGetByte(ip + 1, out addrByte)))
                    break;
                Deccom.GenerateControlSignals(operationByte);
                ControlSignals.SetBrokenSignals();
                var IA = addrByte + Ir.IR;
                byte SP;
                if (!memoryManager.TryGetByte(IA , out SP))
                    throw new Exception(string.Format("Process error , no data by offset {0}" , IA));
                var res = M.Calculate(SP, IA, Rvv.RVV);
                var aluRes = Alu.Calculate(operationByte, Ron.Sum, res);

                var mRes = M.Calculate(aluRes);
                if (ControlSignals.Zam2)
                    Ir.IR = mRes;

                if (ControlSignals.Zam1)
                    Ron.Set(aluRes , Alu.pr , operationByte);

                if (ControlSignals.Zapp)
                    memoryManager.TryWrite((byte) aluRes, IA);

                if (!ControlSignals.Pusk)
                    break;
                ip = M.NextOffset(IA, ip, operationByte);
            }
            var sb = new StringBuilder("Memory:\n");
            sb.Append(memoryManager);
            sb.AppendFormat("IP : {0}\n", ip);
            sb.AppendFormat("RON : {0:x2}\n", Ron.Sum);
            sb.AppendFormat("IR : {0:x2}\n", Ir.IR);
            Console.WriteLine(sb);
        }
    }
}
