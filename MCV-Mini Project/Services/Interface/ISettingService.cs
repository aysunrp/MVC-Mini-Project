namespace MCV_Mini_Project.Services.Interface
{
    public interface ISettingService
    {
        Task<Dictionary<string, string>> GetAllUIAsync();
    }
}
