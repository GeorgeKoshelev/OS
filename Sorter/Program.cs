using System;
using System.IO;
using log4net;
using log4net.Config;

namespace Sorter
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Program));

        static void Main(string[] args)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings/log.config.xml")));
            if (args.Length < 2)
            {
                Usage();
                return;
            }
            var fm = new FileManager();
            var fileReader = new FileReader();
            for (var i = 0 ; i < args.Length-1 ; i++)
            {
                StreamReader stream;
                if (!fm.TryGetStream(args[i] ,  out stream)) continue;
                fileReader.ReadIntValuesFromStream(stream);               
            }
            var list = fileReader.StorageHandler.Storage;
            var fileWriter = new FileWriter();
            list.Sort();
            fileWriter.WriteToFile(list , args[args.Length-1]);
        }

        public static void Usage()
        {
            Console.WriteLine("usage >Sorter.exe file1 ... outputfile");
            Log.Error("[Program-Main] not enought arguments");
        }
    }
}
