using AppCore.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateAplication.Enitits
{
    public class Sms : AbstractEntity
    {
        public virtual DateTime Timestamp { get; set; }
        public virtual string Message { get; set; }
        public virtual DateTime? Sent { get; set; }

        public Sms()
        {
            Timestamp = DateTime.Now;
        }
    }
}
