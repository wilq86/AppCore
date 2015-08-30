using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.NHibernate
{
    public abstract class AbstractEntity
    {
        public virtual long Id { get; set; }

        public static bool EqualsByIdOrNull(AbstractEntity a, AbstractEntity b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null && b != null)
            {
                return false;
            }
            else if (a != null && b == null)
            {
                return false;
            }
            else
            {
                return a.Id == b.Id;
            }
        }
    }
}
