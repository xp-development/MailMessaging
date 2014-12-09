namespace MailMessaging.Plain.IntegrationTest
{
    public interface IStreamWriter
    {
        void WriteString(string message);
        void StoreAsync();
    }
}