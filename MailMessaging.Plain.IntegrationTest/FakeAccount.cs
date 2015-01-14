namespace MailMessaging.Plain.IntegrationTest
{
    public class FakeAccount : IFakeAccount
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}