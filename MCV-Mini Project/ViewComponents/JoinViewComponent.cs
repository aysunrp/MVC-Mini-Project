using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using MVC_MiniProject.ViewModels.Setting;

namespace MVC_MiniProject.ViewComponents
{
    public class JoinViewComponent : ViewComponent
    {
        private readonly ISettingService _settingService;
        public JoinViewComponent(ISettingService settingService)
        {
            _settingService = settingService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settings = await _settingService.GetAllUIAsync();
            return View(new SettingUIVM
            {
                Settings = settings
            });
        }
    }
}