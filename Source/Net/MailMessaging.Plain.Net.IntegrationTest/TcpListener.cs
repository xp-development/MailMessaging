using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using MailMessaging.Plain.IntegrationTest.Contracts;

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

            var stream = _useTls ? (Stream)new SslStream(tcpClient.GetStream()) : tcpClient.GetStream();
            var sslStream = stream as SslStream;
            if(sslStream != null)
            {
                byte[] certificate;
                using (var certStream = Assembly.GetAssembly(typeof(TcpListener)).GetManifestResourceStream("MailMessaging.Plain.IntegrationTest.TestFiles.MailMessaging.pfx"))
                {
                    certificate = new byte[certStream.Length];
                    certStream.Read(certificate, 0, (int)certStream.Length);
                }
                sslStream.AuthenticateAsServer(new X509Certificate2(certificate, "MailMessaging"));
            }

            InvokeConnectionReceived(new ConnectionReceivedEventHandlerArgs(new StreamReader(stream), new StreamWriter(stream)));
        }

        public void Stop()
        {
            _listener?.Stop();
        }

        public void UseTls(bool useTls)
        {
            _useTls = useTls;
        }

        private void InvokeConnectionReceived(ConnectionReceivedEventHandlerArgs args)
        {
            ConnectionReceived?.Invoke(this, args);
        }

        private System.Net.Sockets.TcpListener _listener;
        private bool _useTls;
    }
}