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
using System;

namespace AppCore.BasicConfiguration
{
    public class SessionFactoryWrapper : ISessionFactory
    {
        public ISessionFactory SessionFactory { get; private set; }
        public Configuration Configuration { get; private set; }

        public SessionFactoryWrapper(ISessionFactory factory, Configuration configuration)
        {
            SessionFactory = factory;
            Configuration = configuration;
        }

        public ICollection<string> DefinedFilterNames
        {
            get
            {
                return SessionFactory.DefinedFilterNames;
            }
        }

        public bool IsClosed
        {
            get
            {
                return SessionFactory.IsClosed;
            }
        }

        public IStatistics Statistics
        {
            get
            {
                return SessionFactory.Statistics;
            }
        }

        public void Close()
        {
            SessionFactory.Close();
        }

        public void Dispose()
        {
            SessionFactory.Dispose();
        }

        public void Evict(Type persistentClass)
        {
            SessionFactory.Evict(persistentClass);
        }

        public void Evict(Type persistentClass, object id)
        {
            SessionFactory.Evict(persistentClass, id);
        }

        public void EvictCollection(string roleName)
        {
            SessionFactory.EvictCollection(roleName);
        }

        public void EvictCollection(string roleName, object id)
        {
            SessionFactory.EvictCollection(roleName, id);
        }

        public void EvictEntity(string entityName)
        {
            SessionFactory.EvictEntity(entityName);
        }

        public void EvictEntity(string entityName, object id)
        {
            SessionFactory.EvictEntity(entityName, id);
        }

        public void EvictQueries()
        {
            SessionFactory.EvictQueries();
        }

        public void EvictQueries(string cacheRegion)
        {
            SessionFactory.EvictQueries(cacheRegion);
        }

        public IDictionary<string, IClassMetadata> GetAllClassMetadata()
        {
            return SessionFactory.GetAllClassMetadata();
        }

        public IDictionary<string, ICollectionMetadata> GetAllCollectionMetadata()
        {
            return SessionFactory.GetAllCollectionMetadata();
        }

        public IClassMetadata GetClassMetadata(string entityName)
        {
            return SessionFactory.GetClassMetadata(entityName);
        }

        public IClassMetadata GetClassMetadata(Type persistentClass)
        {
            return SessionFactory.GetClassMetadata(persistentClass);
        }

        public ICollectionMetadata GetCollectionMetadata(string roleName)
        {
            return SessionFactory.GetCollectionMetadata(roleName);
        }

        public ISession GetCurrentSession()
        {
            return SessionFactory.GetCurrentSession();
        }

        public FilterDefinition GetFilterDefinition(string filterName)
        {
            return SessionFactory.GetFilterDefinition(filterName);
        }

        public ISession OpenSession()
        {
            ISession session = SessionFactory.OpenSession();

            var export = new SchemaExport(Configuration);
            export.Execute(true, true, false, session.Connection, null);

            return session;
        }

        public ISession OpenSession(IInterceptor sessionLocalInterceptor)
        {
            ISession session = SessionFactory.OpenSession(sessionLocalInterceptor);

            var export = new SchemaExport(Configuration);
            export.Execute(true, true, false, session.Connection, null);

            return session;
        }

        public ISession OpenSession(IDbConnection conn)
        {
            ISession session = SessionFactory.OpenSession(conn);

            var export = new SchemaExport(Configuration);
            export.Execute(true, true, false, session.Connection, null);

            return session;
        }

        public ISession OpenSession(IDbConnection conn, IInterceptor sessionLocalInterceptor)
        {
            ISession session = SessionFactory.OpenSession(conn, sessionLocalInterceptor);

            var export = new SchemaExport(Configuration);
            export.Execute(true, true, false, session.Connection, null);

            return session;
        }

        public IStatelessSession OpenStatelessSession()
        {
            IStatelessSession session = SessionFactory.OpenStatelessSession();

            var export = new SchemaExport(Configuration);
            export.Execute(true, true, false, session.Connection, null);

            return session;
        }

        public IStatelessSession OpenStatelessSession(IDbConnection connection)
        {
            IStatelessSession session = SessionFactory.OpenStatelessSession(connection);

            var export = new SchemaExport(Configuration);
            export.Execute(true, true, false, session.Connection, null);

            return session;
        }
    }
}

