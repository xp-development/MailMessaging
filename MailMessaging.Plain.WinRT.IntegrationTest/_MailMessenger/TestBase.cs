using MailMessaging.Plain.Contracts.Services;
using MailMessaging.Plain.Core.Commands;
using MailMessaging.Plain.Core.Services;
using NUnit.Framework;

namespace MailMessaging.Plain.IntegrationTest._MailMessenger
{
    public class TestBase
    {
        [SetUp]
        public void SetUp()
        {
            _tagService = new TagService();
        }

        protected ITagService _tagService;
    }
}
