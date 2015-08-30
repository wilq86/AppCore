using AppCore.NHibernate;
using System;

namespace TemplateAplication
{
    public class LogMessage : AbstractEntity
    {
        public virtual DateTime Timestamp { get; set; }
        public virtual string Message { get; set; }

        public LogMessage()
        {
            Timestamp = DateTime.Now;
        }
    }
}