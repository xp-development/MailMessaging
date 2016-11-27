using FluentAssertions;
using MailMessaging.Plain.Core.Text;
using Xunit;

namespace MailMessaging.Plain.Net.UnitTest._Text._ModifiedUTF7
{
    public class Encode
    {
        [Fact]
        public void Usage()
        {
            ModifiedUTF7.Encode("Entwürfe").Should().Be("Entw&APw-rfe");
            ModifiedUTF7.Encode("Gelöscht").Should().Be("Gel&APY-scht");
            ModifiedUTF7.Encode("Fix & Foxi").Should().Be("Fix &ACY- Foxi");
        }
    }
}