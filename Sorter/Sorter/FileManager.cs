using System;
using System.IO;

namespace Sorter
{
    class FileManager
    {
        public bool TryGetStream(String fileName , out StreamReader stream)
        {
            stream = null;
            try
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("File {0} not found", fileName);
                    return false;
                }
                stream = new StreamReader(filePath);
            }
            catch(Exception e)
            {
                Console.WriteLine("Can't get stream for {0}, ignoring file : {1}", fileName , e.Message);
                return false;
            }
            return true;
        }

    }
}
