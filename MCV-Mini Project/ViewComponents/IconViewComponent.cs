using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.ViewComponents
{
    public class IconViewComponent : ViewComponent
    {
        private readonly IIconService _iconService;

        public IconViewComponent(IIconService iconService)
        {
            _iconService = iconService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var icons = await _iconService.GetIconUIVMAsync();

            return View(icons);
        }
    }

}
