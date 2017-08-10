using System;
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
    public class DatabaseContext
    {
        private static ISessionFactory _sessionFactory;
        private static readonly object syncRoot = new object();

        public ISession MakeSession()
        {
                lock (syncRoot)
                {
                    if (_sessionFactory == null)
                    {
                        var connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
                        _sessionFactory = Fluently.Configure()
                            .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                            .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DatabaseContext>())
                            .Mappings(m =>
                                      m.FluentMappings
                                          .AddFromAssemblyOf<Category>())
                           .Mappings(m =>
                                      m.FluentMappings
                                          .AddFromAssemblyOf<Article>())
                            .ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))
                            .BuildSessionFactory();
                    //new SchemaUpdate(cfg).Execute(false, true)
                    }       
                }
            return _sessionFactory.OpenSession();
        }

        public IUserStore<User, int> Users => new IdentityStore(MakeSession());
        public CategoryRepository Categores => new CategoryRepository(MakeSession());
        public ArticleRepository Articles => new ArticleRepository(MakeSession());

    }
}