using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Sorter
{
    class FileReader
    {
        private readonly ILog _log;
        public StorageHandler StorageHandler;
        public FileReader()
        {
            _log = LogManager.GetLogger(typeof (FileReader));
            StorageHandler = new StorageHandler();
        }

        public void TryReadIntValuesFromStream(StreamReader stream)
        {
            try
            {
                while(stream.Peek() >= 0)
                {
                    var line = stream.ReadLine();
                    if (line == null) continue;
                    var elements = line.Split(' ');
                    foreach (var element in elements)
                    {
                        Int64 value;
                        if (Int64.TryParse(element , out value))
                        {
                            if (!StorageHandler.TryAddValue(value))
                                return;
                        }
                        else
                        {
                            _log.ErrorFormat("[FileReader-ReadIntValuesFromStream] incorrect value {0}" , element);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                _log.ErrorFormat("[FileReader-ReadIntValuesFromStream] error : {0}" , e.Message);
            }
        }
    }
}
