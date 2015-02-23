using System.Linq;
using FluentAssertions;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Contracts.Services;
using MailMessaging.Plain.Core.Commands;
using Moq;
using NUnit.Framework;

namespace MailMessaging.Plain.Net.UnitTest._Commands._ListCommand
{
    [TestFixture]
    public class ParseResponse
    {
        [Test]
        public void ShouldParseSuccessfulMessage()
        {
            const string responseMessage = "* LIST (\\Drafts \\NoInferiors) \"/\" Drafts\r\n" +
                                           "* LIST (\\Trash \\HasNoChildren) \"/\" Trash\r\n" +
                                           "* LIST (\\Sent \\NoInferiors) \"/\" Sent\r\n" +
                                           "* LIST (\\HasChildren) \"/\" INBOX\r\n" +
                                           "* LIST (\\HasChildren) \"/\" INBOX/Subfolder\r\n" +
                                           "* LIST (\\NoInferiors) \"/\" INBOX/Subfolder/Subsubfolder\r\n" +
                                           "* LIST (\\NoInferiors) \"/\" OUTBOX\r\n" +
                                           "* LIST (\\Junk \\NoInferiors) \"/\" Junk\r\n" +
                                           "A0001 OK LIST completed\r\n";

            var tagServiceMock = new Mock<ITagService>();
            tagServiceMock.Setup(item => item.GetNextTag()).Returns("A0001");
            var command = new ListCommand(tagServiceMock.Object, "", "*");

            var response = command.ParseResponse(responseMessage);

            response.Result.Should().Be(ResponseResult.OK);
            var listFolders = response.Folders.ToArray();
            listFolders.Length.Should().Be(8);

            listFolders[0].Attributes.Count().Should().Be(2);
            listFolders[0].Attributes.ElementAt(0).Should().Be("\\Drafts");
            listFolders[0].Attributes.ElementAt(1).Should().Be("\\NoInferiors");
            listFolders[0].HierarchyDelimiter.Should().Be("/");
            listFolders[0].Name.Should().Be("Drafts");

            listFolders[1].Attributes.Count().Should().Be(2);
            listFolders[1].Attributes.ElementAt(0).Should().Be("\\Trash");
            listFolders[1].Attributes.ElementAt(1).Should().Be("\\HasNoChildren");
            listFolders[1].HierarchyDelimiter.Should().Be("/");
            listFolders[1].Name.Should().Be("Trash");

            listFolders[2].Attributes.Count().Should().Be(2);
            listFolders[2].Attributes.ElementAt(0).Should().Be("\\Sent");
            listFolders[2].Attributes.ElementAt(1).Should().Be("\\NoInferiors");
            listFolders[2].HierarchyDelimiter.Should().Be("/");
            listFolders[2].Name.Should().Be("Sent");

            listFolders[3].Attributes.Count().Should().Be(1);
            listFolders[3].Attributes.ElementAt(0).Should().Be("\\HasChildren");
            listFolders[3].HierarchyDelimiter.Should().Be("/");
            listFolders[3].Name.Should().Be("INBOX");

            listFolders[4].Attributes.Count().Should().Be(1);
            listFolders[4].Attributes.ElementAt(0).Should().Be("\\HasChildren");
            listFolders[4].HierarchyDelimiter.Should().Be("/");
            listFolders[4].Name.Should().Be("INBOX/Subfolder");

            listFolders[5].Attributes.Count().Should().Be(1);
            listFolders[5].Attributes.ElementAt(0).Should().Be("\\NoInferiors");
            listFolders[5].HierarchyDelimiter.Should().Be("/");
            listFolders[5].Name.Should().Be("INBOX/Subfolder/Subsubfolder");

            listFolders[6].Attributes.Count().Should().Be(1);
            listFolders[6].Attributes.ElementAt(0).Should().Be("\\NoInferiors");
            listFolders[6].HierarchyDelimiter.Should().Be("/");
            listFolders[6].Name.Should().Be("OUTBOX");

            listFolders[7].Attributes.Count().Should().Be(2);
            listFolders[7].Attributes.ElementAt(0).Should().Be("\\Junk");
            listFolders[7].Attributes.ElementAt(1).Should().Be("\\NoInferiors");
            listFolders[7].HierarchyDelimiter.Should().Be("/");
            listFolders[7].Name.Should().Be("Junk");
        }

        [Test]
        public void ShouldParseEmptyResponse()
        {
            const string responseMessage = "A0001 OK LIST completed\r\n";

            var tagServiceMock = new Mock<ITagService>();
            tagServiceMock.Setup(item => item.GetNextTag()).Returns("A0001");
            var command = new ListCommand(tagServiceMock.Object, "", "*");

            var response = command.ParseResponse(responseMessage);

            response.Result.Should().Be(ResponseResult.OK);
            var listFolders = response.Folders.ToArray();
            listFolders.Length.Should().Be(0);
        }

        [Test]
        public void ShouldParseResponseWithoutFolderAttributes()
        {
            const string responseMessage = "* LIST () \"/\" INBOX\r\n" +
                                          "A0001 OK LIST completed\r\n";

            var tagServiceMock = new Mock<ITagService>();
            tagServiceMock.Setup(item => item.GetNextTag()).Returns("A0001");
            var command = new ListCommand(tagServiceMock.Object, "", "*");

            var response = command.ParseResponse(responseMessage);

            response.Result.Should().Be(ResponseResult.OK);
            var listFolders = response.Folders.ToArray();
            listFolders.Length.Should().Be(1);
            listFolders[0].Attributes.Count().Should().Be(0);
            listFolders[0].HierarchyDelimiter.Should().Be("/");
            listFolders[0].Name.Should().Be("INBOX");
        }
    }
}