using MCV_Mini_Project.ViewModels.Events;
using MCV_Mini_Project.ViewModels.Icons;
using MCV_Mini_Project.ViewModels.News;
using MCV_Mini_Project.ViewModels.Slider;
using MCV_Mini_Project.ViewModels.Videos;

namespace MCV_Mini_Project.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<IconUIVM> Icons { get; set; }
        public IEnumerable<SliderUIVM> Sliders { get; set; }
        public IEnumerable<EventUIVM> Events { get; set; }
        public IEnumerable<NewsUIVM> News { get; set; } 
        public Dictionary<string, string> Settings { get; set; }
        public VideoUIVM Video { get; set; }
    }
}
