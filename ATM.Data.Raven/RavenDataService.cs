using ATM.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client.Document;
using Raven.Imports.Newtonsoft.Json;
namespace ATM.Data.Raven
{
    public class RavenDataService : IDataService
    {
        static string DbUrl = "http://windows8:8892/";
        static void InitializeStore(DocumentStore documentStore)
        {
            documentStore.Url = DbUrl;
            documentStore.DefaultDatabase = "Commerce";
            //documentStore.Conventions = new DocumentConvention
            //{
            //    CustomizeJsonSerializer=(s)=>
            //        {
            //            s.                       
            //        }
            //};
         
            //documentStore.RegisterListener(new UniqueConstraintsStoreListener());
           
            documentStore.Initialize();
        }
        public void Save<T>(T entity)
        {

            using (var documentStore = new DocumentStore())
            {

                InitializeStore(documentStore);
                using (var session = documentStore.OpenSession())
                {
            
                    session.Store(entity);
                    session.SaveChanges();
                }
            }


        }
   
        public T Load<T>(Guid id)
        {
            Func<DocumentSession, T> execution = (session) =>
                {
                    return session.Load<T>(id.ToString());
                };
            var result = Execute<DocumentSession, T>(execution);
            return result;
        }
        public T Fetch<T>(Func<T, bool> predicate)
        {

            Func<DocumentSession, T> execution = (session) =>
            {               
               
                return session.Advanced.LuceneQuery<T>().SingleOrDefault<T>(predicate);
            };
            var result = Execute<DocumentSession, T>(execution);
            return result;
        }
        public List<T> FetchAll<T>(Func<T, bool> predicate)
        {

            Func<DocumentSession, List<T>> execution = (session) =>
            {
                var items = session.Advanced.LuceneQuery<T>().Where<T>(predicate).ToList();
                return items;
            };
            var result = Execute<DocumentSession, List<T>>(execution);
            return result;
        }
        public T Execute<Q, T>(Func<Q, T> operation)
        {

            T result = default(T);
            using (var documentStore = new DocumentStore())
            {

                InitializeStore(documentStore);
                using (var session = documentStore.OpenSession())
                {
                    result = operation((Q)session);
                }
            }
            return result;


        }
    }
}
