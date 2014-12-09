using System;

namespace MailMessaging.Plain.IntegrationTest
{
    public class ConnectionReceivedEventHandlerArgs : EventArgs
    {
        public IStreamReader StreamReader { get; private set; }
        public IStreamWriter StreamWriter { get; private set; }

        public ConnectionReceivedEventHandlerArgs(IStreamReader streamReader, IStreamWriter streamWriter)
        {
            StreamReader = streamReader;
            StreamWriter = streamWriter;
        }
    }
}