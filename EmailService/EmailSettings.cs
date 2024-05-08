namespace EmailService
{
    public class EmailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }

        public EmailSettings(string mailServer, int mailPort, string sender, string password)
        {
            MailServer = mailServer;
            MailPort = mailPort;
            Sender = sender;
            Password = password;
        }
    }
}
