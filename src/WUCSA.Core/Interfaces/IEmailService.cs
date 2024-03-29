﻿using System.Threading.Tasks;

namespace WUCSA.Core.Interfaces
{
    public interface IEmailService
    {
        Task Send(string to, string subject, string html);
        Task SendAsync(string to, string subject, string html);
        Task SendTGAsync(string msg);
        Task SendToAllTGAsync(string msg);
    }
}
