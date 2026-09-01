using MCV_Mini_Project.Models;

namespace MCV_Mini_Project.ViewModels.Teachers
{
    public class TeacherUIVM
    {
        public string FullName { get; set; }
        public string Image { get; set; }
        public Position Position { get; set; }
        public int PositionId { get; set; }
        public ICollection<CourseInfo> CourseInfos { get; set; }
    }
}
