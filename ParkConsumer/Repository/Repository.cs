using ParkConsumer.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkConsumer.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public Task<bool> CreateAsync(string url, T objToCreate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(string url, int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(string url, int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(string url, T objToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
