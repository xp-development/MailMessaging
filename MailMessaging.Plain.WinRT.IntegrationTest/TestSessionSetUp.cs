using System.Reflection;
using System.Xml.Serialization;
using NUnit.Framework;

namespace MailMessaging.Plain.IntegrationTest
{
    [SetUpFixture]
    public class TestSessionSetUp
    {
        [SetUp]
        public void SetUp()
        {
            var fakeAccount = GetFakeAccount();
            _server = new FakeImapServer(new TcpListener(), fakeAccount);
            _server.SetConfiguration(new ImapServerConfiguration("127.0.0.1", 51234, false));

            _server.Start();
        }

        private static FakeAccount GetFakeAccount()
        {
            var xmlSerializer = new XmlSerializer(typeof (FakeAccount));

#if WinRT
            var assembly = typeof (FakeAccount).GetTypeInfo().Assembly;
#elif NET
 var assembly = typeof(FakeAccount).Assembly;
#endif

            var xmlStream =
                assembly.GetManifestResourceStream("MailMessaging.Plain.IntegrationTest.TestFiles.FakeAccount.xml");

            var manifestResourceNames = assembly.GetManifestResourceNames();
            var fakeAccount = xmlSerializer.Deserialize(xmlStream);

            return (FakeAccount) fakeAccount;
        }

        [TearDown]
        public void TearDown()
        {
            _server.Stop();
        }

        private FakeImapServer _server;
    }
}