using System;
using System.IO;

namespace binaryReader
{
    class Program
    {
        static void Main(string[] args)
        {
            //check if both arguments were typed
            if(args.Length == 2)
            {
                string inputFile = args[0];
                string outputFile = args[1];
                AdbConverter adb = new AdbConverter(inputFile, outputFile);
                adb.Process();
                Console.WriteLine("ADB FILE SUCCESSFULY PROCESSED");
            }
            //else show information message and close app
            else
            {
                Console.WriteLine("APPLICATION TAKES 2 PARAMETERS [inputFile] [outputFile]");
            }
        }
    }
}
