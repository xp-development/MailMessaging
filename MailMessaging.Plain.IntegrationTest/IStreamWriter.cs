using System.Threading.Tasks;

namespace MailMessaging.Plain.IntegrationTest
{
    public interface IStreamWriter
    {
        Task WriteStringAsync(string message);
    }
}