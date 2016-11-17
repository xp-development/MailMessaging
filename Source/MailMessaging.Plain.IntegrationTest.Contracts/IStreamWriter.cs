using System.Threading;
using System.Threading.Tasks;

namespace MailMessaging.Plain.IntegrationTest.Contracts
{
    public interface IStreamWriter
    {
        Task WriteStringAsync(string message, CancellationToken cancellationToken);
    }
}