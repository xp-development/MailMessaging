namespace MailMessaging.Plain.Contracts.Commands
{
    public class ResponseBase : IResponse
    {
        public ResponseResult Result { get; protected set; }
        public string Message { get; protected set; }
    }
}