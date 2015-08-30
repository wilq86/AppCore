using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.NHibernate
{
    public interface IDbConfiguration : IDisposable
    {
        ISessionFactory GetSessionFactory();
    }
}
