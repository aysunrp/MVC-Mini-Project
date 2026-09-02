using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.ViewComponents
{
    public class AboutPageHeroViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;

        public AboutPageHeroViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settings = await _settingService.GetAllUIAsync();

            return View(settings);
        }
    }
}