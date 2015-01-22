namespace MailMessaging.Plain.IntegrationTest
{
    public interface ITcpListener
    {
        event ConnectionReceivedEventHandler ConnectionReceived;
        void Start(string host, int port);
        void Stop();
    }
}