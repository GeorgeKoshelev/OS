using System;
using System.IO;

namespace Sorter
{
    class FileReader
    {
        public StorageHandler StorageHandler;
        
        public FileReader()
        {
            StorageHandler = new StorageHandler();
        }

        public void ReadIntValuesFromStream(StreamReader stream)
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
                        if (Int64.TryParse(element, out value))
                        {
                            if (!StorageHandler.TryAddValue(value))
                                return;
                        }
                        else
                            Console.WriteLine("incorrect value {0}", element);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Can't read from file : {0}" , e.Message);
            }
            finally
            {
                if (stream !=null)
                    stream.Close();
            }
        }
    }
}
