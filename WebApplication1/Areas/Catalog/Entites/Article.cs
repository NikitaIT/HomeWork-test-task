using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Areas.Catalog.Entites
{
    public class Article
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Author { get; set; }
        [DataType(DataType.MultilineText)]
        public virtual string Content { get; set; }
        public virtual Category Category { get; set; }
    }
}