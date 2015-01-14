using MailMessaging.Plain.Contracts.Commands;

namespace MailMessaging.Plain.Core.Commands
{
    public class ResponseBase : IResponse
    {
        public ResponseResult Result { get; protected set; }
        public string Message { get; protected set; }
    }
}