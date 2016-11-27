using FluentAssertions;
using MailMessaging.Plain.Core.Text;
using Xunit;

namespace MailMessaging.Plain.Net.UnitTest._Text._ModifiedUTF7
{
    public class Decode
    {
        [Fact]
        public void Usage()
        {
            ModifiedUTF7.Decode("Entw&APw-rfe").Should().Be("Entwürfe");
            ModifiedUTF7.Decode("Gel&APY-scht").Should().Be("Gelöscht");
            ModifiedUTF7.Decode("Fix &ACY- Foxi").Should().Be("Fix & Foxi");
        }
    }
}