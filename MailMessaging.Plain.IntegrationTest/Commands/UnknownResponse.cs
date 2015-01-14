﻿using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MailMessaging.Plain.IntegrationTest.Commands
{
    public class UnknownResponse : ResponseBase
    {
        private UnknownResponse()
        {
        }

        public static UnknownResponse Create(string expectedTag, string responseMessage)
        {
            var regex = new Regex(@"^" + expectedTag + "\\s(OK|NO|BAD)\\s(.*)$");

            var matches = regex.Matches(responseMessage);

            if (matches.Count == 0)
                throw new InvalidOperationException();

            var response = new UnknownResponse
            {
                Result = (ResponseResult)Enum.Parse(typeof(ResponseResult), matches[0].Groups[1].Value),
                Message = responseMessage
            };

            return response;
        }
    }
}
