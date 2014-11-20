using System;
using System.Text;
using System.Threading.Tasks;

namespace MailMessaging.Plain.Core
{
    public class MailMessenger : IMailMessenger
    {
        public MailMessenger(IAccount account, ITcpClient tcpClient)
        {
            _account = account;
            _tcpClient = tcpClient;
        }

        public bool IsConnected { get; private set; }

        public async Task<ConnectResult> Connect()
        {
            try
            {
                await _tcpClient.Connect(_account.Server, _account.Port, _account.UseSsl);

                IsConnected = (await ReadData()).StartsWith("* OK");
                return IsConnected ? ConnectResult.Connected : ConnectResult.UnknownHost;
            }
            catch (Exception e)
            {
                return ConnectResult.UnknownHost;
            }
        }

        private async Task<string> ReadData()
        {
            var response = await _tcpClient.ReadAsync();
            var builder = new StringBuilder();
            builder.Append(response);

            while (!string.IsNullOrEmpty(response))
            {
                response = await _tcpClient.ReadAsync();
                builder.Append(response);
            }

            return builder.ToString();
        }

        private readonly IAccount _account;
        private readonly ITcpClient _tcpClient;
    }
}