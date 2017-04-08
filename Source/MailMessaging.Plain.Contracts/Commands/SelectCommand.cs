using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MailMessaging.Plain.Contracts.Services;

namespace MailMessaging.Plain.Contracts.Commands
{
	public class SelectCommand
		: CommandBase<SelectCommand, SelectCommand.SelectResponse>
	{
		public override string Request => PrepareCommand($"SELECT \"{_mailboxName}\"");

		public SelectCommand(ITagService tagService, string mailboxName)
			: base(tagService)
		{
			_mailboxName = mailboxName;
		}

		public override SelectResponse ParseResponse(string responseMessage)
		{
			return SelectResponse.Create(Tag, responseMessage);
		}

		private readonly string _mailboxName;

		public class SelectResponse : ResponseBase
		{
			public int Exists { get; private set; }
			public int Recent { get; private set; }
			public int? Unseen { get; private set; }
			public ulong? UidValidity { get; private set; }
			public ulong? UidNext { get; private set; }

			public IEnumerable<string> Flags => _flags;

			public IEnumerable<string> PermanentFlags => _permanentFlags;

			public MailboxPermission Permission { get; private set; }

			public SelectResponse()
			{
				_flags = new List<string>();
				_permanentFlags = new List<string>();
			}

			public static SelectResponse Create(string expectedTag, string responseMessage)
			{
				var lines = responseMessage.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);

				var matches = Regex.Matches(lines.Last(), @"^" + expectedTag + "\\s(OK|NO|BAD)\\s(.*)$");
				if(matches.Count == 0)
					throw new InvalidOperationException();

				var selectResponse = new SelectResponse
				{
					Result = (ResponseResult) Enum.Parse(typeof(ResponseResult), matches[0].Groups[1].Value),
					Message = responseMessage
				};

				if(selectResponse.Result != ResponseResult.OK)
					return selectResponse;

				var existsMatch = Regex.Match(responseMessage, @"\*\s(\d+)\sEXISTS", RegexOptions.Singleline);
				var recentMatch = Regex.Match(responseMessage, @"\*\s(\d+)\sRECENT", RegexOptions.Singleline);

				selectResponse.Recent = Convert.ToInt32(recentMatch.Groups[1].Value);
				selectResponse.Exists = Convert.ToInt32(existsMatch.Groups[1].Value);

				var flagsMatch = Regex.Match(responseMessage, @"\*\sFLAGS\s+\((.*?)\)", RegexOptions.Singleline);
				if(!flagsMatch.Success)
					throw new NotSupportedException("Invalid response for SELECT command: " + Environment.NewLine + responseMessage);

				selectResponse._flags.AddRange(flagsMatch.Groups[1].Value.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));

				var permanentFlagsMatch = Regex.Match(responseMessage, @"\*\sOK\s\[PERMANENTFLAGS\s+\((.*?)\)\]", RegexOptions.Singleline);
				if (permanentFlagsMatch.Success)
					selectResponse._permanentFlags.AddRange(permanentFlagsMatch.Groups[1].Value.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));

				var unseenMatch = Regex.Match(responseMessage, @"\*\sOK\s\[UNSEEN\s(\d+)\]", RegexOptions.Singleline);
				if(unseenMatch.Success)
					selectResponse.Unseen = Convert.ToInt32(unseenMatch.Groups[1].Value);

				var uidValidityMatch = Regex.Match(responseMessage, @"\*\sOK\s\[UIDVALIDITY\s(\d+)\]", RegexOptions.Singleline);
				if(uidValidityMatch.Success)
					selectResponse.UidValidity = Convert.ToUInt64(uidValidityMatch.Groups[1].Value);

				var uidNextMatch = Regex.Match(responseMessage, @"\*\sOK\s\[UIDNEXT\s(\d+)\]", RegexOptions.Singleline);
				if(uidNextMatch.Success)
					selectResponse.UidNext = Convert.ToUInt64(uidNextMatch.Groups[1].Value);

				return selectResponse;
			}

			private readonly List<string> _flags;
			private readonly List<string> _permanentFlags;
		}
	}
}