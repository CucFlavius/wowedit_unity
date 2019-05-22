using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using MimeKit;

namespace CASCLib
{
    public class RibbitClient : IDisposable
    {
        private const string ribbitHost = ".version.battle.net";
        private TcpClient Client = new TcpClient();

        public RibbitClient(string region) => Client = new TcpClient($"{region}{ribbitHost}", 1119);

        public string Get(string request)
        {
            using (NetworkStream stream = Client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                byte[] req = Encoding.ASCII.GetBytes(request + "\r\n");
                stream.Write(req, 0, req.Length);

                var message = MimeMessage.Load(stream);
                return message.TextBody;
            }
        }

        public Stream GetAsStream(string request)
        {
            using (NetworkStream stream = Client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                byte[] req = Encoding.ASCII.GetBytes(request + "\r\n");

                stream.Write(req, 0, req.Length);

                var message = MimeMessage.Load(stream);

                return new MemoryStream(Encoding.ASCII.GetBytes(message.TextBody));
            }
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
