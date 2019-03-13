using System;
using ArchitectNow.ApiStarter.Common.BaseDb;

namespace ArchitectNow.ApiStarter.Common.Models
{
    public class Address : BaseDocument
    {
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public float? Latitude { get; set; }

        public float? Longitude { get; set; }
        public Guid PersonId { get; set; }
        
        public Person Person { get; set; }
    }
}