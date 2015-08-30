using AppCore;
using AppCore.NHibernate;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateAplication.Enitits;

namespace TemplateAplication
{
    public class SmsCrud : Crud
    {
        public SmsCrud(ApplicationKernel kernel)
            :base(kernel)
        {
            
        }

        public List<Sms> GetAll()
        {
            return GetAll<Sms>();
        }
    }
}
