using System;
using System.Collections.Generic;
using log4net;

namespace Sorter
{
    class StorageHandler
    {
        private readonly ILog _log;
        public List<Int64> Storage { get; private set; }

        public StorageHandler()
        {
            _log = LogManager.GetLogger(typeof (StorageHandler));
            Storage = new List<Int64>();
        }

        public bool TryAddValue( Int64 value)
        {
            try
            {
                Storage.Add(value);
            }
            catch(OutOfMemoryException e)
            {
                _log.ErrorFormat("[StorageHandler-TryAddValue] Can't add value to storage : {0}", e.Message);
                Console.WriteLine("Невозможно добавить значение в память, переполнение : {0}", e.Message);
                return false;
            }
            catch(Exception e)
            {
                _log.ErrorFormat("[StorageHandler-TryAddValue] Can't add value to storage : {0}" , e.Message);
                Console.WriteLine("Невозможно добавить значение в память : {0}", e.Message);
                return false;
            }
            return true;
        }
    }
}
