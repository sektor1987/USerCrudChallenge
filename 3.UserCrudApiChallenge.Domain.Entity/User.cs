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
        public string Id { get;  set; }
        public string Password { get;  set; }
        public string Email { get;  set; }

        public User(string id, string userName, string password, string email)
        { 
            Id = id;
            Name = userName;
            Password = password;
            Email = email;
        }

        public User() { }


    }
}
