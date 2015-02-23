using System.Reflection;
using System.Xml.Serialization;
using FluentAssertions;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Contracts.Services;
using MailMessaging.Plain.Core;
using MailMessaging.Plain.Core.Services;
using MailMessaging.Plain.Core.Commands;
using NUnit.Framework;

namespace MailMessaging.Plain.IntegrationTest
{
    public class TestBase
    {
        [SetUp]
        public void SetUp()
        {
            TagService = new TagService();

            var fakeAccount = GetFakeAccount();
            _server = new FakeImapServer(new TcpListener(), fakeAccount);
            _server.SetConfiguration(new ImapServerConfiguration(TestServer, TestPort, false));

            _server.Start();
        }

        protected MailMessenger GetLoggedInMailMessenger()
        {
            var account = new Account(TestServer, TestPort, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(account, tcpClient);
            messenger.Connect().Result.Should().Be(ConnectResult.Connected);

            messenger.Send(new LoginCommand(TagService, "validUserName", "validPassword")).Result.Result.Should().Be(ResponseResult.OK);
            return messenger;
        }

        private static FakeAccount GetFakeAccount()
        {
            var xmlSerializer = new XmlSerializer(typeof (FakeAccount));

#if WinRT
            var assembly = typeof (TcpListener).GetTypeInfo().Assembly;
#elif NET
            var assembly = typeof (TcpListener).Assembly;
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
        protected ITagService TagService;
        protected int TestPort = 51234;
        protected string TestServer = "127.0.0.1";
    }
}