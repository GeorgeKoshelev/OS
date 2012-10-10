using System;
using System.Collections.Generic;
using System.IO;
using log4net;

namespace Sorter
{
    class FileWriter
    {

        private readonly ILog _log;

        public FileWriter()
        {
            _log = LogManager.GetLogger(typeof(FileWriter));
        }

        public void WriteToFile(List<Int64> values , string fileName)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (!File.Exists(filePath))
            {
                Console.WriteLine("No file {0} , try to create" , fileName);
                _log.ErrorFormat("[FileWriter-WriteToFile] no file {0},creating",fileName);
                try
                {
                    File.Create(filePath).Dispose();
                }
                catch(Exception e)
                {
                    _log.ErrorFormat("Can't create file {0} : {1}", fileName , e.Message);
                    Console.WriteLine("Can't create file {0} , view logs , for details", fileName);
                    return;
                }
                StreamWriter stream = null;
                try
                {
                    stream = new StreamWriter(filePath);
                    foreach (var value in values)
                    {
                        stream.WriteLine(value);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Can't write to file , view logs for details");
                    _log.ErrorFormat("[FileWriter-WriteToFile] Error in processing writing : {0}" , e.Message);
                }
                finally
                {
                    if (stream!=null)
                        stream.Close();
                }
            }

        }

    }
}
