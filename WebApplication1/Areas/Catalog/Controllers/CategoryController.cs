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
    public class CategoryController : Controller
    {
        private ISession session = NHibernateHelper.OpenSession();

        public CategoryController()
        {
            //TODO: Вынести в отдельный класс
            Mapper.Initialize(cfg => cfg.CreateMap<Category, CategoryTreeViewModel>()
                .ForMember(x => x.NestingLevel, o => o.Ignore())
                .ForMember(x => x.IsExpanded, o => o.Ignore())
                .ForMember(x => x.IsSelected, o => o.Ignore())
                .ForMember(x => x.ParentCategory, o => o.Ignore())
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
        // GET: Catalog/Article/5
        public ActionResult GetListInCategory(int categoryId)
        {
            var articles = session.Get<Category>(categoryId).Articles ?? new List<Article>();
            ViewBag.CategoryId = categoryId;
            return View(articles);
        }
        public ActionResult Tree()
        {
            var rootCategory = session.Get<Category>(1);
            var categoryTree = Mapper.Map<Category, CategoryTreeViewModel>(rootCategory);
            return View(categoryTree);
        }

        public ActionResult List(int? Category)
        {
                var employees = session.Query<Category>().ToList();
                return View(employees);
        }
        /*// GET: Catalog/Category
        public ActionResult Index()
        {
            return PartialView();
        }*/

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
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var parantCategory = session.Get<Category>(parantCategoryId);
                    if (parantCategory == null)
                    {
                        throw new NullReferenceException();
                    }
                    category.ParentCategory = parantCategory;
                    session.Save(category);
                    transaction.Commit();
                }
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
            var category = session.Get<Category>(id);
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
                var categorytoUpdate = session.Get<Category>(id);

                categorytoUpdate.Name = category.Name;
                categorytoUpdate.IsActive = category.IsActive;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(categorytoUpdate);
                    transaction.Commit();
                }
     
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
            var category = session.Get<Category>(id);
            return PartialView(category);
        }

        // POST: Catalog/Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Category category)
        {
            if (id==1) return RedirectToAction("List", "Article");

            try
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    //TODO: разобраться почему session.Delete требует value соответствие
                    if (string.IsNullOrEmpty(category.Name))
                    {
                        category = session.Get<Category>(id);
                    }
                    foreach (var cat in category.ChildCategories)
                    {
                        Delete(cat.Id, cat);
                    }
                    if (!category.ChildCategories.IsEmpty())
                    {
                        category = session.Get<Category>(id);
                    }
                    session.Delete(category);
                    transaction.Commit();
                }
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
                session.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
