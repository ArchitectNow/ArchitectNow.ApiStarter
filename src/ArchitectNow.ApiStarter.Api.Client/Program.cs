using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ArchitectNow.ApiStarter.Api.Client
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Debug.WriteLine("Starting test console app...");

            var securityClient = new SecurityClient();

            var token = "";

//            client.BaseUrl = "";

            try
            {
                var loginParams = new LoginVm();

                loginParams.Email = "kvgros@architectnow.net";
                loginParams.Password = "testtest";

                var loginResult = await securityClient.LoginAsync(loginParams);

                token = loginResult.AuthToken;

                Console.WriteLine("Successful login!");
                Console.WriteLine("Logged in as: " + loginResult.CurrentUser.Email);
            }
            catch (Exception e)
            {
                Console.WriteLine("Login Error: " + e.Message);
            }

            if (!string.IsNullOrEmpty(token))
            {
                var personClient = new PersonClient();

                personClient.Token = token;

                try
                {
                    var result = await personClient.SecurityTestAsync();

                    if (result != null)
                    {
                        Console.WriteLine("Successfully called a secure endpoint");
                        Console.WriteLine("Validated token as user: " + result.Email);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Security Error: " + e.Message);
                }
            }

            Console.WriteLine("Press the any key to continue...");
            Console.ReadLine();

            return 1;
        }
    }
}