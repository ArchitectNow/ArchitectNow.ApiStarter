using System;

namespace ArchitectNow.ApiStarter.Common.MongoDb
{
    public class DataContext : IDataContext
    {
        public DataContext()
        {
            
        }
        
        public Guid? CurrentUserId { get; set; }
        public string EnvironmentName { get; set; }
    }
}