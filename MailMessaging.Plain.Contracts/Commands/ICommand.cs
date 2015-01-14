namespace MailMessaging.Plain.Contracts.Commands
{
    public interface ICommand<TRequest, out TResponse>
        where TResponse : IResponse
    {
        string Request { get; }
        TResponse ParseResponse(string responseMessage);
    }
}