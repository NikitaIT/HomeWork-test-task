using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using NHibernate;
using NHibernate.Linq;
using WebApplication1.Areas.Catalog.Entites;
using WebApplication1.Areas.Catalog.Mappings;

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
        private void SetCurrentIDTo1()
        {
           /* --Set current ID to "1"
              -- If table already contains data, use "0"
              -- If table is empty and never insert data, use "1"
              -- Use SP https://github.com/reduardo7/TableTruncate
           */
            var sql = $"DBCC CHECKIDENT ([Category], RESEED,1)";
            var query = session.CreateSQLQuery(sql);
            query.ExecuteUpdate();
        }
        public void Add(Category item, int parantCategoryId)
        {
            if (parantCategoryId == 0)
            {
                if (Get(1) != null)
                {
                    return;
                }
                SetCurrentIDTo1();
            }
            using (ITransaction transaction = session.BeginTransaction())
            {
                var parantCategory = Get(parantCategoryId);
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
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(item);
                transaction.Commit();
            }
        }

        public void Delete(int id, Category category)
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
            category.ChildCategories.Clear();
            Delete(category);
        }
    }
}