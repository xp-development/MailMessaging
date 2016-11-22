using System;
using System.Text;

namespace MailMessaging.Plain.IntegrationTest
{
    public class CommandManager
    {
        public CommandManager(FakeAccount fakeAccount)
        {
            _fakeAccount = fakeAccount;
        }

        public string Process(string tag, string command, string commandArgumentString)
        {
            if (command.Equals("LOGIN"))
            {
                var username = GetNextArgument(ref commandArgumentString);
                var password = GetNextArgument(ref commandArgumentString);

                if (username == _fakeAccount.UserName && password == _fakeAccount.Password)
                    return BuildResponse(tag, "OK " + command + " completed");

                return BuildResponse(tag, "NO authentication failed");
            }

            if (command.Equals("LIST"))
            {
                var referenceName = GetNextArgument(ref commandArgumentString);
                var mailboxName = GetNextArgument(ref commandArgumentString);
                var path = referenceName + mailboxName;

                var folders = new StringBuilder();
                foreach (var mailFolder in _fakeAccount.MailFolders)
                    AddMailboxFolder(path, folders, mailFolder);

                return BuildResponse(tag, "OK " + command + " completed", folders.ToString());
            }

            return BuildResponse(tag, "BAD unknown command");
        }

        private static string GetNextArgument(ref string commandArgumentString)
        {
            if(string.IsNullOrWhiteSpace(commandArgumentString))
                return "";

            commandArgumentString = commandArgumentString.Trim();

            if (commandArgumentString.StartsWith("\""))
            {
                var indexOfNextQuote = commandArgumentString.IndexOf("\"", 1, StringComparison.Ordinal);
                var argument = commandArgumentString.Substring(1, indexOfNextQuote - 1);
                commandArgumentString = commandArgumentString.Substring(indexOfNextQuote + 1);
                return argument;
            }

            if(commandArgumentString.Contains(" "))
            {
                var indexOfNextQuote = commandArgumentString.IndexOf(" ", StringComparison.Ordinal);
                var argument = commandArgumentString.Substring(0, indexOfNextQuote);
                commandArgumentString = commandArgumentString.Substring(indexOfNextQuote + 1);
                return argument;
            }

            return commandArgumentString.TrimEnd();
        }

        private static void AddMailboxFolder(string path, StringBuilder folders, MailFolder mailFolder, string parentName = "")
        {
            var mailboxFolderName = $"{parentName}{mailFolder.Name}/";
            var mailboxFolderNameWithoutSlash = mailboxFolderName.TrimEnd('/');

            if (path == "/*" || path.TrimEnd('/') == mailboxFolderNameWithoutSlash || path.EndsWith("*") && mailboxFolderName.StartsWith(path.TrimEnd('*')))
            {
                folders.AppendFormat("* LIST ({0}) \"/\" {1}\r\n", string.Join(" ", mailFolder.Attributes), mailboxFolderNameWithoutSlash.Contains(" ") ? $"\"{mailboxFolderNameWithoutSlash}\"" : mailboxFolderNameWithoutSlash);
            }

            foreach (var folder in mailFolder.MailFolders)
            {
                AddMailboxFolder(path, folders, folder, mailboxFolderName);
            }
        }

        public static string BuildResponse(string tag, string message, string informations = "")
        {
            var response = $"{tag} {message}\r\n";
            return !string.IsNullOrEmpty(informations) ? $"{informations}\r\n{response}" : response;
        }

        private readonly FakeAccount _fakeAccount;
    }
}