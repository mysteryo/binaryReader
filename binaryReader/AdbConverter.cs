using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace binaryReader
{
    public class AdbConverter
    {
        private double Latitude { get; set; }
        private double Longitude { get; set; }
        private double Altitude { get; set; }
        private string RoverID { get; set; }
        private string RoverIP { get; set; }
        private byte SourceType { get; set; }
        private string ServiceName { get; set; }
        private Int16 ServiceType { get; set; }
        private Int16 ConnectionType { get; set; }
        private string UserName { get; set; }
        private int StartWeek { get; set; }
        private int StartTime { get; set; }
        private int EndWeek { get; set; }
        private int EndTime { get; set; }
        private byte[] Nothing1 { get; set; }
        private int BytesSent { get; set; }
        private byte[] Nothing2 { get; set; }

        private string _inputFile;
        private string _outputFile;
        private readonly string _dummyFile = Directory.GetCurrentDirectory() + @"\DUMMYOUTPUT.txt";

        public AdbConverter(string inputFile, string outputFile)
        {
            _inputFile = Path.GetFullPath(inputFile);
            _outputFile = Path.GetFullPath(outputFile);
        }

        public void Process()
        {
            //if input file dont exist ask for new path
            while (!File.Exists(_inputFile))
            {
                Console.WriteLine($"ERROR: FILE {_inputFile} DOES NOT EXIST, PLEASE TYPE IN CORRECT PATH");
                _inputFile = Console.ReadLine();
            }
            //if output file doesnt exist create new txt at exe location
            if (!File.Exists(_outputFile))
            {
                Console.WriteLine($"WARNING: FILE {_outputFile} DOES NOT EXIST, CREATING NEW FILE: {_dummyFile}");
                File.Create(_dummyFile).Close();
                _outputFile = _dummyFile;
            }
            try
            {
                using (BinaryReader br = new BinaryReader(new FileStream(_inputFile, FileMode.Open)))
                {
                    //throw away header
                    br.ReadBytes(96);

                    //read bytes and convert them into properties
                    Latitude = BitConverter.ToDouble(br.ReadBytes(8));
                    Longitude = BitConverter.ToDouble(br.ReadBytes(8));
                    Altitude = BitConverter.ToDouble(br.ReadBytes(8));
                    RoverID = Encoding.ASCII.GetString(br.ReadBytes(255));
                    RoverIP = Encoding.ASCII.GetString(br.ReadBytes(255));
                    SourceType = br.ReadByte();
                    ServiceName = Encoding.ASCII.GetString(br.ReadBytes(255));
                    ServiceType = BitConverter.ToInt16(br.ReadBytes(2));
                    ConnectionType = BitConverter.ToInt16(br.ReadBytes(2));
                    UserName = Encoding.ASCII.GetString(br.ReadBytes(255));
                    StartWeek = BitConverter.ToInt32(br.ReadBytes(4));
                    StartTime = BitConverter.ToInt32(br.ReadBytes(4));
                    EndWeek = BitConverter.ToInt32(br.ReadBytes(4));
                    EndTime = BitConverter.ToInt32(br.ReadBytes(4));
                    Nothing1 = br.ReadBytes(32);
                    BytesSent = BitConverter.ToInt32(br.ReadBytes(4));
                    Nothing2 = br.ReadBytes(2);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION THROWN IN PROCESS(): " + e);
            }

            try
            {
                //truncate to clear txt before writing
                using (StreamWriter sw = new StreamWriter(new FileStream(_outputFile, FileMode.Truncate, FileAccess.Write)))
                {
                    //replace all null characters to with empty chars
                    sw.Write(RoverID.Replace("\0", string.Empty));
                    sw.Write(";");
                    sw.Write(RoverIP.Replace("\0", string.Empty));
                    sw.Write(";");
                    sw.Write(SourceType);
                    sw.Write(";");
                    sw.Write(ServiceName.Replace("\0", string.Empty));
                    sw.Write(";");
                    sw.Write(ServiceType);
                    sw.Write(";");
                    sw.Write(UserName.Replace("\0", string.Empty));
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine("EXCEPTION THROWN IN PROCESS(): " + e);
            }
        }
    }
}
