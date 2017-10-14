using System;
using System.Collections.Generic;
using FluentValidation.Results;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Newtonsoft.Json;

namespace ArchitectNow.ApiStarter.Common.MongoDb
{
    public abstract class BaseDocument
    {
        public BaseDocument()
        {
            CreatedDate = DateTime.UtcNow;
            IsActive = true;
        }
        
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTimeOffset? CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTimeOffset? UpdatedDate { get; set; } = DateTime.UtcNow;

        public Guid? OwnerUserId { get; set; }

        [JsonIgnore]
        [BsonIgnore]
        public List<ValidationResult> ValidationErrors { get; set; }

        [BsonExtraElements]
        public Dictionary<string, object> ExtraElements { get; set; }
    }
}