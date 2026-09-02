using MCV_Mini_Project.Data;
using MCV_Mini_Project.Services.Interface;
using MCV_Mini_Project.ViewModels.NewsAndEvents;
using Microsoft.AspNetCore.Mvc;

namespace MCV_Mini_Project.ViewComponents
{
    public class NewsAndEventsViewComponent :ViewComponent
    {
        private readonly IEventService _eventService;
        private readonly INewsService _newsService;
        public NewsAndEventsViewComponent(IEventService eventService, INewsService newsService)
        {
            _eventService = eventService;
            _newsService = newsService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var news = await _newsService.GetNewsUIVMAsync();
            var evet = await _eventService.GetEventUIVMAsync();
            return View(new EventAndNewsVM
            {
                News = news,
                Events = evet
            });
        }
    }
}
