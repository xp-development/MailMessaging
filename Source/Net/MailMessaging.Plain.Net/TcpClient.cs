using System.IO;
using System.Text;
using System.Threading.Tasks;
using MailMessaging.Plain.Contracts;

namespace MailMessaging.Plain.Core
{
    public class TcpClient : ITcpClient
    {
        public async Task Connect(string hostName, int port, bool useTls)
        {
            _client = new System.Net.Sockets.TcpClient();
            await _client.ConnectAsync(hostName, port);

            _stream = _client.GetStream();

            _streamWriter = new StreamWriter(_stream);
            _streamReader = new StreamReader(_stream);
        }

        public void Disconnect()
        {
            _client.Close();
        }

        public async Task WriteStringAsync(string message)
        {
            await _streamWriter.WriteAsync(message);
            await _streamWriter.FlushAsync();
        }

        public async Task<string> ReadAsync()
        {
            var buffer = new char[4096];
            var length = await _streamReader.ReadAsync(buffer, 0, buffer.Length);

            var builder = new StringBuilder();
            builder.Append(buffer, 0, length);

            return builder.ToString();
        }

        private System.Net.Sockets.TcpClient _client;
        private Stream _stream;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;
    }
}