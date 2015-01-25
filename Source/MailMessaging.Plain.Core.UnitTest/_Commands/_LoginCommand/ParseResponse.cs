using System;
using FluentAssertions;
using MailMessaging.Plain.Contracts.Commands;
using MailMessaging.Plain.Contracts.Services;
using MailMessaging.Plain.Core.Commands;
using Moq;
using NUnit.Framework;

namespace MailMessaging.Plain.Core.UnitTest._Commands._LoginCommand
{
    [TestFixture]
    public class ParseResponse
    {
        [Test]
        public void ShouldParseSuccessfulMessage()
        {
            var tagServiceMock = new Mock<ITagService>();
            tagServiceMock.Setup(item => item.GetNextTag()).Returns("A0001");
            var command = new LoginCommand(tagServiceMock.Object, "userName", "password");

            var response = command.ParseResponse("A0001 OK LOGIN completed\r\n");

            response.Result.Should().Be(ResponseResult.OK);
        }

        [Test]
        public void ShouldReceiveRepsonseResultNoIfLoginDataIsWrong()
        {
            var tagServiceMock = new Mock<ITagService>();
            tagServiceMock.Setup(item => item.GetNextTag()).Returns("A0001");
            var command = new LoginCommand(tagServiceMock.Object, "wrongUserName", "wrongPassword");

            var response = command.ParseResponse("A0001 NO authentication failed\r\n");

            response.Result.Should().Be(ResponseResult.NO);
        }

        [Test]
        public void ShouldThrowInvalidOperationExceptionIfTagIsWrong()
        {
            var tagServiceMock = new Mock<ITagService>();
            tagServiceMock.Setup(item => item.GetNextTag()).Returns("A0001");
            var command = new LoginCommand(tagServiceMock.Object, "userName", "password");

            Assert.Throws<InvalidOperationException>(() => command.ParseResponse("A4321 OK LOGIN completed\r\n"));
        }
    }
}