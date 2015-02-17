using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using NUnit.Framework;

namespace MailMessaging.Plain.IntegrationTest
{
    [TestFixture, Explicit]
    public class Manual
    {
        [Test]
        public void CreateFakeAccountXml()
        {
            var xmlSerializer = new XmlSerializer(typeof(FakeAccount));
            var stringBuilder = new StringBuilder();
            xmlSerializer.Serialize(new StringWriter(stringBuilder), GetFakeAccount());
            Debug.WriteLine(stringBuilder.ToString());
        }

        private static FakeAccount GetFakeAccount()
        {
            var fakeAccount = new FakeAccount
            {
                UserName = "validUserName",
                Password = "validPassword"
            };

            var drafts = new MailFolder { Name = "Drafts" };
            drafts.Attributes.Add("\\Drafts");
            drafts.Attributes.Add("\\NoInferiors");
            fakeAccount.MailFolders.Add(drafts);

            var trash = new MailFolder { Name = "Trash" };
            trash.Attributes.Add("\\Trash");
            trash.Attributes.Add("\\HasNoChildren");
            fakeAccount.MailFolders.Add(trash);

            var sent = new MailFolder { Name = "Sent" };
            sent.Attributes.Add("\\Sent");
            sent.Attributes.Add("\\NoInferiors");
            fakeAccount.MailFolders.Add(sent);

            var inboxSubsubfolder = new MailFolder { Name = "Subsubfolder" };
            inboxSubsubfolder.Attributes.Add("\\NoInferiors");

            var inboxSubfolder = new MailFolder { Name = "Subfolder" };
            inboxSubfolder.Attributes.Add("\\HasChildren");
            inboxSubfolder.MailFolders.Add(inboxSubsubfolder);

            var inbox = new MailFolder { Name = "INBOX" };
            inbox.Attributes.Add("\\HasChildren");
            inbox.MailFolders.Add(inboxSubfolder);
            fakeAccount.MailFolders.Add(inbox);

            var outbox = new MailFolder { Name = "OUTBOX" };
            outbox.Attributes.Add("\\NoInferiors");
            fakeAccount.MailFolders.Add(outbox);

            var junk = new MailFolder { Name = "Junk" };
            junk.Attributes.Add("\\Junk");
            junk.Attributes.Add("\\NoInferiors");
            fakeAccount.MailFolders.Add(junk);

            return fakeAccount;
        }
    }
}