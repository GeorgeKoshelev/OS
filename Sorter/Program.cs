using System;
using System.IO;
using log4net;
using log4net.Config;

[assembly: XmlConfigurator(ConfigFile = "App.config", Watch = true)]
namespace Sorter
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (Program));

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Usage();
                return;
            }
            var outputFileName = args[args.Length-1];
            var fm = new FileManager();
            var fileReader = new FileReader();
            for (var i = 0 ; i < args.Length-1 ; i++)
            {
                StreamReader stream;
                if (!fm.TryGetStream(args[i] , out stream)) continue;
                fileReader.TryReadIntValuesFromStream(stream);               
            }
            var list = fileReader.StorageHandler.Storage;
            foreach (var value in list)
            {       
                Console.WriteLine(value);
            }
        }

        public static void Usage()
        {
            Console.WriteLine("usage >*.exe file1 ... outputfile");
            Log.Error("[Program-Main] not enought arguments");
        }
    }
}
