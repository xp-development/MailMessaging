namespace MailMessaging.Plain.IntegrationTest.Contracts
{
    public interface ITcpListener
    {
        event ConnectionReceivedEventHandler ConnectionReceived;
        void Start(string host, int port);
        void Stop();
    }
}