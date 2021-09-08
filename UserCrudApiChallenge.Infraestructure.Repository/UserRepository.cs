using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Transactions;
using UserCrudApiChallenge.Infraestructure.Interface;
using UserCrudApiChallenge.CrossCutting.User;
using UserCrudApiChallenge.Domain.Entity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon;

namespace UserCrudApiChallenge.Infraestructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BasicAWSCredentials _connection;

        public UserRepository()
        {
            _connection =  new BasicAWSCredentials("AKIARZCKHZFCSPH72PKB", "97MalXp8LrbXwynGZFVRhy2/BMjXT2+Wf8hshxJ0");
        }

        public async Task<User> AddUserAsync(User user)
        {
            try
            {
                var client_ = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);
                var table = Table.LoadTable(client_, "Users");
                var context = new DynamoDBContext(client_);
                var result = await table.PutItemAsync(context.ToDocument(user));

                return user;
            }
            catch (Exception ex)
            {

                throw;
            }
        
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                var client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);

                UpdateItemRequest updateRequest = new UpdateItemRequest()
                {
                    TableName = "Users",
                    Key = new Dictionary<string, AttributeValue>
                {
                    {"UserName", new AttributeValue {S = user.UserName } }
                },
                    AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
                {
                    {"Email", new AttributeValueUpdate
                        {
                            Value = new AttributeValue{ S =  user.Email },
                            Action = AttributeAction.PUT
                        }
                    }
                }
                };


                await client.UpdateItemAsync(updateRequest);

                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
     
        }

        public async Task<User> FindUserByIdAsync(string userId)
        {

            try
            {
                var client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);
                var table = Table.LoadTable(client, "Users");
                var result = await table.GetItemAsync(userId);
                return MapUserWithPassword(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<User> FindUserByUserName(string username)
        {
            var client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);

            var query = new QueryRequest
            {
                TableName = "Users",
                KeyConditionExpression = "UserName = :username",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":username", new AttributeValue { S = username } } }
            };

            var result = await client.QueryAsync(query);

            if (result.Count == 0)
            {
                throw new Exception("NO exist!!");
            }

            return UsersMapper(result.Items.FirstOrDefault());
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            try
            {
                var client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);

                var request = new DeleteItemRequest
                {
                    TableName = "Users",
                    Key = new Dictionary<string, AttributeValue> { { "UserName", new AttributeValue { S = username } } }
                };

                await client.DeleteItemAsync(request);

                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
       
        }


        private User MapUserWithPassword(Document document)
        {
            var user = new User(document["UserId"], document["UserName"], string.Empty, document["Email"]);

            return user;
        }

        private User UsersMapper(Dictionary<string, AttributeValue> item)
        {

            try
            {
                var user = new User(item["Id"].S, item["UserName"].S, item["Password"].S, item["Email"].S);

                return user;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }

}
