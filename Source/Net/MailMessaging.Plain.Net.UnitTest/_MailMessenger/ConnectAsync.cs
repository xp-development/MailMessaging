using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using MailMessaging.Plain.Contracts;
using MailMessaging.Plain.Core;
using Moq;
using Xunit;

namespace MailMessaging.Plain.Net.UnitTest._MailMessenger
{
    public class ConnectAsync
    {
        [Fact]
        public void ShouldConnectServer()
        {
            var readAsyncMessages = new Queue<string>();
            readAsyncMessages.Enqueue("* OK IMAP server ready");
            readAsyncMessages.Enqueue(string.Empty);

            var account = new Account("valid.server", 42, true);
            var tcpClientMock = new Mock<ITcpClient>();
            tcpClientMock.Setup(item => item.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.Run(() => { }));
            tcpClientMock.Setup(item => item.ReadAsync()).Returns(() => Task.FromResult(readAsyncMessages.Dequeue()));

            var messenger = new MailMessenger(tcpClientMock.Object);
            messenger.ConnectAsync(account).Result.Should().Be(ConnectResult.Connected);
        }

        [Fact]
        public void ShouldNotConnectIfHostNameIsWrong()
        {
            var account = new Account("invalid.server", 42, true);
            var tcpClientMock = new Mock<ITcpClient>();
            tcpClientMock.Setup(item => item.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>())).Throws<Exception>();

            var messenger = new MailMessenger(tcpClientMock.Object);
            messenger.ConnectAsync(account).Result.Should().Be(ConnectResult.UnknownHost);
        }

        [Fact]
        public void ShouldNotConnectIfTlsIsRequiredButNotUsed()
        {
            var account = new Account("valid.server", 42, false);
            var tcpClientMock = new Mock<ITcpClient>();
            tcpClientMock.Setup(item => item.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<bool>())).Returns(Task.Run(() => { }));
            tcpClientMock.Setup(item => item.ReadAsync()).ReturnsAsync(string.Empty);

            var messenger = new MailMessenger(tcpClientMock.Object);
            messenger.ConnectAsync(account).Result.Should().Be(ConnectResult.UnknownHost);
        }
    }
}