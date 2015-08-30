using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateAplication.Enitits;

namespace TemplateAplication.Maps
{
    public class SmsMap2 : ClassMap<Sms2>
    {
        public SmsMap2()
        {
            Table("sms2");
            DynamicUpdate();
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Increment().Column("id");
            Map(x => x.Timestamp).Column("timestamp");
            Map(x => x.Message).Column("message").Length(140);
            Map(x => x.Sent).Column("sent");
        }
    }
}
