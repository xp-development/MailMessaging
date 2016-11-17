namespace MailMessaging.Plain.Contracts.Commands
{
    public interface IResponse
    {
        ResponseResult Result { get; }
        string Message { get; }
    }
}