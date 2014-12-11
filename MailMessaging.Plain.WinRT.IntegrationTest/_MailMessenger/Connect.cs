using FluentAssertions;
using MailMessaging.Plain.Core;
using NUnit.Framework;
#if WinRT
 using MailMessaging.Plain.WinRT;
#elif NET
 using MailMessaging.Plain.Net;
#endif

namespace MailMessaging.Plain.IntegrationTest._MailMessenger
{
    [TestFixture]
    public class Connect
    {
        [Test]
        public void ShouldConnectServer()
        {
            var account = new Account("127.0.0.1", 51234, "validUserName", "validPassword", false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(account, tcpClient);
            messenger.Connect().Result.Should().Be(ConnectResult.Connected);
        }

        [Test]
        public void ShouldNotConnectServerIfHostIsWrong()
        {
            var account = new Account("222.222.222.222", 51234, "validUserName", "validPassword", false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(account, tcpClient);
            messenger.Connect().Result.Should().Be(ConnectResult.UnknownHost);
        }

        [Test]
        public void ShouldNotConnectServerIfPortIsWrong()
        {
            var account = new Account("127.0.0.1", 11111, "validUserName", "validPassword", false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(account, tcpClient);
            messenger.Connect().Result.Should().Be(ConnectResult.UnknownHost);
        }
    }
}