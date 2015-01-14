using FluentAssertions;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Core;
using MailMessaging.Plain.Core.Commands;
#if WinRT
 using MailMessaging.Plain.WinRT;
#elif NET
 using MailMessaging.Plain.Net;
#endif
using NUnit.Framework;

namespace MailMessaging.Plain.IntegrationTest._MailMessenger
{
    [TestFixture]
    public class Login : TestBase
    {
        [Test]
        public void ShouldConnectServer()
        {
            var account = new Account("127.0.0.1", 51234, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(account, tcpClient);
            messenger.Connect().Result.Should().Be(ConnectResult.Connected);

            var response = messenger.Send(new LoginCommand(_tagService, "validUserName", "validPassword")).Result;

            response.Result.Should().Be(ResponseResult.OK);
            response.Message.Should().Be("A0001 OK LOGIN completed\r\n");
        }

        [Test]
        public void ShouldConnectServerIfLoginDataAreInvalid()
        {
            var account = new Account("127.0.0.1", 51234, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(account, tcpClient);
            messenger.Connect().Result.Should().Be(ConnectResult.Connected);

            var response = messenger.Send(new LoginCommand(_tagService, "invalidUserName", "invalidPassword")).Result;

            response.Result.Should().Be(ResponseResult.NO);
            response.Message.Should().Be("A0001 NO authentication failed\r\n");
        }
    }
}