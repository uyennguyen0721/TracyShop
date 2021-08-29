using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TracyShop.ViewModels;

namespace TracyShop.Services
{
    public interface IEmailService
    {
        Task SendTestEmail(UserEmailOptions userEmailOptions);

        Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions);

        Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions);
    }
}
