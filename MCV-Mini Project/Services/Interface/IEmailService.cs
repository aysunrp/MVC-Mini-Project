namespace MCV_Mini_Project.Services.Interface
{
    public interface IEmailService
    {
        bool IsConfigured { get; }

        Task SendAsync(string to, string subject, string htmlBody);

        Task SendEmailConfirmationAsync(string to, string confirmationLink);
    }
}
