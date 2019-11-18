using DTRQXamarinApp.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp.Repository
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly IDatabase DatabaseConnection;

        public Repository(IDatabase database)
        {
            DatabaseConnection = database;         
        }
        public IEnumerable<T> GetAll()
        {
            try
            {
                using (var db = DatabaseConnection.Connection())
                {
                    return db.Table<T>().ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        public T GetById(int id)
        {
            try
            {
                using (var db = DatabaseConnection.Connection())
                {
                    return db.Find<T>(id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            
        }
        //public TableQuery<T> AsQueryable()
        //{
        //    return _database.Table<T>();
        //}
        public int Add(T entity)
        {
            try
            {
                using (var db = DatabaseConnection.Connection())
                {
                    return db.Insert(entity);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            
        }
        public T Update(T entity)
        {
            try
            {
                using (var db = DatabaseConnection.Connection())
                {
                    db.Update(entity);
                    return entity;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

        }
        public int Delete(int id)
        {
            try
            {
                using (var db = DatabaseConnection.Connection())
                {
                    return db.Delete<T>(id);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }            
        }
        public bool ClearTable()
        {
            try
            {
                using (var db = DatabaseConnection.Connection())
                {
                    return db.DropTable<T>() == 1 && db.CreateTable<T>() == SQLite.CreateTableResult.Created;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }

            
        }
    }
}
