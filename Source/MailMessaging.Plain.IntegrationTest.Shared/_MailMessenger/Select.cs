using System;
using System.Linq;
using FluentAssertions;
using MailMessaging.Plain.Contracts.Commands;
using Xunit;

namespace MailMessaging.Plain.IntegrationTest._MailMessenger
{
	public class Select : TestBase
	{
		[Fact]
		public void Usage()
		{
			var messenger = GetLoggedInMailMessenger();

			var response = messenger.SendAsync(new SelectCommand(TagService, "INBOX")).Result;

			response.Result.Should().Be(ResponseResult.OK);
			response.Exists.Should().Be(5);
			response.Recent.Should().Be(2);
			var flags = response.Flags.ToArray();
			flags.Length.Should().Be(5);
			flags[0].Should().Be("\\Answered");
			flags[1].Should().Be("\\Flagged");
			flags[2].Should().Be("\\Deleted");
			flags[3].Should().Be("\\Seen");
			flags[4].Should().Be("\\Draft");
			response.Permission.Should().Be(MailboxPermission.ReadWrite);
		}

		[Fact]
		public void ShouldReceiveRepsonseResultNoIfUnknownFolder()
		{
			var messenger = GetLoggedInMailMessenger();

			var response = messenger.SendAsync(new SelectCommand(TagService, "invalidFolder")).Result;

			response.Result.Should().Be(ResponseResult.NO);
			Console.WriteLine(response.Message);
			response.Message.Should().Be("A0002 NO unknown folder\r\n");
		}
	}
}