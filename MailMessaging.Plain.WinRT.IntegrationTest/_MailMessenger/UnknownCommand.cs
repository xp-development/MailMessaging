using FluentAssertions;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Core;
using TestCommands = MailMessaging.Plain.IntegrationTest.Commands;
#if WinRT
 using MailMessaging.Plain.WinRT;
#elif NET
 using MailMessaging.Plain.Net;
#endif
using NUnit.Framework;

namespace MailMessaging.Plain.IntegrationTest._MailMessenger
{
    [TestFixture]
    public class UnknownCommand : TestBase
    {
        [Test]
        public void ShouldReceiveBadResponseIfCommandIsUnknown()
        {
            var account = new Account("127.0.0.1", 51234, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(account, tcpClient);
            messenger.Connect().Result.Should().Be(ConnectResult.Connected);

            var response = messenger.Send(new TestCommands.UnknownCommand(_tagService)).Result;

            response.Result.Should().Be(ResponseResult.BAD);
            response.Message.Should().Be("A0001 BAD unknown command\r\n");
        }
    }
}
