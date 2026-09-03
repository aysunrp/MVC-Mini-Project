namespace MCV_Mini_Project.Areas.Admin.ViewModels.CourseInfo
{
    public class CourseInfoListVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public int SalesCount { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsNew { get; set; }
        public string TeacherName { get; set; }
        public string? Image { get; set; }
    }
}
