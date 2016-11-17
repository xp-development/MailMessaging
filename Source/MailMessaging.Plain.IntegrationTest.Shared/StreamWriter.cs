using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MailMessaging.Plain.IntegrationTest.Contracts;

namespace MailMessaging.Plain.IntegrationTest
{
    public class StreamWriter : IStreamWriter
    {
        public StreamWriter(Stream outputStream)
        {
            _stream = outputStream;
        }

        public async Task WriteStringAsync(string message, CancellationToken cancellationToken)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            await _stream.WriteAsync(bytes, 0, bytes.Length, cancellationToken);
            await _stream.FlushAsync();
        }

        private readonly Stream _stream;
    }
}