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
                AmazonDynamoDBClient client_ = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);
                Table table = Table.LoadTable(client_, "TblUsers");
                DynamoDBContext context = new DynamoDBContext(client_);
                Document result = await table.PutItemAsync(context.ToDocument(user));

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
                AmazonDynamoDBClient client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);
                UpdateItemRequest updateRequest = new UpdateItemRequest()
                {
                    TableName = "TblUsers",
                    Key = new Dictionary<string, AttributeValue>
                {
                    {"Name", new AttributeValue {S = user.Name } }
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
                //var updreq = new UpdateItemRequest()
                //{
                //    TableName = "TblUsers",
                //    Key = new Dictionary<string, AttributeValue> { { "Name", new AttributeValue { S = user.Name } } },
                //    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                //{
                //    { ":Email", new AttributeValue{ S = user.Email }}
                //},
                //    ExpressionAttributeNames = new Dictionary<string, string>
                //{
                //    { "#email", "Email"}
                //},
                //    UpdateExpression = "SET #email = :Email"
                //};

                //await client.UpdateItemAsync(updreq);

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
                AmazonDynamoDBClient client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);
                Table table = Table.LoadTable(client, "TblUsers");
                Document result = await table.GetItemAsync(userId);
                return MapUserWithPassword(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<User> FindUserByUserName(string username)
        {
            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);

                QueryRequest qry = new QueryRequest
                {
                    TableName = "TblUsers",
                    ExpressionAttributeNames = new Dictionary<string, string>
                    {
                      { "#Name", "Name" }
                    },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":name", new AttributeValue { S = username } } },
                    KeyConditionExpression = "#Name = :name",
                };

                var result = await client.QueryAsync(qry);

                if (result.Count == 0 || result is null)
                {
                    throw new Exception("No user");
                }



                return UsersMapper(result.Items.FirstOrDefault());
            }
            catch (Exception ex)
            {

                throw;
            }

        }



        public async Task<List<User>> GetUsers()
        {
            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);

                DynamoDBContext context = new DynamoDBContext(client);
                Table table = Table.LoadTable(client, "TblUsers");

                //get all records
                var conditions = new List<ScanCondition>();
                // you can add scan conditions, or leave empty
                List<User> allUsers = await context.ScanAsync<User>(conditions).GetRemainingAsync();
                return allUsers;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<bool> DeleteUserAsync(string username)
        {
            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);

                DeleteItemRequest request = new DeleteItemRequest
                {
                    TableName = "TblUsers",
                    Key = new Dictionary<string, AttributeValue> { { "Name", new AttributeValue { S = username } } }
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
            User user = new User(document["UserId"], document["UserName"], string.Empty, document["Email"]);
            return user;
        }

        private User UsersMapper(Dictionary<string, AttributeValue> item)
        {

            try
            {
                User user = new User(item["Id"].S, item["Name"].S, item["Password"].S, item["Email"].S);

                return user;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }

}
