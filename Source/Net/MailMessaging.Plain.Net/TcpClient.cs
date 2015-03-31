using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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

            _stream = useTls ? (Stream) new SslStream(_client.GetStream(), true, UserCertificateValidationCallback) : _client.GetStream();
            var sslStream = _stream as SslStream;
            if(sslStream != null)
                sslStream.AuthenticateAsClient(hostName);

            _streamWriter = new StreamWriter(_stream);
            _streamReader = new StreamReader(_stream);
        }

        private static bool UserCertificateValidationCallback(object sender, X509Certificate certificate,
            X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
#if TEST
            return true;
#endif

            return sslPolicyErrors == SslPolicyErrors.None;
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