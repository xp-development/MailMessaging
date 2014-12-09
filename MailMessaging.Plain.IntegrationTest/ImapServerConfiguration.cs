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

        public bool UseSsl
        {
            get { return _useSsl; }
        }

        public ImapServerConfiguration(string ipAddress, int port, bool useSsl)
        {
            _ipAddress = ipAddress;
            _port = port;
            _useSsl = useSsl;
        }

        private readonly string _ipAddress;
        private readonly int _port;
        private readonly bool _useSsl;
    }
}