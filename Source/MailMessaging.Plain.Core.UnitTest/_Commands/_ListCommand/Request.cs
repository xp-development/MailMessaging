using FluentAssertions;
using MailMessaging.Plain.Contracts.Services;
using MailMessaging.Plain.Core.Commands;
using Moq;
using NUnit.Framework;

namespace MailMessaging.Plain.Core.UnitTest._Commands._ListCommand
{
    [TestFixture]
    public class Request
    {
        [TestCase("", "")]
        [TestCase("", "*")]
        [TestCase("INBOX", "*")]
        public void ShouldCreateListRequest(string referenceName, string mailboxName)
        {
            var tagServiceMock = new Mock<ITagService>();
            tagServiceMock.Setup(item => item.GetNextTag()).Returns("A0001");

            var command = new ListCommand(tagServiceMock.Object, referenceName, mailboxName);

            command.Request.Should().Be(string.Format("A0001 LIST \"{0}\" \"{1}\"\r\n", referenceName, mailboxName));
        }

        [Test]
        public void ShouldCreateFullListCommandRequest()
        {
            var tagServiceMock = new Mock<ITagService>();
            tagServiceMock.Setup(item => item.GetNextTag()).Returns("A0001");

            var command = ListCommand.CreateFullListCommand(tagServiceMock.Object);

            command.Request.Should().Be("A0001 LIST \"\" \"*\"\r\n");
        }
    }
}