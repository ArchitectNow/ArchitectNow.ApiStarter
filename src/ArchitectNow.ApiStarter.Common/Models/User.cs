using ArchitectNow.ApiStarter.Common.MongoDb;

namespace ArchitectNow.ApiStarter.Common.Models
{
    public class User : BaseDocument
    {
        public User()
        {
            UserRole = "";
        }

        public string Email { get; set; }

        public string Password { get; set; }

        public string NameFirst { get; set; }

        public string NameLast { get; set; }

        public string UserRole { get; set; }
    }
}