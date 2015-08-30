using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using FluentNHibernate.Cfg;
using NHibernate.Cfg;
using System.Reflection;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg.Db;
using NHibernate.Engine;
using NHibernate.Metadata;
using NHibernate.Stat;
using System.Data;
using AppCore.NHibernate;
using AppCore.BasicConfiguration;

namespace AppCore.BasicConfiguration
{
    public class SqliteMemoryDao : IDbConfiguration
    {
        private Configuration Configuration { get; set; }
        private ISessionFactory sessionFactory = null;

        private Assembly EntityAssembly;

        public SqliteMemoryDao(Assembly entityAssembly)
        {
            EntityAssembly = entityAssembly;
        }

        public ISessionFactory GetSessionFactory()
        {
            //Zgodnie z dobrymi zwyczajami hibernate fabryka
            //jest jedna na całe dao - uwaga trzeba ją zwolnić
            if (sessionFactory == null)
            {
                //Mapowanie fluently może trwać
                sessionFactory = Fluently.Configure()
                    .Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
                    .Mappings(m => m.FluentMappings.AddFromAssembly(EntityAssembly))
                    .ExposeConfiguration(cfg => Configuration = cfg)
                    .BuildSessionFactory();
            }
            return new SessionFactoryWrapper(sessionFactory, Configuration);
        }

        public ISession OpenSession()
        {
            ISession session = sessionFactory.OpenSession();

            var export = new SchemaExport(Configuration);
            export.Execute(true, true, false, session.Connection, null);

            return session;
        }

        public void Config(Configuration configuration)
        {
            new SchemaUpdate(Configuration).Execute(true, true);
        }

        public void Dispose()
        {
            if (sessionFactory != null)
            {
                sessionFactory.Close();
                sessionFactory.Dispose();
                sessionFactory = null;
            }
            Configuration = null;
        }
    }
}
