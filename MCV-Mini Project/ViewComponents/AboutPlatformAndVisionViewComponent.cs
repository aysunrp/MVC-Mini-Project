using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.AboutPlatformandVision;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.ViewComponents
{
    public class AboutPlatformAndVisionViewComponent :ViewComponent
    {
        private readonly IPlatformService _aboutPlatformService;
        private readonly IVisionService _visionService;
        public AboutPlatformAndVisionViewComponent(IPlatformService aboutPlatformService, IVisionService visionService)
        {
            _aboutPlatformService = aboutPlatformService;
            _visionService = visionService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var platform = await _aboutPlatformService.GetAllUIAsync();
            var vision = await _visionService.GetAllUIAsync();

            return View(new AboutPlatformandVisionVM
            {
                Visions = vision,
                Platforms = platform
            });
        }
    }
}
