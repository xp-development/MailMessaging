using System.Threading.Tasks;

namespace MailMessaging.Plain.IntegrationTest.Contracts
{
    public interface IStreamReader
    {
        Task<string> ReadStringAsync();
    }
}