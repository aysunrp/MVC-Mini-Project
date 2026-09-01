namespace MCV_Mini_Project.Models
{
    public class CourseImage : BaseEntity
    {
        public string Name { get; set; }
        public bool IsMain { get; set; }
        public int CourseInfoId { get; set; }
    }
}
