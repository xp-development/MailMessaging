using Windows.Storage.Streams;

namespace MailMessaging.Plain.IntegrationTest
{
    public class StreamWriter : IStreamWriter
    {
        public StreamWriter(IOutputStream outputStream)
        {
            _dataWriter = new DataWriter(outputStream);
        }

        public void WriteString(string message)
        {
            _dataWriter.WriteString(message);
        }

        public void StoreAsync()
        {
            _dataWriter.StoreAsync();
        }

        private readonly DataWriter _dataWriter;
    }
}