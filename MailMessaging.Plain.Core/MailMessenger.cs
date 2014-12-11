﻿using System;
using System.Text;
using System.Threading.Tasks;

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

            return builder.ToString();
        }

        private readonly IAccount _account;
        private readonly ITcpClient _tcpClient;
    }
}