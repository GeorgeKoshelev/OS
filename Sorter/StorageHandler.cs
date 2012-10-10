using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            catch(Exception e)
            {
                _log.ErrorFormat("[StorageHandler-TryAddValue] Can't add value to storage : {0}" , e.Message);
                Console.WriteLine("Can't add value to storage : {0}" , e.Message);
            }
            return true;
        }
    }
}
