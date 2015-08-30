using AppCore.BasicConfiguration;
using AppCore.NHibernate;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TemplateAplication
{
    public class TestModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbConfiguration>().To<SqliteMemoryDao>().WithConstructorArgument(Assembly.GetExecutingAssembly());
        }
    }
}