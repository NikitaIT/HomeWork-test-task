using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using WebApplication1.Areas.Catalog.Entites;

namespace WebApplication1.Areas.Catalog.Repositores
{
    public class ArticleRepository : IRepository<Article>
    {
        protected readonly ISession session;

        public ArticleRepository(ISession session)
        {
            this.session = session;
        }
        public void Create(Article item)
        {
            session.Save(item);
        }

        public void Delete(Article item)
        {
            session.Delete(item);
        }

        public void Dispose()
        {
            session.Dispose();
        }

        public Article Get(int id)
        {
            return session.Get<Article>(id);
        }

        public List<Article> GetList()
        {
            return session.Query<Article>().ToList();
        }

        public void Update(Article item)
        {
            session.SaveOrUpdate(item);
        }

        public void Create(Article article, int categoryId)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var category = session.Get<Category>(categoryId);
                if (category == null)
                {
                    throw new NullReferenceException();
                }
                article.Category = category;
                Create(article);
                transaction.Commit();
            }
        }

        public void Update(int categoryId, Article article)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                var articletoUpdate = Get(article.Id);

                articletoUpdate.Title = article.Title;
                articletoUpdate.Author = article.Author;
                articletoUpdate.Content = article.Content;
                articletoUpdate.Category = session.Get<Category>(categoryId);

                Update(articletoUpdate);
                transaction.Commit();
            }
        }

        public void Delete(int id)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                //TODO: разобраться почему session.Delete требует value соответствие
                var article1 = session.Get<Article>(id);
                session.Delete(article1);
                transaction.Commit();
            }
        }
    }
}