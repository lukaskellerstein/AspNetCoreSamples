using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerApp
{
    class Program
    {
        private static TcpListener listener { get; set; }


        public static void Main(string[] args)
        {
            // Start the server  
            IPAddress address = IPAddress.Parse("127.0.0.1");
            listener = new TcpListener(address, 5678);
            listener.Start();
            Console.WriteLine($"Server started. Listening to TCP clients at 127.0.0.1:5678");

            // Start listening. 
            while (true)
            {
                var clientTask = listener.AcceptTcpClientAsync(); // Get the client  
                if (clientTask.Result != null)
                {
                    Console.WriteLine("Client connected. Waiting for data.");
                    var client = clientTask.Result;
                    string message = "";

                    while (message != null && !message.StartsWith("quit"))
                    {
                        byte[] data = Encoding.ASCII.GetBytes("Send next data: [enter 'quit' to terminate] ");
                        client.GetStream().Write(data, 0, data.Length);

                        byte[] buffer = new byte[1024];
                        client.GetStream().Read(buffer, 0, buffer.Length);

                        message = Encoding.ASCII.GetString(buffer);
                        Console.WriteLine(message);
                    }
                    Console.WriteLine("Closing connection.");
                    client.GetStream().Dispose();
                }
            }
        }

    }
}
