using Microsoft.AspNetCore.Mvc;
using EmailService;
using EmailService.Services;


namespace EmailService.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    private readonly EmailSender _emailSender;

    // The EmailSender is injected here by the ASP.NET Core dependency injection system.
    public EmailController(EmailSender emailSender)
    {
        _emailSender = emailSender; // The injected EmailSender is assigned to a private field.
    }
    /*
    [HttpGet("test")]
    public IActionResult GetTestEmail()
    {
        var dummyEmail = new Email
        {
            To = "test@example.com",
            Subject = "Test Email",
            Body = "This is a test email."
        };
        return Ok(dummyEmail);
    }
    */
    [HttpPost("send")]
    public IActionResult SendEmail([FromBody] Email email)
    {
        _emailSender.SendEmail(email.To, email.Subject, email.Body);
        // For now, just return a confirmation
        return Ok($"Email to {email.To} with subject '{email.Subject}' has been 'sent'.");
    }
}
