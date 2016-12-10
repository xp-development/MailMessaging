using System;
using System.Xml.Serialization;
using FluentAssertions;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Contracts.Services;
using MailMessaging.Plain.Core;
using MailMessaging.Plain.Core.Services;

namespace MailMessaging.Plain.IntegrationTest
{
    public class TestBase : IDisposable
    {
        public TestBase()
        {
            TagService = new TagService();

            var fakeAccount = GetFakeAccount();
            _server = new FakeImapServer(new TcpListener(), fakeAccount);
            _server.SetConfiguration(new ImapServerConfiguration(TestServer, TestPort, false));

            _server.Start();
        }

        public void Dispose()
        {
            _server.Stop();
        }

        protected MailMessenger GetLoggedInMailMessenger()
        {
            var account = new Account(TestServer, TestPort, false);

            var tcpClient = new TcpClient();
            var messenger = new MailMessenger(tcpClient);
            messenger.ConnectAsync(account).Result.Should().Be(ConnectResult.Connected);

            messenger.SendAsync(new LoginCommand(TagService, "validUserName", "validPassword")).Result.Result.Should().Be(ResponseResult.OK);
            return messenger;
        }

        private static FakeAccount GetFakeAccount()
        {
            var xmlSerializer = new XmlSerializer(typeof (FakeAccount));

#if WinRT
            var assembly = typeof (TcpListener).GetTypeInfo().Assembly;
#else
            var assembly = typeof (TcpListener).Assembly;
#endif

            var xmlStream = assembly.GetManifestResourceStream("MailMessaging.Plain.IntegrationTest.TestFiles.FakeAccount.xml");

            return (FakeAccount) xmlSerializer.Deserialize(xmlStream);
        }

        protected void ActivateServerTls()
        {
            _server.UseTls(true);
        }

        private readonly FakeImapServer _server;
        protected ITagService TagService;
        protected int TestPort = 51234;
        protected string TestServer = "127.0.0.1";
    }
}