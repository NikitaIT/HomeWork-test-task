using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using FluentNHibernate.Conventions;
using NHibernate;
using NHibernate.Linq;
using WebApplication1.Areas.Catalog.Entites;
using WebApplication1.Areas.Catalog.Models;
using WebApplication1.Areas.Catalog.Utils;

namespace WebApplication1.Areas.Catalog.Controllers
{
    public class ArticleController : Controller
    {
        private ISession session = NHibernateHelper.OpenSession();

        public ArticleController()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Category, CategoryTreeViewModel>()
                .ForMember(x => x.NestingLevel, o => o.Ignore())
                .ForMember(x => x.IsExpanded, o => o.Ignore())
                .ForMember(x => x.IsSelected, o => o.Ignore())
                .ForMember(x => x.HasChildCategories, o => o.Ignore())
                .AfterMap((category, categoryModel) =>
                {
                    // Mapper doesn't know about categories relationship, so we need assign them explicitly.
                    // Without it setting IsExpanded = true will not have effect on childCategory.ParentCategory
                    // as this is another object
                    if (categoryModel == null || categoryModel.ChildCategories.IsEmpty())
                    {
                        return;
                    }
                    foreach (var childCategory in categoryModel.ChildCategories)
                    {
                        childCategory.ParentCategory = categoryModel;
                    }
                }));
        }

        // GET: Catalog/Article
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var articles = session.Query<Article>().ToList();
            if (articles.IsEmpty())
            {
                articles = new List<Article>();
            }
            return View(articles);
        }

        // GET: Catalog/Article/Details/5
        public ActionResult Details(int id)
        {
            var article = session.Get<Article>(id);
            return PartialView(article);
        }

        // GET: Catalog/Article/Create
        public ActionResult Create(int categoryId)
        {
            ViewBag.CategoryId = categoryId;
            return PartialView();
        }

        // POST: Catalog/Article/Create
        [HttpPost]
        public ActionResult Create(Article article, int categoryId)
        {
            try
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var category = session.Get<Category>(categoryId);
                    if (category == null)
                    {
                        throw new NullReferenceException();
                    }
                    article.Category = category;
                    session.Save(article);
                    transaction.Commit();
                }
                return RedirectToAction("List");
            }
            catch
            {
                return PartialView();
            }
        }

        // GET: Catalog/Article/Edit/5
        public ActionResult Edit(int id)
        {
            var article = session.Get<Article>(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return PartialView(article);
        }

        // POST: Catalog/Article/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Article article)
        {
            try
            {
                var articletoUpdate = session.Get<Article>(id);

                articletoUpdate.Title = article.Title;
                articletoUpdate.Author = article.Author;
                articletoUpdate.Content = article.Content;
                articletoUpdate.Category = article.Category;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(articletoUpdate);
                    transaction.Commit();
                }
                return RedirectToAction("List");
            }
            catch
            {
                return PartialView();
            }
        }

        // GET: Catalog/Article/Delete/5
        public ActionResult Delete(int id)
        {
            var article = session.Get<Article>(id);
            return PartialView(article);
        }

        // POST: Catalog/Article/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Article article)
        {
            try
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    //TODO: разобраться почему session.Delete требует value соответствие
                    var article1 = session.Get<Article>(id);
                    session.Delete(article1);
                    transaction.Commit();
                }

                return RedirectToAction("List");
            }
            catch
            {
                return PartialView();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                session.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
