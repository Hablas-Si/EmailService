namespace EmailService.Models
{

    public class Email
    {
        public Guid _id {  get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(2);

        public Email(string to, string subject, string body, DateTime createddate)
        {
             _id = Guid.NewGuid();
            To = to;
            Subject = subject;
            Body = body;
            CreatedDate = createddate;
        }
        public Email() { }
    }
}