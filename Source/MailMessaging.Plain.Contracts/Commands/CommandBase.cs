using System;
using MailMessaging.Plain.Contracts.Services;

namespace MailMessaging.Plain.Contracts.Commands
{
    public abstract class CommandBase<TRequest, TResponse>
        : IRequest, ICommand<TRequest, TResponse>
        where TRequest : IRequest
        where TResponse : IResponse
    {
        public string Tag { get; }

        public abstract string Request { get; }

        protected CommandBase(ITagService tagService)
        {
            Tag = tagService.GetNextTag();
        }

        public abstract TResponse ParseResponse(string responseMessage);

        protected string PrepareCommand(string message)
        {
            if (_wasExecuted)
                throw new InvalidOperationException("Use a command only once!");

            _wasExecuted = true;

            return $"{Tag} {message}\r\n";
        }

        private bool _wasExecuted;
    }
}