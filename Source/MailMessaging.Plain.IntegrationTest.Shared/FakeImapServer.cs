using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MailMessaging.Plain.IntegrationTest.Contracts;

namespace MailMessaging.Plain.IntegrationTest
{
    public class FakeImapServer
    {
        public FakeImapServer(ITcpListener tcpListener, FakeAccount fakeAccount)
        {
            _tcpListener = tcpListener;
            _commandManager = new CommandManager(fakeAccount);
        }

        public void SetConfiguration(ImapServerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Start()
        {
            if (_configuration == null)
                throw new Exception("Imap server is not configured! Call SetConfiguration(ImapServerConfiguration configuration).");

            Debug.WriteLine("Start fake imap server '{0}:{1}'.", _configuration.IpAddress, _configuration.Port);
            _cancellationToken = new CancellationTokenSource();
            _tcpListener.ConnectionReceived += OnTcpListenerOnConnectionReceived;
            _tcpListener.Start(_configuration.IpAddress, _configuration.Port);
        }

        private async void OnTcpListenerOnConnectionReceived(object sender, ConnectionReceivedEventHandlerArgs args)
        {
            Debug.WriteLine("Receive connection.");
            await SendMessage("* OK IMAP server ready\r\n", args.StreamWriter, _cancellationToken.Token);

            while (!_cancellationToken.IsCancellationRequested)
            {
                var receivedMessage = await args.StreamReader.ReadStringAsync(_cancellationToken.Token);
                Debug.WriteLine("Receive: {0}", receivedMessage);
                var regex = new Regex("^(A\\d{4})\\s(.*?)\\s(.*)");

                var matches = regex.Matches(receivedMessage);

                var tag = matches[0].Groups[1].Value;
                var command = matches[0].Groups[2].Value;
                var commandArgs = matches[0].Groups[3].Value;

                var response = _commandManager.Process(tag, command, commandArgs);

                if (command == "UnknownCommand")
                {
                    await SendMessage(response, args.StreamWriter, _cancellationToken.Token);
                    continue;
                }

                if (command == "LOGIN" && response.StartsWith(string.Format("{0} OK", tag)))
                    _isLoggedIn = true;

                if (!_isLoggedIn && command != "LOGIN")
                {
                    await SendMessage(CommandManager.BuildResponse(tag, "NO please login first"), args.StreamWriter, _cancellationToken.Token);
                    continue;
                }

                await SendMessage(response, args.StreamWriter, _cancellationToken.Token);
            }
        }

        public void Stop()
        {
            if (_tcpListener == null)
                return;

            Debug.WriteLine("Stop fake imap server '{0}:{1}'.", _configuration.IpAddress, _configuration.Port);

            _cancellationToken.Cancel();
            _tcpListener.ConnectionReceived -= OnTcpListenerOnConnectionReceived;
            _tcpListener.Stop();
        }

        public void UseTls(bool useTls)
        {
            _tcpListener.UseTls(useTls);
        }

        private static async Task SendMessage(string message, IStreamWriter streamWriter, CancellationToken token)
        {
            Debug.WriteLine("Send: {0}", message);
            await streamWriter.WriteStringAsync(message, token);
        }

        private ImapServerConfiguration _configuration;
        private readonly CommandManager _commandManager;
        private readonly ITcpListener _tcpListener;
        private bool _isLoggedIn;
        private CancellationTokenSource _cancellationToken;
    }
}