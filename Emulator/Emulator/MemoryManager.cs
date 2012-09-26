using System;
using System.IO;
using System.Text;

namespace Emulator
{
    class MemoryManager
    {
        private byte[] _memory;
        private readonly string _pathToMemory;
        

        public MemoryManager(string pathToMemory)
        {
            _pathToMemory = pathToMemory;
        }

        public bool TryInitFromFile()
        {
            FileStream stream = null;
            try
            {
                stream = File.Open(_pathToMemory, FileMode.Open, FileAccess.Read);
                _memory = new byte[stream.Length];
                stream.Read(_memory, 0, Convert.ToInt32(stream.Length));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return true;
        }

        public bool TryGetByte(int offset , out byte b)
        {
            b = 0;
            if (offset >= 0 && offset <= _memory.Length)
            {
                b = _memory[offset];
                return true;
            }
            return false;
        }

        public bool TryWrite(byte value , int offset)
        {
            if (offset >=0 && offset <= _memory.Length)
            {
                _memory[offset] = value;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var i = 0;
            foreach (var b in _memory)
            {
                sb.AppendFormat("{0:x2} ", b);
                i++;
                if ((i % 15) == 0)
                    sb.Append("\n");
            }
            return sb.ToString();
        }
    }
}
