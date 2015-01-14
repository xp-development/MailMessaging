using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Contracts.Services;
using MailMessaging.Plain.Core.Services;
using System;

namespace MailMessaging.Plain.Core.Commands
{
    public abstract class CommandBase<TRequest, TResponse>
        : IRequest, ICommand<TRequest, TResponse>
        where TRequest : IRequest
        where TResponse : IResponse
    {
        public string Tag { get; private set; }

        public abstract string Request { get; }

        protected CommandBase(ITagService tagService)
        {
            Tag = tagService.GetNextTag();
        }

        public abstract TResponse ParseResponse(string responseMessage);

        protected string PrepareMessage(string message)
        {
            if (_wasExecuted)
                throw new InvalidOperationException("Use a command only once!");

            _wasExecuted = true;

            return string.Format("{0} {1}{2}", Tag, message, "\r\n");
        }

        private bool _wasExecuted;
    }
}