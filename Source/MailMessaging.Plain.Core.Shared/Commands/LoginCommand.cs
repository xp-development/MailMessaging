using System;
using System.Text.RegularExpressions;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Contracts.Services;

namespace MailMessaging.Plain.Core.Commands
{
    public class LoginCommand : CommandBase<LoginCommand, LoginCommand.LoginResponse>
    {
        public override string Request => PrepareCommand($"LOGIN {_userName} {_password}");

        public LoginCommand(ITagService tagService, string userName, string password)
            : base(tagService)
        {
            _userName = userName;
            _password = password;
        }

        public override LoginResponse ParseResponse(string responseMessage)
        {
            return LoginResponse.Create(Tag, responseMessage);
        }

        private readonly string _password;
        private readonly string _userName;

        public class LoginResponse : ResponseBase
        {
            private LoginResponse()
            {
            }

            public static LoginResponse Create(string expectedTag, string responseMessage)
            {
                var matches = Regex.Matches(responseMessage, @"^" + expectedTag + "\\s(OK|NO|BAD)\\s(.*)$");

                if (matches.Count == 0)
                    throw new InvalidOperationException();

                var response = new LoginResponse
                {
                    Result = (ResponseResult) Enum.Parse(typeof (ResponseResult), matches[0].Groups[1].Value),
                    Message = responseMessage
                };

                return response;
            }
        }
    }
}