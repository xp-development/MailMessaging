using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using MailMessaging.Plain.Core;

namespace MailMessaging.Plain.WinRT
{
    public class TcpClient : ITcpClient
    {
        public TcpClient()
        {
            _socket = new StreamSocket();

            _dataWriter = new DataWriter(_socket.OutputStream);
            _dataReader = new DataReader(_socket.InputStream)
            {
                InputStreamOptions = InputStreamOptions.Partial
            };
        }

        public async Task Connect(string hostName, int port, bool useTls)
        {
            await _socket.ConnectAsync(new HostName(hostName), port.ToString(), useTls ? SocketProtectionLevel.Tls12 : SocketProtectionLevel.PlainSocket);
        }

        public void Disconnect()
        {
            if (_socket == null)
                return;

            _socket.Dispose();
        }

        public async Task WriteStringAsync(string message)
        {
            _dataWriter.WriteString(message);
            await _dataWriter.StoreAsync();
        }

        public async Task<string> ReadAsync()
        {
            await _dataReader.LoadAsync(4096);
            var builder = new StringBuilder();
            while (_dataReader.UnconsumedBufferLength > 0)
            {
                var data = _dataReader.ReadString(_dataReader.UnconsumedBufferLength);
                builder.Append(data);
            }

            return builder.ToString();
        }

        private readonly StreamSocket _socket;
        private readonly DataWriter _dataWriter;
        private readonly DataReader _dataReader;
    }
}