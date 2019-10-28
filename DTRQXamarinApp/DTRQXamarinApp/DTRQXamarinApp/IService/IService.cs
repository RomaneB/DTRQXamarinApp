using System;
using System.Collections.Generic;
using System.Text;

namespace DTRQXamarinApp.IService
{
    public interface IService<T>
    {
        IEnumerable<T> GetAll();
        T GetByid(int id);
        int Add(T entity);
        int Update(T entity);
        int Delete(int id);
        int ClearTable();
    }
}
