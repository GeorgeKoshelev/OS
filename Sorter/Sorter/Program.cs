using System;
using System.IO;

namespace Sorter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    Usage();
                    return;
                }
                var fm = new FileManager();
                var fileReader = new FileReader();
                for (var i = 0; i < args.Length - 1; i++)
                {
                    StreamReader stream;
                    if (!fm.TryGetStream(args[i], out stream)) continue;
                    fileReader.ReadIntValuesFromStream(stream);
                }
                var list = fileReader.StorageHandler.Storage;
                var fileWriter = new FileWriter();
                list.Sort();
                fileWriter.WriteToFile(list, args[args.Length - 1]);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error , program stop working : {0}" ,e.Message);
            }
        }

        public static void Usage()
        {
            Console.WriteLine("usage >Sorter.exe file1 ... outputfile");
        }
    }
}
