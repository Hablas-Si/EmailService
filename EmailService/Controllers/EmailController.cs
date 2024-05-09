using Microsoft.AspNetCore.Mvc;
using EmailService.Services;
using EmailService.Models;
using DnsClient;

namespace EmailService.Controllers;

[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    private readonly EmailSender _emailSender;
    private readonly ILoggingService _loggingService;
    private readonly IConfiguration _config;


    // The EmailSender is injected here by the ASP.NET Core dependency injection system.
    public EmailController(EmailSender emailSender, ILoggingService loggingService, IConfiguration config)
    {
        _emailSender = emailSender; // The injected EmailSender is assigned to a private field.
        _loggingService = loggingService;
        _config = config;
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
        _loggingService.LogEmailSent(email);

        // For now, just return a confirmation
        return Ok($"Email to {email.To} with subject '{email.Subject}' has been 'sent' at {DateTime.UtcNow}.");
    }

    [HttpPost("log")]
    public async Task<IActionResult> PostLog([FromBody] Email email)
    {
        email.CreatedDate = DateTime.UtcNow.AddHours(2);
        await _loggingService.LogEmailSent(email);
        return Ok("Email is logged");
    }
}
