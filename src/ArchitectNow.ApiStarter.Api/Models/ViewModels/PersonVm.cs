using System;
using System.Collections.Generic;
using ArchitectNow.ApiStarter.Common.Models;

namespace ArchitectNow.ApiStarter.Api.Models.ViewModels
{
    public class PersonVm
    {
        public PersonVm()
        {
            Addresses = new List<Address>();
        }

        /// <summary>
        /// Unique identifier for this object 
        /// </summary>
        public Guid? Id { get; set; }
        
        public string NameFirst { get; set; }
        public string NameLast { get; set; }

        public List<Address> Addresses { get; set; }
    }
}