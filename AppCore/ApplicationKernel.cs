using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore
{
    public class ApplicationKernel
    {
        public IKernel Kernel { get; private set; }

        public ApplicationKernel(NinjectModule module)
        {
            Kernel = new StandardKernel(module);
        }
    }
}
