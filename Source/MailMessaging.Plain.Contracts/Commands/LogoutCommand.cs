using System;
using System.Linq;
using System.Text.RegularExpressions;
using MailMessaging.Plain.Contracts.Services;

namespace MailMessaging.Plain.Contracts.Commands
{
    public class LogoutCommand : CommandBase<LogoutCommand, LogoutCommand.LogoutResponse>
    {
        public override string Request => PrepareCommand("LOGOUT");

        public LogoutCommand(ITagService tagService)
            : base(tagService)
        {
        }

        public override LogoutResponse ParseResponse(string responseMessage)
        {
            return LogoutResponse.Create(Tag, responseMessage);
        }

        public class LogoutResponse : ResponseBase
        {
            private LogoutResponse()
            {
            }

            public static LogoutResponse Create(string expectedTag, string responseMessage)
            {
                var lines = responseMessage.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                var matches = Regex.Matches(lines.First(line => !line.StartsWith("*")), @"^" + expectedTag + "\\s(OK|NO|BAD)\\s(.*)$");
                if (matches.Count == 0)
                    throw new InvalidOperationException();

                var response = new LogoutResponse
                {
                    Result = (ResponseResult) Enum.Parse(typeof (ResponseResult), matches[0].Groups[1].Value),
                    Message = responseMessage
                };

                return response;
            }
        }
    }
}