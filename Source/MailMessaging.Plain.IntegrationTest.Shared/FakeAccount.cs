using System.Collections.Generic;
using MailMessaging.Plain.IntegrationTest.Contracts;

namespace MailMessaging.Plain.IntegrationTest
{
    public class FakeAccount : IFakeAccount
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<MailFolder> MailFolders { get; set; }

        public FakeAccount()
        {
            MailFolders = new List<MailFolder>();
        }
    }
}