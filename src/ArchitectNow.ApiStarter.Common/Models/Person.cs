using System.Collections.Generic;
using ArchitectNow.ApiStarter.Common.MongoDb;

namespace ArchitectNow.ApiStarter.Common.Models
{
    public class Person : BaseDocument
    {
        public Person()
        {
            Addresses = new List<Address>();
        }
        
        public string NameFirst { get; set; }
        public string NameLast { get; set; }
        
        public List<Address> Addresses { get; set; }
        
    }
}