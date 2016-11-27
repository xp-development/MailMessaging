using System;
using System.Threading.Tasks;

namespace MailMessaging.Plain.Contracts
{
    public interface ITcpClient : IDisposable
    {
        Task ConnectAsync(string hostName, int port, bool useTls);
        void Disconnect();
        Task WriteStringAsync(string message);
        Task<string> ReadAsync();
    }
}