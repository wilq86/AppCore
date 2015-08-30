using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace AppCore.NHibernate
{
    public class Crud
    {
        public IDbConfiguration Configuration { get; private set; }

        public Crud(ApplicationKernel kernel)
        {
            Configuration = kernel.Kernel.Get<IDbConfiguration>();
        }

        public Crud(IDbConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Podstawowa metoda dostępu do bazy danych - pamiętaj o mergowaniu obiektów utworzonych w innej sesji
        /// </summary>
        /// <param name="operation">Anonimowa metoda dostepu do danych</param>
        protected void InSession(Action<ISession> operation)
        {
            using (var session = Configuration.GetSessionFactory().OpenSession())
            {
                operation(session);
            }
        }

        /// <summary>
        /// Transakcja na bazie - w przypadku błędu
        /// na całości jest rollback - ale wyjątek jest wyrzucany
        /// </summary>
        /// <param name="operation"></param>
        protected void InTransaction(Action<ISession> operation)
        {
            using (var session = Configuration.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        operation(session);
                        transaction.Commit();
                        session.Flush();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Pobieranie wszystkich encji danego typu
        /// </summary>
        /// <typeparam name="T">Klasa MUSI być zmapowana</typeparam>
        /// <returns>Lista obiektów danej klasy - uwaga nigdy nie zwracamy null</returns>
        public List<T> GetAll<T>() where T : class
        {
            var result = new List<T>();
            InSession(session =>
            {
                result = session.CreateCriteria<T>().List<T>().ToList();
            });
            return result;
        }

        /// <summary>
        /// Dodanie encji w transakcji
        /// Jeśli encja istnieje - rzuci błędem
        /// Uwaga - założenie że NHibernate doda wszystkie pola (bo są zmapowane)
        /// Dlatego zwracam encje wejściową uzupełnioną o identyfikator
        /// W przypadku nie dodania - jest null
        /// </summary>
        /// <typeparam name="T">Klasa MUSI być mapowana - i najlepiej jak dziedziczy po AbstractEntity - zawiera Id</typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Add<T>(T entity) where T : class
        {
            T result = null;
            InTransaction(session =>
            {
                var savedItem = session.Save(entity);
                if (savedItem is T)
                {
                    result = savedItem as T;
                }
                else if (savedItem is int)
                {
                    result = entity;
                    if (result is AbstractEntity)
                        (result as AbstractEntity).Id = (int)savedItem;
                }
                else if (savedItem is long)
                {
                    result = entity;
                    if (result is AbstractEntity)
                        (result as AbstractEntity).Id = (long)savedItem;
                }
            });
            return result;
        }

        /// <summary>
        /// Dodawanie grupowe encji
        /// Błąd jednego - wywala całość
        /// Dlatego uważaj co insertujesz
        /// </summary>
        /// <typeparam name="T">Klasa MUSI być mapowana - i najlepiej jak dziedziczy po AbstractEntity - zawiera Id</typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<T> AddGroup<T>(IEnumerable<T> data) where T : class
        {
            var result = new List<T>();
            InTransaction(session =>
            {
                foreach (var entity in data)
                {
                    var savedItem = session.Save(entity);
                    if (savedItem is T)
                        result.Add(savedItem as T);
                    else if (savedItem is int)
                    {
                        var itm = entity;
                        if (itm is AbstractEntity)
                            (itm as AbstractEntity).Id = (int)savedItem;
                        result.Add(itm);
                    }
                }
            });

            return result;
        }

        /// <summary>
        /// Aktualizacja encji w transakcji- UWAGA obiekt musi istnieć w bazie - bez tego będzie błąd
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns>Zaktualizowany obiekt</returns>
        public T Update<T>(T entity) where T : class
        {
            T result = null;
            InTransaction(session =>
            {
                entity = session.Merge(entity);
                session.Update(entity);
                result = entity;
            });
            return result;
        }

        /// <summary>
        /// Grupowa aktualizacja encji - brak istnienia jednej powoduje rollback na wszystkich
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<T> UpdateGroup<T>(IEnumerable<T> data) where T : class
        {
            var result = new List<T>();
            InTransaction(session =>
            {
                foreach (var entity in data)
                {
                    var itm = session.Merge(entity);
                    session.Update(itm);
                    result.Add(itm);
                }
            });
            return result;
        }

        /// <summary>
        /// Aktualizacja/dodanie encji zależne od warunku czy Id>0
        /// </summary>
        /// <typeparam name="T">Klasa musi dziedziczyć po AbstractEntity - tj posiadać Id</typeparam>
        /// <param name="entity"></param>
        /// <returns>Dodany/zaktualizowany rekord</returns>
        public T Save<T>(T entity) where T : AbstractEntity
        {
            T result = null;
            result = entity.Id == 0 ? Add(entity) : Update(entity);
            return result;
        }

        /// <summary>
        /// Kasowanie mapowanej encji w transakcji
        /// </summary>
        /// <typeparam name="T">Encja musi być mapowana</typeparam>
        /// <param name="entity"></param>
        public void Delete<T>(T entity) where T : class
        {
            InTransaction(session => session.Delete(entity));
        }

        /// <summary>
        /// Grupowe usuwanie
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public void DeleteGroup<T>(IEnumerable<T> data) where T : class
        {
            InTransaction(session =>
            {
                foreach (var item in data)
                {
                    session.Delete(item);
                }
            });
        }

        /// <summary>
        /// Pobieranie encji za pomocą podstawowego identyfikatora
        /// </summary>
        /// <typeparam name="T">Klasa MUSI dziedzić po AbstractEntity</typeparam>
        /// <param name="id">Identyfikator >0 </param>
        /// <returns>W przypadku braku encji zwracamy null</returns>
        public T GetById<T>(long id) where T : AbstractEntity
        {
            T result = null;
            InSession(session =>
            {
                result = session.Query<T>().SingleOrDefault(x => x.Id == id);
            });
            return result;
        }
    }
}
