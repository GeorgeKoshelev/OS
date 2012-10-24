using System;
using System.IO;
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

        public void ReadIntValuesFromStream(StreamReader stream)
        {
            try
            {
                while(stream.Peek() >= 0)
                {
                    string line;
                    try
                    {
                        line = stream.ReadLine();
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Can't read string , read logs for details");
                        _log.ErrorFormat("[FileReader-ReadIntValuesFromStream] Can't read line : {0}" , e.Message);
                        continue;
                    }
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
            finally
            {
                if (stream!=null)
                    stream.Close();
            }
        }
    }
}
