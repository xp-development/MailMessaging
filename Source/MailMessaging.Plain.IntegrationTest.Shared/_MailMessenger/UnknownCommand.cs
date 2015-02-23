using FluentAssertions;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Core;
using NUnit.Framework;
using TestCommands = MailMessaging.Plain.IntegrationTest.Commands;

namespace MailMessaging.Plain.IntegrationTest._MailMessenger
{
    [TestFixture]
    public class UnknownCommand : TestBase
    {
        [Test]
        public void ShouldReceiveBadResponseIfCommandIsUnknown()
        {
            var account = new Account(TestServer, TestPort, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(account, tcpClient);
            messenger.Connect().Result.Should().Be(ConnectResult.Connected);

            var response = messenger.Send(new TestCommands.UnknownCommand(TagService)).Result;

            response.Result.Should().Be(ResponseResult.BAD);
            response.Message.Should().Be("A0001 BAD unknown command\r\n");
        }
    }
}