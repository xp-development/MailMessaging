using MailMessaging.Plain.Contracts.Services;

namespace MailMessaging.Plain.Core.Services
{
    public class TagService : ITagService
    {
        public TagService(int maxCount = 9999)
        {
            _maxCount = maxCount;
        }

        public string GetNextTag()
        {
            if(_counter > _maxCount)
                _counter = 0;

            return $"A{++_counter:0000}";
        }

        private readonly int _maxCount;
        private int _counter;
    }
}