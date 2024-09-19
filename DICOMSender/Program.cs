using System;
using System.IO;
using System.Net.Sockets;
using Dicom;

namespace DicomSender
{
    class Program
    {
        private const string ServerIp = "127.0.0.1"; // Replace with the server IP address
        private const int Port = 32000; // The port to connect to

        static void Main(string[] args)
        {
            //if (args.Length == 0)
            //{
            //    Console.WriteLine("Usage: DicomSender <dicom-file-path>");
            //    return;
            //}

            string dicomFilePath = "C:/Users/asomwanshi/OneDrive - Microsoft/Hack2024/SecureHealthBridge/DICOM_Data/sampleDataCR.dcm";

            if (!File.Exists(dicomFilePath))
            {
                Console.WriteLine("File does not exist.");
                return;
            }

            try
            {
                // Read the DICOM file
                byte[] dicomData = File.ReadAllBytes(dicomFilePath);

                // Send the DICOM file
                SendDicomFile(dicomData);

                Console.WriteLine("DICOM file sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static void SendDicomFile(byte[] data)
        {
            using (TcpClient client = new TcpClient(ServerIp, Port))
            using (NetworkStream stream = client.GetStream())
            {
                // Send the data
                stream.Write(data, 0, data.Length);
            }
        }
    }
}
