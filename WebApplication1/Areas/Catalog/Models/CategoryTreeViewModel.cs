using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Conventions;

namespace WebApplication1.Areas.Catalog.Models
{
    public class CategoryTreeViewModel
    {
        //TODO: TreeView основанное на множествах
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryTreeViewModel ParentCategory { get; set; }
        public List<CategoryTreeViewModel> ChildCategories { get; set; }

        public CategoryTreeViewModel()
        {
            ChildCategories = new List<CategoryTreeViewModel>();
        }
        public int NestingLevel
        {
            get
            {
                if (this.ParentCategory == null)
                {
                    return 0;
                }
                return (this.ParentCategory.NestingLevel + 1);
            }
        }

        public bool IsExpanded { get; set; }
        public bool IsActive { get; set; }
        public bool IsSelected { get; set; }
        public bool HasChildCategories
        {
            get { return (!this.ChildCategories.IsEmpty()); }
        }
    }
}