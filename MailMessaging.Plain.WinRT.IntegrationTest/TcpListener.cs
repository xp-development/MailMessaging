using Windows.Networking;
using Windows.Networking.Sockets;

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
            InvokeConnectionReceived(new ConnectionReceivedEventHandlerArgs(new StreamReader(args.Socket.InputStream), new StreamWriter(args.Socket.OutputStream)));
        }

        public void Stop()
        {
            if (_listener == null)
                return;

            _listener.Dispose();
            _listener = null;
        }

        private StreamSocketListener _listener;

        private void InvokeConnectionReceived(ConnectionReceivedEventHandlerArgs args)
        {
            if (ConnectionReceived != null)
                ConnectionReceived(this, args);
        }
    }
}