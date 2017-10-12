using System;

namespace ArchitectNow.ApiStarter.Common.Models
{
    public abstract class BaseDocument
    {
        public BaseDocument()
        {
            CreatedDate = DateTime.UtcNow;
            IsActive = true;
        }
        
        public Guid Id { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}