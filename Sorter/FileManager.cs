using System;
using System.IO;
using log4net;

namespace Sorter
{
    class FileManager
    {
        private readonly ILog _log;

        public FileManager()
        {
            _log = LogManager.GetLogger(typeof (FileManager));
        }

        public bool TryGetStream(String fileName , out StreamReader stream)
        {
            stream = null;
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No file {0}" , fileName);
                _log.ErrorFormat("[FileManager-TryGetStream] file {0} not found" , filePath);
                return false;
            }
            try
            {
                stream = new StreamReader(filePath);
            }
            catch(Exception e)
            {
                Console.WriteLine("Can't creater stream for {0}, ignoring file",fileName);
                _log.ErrorFormat("[FileManager-TryGetStream] error : {0}" , e.Message);
                return false;
            }
            return true;
        }

    }
}
