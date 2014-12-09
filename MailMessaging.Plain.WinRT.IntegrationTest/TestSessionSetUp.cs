using NUnit.Framework;

namespace MailMessaging.Plain.IntegrationTest
{
    [SetUpFixture]
    public class TestSessionSetUp
    {
        [SetUp]
        public void SetUp()
        {
            _server = new FakeImapServer(new TcpListener());
            _server.SetConfiguration(new ImapServerConfiguration("127.0.0.1", 51234, false));

            _server.Start();
        }

        [TearDown]
        public void TearDown()
        {
            _server.Stop();
        }

        private FakeImapServer _server;
    }
}