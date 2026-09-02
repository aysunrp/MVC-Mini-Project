using MCV_Mini_Project.ViewModels.Events;
using MCV_Mini_Project.ViewModels.News;

namespace MCV_Mini_Project.ViewModels.NewsAndEvents
{
    public class EventAndNewsVM
    {
        public IEnumerable<EventUIVM> Events { get; set; }
        public IEnumerable<NewsUIVM> News { get; set; }
    }
}
