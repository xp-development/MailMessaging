using System.Reflection;
using System.Xml.Serialization;
using MailMessaging.Plain.Contracts.Services;
using MailMessaging.Plain.Core.Services;
using NUnit.Framework;

namespace MailMessaging.Plain.IntegrationTest
{
    public class TestBase
    {
        [SetUp]
        public void SetUp()
        {

            _tagService = new TagService();

            var fakeAccount = GetFakeAccount();
            _server = new FakeImapServer(new TcpListener(), fakeAccount);
            _server.SetConfiguration(new ImapServerConfiguration("127.0.0.1", 51234, false));

            _server.Start();
        }

        private static FakeAccount GetFakeAccount()
        {
            var xmlSerializer = new XmlSerializer(typeof (FakeAccount));

#if WinRT
            var assembly = typeof (TcpListener).GetTypeInfo().Assembly;
#elif NET
            var assembly = typeof(TcpListener).Assembly;
#endif

            var xmlStream = assembly.GetManifestResourceStream("MailMessaging.Plain.IntegrationTest.FakeAccount.xml");

            return (FakeAccount) xmlSerializer.Deserialize(xmlStream);
        }

        [TearDown]
        public void TearDown()
        {
            _server.Stop();
        }

        private FakeImapServer _server;
        protected ITagService _tagService;
    }
}