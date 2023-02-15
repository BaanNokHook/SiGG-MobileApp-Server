using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Security
{
    public sealed class ServerUser
    {
        public ServerUser(string identifier, string name,
            string email, string organization)
        {
            Identifier = identifier;
            Name = name;
            Email = email;
            Organization = organization;
            Roles = new List<UserRole>();
        }

        public string Identifier { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Organization { get; private set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Organization))
                return Name;
            else
                return $"{Name}@{Organization}";
        }

        internal List<UserRole> Roles { get; set; }

        internal bool IsRolesAccessControllerEnabled
        {
            get
            {
                return Roles.Count > 0;
            }
        }

        public void AddRole(string controller, string action, bool enableAccess)
        {
            if (string.IsNullOrEmpty(controller))
                throw new Exception("It is mandatory to enter a name of a valid Controller on the server");
            if (string.IsNullOrEmpty(action))
                throw new Exception("It is not allowed to use empty strings for Action. If your intention is to cover all Controller Actions, enter the character '*' instead of an empty string");

            Roles.Add(new UserRole(controller, action, enableAccess));
        }
    }
}
