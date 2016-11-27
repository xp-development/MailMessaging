using FluentAssertions;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Core;
using Xunit;
using TestCommands = MailMessaging.Plain.IntegrationTest.Commands;

namespace MailMessaging.Plain.IntegrationTest._MailMessenger
{
    public class UnknownCommand : TestBase
    {
        [Fact]
        public void ShouldReceiveBadResponseIfCommandIsUnknown()
        {
            var account = new Account(TestServer, TestPort, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(tcpClient);
            messenger.ConnectAsync(account).Result.Should().Be(ConnectResult.Connected);

            var response = messenger.SendAsync(new TestCommands.UnknownCommand(TagService)).Result;

            response.Result.Should().Be(ResponseResult.BAD);
            response.Message.Should().Be("A0001 BAD unknown command\r\n");
        }
    }
}