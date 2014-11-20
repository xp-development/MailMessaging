namespace MailMessaging.Plain.Core
{
    public interface IAccount
    {
        string Server { get; }
        int Port { get; }
        string UserName { get; }
        string Password { get; }
        bool UseSsl { get; }
    }
}