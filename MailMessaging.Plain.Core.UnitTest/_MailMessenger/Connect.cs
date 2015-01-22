using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MailMessaging.Plain.Contracts;
using Moq;
using NUnit.Framework;

namespace MailMessaging.Plain.Core.UnitTest._MailMessenger
{
    [TestFixture]
    public class Connect
    {
        [Test]
        public void ShouldConnectServer()
        {
            var readAsyncMessages = new Queue<string>();
            readAsyncMessages.Enqueue("* OK IMAP server ready");
            readAsyncMessages.Enqueue(string.Empty);

            var account = new Account("valid.server", 42, true);
            var tcpClientMock = new Mock<ITcpClient>();
            tcpClientMock.Setup(item => item.Connect(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.Run(() => { }));
            tcpClientMock.Setup(item => item.ReadAsync()).Returns(() => Task.FromResult(readAsyncMessages.Dequeue()));

            var messenger = new MailMessenger(account, tcpClientMock.Object);
            messenger.Connect().Result.Should().Be(ConnectResult.Connected);
        }

        [Test]
        public void ShouldNotConnectIfHostNameIsWrong()
        {
            var account = new Account("invalid.server", 42, true);
            var tcpClientMock = new Mock<ITcpClient>();
            tcpClientMock.Setup(item => item.Connect(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Throws<Exception>();

            var messenger = new MailMessenger(account, tcpClientMock.Object);
            messenger.Connect().Result.Should().Be(ConnectResult.UnknownHost);
        }

        [Test]
        public void ShouldNotConnectIfSslIsRequiredButNotUsed()
        {
            var account = new Account("valid.server", 42, false);
            var tcpClientMock = new Mock<ITcpClient>();
            tcpClientMock.Setup(item => item.Connect(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.Run(() => { }));
            tcpClientMock.Setup(item => item.ReadAsync()).ReturnsAsync(string.Empty);

            var messenger = new MailMessenger(account, tcpClientMock.Object);
            messenger.Connect().Result.Should().Be(ConnectResult.UnknownHost);
        }
    }
}