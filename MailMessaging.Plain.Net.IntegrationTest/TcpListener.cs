using System;
using System.Net;

namespace MailMessaging.Plain.IntegrationTest
{
    public class TcpListener : ITcpListener
    {
        public event ConnectionReceivedEventHandler ConnectionReceived;

        public void Start(string host, int port)
        {
            _listener = new System.Net.Sockets.TcpListener(IPAddress.Parse(host), port);
            _listener.Start();
            _listener.BeginAcceptTcpClient(OnAcceptTcpClient, _listener);
        }

        private void OnAcceptTcpClient(IAsyncResult asyncResult)
        {
            var listener = (System.Net.Sockets.TcpListener) asyncResult.AsyncState;

            _listener.BeginAcceptTcpClient(OnAcceptTcpClient, _listener);
            var tcpClient = listener.EndAcceptTcpClient(asyncResult);

            InvokeConnectionReceived(new ConnectionReceivedEventHandlerArgs(new StreamReader(tcpClient.GetStream()),
                new StreamWriter(tcpClient.GetStream())));
        }

        public void Stop()
        {
            if (_listener == null)
                return;

            _listener.Stop();
        }

        private void InvokeConnectionReceived(ConnectionReceivedEventHandlerArgs args)
        {
            if (ConnectionReceived != null)
                ConnectionReceived(this, args);
        }

        private System.Net.Sockets.TcpListener _listener;
    }
}