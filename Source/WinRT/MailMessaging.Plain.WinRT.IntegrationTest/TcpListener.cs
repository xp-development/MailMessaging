using System.Diagnostics;
using System.IO;
using Windows.Networking;
using Windows.Networking.Sockets;
using MailMessaging.Plain.IntegrationTest.Contracts;

namespace MailMessaging.Plain.IntegrationTest
{
    public class TcpListener : ITcpListener
    {
        public event ConnectionReceivedEventHandler ConnectionReceived;

        public void Start(string host, int port)
        {
            _listener = new StreamSocketListener();
            _listener.ConnectionReceived += OnListenerOnConnectionReceived;
            _listener.BindEndpointAsync(new HostName(host), port.ToString());
        }

        private void OnListenerOnConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            var inputStream = args.Socket.InputStream.AsStreamForRead();
            var outputStream = args.Socket.OutputStream.AsStreamForWrite();
            InvokeConnectionReceived(new ConnectionReceivedEventHandlerArgs(new StreamReader(inputStream), new StreamWriter(outputStream)));
        }

        public void Stop()
        {
            if (_listener == null)
                return;

            _listener.Dispose();
            _listener = null;
        }

        public void UseTls(bool useTls)
        {
            Debug.WriteLine("Tls is not supported.");
        }

        private void InvokeConnectionReceived(ConnectionReceivedEventHandlerArgs args)
        {
            ConnectionReceived?.Invoke(this, args);
        }

        private StreamSocketListener _listener;
    }
}