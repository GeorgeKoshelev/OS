﻿using System;
using System.Collections.Generic;

namespace Emulator.Elements
{
    class Commands
    {
// ReSharper disable InconsistentNaming
        private static readonly Dictionary<int, Command> _commands = new Dictionary<int, Command>
// ReSharper restore InconsistentNaming
        {
            {00 , new Command{OperationCode = 00 , I = 0 , P = 0 , Op = 0}},
            {17 , new Command{OperationCode = 17 , I = 0 , P = 1 , Op = 1}},
            {21 , new Command{OperationCode = 21 , I = 1 , P = 1 , Op = 1}},
            {02 , new Command{OperationCode = 02 , I = 0 , P = 2 , Op = 0}},
            {33 , new Command{OperationCode = 33 , I = 0 , P = 1 , Op = 2}},
            {37 , new Command{OperationCode = 37 , I = 1 , P = 1 , Op = 2}},
            {49 , new Command{OperationCode = 49 , I = 0 , P = 1 , Op = 3}},
            {254 , new Command{OperationCode = 254 , I = 0 , P = 4 , Op = 15}},
            {240 , new Command{OperationCode = 240 , I = 0 , P = 4 , Op = 15}},
            {241 , new Command{OperationCode = 241 , I = 0 , P = 4 , Op = 15}},
            {244 , new Command{OperationCode = 244 , I = 0 , P = 4 , Op = 15}},
            {245 , new Command{OperationCode = 245 , I = 0 , P = 4 , Op = 15}},
            {255 , new Command{OperationCode = 255 , I = 0 , P = 4 , Op = 15}},
        };

        public static Command GetCommandByOperationCode(int operationCode)
        {
            if (_commands.ContainsKey(operationCode))
                return _commands[operationCode];
            throw new Exception(string.Format("Invalid operation code : {0}" , operationCode));
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
