using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UserCrudApiChallenge.Domain.Entity;
using UserCrudApiChallenge.Infraestructure.Interface;

namespace UserCrudApiChallenge.Infraestructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BasicAWSCredentials _connection;

        public UserRepository()
        {
            _connection = new BasicAWSCredentials("AKIARZCKHZFCSPH72PKB", "97MalXp8LrbXwynGZFVRhy2/BMjXT2+Wf8hshxJ0");
        }

        public async Task<User> AddUserAsync(User user)
        {
            try
            {
                AmazonDynamoDBClient client_ = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);
                Console.WriteLine(_connection.GetCredentials().AccessKey);
                Console.WriteLine(_connection.GetCredentials().SecretKey);
                Console.WriteLine(_connection.GetCredentials().Token);
                Table table = Table.LoadTable(client_, "TblUsers_");
                DynamoDBContext context = new DynamoDBContext(client_);
                Document result = await table.PutItemAsync(context.ToDocument(user));

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("llegue al repository error: " + ex.ToString());

                Console.WriteLine("FAILED to write the new user, because:\n       {0}.", ex.Message);
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
                    TableName = "TblUsers_",
                    Key = new Dictionary<string, AttributeValue>
                {
                    {"Id", new AttributeValue {S = user.Id } }
                },
                    AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
                {
                        {"Name", new AttributeValueUpdate
                        {
                            Value = new AttributeValue{ S =  user.Name },
                            Action = AttributeAction.PUT
                        }
                    },
                    {"Email", new AttributeValueUpdate
                        {
                            Value = new AttributeValue{ S =  user.Email },
                            Action = AttributeAction.PUT
                        }
                    },
                     {"Password", new AttributeValueUpdate
                        {
                            Value = new AttributeValue{ S =  user.Password },
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
                AmazonDynamoDBClient client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);
                Table table = Table.LoadTable(client, "TblUsers_");
                Document result = await table.GetItemAsync(userId);
                return MapUserWithPassword(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> ValidateUserLogin(string email, string password)
        {
            try
            {
                bool validate = true;
                AmazonDynamoDBClient client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);

                DynamoDBContext context = new DynamoDBContext(client);
                Table table = Table.LoadTable(client, "TblUsers_");

                //get all records
                var conditions = new List<ScanCondition>();
                // you can add scan conditions, or leave empty
                List<User> allUsers = await context.ScanAsync<User>(conditions).GetRemainingAsync();

                User user_ = allUsers.Where(x => x.Email == email && x.Password == password).FirstOrDefault();

                validate = user_ == null ? false : true;

                return validate;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> FindUserById(string id)
        {
            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);

                QueryRequest qry = new QueryRequest
                {
                    TableName = "TblUsers_",
                    ExpressionAttributeNames = new Dictionary<string, string>
                    {
                      { "#Id", "Id" }
                    },
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { ":id", new AttributeValue { S = id } } },
                    KeyConditionExpression = "#Id = :id",
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
                Table table = Table.LoadTable(client, "TblUsers_");

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

        public async Task<bool> DeleteUserAsync(string id)
        {
            try
            {
                AmazonDynamoDBClient client = new AmazonDynamoDBClient(_connection, RegionEndpoint.USEast2);

                DeleteItemRequest request = new DeleteItemRequest
                {
                    TableName = "TblUsers_",
                    Key = new Dictionary<string, AttributeValue> { { "Id", new AttributeValue { S = id } } }
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
            User user = new User(document["Id"], document["Name"], string.Empty, document["Email"]);
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