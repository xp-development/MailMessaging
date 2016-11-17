namespace MailMessaging.Plain.IntegrationTest
{
    public class ImapServerConfiguration
    {
        public string IpAddress
        {
            get { return _ipAddress; }
        }

        public int Port
        {
            get { return _port; }
        }

        public bool UseTls
        {
            get { return _useTls; }
        }

        public ImapServerConfiguration(string ipAddress, int port, bool useTls)
        {
            _ipAddress = ipAddress;
            _port = port;
            _useTls = useTls;
        }

        private readonly string _ipAddress;
        private readonly int _port;
        private readonly bool _useTls;
    }
}