using AppCore.NHibernate;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.DataBaseLoger
{
    public class DbLogger<Log> where Log : AbstractEntity
    {
        public Crud Crud { get; private set; }
        private ConcurrentQueue<Log> Que;
        private Thread Worker;

        public DbLogger(IDbConfiguration configuration)
        {
            Crud = new Crud(configuration);
            Que = new ConcurrentQueue<Log>();

            Worker = new Thread(Run);
            Worker.IsBackground = true;
            Worker.Start();
        }

        public void Write(Log log)
        {
            Que.Enqueue(log);
        }

        public void Run()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        if (Que.Count > 0)
                        {
                            Log log;
                            while (Que.TryDequeue(out log))
                            {
                                if (log != null)
                                {
                                    Crud.Update(log);
                                }
                            }
                        }
                           Thread.Sleep(TimeSpan.FromSeconds(5));
                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                        //FileLogger.Error("Błąd podczas zapisu logów {0}", e);
                    }
                }
            }
            catch (Exception e)
            {
                //FileLogger.Error("Błąd podczas pracy loggera {0}", e);
            }
            finally
            {
                //FileLogger.Error("Koniec pracy loggera");
            }
        }
    }
}
