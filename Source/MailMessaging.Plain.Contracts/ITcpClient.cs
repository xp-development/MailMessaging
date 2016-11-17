using System.Threading.Tasks;

namespace MailMessaging.Plain.Contracts
{
    public interface ITcpClient
    {
        Task Connect(string hostName, int port, bool useTls);
        void Disconnect();
        Task WriteStringAsync(string message);
        Task<string> ReadAsync();
    }
}