using FluentAssertions;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Core;
using Xunit;

namespace MailMessaging.Plain.IntegrationTest._MailMessenger
{
    public class Connect : TestBase
    {
        [Fact]
        public void ShouldConnectServerWithoutTls()
        {
            var account = new Account(TestServer, TestPort, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(account, tcpClient);
            messenger.Connect().Result.Should().Be(ConnectResult.Connected);
        }

        [Fact]
        public void ShouldConnectServerWithTls()
        {
            ActivateServerTls();

            var account = new Account(TestServer, TestPort, true);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(account, tcpClient);
           messenger.Connect().Result.Should().Be(ConnectResult.Connected);
        }

        [Fact]
        public void ShouldNotConnectServerIfHostIsWrong()
        {
            var account = new Account("222.222.222.222", TestPort, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(account, tcpClient);
            messenger.Connect().Result.Should().Be(ConnectResult.UnknownHost);
        }

        [Fact]
        public void ShouldNotConnectServerIfPortIsWrong()
        {
            var account = new Account(TestServer, 11111, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(account, tcpClient);
            messenger.Connect().Result.Should().Be(ConnectResult.UnknownHost);
        }
    }
}