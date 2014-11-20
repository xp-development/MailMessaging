namespace MailMessaging.Plain.Core
{
    public class Account : IAccount
    {
        public string Server { get; private set; }
        public int Port { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public bool UseSsl { get; private set; }

        public Account(string server, int port, string userName, string password, bool useSsl)
        {
            Server = server;
            Port = port;
            UserName = userName;
            Password = password;
            UseSsl = useSsl;
        }
    }
}