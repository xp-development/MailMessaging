using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace MailMessaging.Plain.IntegrationTest
{
    public class StreamWriter : IStreamWriter
    {
        public StreamWriter(IOutputStream outputStream)
        {
            _dataWriter = new DataWriter(outputStream);
        }

        public async Task WriteStringAsync(string message)
        {
            _dataWriter.WriteString(message);
            await _dataWriter.StoreAsync();
        }

        private readonly DataWriter _dataWriter;
    }
}