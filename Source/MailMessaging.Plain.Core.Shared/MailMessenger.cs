using System;
using System.Text;
using System.Threading.Tasks;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Contracts.Commands;
using MetroLog;

namespace MailMessaging.Plain.Core
{
    public class MailMessenger : IMailMessenger
    {
        public bool IsConnected { get; private set; }

        public MailMessenger(IAccount account, ITcpClient tcpClient)
        {
            _account = account;
            _tcpClient = tcpClient;
        }

        public async Task<ConnectResult> Connect()
        {
            try
            {
                await _tcpClient.Connect(_account.Server, _account.Port, _account.UseSsl);

                IsConnected = (await ReadData()).StartsWith("* OK");
                return IsConnected ? ConnectResult.Connected : ConnectResult.UnknownHost;
            }
            catch (Exception)
            {
                return ConnectResult.UnknownHost;
            }
        }

        public async Task<TResponse> Send<TRequest, TResponse>(ICommand<TRequest, TResponse> message)
            where TRequest : IRequest
            where TResponse : IResponse
        {
            var request = message.Request;

            _logger.Info("Send: " + request);
            await _tcpClient.WriteStringAsync(request);
            return message.ParseResponse(await ReadData());
        }

        private async Task<string> ReadData()
        {
            var response = await _tcpClient.ReadAsync();
            var builder = new StringBuilder();
            builder.Append(response);

            while (!string.IsNullOrEmpty(response))
            {
                if (response.EndsWith("\r\n"))
                    break;

                response = await _tcpClient.ReadAsync();
                builder.Append(response);
            }

            var receivedMessage = builder.ToString();
            _logger.Info("Receive: " + receivedMessage);
            return receivedMessage;
        }

        private readonly IAccount _account;
        private readonly ITcpClient _tcpClient;

        private static readonly ILogger _logger = LogManagerFactory.DefaultLogManager.GetLogger<MailMessenger>();
    }
}