using System.IO;
using System.Text;
using System.Threading.Tasks;
using MailMessaging.Plain.IntegrationTest.Contracts;

namespace MailMessaging.Plain.IntegrationTest
{
    public class StreamReader : IStreamReader
    {
        public StreamReader(Stream inputStream)
        {
            _inputStream = inputStream;
        }

        public async Task<string> ReadStringAsync()
        {
            var buffer = new byte[4096];
            await _inputStream.ReadAsync(buffer, 0, buffer.Length);

            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        private readonly Stream _inputStream;
    }
}