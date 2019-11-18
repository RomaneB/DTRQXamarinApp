using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp.IRepository
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        int Add(T entity);
        T Update(T entity);
        int Delete(int id);
    }
}
