using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Contracts.Services;

namespace MailMessaging.Plain.IntegrationTest.Commands
{
    public class UnknownCommand : CommandBase<UnknownCommand, UnknownResponse>
    {
        public override string Request => PrepareCommand("UnknownCommand");

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