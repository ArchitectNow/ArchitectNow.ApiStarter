using System;

namespace ArchitectNow.ApiStarter.Api.Models.ViewModels
{
    public class AddressVm
    {
        public Guid Id { get; set; }
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public float? Latitude { get; set; }

        public float? Longitude { get; set; }
    }
}