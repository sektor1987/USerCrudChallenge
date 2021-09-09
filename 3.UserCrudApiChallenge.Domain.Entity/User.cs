using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UserCrudApiChallenge.Domain.Entity
{
    [DynamoDBTable("TblUsers")]
    public class User
    {
        [DynamoDBHashKey]
        public string Name { get; set; }
        //[DynamoDBRangeKey]
        public string Password { get;  set; }
        public string Email { get;  set; }

        public User(string userName, string password, string email)
        { 
 
            Name = userName;
            Password = password;
            Email = email;
        }

        public User() { }


    }
}
