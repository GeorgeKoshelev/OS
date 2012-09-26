using System;
using System.Collections.Generic;

namespace Emulator.Elements
{
    class Commands
    {
        private static readonly Dictionary<int, Command> _commands = new Dictionary<int, Command>
        {
            {00 , new Command{OperationCode = 00 , I = -1 , P = 0 , Op = 0}},
            {11 , new Command{OperationCode = 11 , I = 0 , P = 1 , Op = 1}},
            {15 , new Command{OperationCode = 15 , I = 1 , P = 1 , Op = 1}},
            {02 , new Command{OperationCode = 02 , I = -1 , P = 2 , Op = 0}},
            {21 , new Command{OperationCode = 21 , I = 0 , P = 1 , Op = 2}},
            {25 , new Command{OperationCode = 25 , I = 1 , P = 1 , Op = 2}},
            {31 , new Command{OperationCode = 31 , I = 0 , P = 1 , Op = 3}},
            {254 , new Command{OperationCode = 254 , I = -1 , P = 4 , Op = 15}},
            {240 , new Command{OperationCode = 240 , I = -1 , P = 4 , Op = 15}},
            {241 , new Command{OperationCode = 241 , I = -1 , P = 4 , Op = 15}},
            {244 , new Command{OperationCode = 244 , I = -1 , P = 4 , Op = 15}},
            {245 , new Command{OperationCode = 245 , I = -1 , P = 4 , Op = 15}},
            {255 , new Command{OperationCode = 255 , I = -1 , P = 4 , Op = 15}},
        };

        private static readonly Dictionary<int, int> _i = new Dictionary<int, int>
        {
            {11 , 0} , {15 , 1} , {21 , 0} , {25 , 1} , {31 , 0}
        }; 

        public static Command GetCommandByOperationCode(int operationCode)
        {
            if (_commands.ContainsKey(operationCode))
                return _commands[operationCode];
            throw new Exception(string.Format("Invalid operation code : {0}" , operationCode));
        }

        public static bool TryGetIByOperationCode(int operationCode , out int i)
        {
            i = -1;
            if (!_i.ContainsKey(operationCode)) return false;
            i = _i[operationCode];
            return true;
        }

    }

    public struct Command
    {
        public int OperationCode { get; set; }
        public int I { get; set; }
        public int P { get; set; }
        public int Op { get; set; }
    }

}
