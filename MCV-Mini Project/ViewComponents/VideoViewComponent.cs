using MCV_Mini_Project.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.ViewComponents
{
    public class VideoViewComponent : ViewComponent
    {
        private readonly IVideoService _videoService;

        public VideoViewComponent(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var video = await _videoService.GettAllUIAsync();

            return View(video);
        }
    }
}
