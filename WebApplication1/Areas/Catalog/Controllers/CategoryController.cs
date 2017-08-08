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
    public class CategoryController : Controller
    {
        private readonly CategoryRepository dbCategores = new DatabaseContext().Categores;

        // GET: Catalog/Article/5
        public ActionResult GetListInCategory(int categoryId)
        {
            var articles = dbCategores.Get(categoryId).Articles ?? new List<Article>();
            ViewBag.CategoryId = categoryId;
            return View(articles);
        }
        public ActionResult Tree()
        {
            var rootCategory = dbCategores.Get(1) ?? new Category
            {
                Name = "Временная",
                Id = 0
            };
            var categoryTree = Mapper.Map<Category, CategoryTreeViewModel>(rootCategory);
            return View(categoryTree);
        }

        public ActionResult List(int? Category)
        {
                var employees = dbCategores.GetList();
                return View(employees);
        }

        // GET: Catalog/Category/Create
        public ActionResult Create(int id)
        {
            ViewBag.ParantCategoryId = id;
            return PartialView();
        }

        // POST: Catalog/Category/Create
        [HttpPost]
        public ActionResult Create(Category category, int parantCategoryId)
        {
            try
            {
                dbCategores.Add(category, parantCategoryId);
                return RedirectToAction("List", "Article");
            }
            catch
            {
                return PartialView();
            }
        }

        // GET: Catalog/Category/Edit/5
        public ActionResult Edit(int id)
        {
            var category = dbCategores.Get(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return PartialView(category);
        }

        // POST: Catalog/Category/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                dbCategores.Update(id, category);
                return RedirectToAction("List", "Article");
            }
            catch
            {
                return View();
            }
        }

        // GET: Catalog/Category/Delete/5
        public ActionResult Delete(int id)
        {
            var category = dbCategores.Get(id);
            return PartialView(category);
        }

        // POST: Catalog/Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Category category)
        {
            if (id==1) return RedirectToAction("List", "Article");
            try
            {
                dbCategores.Delete(id,category);
                return RedirectToAction("List", "Article");
            }
            catch
            {
                return View();
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbCategores.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
