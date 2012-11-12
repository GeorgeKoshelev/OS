using System;
using System.Collections.Generic;

namespace Sorter
{
    class StorageHandler
    {
        public List<Int64> Storage { get; private set; }

        public StorageHandler()
        {
            Storage = new List<Int64>();
        }

        public bool TryAddValue( Int64 value)
        {
            try
            {
                Storage.Add(value);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("Невозможно добавить значение в память : {0}", e.Message);
                return false;
            }
        }
    }
}
