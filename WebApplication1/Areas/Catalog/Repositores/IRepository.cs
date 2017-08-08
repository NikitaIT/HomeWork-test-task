using System;
using System.Collections.Generic;

namespace WebApplication1.Areas.Catalog.Repositores
{
    public interface IRepository<T> : IDisposable
    {
        List<T> GetList();
        T Get(int id);
        void Create(T item);
        void Update(T item);
        void Delete(T item);
    }
}