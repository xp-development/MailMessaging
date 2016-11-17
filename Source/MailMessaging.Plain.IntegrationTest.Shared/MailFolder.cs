using System.Collections.Generic;

namespace MailMessaging.Plain.IntegrationTest
{
    public class MailFolder
    {
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<MailFolder> MailFolders { get; set; }

        public MailFolder()
        {
            Attributes = new List<string>();
            MailFolders = new List<MailFolder>();
        }
    }
}