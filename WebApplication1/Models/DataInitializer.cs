using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Areas.Catalog.Entites;

namespace WebApplication1.Models
{
    public class DataInitializer
    {
        private static bool isSet;

        public DataInitializer()
        {
            if (!isSet)
            {
                Seed();
                isSet = true;
            }
        }
        private void Seed()
        {
            
            //подключение к базе
            var dbArticles = new DatabaseContext().Articles;
            var dbCategores = new DatabaseContext().Categores;

            //создание корневой
            dbCategores.Add(new Category() { Name = "Root", Id = 1}, 0);

            for (int f = 2; f < 6; f++)
            {
                dbCategores.Add(new Category()
                {
                    Articles = new List<Article>(),
                    ChildCategories = new List<Category>(),
                    Name = $"Cat1. {f}"
                }, 1);
                dbArticles.Create(new Article()
                {
                    Author = $"Author {f}",
                    Content = new string('с', 52),
                    Title = $"Title {f}"
                }, f);
                for (int id = 0; id < 5; id++)
                {
                    dbCategores.Add(new Category()
                    {
                        Articles = new List<Article>(),
                        ChildCategories = new List<Category>(),
                        Name = $"Cat2. {f}-{id}",
                    }, f);
                    dbArticles.Create(new Article()
                    {
                        Author = $"Author {f}-{id}",
                        Content = new string('с', 52),
                        Title = $"Title {f}-{id}"
                    }, f);
                }
            }
        }
    }
}