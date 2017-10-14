using System;

namespace ArchitectNow.ApiStarter.Common.MongoDb
{
    public interface IDataContext
    {
        Guid? CurrentUserId { get; set; }
        string EnvironmentName { get; set; }
    }
}