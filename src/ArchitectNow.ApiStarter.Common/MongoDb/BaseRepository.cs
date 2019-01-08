﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models.Options;
using ArchitectNow.ApiStarter.Common.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace ArchitectNow.ApiStarter.Common.MongoDb
{
    public abstract class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : BaseDocument
    {
        private readonly MongoOptions _options;
        private readonly IValidator<TModel> _validator;
        private MongoClient _client;
        private IMongoCollection<TModel> _collection;
        private string _databaseName;

        protected BaseRepository(ILogger<TModel> logger,
            ICacheService cacheService,
            IOptions<MongoOptions> options,
            IValidator<TModel> validator = null
        )
        {
            _validator = validator ?? new InlineValidator<TModel>();
            CacheService = cacheService;
            Logger = logger;
            _options = options.Value;

            InitClient();
        }

        protected ICacheService CacheService { get; }
        protected ILogger<TModel> Logger { get; }

        public IMongoDatabase Database => _client.GetDatabase(_databaseName);

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
                    _collection = Database.GetCollection<TModel>(CollectionName);

                return _collection.AsQueryable();
            }
        }

        public abstract string CollectionName { get; }

        public virtual async Task<bool> DeleteAllAsync()
        {
            var filter = new BsonDocument();
            await GetCollection().DeleteManyAsync(filter);

            return true;
        }

        public virtual async Task<List<TModel>> GetAllAsync(bool onlyActive = true)
        {
            List<TModel> results;

            if (onlyActive)
                results = await GetCollection().Find(x => x.IsActive).ToListAsync();
            else
                results = await GetCollection().Find(x => x.IsActive).ToListAsync();

            return results;
        }

        public virtual async Task<TModel> GetOneAsync(Guid id)
        {
            var cacheKey = BuildCacheKey(nameof(GetOneAsync), id);

            var result = CacheService.Get<TModel>(cacheKey);

            if (result != null)
                return result;

            result = await GetCollection().Find(x => x.Id == id).FirstOrDefaultAsync();

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
                await GetCollection().InsertOneAsync(item);
            }
            else
            {
                var filter = Builders<TModel>.Filter.Eq("_id", item.Id);
                await GetCollection().ReplaceOneAsync(filter, item, new UpdateOptions {IsUpsert = true});
            }

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
            var filter = Builders<TModel>.Filter.Eq("_id", id);

            await GetCollection().DeleteOneAsync(filter);

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
        ///     Configures the indexes.
        /// </summary>
        public virtual async Task ConfigureIndexes()
        {
            await CreateIndex("Id", Builders<TModel>.IndexKeys.Ascending(x => x.Id).Ascending(x => x.IsActive));
        }

        private void InitClient()
        {
            var connectionString = _options.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("No DB connection found");

            var pack = new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String),
                new CamelCaseElementNameConvention()
            };

            ConventionRegistry.Register("AN Conventions", pack, t => true);
            MongoDefaults.MaxConnectionIdleTime = TimeSpan.FromMinutes(1);

            _databaseName = _options.DatabaseName;

            if (string.IsNullOrEmpty(_databaseName))
                throw new Exception("No database name found");

            _client = new MongoClient(connectionString);
        }

        /// <summary>
        ///     Gets the collection.
        /// </summary>
        /// <returns></returns>
        protected IMongoCollection<TModel> GetCollection()
        {
            return _collection ?? (_collection = Database.GetCollection<TModel>(CollectionName));
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

        protected virtual async Task CreateIndex(string name, IndexKeysDefinition<TModel> keys)
        {
            var options = new CreateIndexOptions<TModel>
            {
                Name = name
            };

            var model = new CreateIndexModel<TModel>(keys, options);
            await GetCollection().Indexes.CreateOneAsync(model);
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
            return await GetCollection().CountDocumentsAsync(filter);
        }

        protected async Task<List<TModel>> FindAsync(Expression<Func<TModel, bool>> filter)
        {
            return await GetCollection().Find(filter).ToListAsync();
        }

        protected async Task<TModel> FindOneAsync(Expression<Func<TModel, bool>> filter)
        {
            return await GetCollection().Find(filter).FirstOrDefaultAsync();
        }
    }
}