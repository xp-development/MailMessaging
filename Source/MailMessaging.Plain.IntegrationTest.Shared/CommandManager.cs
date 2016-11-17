using System.Text;

namespace MailMessaging.Plain.IntegrationTest
{
    public class CommandManager
    {
        public CommandManager(FakeAccount fakeAccount)
        {
            _fakeAccount = fakeAccount;
        }

        public string Process(string tag, string command, string commandArgs)
        {
            if (command.Equals("LOGIN"))
            {
                var username = commandArgs.Split(' ')[0];
                var password = commandArgs.Split(' ')[1].Trim();

                if (username == _fakeAccount.UserName && password == _fakeAccount.Password)
                    return BuildResponse(tag, "OK " + command + " completed");

                return BuildResponse(tag, "NO authentication failed");
            }

            if (command.Equals("LIST"))
            {
                var referenceName = RemoveQuotes(commandArgs.Split(' ')[0]);
                var mailboxName = RemoveQuotes(commandArgs.Split(' ')[1].Trim());
                var path = referenceName + mailboxName;

                var folders = new StringBuilder();
                foreach (var mailFolder in _fakeAccount.MailFolders)
                    AddMailboxFolder(path, folders, mailFolder);

                return BuildResponse(tag, "OK " + command + " completed", folders.ToString());
            }

            return BuildResponse(tag, "BAD unknown command");
        }

        private static string RemoveQuotes(string name)
        {
            if (name.StartsWith("\"") && name.EndsWith("\""))
                name = name.Substring(1, name.Length - 2);

            return name;
        }

        private static void AddMailboxFolder(string path, StringBuilder folders, MailFolder mailFolder, string parentName = "")
        {
            var mailboxFolderName = string.Format("{0}{1}/", parentName, mailFolder.Name);
            var mailboxFolderNameWithoutSlash = mailboxFolderName.TrimEnd('/');

            if (path == "/*" || path.TrimEnd('/') == mailboxFolderNameWithoutSlash || (path.EndsWith("*") && mailboxFolderName.StartsWith(path.TrimEnd('*'))))
            {
                folders.AppendFormat("* LIST ({0}) \"/\" {1}\r\n", string.Join(" ", mailFolder.Attributes), mailboxFolderNameWithoutSlash);
            }

            foreach (var folder in mailFolder.MailFolders)
            {
                AddMailboxFolder(path, folders, folder, mailboxFolderName);
            }
        }

        public static string BuildResponse(string tag, string message, string informations = "")
        {
            var response = string.Format("{0} {1}\r\n", tag, message);
            return !string.IsNullOrEmpty(informations) ? string.Format("{0}\r\n{1}", informations, response) : response;
        }

        private readonly FakeAccount _fakeAccount;
    }
}