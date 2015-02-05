﻿using MailMessaging.Plain.Contracts.Services;
using MailMessaging.Plain.Core.Commands;

namespace MailMessaging.Plain.IntegrationTest.Commands
{
    public class UnknownCommand : CommandBase<UnknownCommand, UnknownResponse>
    {
        public override string Request
        {
            get { return PrepareCommand("UnknownCommand"); }
        }

        public UnknownCommand(ITagService tagService)
            : base(tagService)
        {
        }

        public override UnknownResponse ParseResponse(string responseMessage)
        {
            return UnknownResponse.Create(Tag, responseMessage);
        }
    }
}