using FluentAssertions;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Contracts.Services;
using Moq;
using Xunit;

namespace MailMessaging.Plain.Net.UnitTest._Commands._SelectCommand
{
  public class Request
  {
    [Fact]
    public void Using()
    {
      var tagServiceMock = new Mock<ITagService>();
      tagServiceMock.Setup(item => item.GetNextTag()).Returns("A0001");

      var command = new SelectCommand(tagServiceMock.Object, "mailboxName");

      command.Request.Should().Be("A0001 SELECT \"mailboxName\"\r\n");
    }
  }
}