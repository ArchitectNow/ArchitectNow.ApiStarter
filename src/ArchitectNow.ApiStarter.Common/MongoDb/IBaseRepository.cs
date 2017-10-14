using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models;

namespace ArchitectNow.ApiStarter.Common.MongoDb
{
    public interface IBaseRepository<T> : IBaseRepository  
        where T : BaseDocument
    {
        Task<bool> DeleteAllAsync();
        Task<List<T>> GetAllAsync(bool onlyActive = true);
        Task<T> GetOneAsync(Guid id);
        Task<T> SaveAsync(T item);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteAsync(T item);
    }

    public interface IBaseRepository 
    {
        string CollectionName { get; }
        Task ConfigureIndexes();
        bool HasValidUser();
    }
}