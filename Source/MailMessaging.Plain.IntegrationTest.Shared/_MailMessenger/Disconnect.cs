using FluentAssertions;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Core;
using Xunit;

namespace MailMessaging.Plain.IntegrationTest._MailMessenger
{
    public class Disconnect : TestBase
    {
        [Fact]
        public void ShouldDisconnect()
        {
            var account = new Account(TestServer, TestPort, false);
            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(tcpClient);
            messenger.ConnectAsync(account).Result.Should().Be(ConnectResult.Connected);

            messenger.Disconnect();

            messenger.IsConnected.Should().BeFalse();
        }

        [Fact]
        public void ShouldNotThrowExceptionIfNotConnected()
        {
            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(tcpClient);

            messenger.Disconnect();

            messenger.IsConnected.Should().BeFalse();
        }
    }
}