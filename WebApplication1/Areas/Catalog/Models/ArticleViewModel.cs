using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.Catalog.Entites;

namespace WebApplication1.Areas.Catalog.Models
{
    public class ArticleViewModel
    {
        public IEnumerable<Article> Articles { get; set; }
        public CategoryTreeViewModel Category { get; set; }
    }
}