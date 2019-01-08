﻿using System;
using System.IO;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace ArchitectNow.ApiStarter.Api
{
    public class Program
    {
        private static IConfiguration _configuration;

        public static async Task<int> Main(string[] args)
        {
            try
            {
                _configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .AddJsonFile(
                        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                        true)
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build();

                Log.Logger = _configuration.ConfigureLogging().CreateLogger();
                var host = BuildWebHost(args);

                //initialization
                await host.RunAsync();
                return 0;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Site terminated");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(_configuration)
                .UseSerilog()
                .UseStartup<Startup>();

            return builder.Build();
        }
    }
}