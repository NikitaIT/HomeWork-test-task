using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNet.Identity;
using NHibernate;
using WebApplication1.Models.Identity;
using System.Configuration;
using NHibernate.Tool.hbm2ddl;

namespace WebApplication1.Models
{
    public class DatabaseContext
    {
        private readonly ISessionFactory sessionFactory;

        public DatabaseContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DatabaseContext>())
                .ExposeConfiguration(cfg => new SchemaUpdate(cfg).Execute(false, true))
                .BuildSessionFactory();
        }
        public ISession MakeSession()
        {
            return sessionFactory.OpenSession();
        }

        public IUserStore<User, int> Users => new IdentityStore(MakeSession());
    }
}