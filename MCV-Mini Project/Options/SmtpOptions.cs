namespace MCV_Mini_Project.Options
{
    public class SmtpOptions
    {
        public const string SectionName = "Smtp";

        public string Host { get; set; } = string.Empty;

        public int Port { get; set; } = 587;

        public string User { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string From { get; set; } = string.Empty;

        public bool EnableSsl { get; set; } = true;

        public string ConfirmationTo { get; set; } = string.Empty;

        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(Host) &&
            !string.IsNullOrWhiteSpace(User) &&
            !string.IsNullOrWhiteSpace(Password);
    }
}
