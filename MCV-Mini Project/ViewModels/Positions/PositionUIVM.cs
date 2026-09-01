using MCV_Mini_Project.Models;

namespace MCV_Mini_Project.ViewModels.Positions
{
    public class PositionUIVM
    {
        public string Name { get; set; }
        public ICollection<Teacher> Teachers { get; set; }
    }
}
