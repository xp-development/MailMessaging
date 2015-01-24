using System;
using System.Text.RegularExpressions;
using MailMessaging.Plain.IntegrationTest.Contracts;

namespace MailMessaging.Plain.IntegrationTest
{
    public class FakeImapServer
    {
        public FakeImapServer(ITcpListener listener, FakeAccount fakeAccount)
        {
            _listener = listener;
            _fakeAccount = fakeAccount;
            _commandManager = new CommandManager(_fakeAccount);
        }

        public void SetConfiguration(ImapServerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Start()
        {
            if (_configuration == null)
                throw new Exception(
                    "Imap server is not configured! Call SetConfiguration(ImapServerConfiguration configuration).");

            _listener.ConnectionReceived += OnListenerOnConnectionReceived;
            _listener.Start(_configuration.IpAddress, _configuration.Port);
        }

        private async void OnListenerOnConnectionReceived(object sender, ConnectionReceivedEventHandlerArgs args)
        {
            await args.StreamWriter.WriteStringAsync("* OK IMAP server ready\r\n");

            while (true)
            {
                var receivedMessage = await args.StreamReader.ReadStringAsync();

                var regex = new Regex("^(A\\d{4})\\s(.*?)\\s(.*)");

                var matches = regex.Matches(receivedMessage);

                var tag = matches[0].Groups[1].Value;
                var command = matches[0].Groups[2].Value;
                var commandArgs = matches[0].Groups[3].Value;

                var response = _commandManager.Process(tag, command, commandArgs);
                await args.StreamWriter.WriteStringAsync(response);
            }
        }

        public void Stop()
        {
            if (_listener == null)
                return;

            _listener.ConnectionReceived -= OnListenerOnConnectionReceived;
            _listener.Stop();
        }

        private ImapServerConfiguration _configuration;
        private readonly CommandManager _commandManager;
        private readonly FakeAccount _fakeAccount;
        private readonly ITcpListener _listener;
    }
}