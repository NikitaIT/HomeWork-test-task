using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using WebApplication1.Areas.Catalog.Entites;

namespace WebApplication1.Areas.Catalog.Utils
{
    public class NHibernateHelper
    {
        public static ISession OpenSession()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            ISessionFactory sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                  .ConnectionString(connectionString)
                              .ShowSql()
                )
               .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<Category>())
               .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<Article>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}