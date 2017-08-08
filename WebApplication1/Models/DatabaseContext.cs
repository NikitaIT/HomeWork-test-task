using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNet.Identity;
using NHibernate;
using WebApplication1.Models.Identity;
using System.Configuration;
using NHibernate.Tool.hbm2ddl;
using WebApplication1.Areas.Catalog.Entites;
using WebApplication1.Areas.Catalog.Repositores;

namespace WebApplication1.Models
{
    interface IDatabaseContext
    {
        IUserStore<User, int> Users { get; }
        CategoryRepository Categores { get; }
        ArticleRepository Articles { get; }
    }
    public class DatabaseContext : IDatabaseContext
    {
        private readonly ISessionFactory sessionFactory;

        public DatabaseContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DatabaseContext>())
                .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<Category>())
               .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<Article>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
        }
        public ISession MakeSession()
        {
            return sessionFactory.OpenSession();
        }

        public IUserStore<User, int> Users => new IdentityStore(MakeSession());
        public CategoryRepository Categores => new CategoryRepository(MakeSession());
        public ArticleRepository Articles => new ArticleRepository(MakeSession());
    }
}