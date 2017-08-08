using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using NHibernate;
using NHibernate.Linq;
using WebApplication1.Areas.Catalog.Entites;

namespace WebApplication1.Areas.Catalog.Repositores
{
    public class CategoryRepository : IRepository<Category>
    {
        protected readonly ISession session;

        public CategoryRepository(ISession session)
        {
            this.session = session;
        }

        public void Dispose()
        {
            session.Dispose();
        }

        public List<Category> GetList()
        {
            return session.Query<Category>().ToList();
        }

        public Category Get(int id)
        {
            return session.Get<Category>(id);
        }

        public void Create(Category item)
        {
            session.Save(item);
        }
        public void Add(Category item, int parantCategoryId)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var parantCategory = Get(parantCategoryId);
                if (parantCategory == null)
                {
                    throw new NullReferenceException();
                }
                item.ParentCategory = parantCategory;
                Create(item);
                transaction.Commit();
            }
        }

        public void Update(Category item)
        {
            session.SaveOrUpdate(item);
        }

        public void Update(int id, Category item)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var categorytoUpdate = Get(id);
                categorytoUpdate.Name = item.Name;
                categorytoUpdate.IsActive = item.IsActive;
                Update(categorytoUpdate);
                transaction.Commit();
            }
        }

        public void Delete(Category item)
        {
            session.Delete(item);
        }
        public void Delete(int id, Category category)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                //TODO: разобраться почему session.Delete требует value соответствие
                if (string.IsNullOrEmpty(category.Name))
                {
                    category = Get(id);
                }
                foreach (var cat in category.ChildCategories)
                {
                    Delete(cat.Id, cat);
                }
                if (!category.ChildCategories.IsEmpty())
                {
                    category = Get(id);
                }
                Delete(category);
                transaction.Commit();
            }
        }
    }
}