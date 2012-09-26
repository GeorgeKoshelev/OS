namespace Emulator.Elements
{
    class Alu
    {
        public static bool pr;

        public static int Calculate(byte operationCode , int sum , int resM)
        {
            var command = Commands.GetCommandByOperationCode(operationCode);
            int res;
            switch (command.P)
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
                    pr = true;
                    break;
                case 240:
                    pr = (res == 0);
                    break;
                case 241:
                    pr = (res > 0);
                    break;
                default:
                    pr = false;
                    break;
            }
            return res;
        }
    }
}
