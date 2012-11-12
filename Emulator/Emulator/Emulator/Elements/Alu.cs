using System;

namespace Emulator.Elements
{
    class Alu
    {
        public static bool Pr;

        public static int Calculate(byte operationCode , int sum , int resM)
        {
            var command = Commands.GetCommandByOperationCode(operationCode);
            int res;
            switch (command.Op)
            {
                case 0:
                    res = sum;
                    break;
                case 1:
                    res = resM;
                    break;
                case 2:
                    res = resM + sum;
                    break;
                case 3:
                    res = resM - sum;
                    break;
                default:
                    res = resM;
                    break;
            }
            switch (operationCode)
            {
                case 254:
                    Pr = true;
                    break;
                case 240:
                    Pr = (res == 0);
                    break;
                case 241:
                    Pr = (res > 0);
                    break;
                default:
                    Pr = false;
                    break;
            }
            return res;
        }
    }
}
