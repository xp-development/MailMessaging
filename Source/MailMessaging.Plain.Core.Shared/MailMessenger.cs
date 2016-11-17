using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Contracts.Commands;

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
                Debug.WriteLine("Connect mail server '{0}:{1}'. UseTls={2}", _account.Server, _account.Port, _account.UseTls);
                await _tcpClient.Connect(_account.Server, _account.Port, _account.UseTls);

                IsConnected = (await ReadData()).StartsWith("* OK");
                return IsConnected ? ConnectResult.Connected : ConnectResult.UnknownHost;
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Cannot connect server! " + exception);
                return ConnectResult.UnknownHost;
            }
        }

        public async Task<TResponse> Send<TRequest, TResponse>(ICommand<TRequest, TResponse> message)
            where TRequest : IRequest
            where TResponse : IResponse
        {
            var request = message.Request;

            Debug.WriteLine("Send: " + request);
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
            Debug.WriteLine("Receive: " + receivedMessage);
            return receivedMessage;
        }

        private readonly IAccount _account;
        private readonly ITcpClient _tcpClient;
    }
}