using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PHP_SRePS
{
    internal class EntityUserAccount
    {
        public EntityUserAccount(string id, string security, string role)
        {
            UserID = id;
            Password = security;
            AccountType = role;
        }

        public string UserID { get; set; }

        public string Password { get; set; }

        public string AccountType { get; set; }
    }
}