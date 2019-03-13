using System;
using System.Collections.Generic;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace ArchitectNow.ApiStarter.Common.BaseDb
{
    public abstract class BaseDocument
    {
        public BaseDocument()
        {
            IsActive = true;
        }

        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTimeOffset? UpdatedDate { get; set; } = DateTime.UtcNow;

        public Guid? OwnerUserId { get; set; }

        [JsonIgnore] public List<ValidationResult> ValidationErrors { get; set; }

         public Dictionary<string, object> ExtraElements { get; set; }
    }
}