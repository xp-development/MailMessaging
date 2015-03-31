namespace MailMessaging.Plain.Contracts
{
    public interface IAccount
    {
        string Server { get; }
        int Port { get; }
        bool UseTls { get; }
    }
}