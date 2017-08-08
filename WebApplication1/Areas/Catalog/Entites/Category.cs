using System.Collections.Generic;
using WebApplication1.Areas.Catalog.Utils;

namespace WebApplication1.Areas.Catalog.Entites
{
    public class Category : PersistentObject<int>
    {
        /*nhibernate requires all properties be virtual*/
        public virtual string Name { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual IList<Category> ChildCategories { get; set; }
        public virtual IList<Article> Articles { get; set; }
        public virtual bool IsActive { get; set; }
        public Category()
        {
            ChildCategories = new List<Category>();
            Articles = new List<Article>();
        }
    }
}