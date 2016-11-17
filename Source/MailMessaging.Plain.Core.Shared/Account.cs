using MailMessaging.Plain.Contracts;

namespace MailMessaging.Plain.Core
{
    public class Account : IAccount
    {
        public string Server { get; private set; }
        public int Port { get; private set; }
        public bool UseTls { get; private set; }

        public Account(string server, int port, bool useTls)
        {
            Server = server;
            Port = port;
            UseTls = useTls;
        }
    }
}