using System.Threading.Tasks;

namespace MailMessaging.Plain.IntegrationTest
{
    public interface IStreamReader
    {
        Task<string> ReadStringAsync();
    }
}