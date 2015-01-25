using MailMessaging.Plain.Contracts;

namespace MailMessaging.Plain.Core
{
    public class Account : IAccount
    {
        public string Server { get; private set; }
        public int Port { get; private set; }
        public bool UseSsl { get; private set; }

        public Account(string server, int port, bool useSsl)
        {
            Server = server;
            Port = port;
            UseSsl = useSsl;
        }
    }
}