using System.Threading.Tasks;

namespace MailMessaging.Plain.Contracts
{
    public interface IMailMessenger
    {
        bool IsConnected { get; }
        Task<ConnectResult> Connect();
    }
}