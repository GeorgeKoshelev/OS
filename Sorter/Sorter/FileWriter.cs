using System;
using System.Collections.Generic;
using System.IO;

namespace Sorter
{
    class FileWriter
    {
        public void WriteToFile(List<Int64> values , string fileName)
        {
            StreamWriter stream = null;
            try
            {
                var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                stream = new StreamWriter(File.Create(filePath));
                foreach (var value in values)
                    stream.WriteLine(value);
            }
            catch(Exception e)
            {
                Console.WriteLine("Can't write to file {0} : {1}" , fileName , e.Message);
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }

    }
}
