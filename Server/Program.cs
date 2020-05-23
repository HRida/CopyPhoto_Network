using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 3003;
            int bufferSize = 1024;

            TcpClient client = new TcpClient();
            NetworkStream netStream;

            // Connect to server
            try
            {
                client.Connect(new IPEndPoint(ipAddress, port));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            netStream = client.GetStream();

            // Read bytes from image
            byte[] data = File.ReadAllBytes("H:\\x.jpg");

            // Build the package
            byte[] dataLength = BitConverter.GetBytes(data.Length);
            byte[] package = new byte[4 + data.Length];
            dataLength.CopyTo(package, 0);
            data.CopyTo(package, 4);

            // Send to server
            int bytesSent = 0;
            int bytesLeft = package.Length;

            while (bytesLeft > 0)
            {

                int nextPacketSize = (bytesLeft > bufferSize) ? bufferSize : bytesLeft;

                netStream.Write(package, bytesSent, nextPacketSize);
                bytesSent += nextPacketSize;
                bytesLeft -= nextPacketSize;

            }

            // Clean up
            netStream.Close();
            client.Close();
        }
    }
}
