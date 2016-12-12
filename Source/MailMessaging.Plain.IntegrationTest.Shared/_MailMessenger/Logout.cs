using FluentAssertions;
using MailMessaging.Plain.Contracts.Commands;
using Xunit;

namespace MailMessaging.Plain.IntegrationTest._MailMessenger
{
    public class Logout : TestBase
    {
        [Fact]
        public void ShouldLogout()
        {
            var messenger = GetLoggedInMailMessenger();

            var response = messenger.SendAsync(new LogoutCommand(TagService)).Result;

            response.Result.Should().Be(ResponseResult.OK);
            response.Message.Should().Be("* BYE Server logging out\r\nA0002 OK LOGOUT completed\r\n");
        }
    }
}