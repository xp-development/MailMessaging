using MailMessaging.Plain.Contracts;

namespace MailMessaging.Plain.Core
{
    public class Account : IAccount
    {
        public string Server { get; }
        public int Port { get; }
        public bool UseTls { get; }

        public Account(string server, int port, bool useTls)
        {
            Server = server;
            Port = port;
            UseTls = useTls;
        }
    }
}