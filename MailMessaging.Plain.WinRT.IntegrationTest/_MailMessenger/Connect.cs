using FluentAssertions;
using MailMessaging.Plain.Core;
#if WinRT
 using MailMessaging.Plain.WinRT;
#elif NET
 using MailMessaging.Plain.Net;
#endif
using NUnit.Framework;


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
    }
}