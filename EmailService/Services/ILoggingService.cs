using EmailService.Models;
using System.Threading.Tasks;

namespace EmailService.Services
{
    public interface ILoggingService
    {
        Task LogEmailSent(Email email);
    }
}
