using FluentAssertions;
using MailMessaging.Plain.Contracts.Services;
using MailMessaging.Plain.Core.Commands;
using Moq;
using NUnit.Framework;

namespace MailMessaging.Plain.Core.UnitTest._Commands._LoginCommand
{
    [TestFixture]
    public class Request
    {
        [Test]
        public void Using()
        {
            var tagServiceMock = new Mock<ITagService>();
            tagServiceMock.Setup(item => item.GetNextTag()).Returns("A0001");

            var command = new LoginCommand(tagServiceMock.Object, "userName", "password");

            command.Request.Should().Be("A0001 LOGIN userName password\r\n");
        }
    }
}