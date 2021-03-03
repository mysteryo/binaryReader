using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace binaryReader
{
    class AdbConverter
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public string RoverID { get; set; }
        public string RoverIP { get; set; }
        public byte SourceType { get; set; }
        public string ServiceName { get; set; }
        public Int16 ServiceType { get; set; }
        public Int16 ConnectionType { get; set; }
        public string UserName { get; set; }
        public int StartWeek { get; set; }
        public int StartTime { get; set; }
        public int EndWeek { get; set; }
        public int EndTime { get; set; }
        public byte[] Nothing1 { get; set; }
        public int BytesSent { get; set; }
        public byte[] Nothing2 { get; set; }

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
            catch (IOException e)
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
            catch(IOException e)
            {
                Console.WriteLine("EXCEPTION THROWN IN PROCESS(): " + e);
            }
            
        }
    }
}
