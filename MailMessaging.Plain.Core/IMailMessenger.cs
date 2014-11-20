using System.Threading.Tasks;

namespace MailMessaging.Plain.Core
{
    public interface IMailMessenger
    {
        bool IsConnected { get; }

        Task<ConnectResult> Connect();
    }
}