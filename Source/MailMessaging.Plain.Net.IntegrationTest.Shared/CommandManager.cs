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

            return BuildResponse(tag, "BAD unknown command");
        }

        private static string BuildResponse(string tag, string message)
        {
            return string.Format("{0} {1}\r\n", tag, message);
        }

        private readonly FakeAccount _fakeAccount;
    }
}