using System;

namespace MailMessaging.Plain.IntegrationTest
{
    public class FakeImapServer
    {
        public FakeImapServer(ITcpListener listener)
        {
            _listener = listener;
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

        private static void OnListenerOnConnectionReceived(object sender, ConnectionReceivedEventHandlerArgs args)
        {
            args.StreamWriter.WriteString("* OK IMAP server ready\r\n");
            args.StreamWriter.StoreAsync();
        }

        public void Stop()
        {
            if (_listener == null)
                return;

            _listener.Stop();
        }

        private ImapServerConfiguration _configuration;
        private readonly ITcpListener _listener;
    }
}