using System;
using FluentValidation.AspNetCore;

namespace ArchitectNow.ApiStarter.Api.Models.Validation
{
    public class FluentValidationOptions
    {
        public Action<FluentValidationMvcConfiguration> Configure;
        public bool Enabled { get; set; }
    }
}