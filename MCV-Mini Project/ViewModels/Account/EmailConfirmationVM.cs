namespace MCV_Mini_Project.ViewModels.Account
{
    public class EmailConfirmationVM
    {
        public string? Email { get; set; }

        public string? ConfirmLink { get; set; }

        public bool EmailSent { get; set; }

        public bool Succeeded { get; set; }

        public bool Failed { get; set; }

        public string? Message { get; set; }
    }
}
