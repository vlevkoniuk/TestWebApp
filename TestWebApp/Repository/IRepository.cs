using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApp.Repository
{
    public interface IRepository<T> where T : class
    {
        T Get(int Id);
        List<T> GetAll();
        bool Update(T employee);
        bool Delete(T employee);
        bool Create(T employee);

    }
}
