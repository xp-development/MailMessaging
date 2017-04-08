using System.Linq;
using FluentAssertions;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Contracts.Services;
using Moq;
using Xunit;

namespace MailMessaging.Plain.Net.UnitTest._Commands._SelectCommand
{
	public class ParseResponse
	{
		[Fact]
		public void ShouldParseSuccessfulMessage()
		{
			const string responseMessage = "* 172 EXISTS\r\n" +
			                               "* 1 RECENT\r\n" +
			                               "* OK [UNSEEN 12] Message 12 is first unseen\r\n" +
			                               "* OK [UIDVALIDITY 3857529045] UIDs valid\r\n" +
			                               "* OK [UIDNEXT 4392] Predicted next UID\r\n" +
			                               "* FLAGS (\\Answered \\Flagged \\Deleted \\Seen \\Draft)\r\n" +
			                               "* OK [PERMANENTFLAGS (\\Deleted \\Seen \\*)] Limited\r\n" +
										   "A0001 OK [READ-WRITE] SELECT completed";

			var tagServiceMock = new Mock<ITagService>();
			tagServiceMock.Setup(item => item.GetNextTag()).Returns("A0001");
			var command = new SelectCommand(tagServiceMock.Object, "INBOX");

			var response = command.ParseResponse(responseMessage);

			response.Result.Should().Be(ResponseResult.OK);
			response.Exists.Should().Be(172);
			response.Recent.Should().Be(1);
			response.Unseen.Should().Be(12);
			response.UidValidity.Should().Be((ulong)3857529045);
			response.UidNext.Should().Be((ulong)4392);
			var flags = response.Flags.ToArray();
			flags.Length.Should().Be(5);
			flags[0].Should().Be("\\Answered");
			flags[1].Should().Be("\\Flagged");
			flags[2].Should().Be("\\Deleted");
			flags[3].Should().Be("\\Seen");
			flags[4].Should().Be("\\Draft");
			var permanentFlags = response.PermanentFlags.ToArray();
			permanentFlags.Length.Should().Be(3);
			permanentFlags[0].Should().Be("\\Deleted");
			permanentFlags[1].Should().Be("\\Seen");
			permanentFlags[2].Should().Be("\\*");
			response.Permission.Should().Be(MailboxPermission.ReadWrite);
		}

		[Fact]
		public void ShouldReceiveRepsonseResultNoIfUnknownFolder()
		{
			var tagServiceMock = new Mock<ITagService>();
			tagServiceMock.Setup(item => item.GetNextTag()).Returns("A0002");
			var command = new SelectCommand(tagServiceMock.Object, "unknownFolder");

			var response = command.ParseResponse("A0002 NO unknown folder\r\n");

			response.Result.Should().Be(ResponseResult.NO);
		}
	}
}