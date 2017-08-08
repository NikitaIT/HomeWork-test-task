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
using WebApplication1.Areas.Catalog.Repositores;
using WebApplication1.Models;

namespace WebApplication1.Areas.Catalog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ArticleRepository dbArticles = new DatabaseContext().Articles;

        // GET: Catalog/Article
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            var articles = dbArticles.GetList();
            return View(articles);
        }

        // GET: Catalog/Article/Details/5
        public ActionResult Details(int id)
        {
            var article = dbArticles.Get(id);
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
                dbArticles.Create(article, categoryId);
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
            var article = dbArticles.Get(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return PartialView(article);
        }

        // POST: Catalog/Article/Edit/5
        [HttpPost]
        public ActionResult Edit(int categoryId, Article article)
        {
            try
            {
                dbArticles.Update(categoryId, article);
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
            var article = dbArticles.Get(id);
            return PartialView(article);
        }

        // POST: Catalog/Article/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Article article)
        {
            try
            {
                dbArticles.Delete(id);
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
                dbArticles.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
