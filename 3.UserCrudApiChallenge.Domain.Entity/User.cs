using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UserCrudApiChallenge.Domain.Entity
{
    public class User
    {
        public string Id { get;  set; }
        public string Password { get;  set; }
        public string UserName { get;  set; }
        public string Email { get;  set; }

        public User(string id, string userName, string password, string email)
        { 
            Id = id;
            UserName = userName;
            Password = password;
            Email = email;
        }

        public User() { }


    }
}
