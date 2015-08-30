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
    public class SmsCrud2 : Crud
    {
        public SmsCrud2(ApplicationKernel kernel)
            : base(kernel)
        {
            
        }

        public List<Sms2> GetAll()
        {
            return GetAll<Sms2>();
        }
    }
}
