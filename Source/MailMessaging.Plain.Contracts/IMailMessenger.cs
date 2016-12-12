using System;
using System.Threading.Tasks;
using MailMessaging.Plain.Contracts.Commands;

namespace MailMessaging.Plain.Contracts
{
    public interface IMailMessenger : IDisposable
    {
        bool IsConnected { get; }

        Task<ConnectResult> ConnectAsync(IAccount account);
        Task<TResponse> SendAsync<TRequest, TResponse>(ICommand<TRequest, TResponse> message)
            where TRequest : IRequest
            where TResponse : IResponse;
        void Disconnect();
    }
}