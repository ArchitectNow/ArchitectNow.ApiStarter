using System;
using System.Collections.Generic;
using ArchitectNow.ApiStarter.Common.Models;

namespace ArchitectNow.ApiStarter.Common
{
    public static class ApiStarterInitializer
    {
        public static void Initialize(ApiStarterContext context)
        {
            var person = new Person()
            {
                Id = new Guid(),
                Addresses = new List<Address>
                {
                    new Address
                    {
                        AddressLine1 = "123 South St",
                        City = "Edwardsville",
                        CreatedDate = DateTimeOffset.Now,
                        Id = new Guid(),
                        IsActive = true
                    }
                },
                IsActive = true,
                NameLast = "will",
                NameFirst = "will"
            };

            context.Person.Add(person);
            context.SaveChangesAsync();
        }
    }
}