using System.Threading.Tasks;

namespace WUCSA.Core.Interfaces
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html);
        Task SendAsync(string to, string subject, string html);
    }
}
