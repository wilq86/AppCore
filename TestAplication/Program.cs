using AppCore;
using AppCore.BasicConfiguration;
using AppCore.DataBaseLoger;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TemplateAplication.Enitits;

namespace TemplateAplication
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationKernel kernel = new ApplicationKernel(new TestModule());

            {
                SmsCrud crud = new SmsCrud(kernel);

                crud.Add(new Sms());
                List<Sms> a = crud.GetAll<Sms>();
            }
            {
                SmsCrud crud = new SmsCrud(kernel);

                crud.Add(new Sms());
                List<Sms> a = crud.GetAll<Sms>();
            }

            {
                SmsCrud2 crud = new SmsCrud2(kernel);

                crud.Add(new Sms());
                List<Sms> a = crud.GetAll<Sms>();
            }

            //DbLogger<LogMessage> logger = new DbLogger<LogMessage>(new SqliteMemoryDao(Assembly.GetExecutingAssembly()));

            //logger.Write(new LogMessage() { Message = "Test" });

            //Thread.Sleep(TimeSpan.FromSeconds(60));
        }
    }
}
