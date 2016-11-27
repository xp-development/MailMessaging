using FluentAssertions;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Core;
using MailMessaging.Plain.Core.Commands;
using Xunit;

namespace MailMessaging.Plain.IntegrationTest._MailMessenger
{
    public class Login : TestBase
    {
        [Fact]
        public void ShouldConnectServer()
        {
            var account = new Account(TestServer, TestPort, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(tcpClient);
            messenger.ConnectAsync(account).Result.Should().Be(ConnectResult.Connected);

            var response = messenger.SendAsync(new LoginCommand(TagService, "validUserName", "validPassword")).Result;

            response.Result.Should().Be(ResponseResult.OK);
            response.Message.Should().Be("A0001 OK LOGIN completed\r\n");
        }

        [Theory]
        [InlineData("invalidUserName", "invalidPassword")]
        [InlineData("validUserName", "invalidPassword")]
        [InlineData("invalidUserName", "validPassword")]
        public void ShouldReceiveNoResponseIfLoginDataAreInvalid(string userName, string invalidpassword)
        {
            var account = new Account(TestServer, TestPort, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(tcpClient);
            messenger.ConnectAsync(account).Result.Should().Be(ConnectResult.Connected);

            var response = messenger.SendAsync(new LoginCommand(TagService, userName, invalidpassword)).Result;

            response.Result.Should().Be(ResponseResult.NO);
            response.Message.Should().Be("A0001 NO authentication failed\r\n");
        }
    }
}