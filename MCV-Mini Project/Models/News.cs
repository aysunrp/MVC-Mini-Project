namespace MCV_Mini_Project.Models
{
    public class News : BaseEntity
    {
        public string Image { get; set; }
        public string Question { get; set; }
        public string Date { get; set; }
        public int AuthorId { get; set; }
        public Author Authors { get; set; }
    }
}
