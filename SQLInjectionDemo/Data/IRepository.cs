using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SQLInjectionDemo.Models;

namespace SQLInjectionDemo.Data
{
    public interface IRepository<T> where T: class
    {
        T Get(string id);

        List<T> GetAll();

        T Add(T entity);

        T Update(string id, T entity);

        T Delete(string id);
    }

    public interface IFileItemRepository : IRepository<FileItem>
    {
        List<FileItem> GetAllByName(string name);
    }
}
