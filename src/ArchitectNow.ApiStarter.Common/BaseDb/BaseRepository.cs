using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ArchitectNow.ApiStarter.Common.BaseDb
{
    public abstract class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : BaseDocument
    {
        private readonly IValidator<TModel> _validator;
        private DbSet<TModel> _collection;

        protected BaseRepository(ILogger<TModel> logger,
            ICacheService cacheService,
            ApiStarterContext context,
            IValidator<TModel> validator = null
        )
        {
            _validator = validator ?? new InlineValidator<TModel>();
            CacheService = cacheService;
            Logger = logger;
            Database = context;
        }

        protected ICacheService CacheService { get; }
        protected ILogger<TModel> Logger { get; }

        public ApiStarterContext Database { get; }

        /// <summary>
        ///     Gets the data query.
        /// </summary>
        /// <value>
        ///     The data query.
        /// </value>
        protected virtual IQueryable<TModel> DataQuery
        {
            get
            {
                if (_collection == null)
                    _collection = Database.Set<TModel>();

                return _collection;
            }
        }

        public abstract string CollectionName { get; }

        public virtual async Task<bool> DeleteAllAsync()
        {
            GetCollection().RemoveRange(GetCollection());

            return true;
        }

        public virtual async Task<List<TModel>> GetAllAsync(bool onlyActive = true)
        {
            List<TModel> results;

            // This doesn't seem correct
            if (onlyActive)
                results = await GetCollection().Where(x => x.IsActive).ToListAsync();
            else
                results = await GetCollection().Where(x => x.IsActive).ToListAsync();

            return results;
        }

        public virtual async Task<TModel> GetOneAsync(Guid id)
        {
            var cacheKey = BuildCacheKey(nameof(GetOneAsync), id);

            var result = CacheService.Get<TModel>(cacheKey);

            if (result != null)
                return result;

            result = await GetCollection().FirstOrDefaultAsync(x => x.Id == id);

            CacheService.Add(cacheKey, result);
            return result;
        }

        public virtual async Task<TModel> SaveAsync(TModel item)
        {
            if (item.Id != Guid.Empty)
                item.UpdatedDate = DateTime.UtcNow;

            var errors = await ValidateObject(item);

            if (errors.Any())
                throw new ValidationException("A validation error has occured saving item of type '" + item.GetType(),
                    errors);

            if (item.Id == Guid.Empty)
            {
                item.Id = Guid.NewGuid();
                await GetCollection().AddAsync(item);
            }
            else
            {
                GetCollection().Update(item);
            }

            await Database.SaveChangesAsync();

            Logger.LogInformation("Entity Saved to {CollectionName}: \'{Id}\' - {@item}",
                CollectionName,
                item.Id, item);

            //make sure we update the cache...
            var cacheKey = BuildCacheKey(nameof(GetOneAsync), item.Id);

            CacheService.Add(cacheKey, item);

            return item;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var filter = await GetCollection().FirstOrDefaultAsync(x => x.Id == id);

            GetCollection().Remove(filter);
            await Database.SaveChangesAsync();

            Logger.LogInformation("Entity Deleted to {CollectionName}: \'{id}\'", CollectionName, id);

            var cacheKey = BuildCacheKey(nameof(GetOneAsync), id);

            CacheService.Remove(cacheKey);

            return true;
        }

        public virtual async Task<bool> DeleteAsync(TModel item)
        {
            var cacheKey = BuildCacheKey(nameof(GetOneAsync), item.Id);

            CacheService.Remove(cacheKey);

            return await DeleteAsync(item.Id);
        }

        /// <summary>
        ///     Gets the collection.
        /// </summary>
        /// <returns></returns>
        protected DbSet<TModel> GetCollection()
        {
            return _collection ?? (_collection = Database.Set<TModel>());
        }


        /// <summary>
        ///     Builds the cache key prefix.
        /// </summary>
        /// <returns></returns>
        protected virtual string BuildCacheKeyPrefix()
        {
            return $"{GetType()}";
        }

        /// <summary>
        ///     Builds the cache key.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected virtual string BuildCacheKey(string methodName, params object[] parameters)
        {
            var key = $"{BuildCacheKeyPrefix()}.{methodName}";

            if (parameters != null && parameters.Length > 0)
                key = parameters.Aggregate(key, (current, param) => current + (".-" + param));

            return key;
        }

        /// <summary>
        ///     Validates the object.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected virtual async Task<IList<ValidationFailure>> ValidateObject(TModel item)
        {
            var validationResult = await _validator.ValidateAsync(item);
            var validationResultErrors = validationResult.Errors;
            return validationResultErrors;
        }

        protected virtual async Task<long> CountAsync(Expression<Func<TModel, bool>> filter)
        {
            return GetCollection().Where(filter).Count();
        }

        protected async Task<List<TModel>> FindAsync(Expression<Func<TModel, bool>> filter, string includes = null)
        {
            var query = GetCollection().AsQueryable();
            if (!string.IsNullOrWhiteSpace(includes))
            {
                query = query.Include(includes);
            }
            return await query.Where(filter).ToListAsync();
        }

        protected async Task<TModel> FindOneAsync(Expression<Func<TModel, bool>> filter)
        {
            return await GetCollection().FirstOrDefaultAsync(filter);
        }
    }
}