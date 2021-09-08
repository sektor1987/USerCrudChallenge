using System;
using System.Data.SqlClient;
using System.Data;
using UserCrudApiChallenge.CrossCutting.User;
using Microsoft.Extensions.Configuration;

namespace UserCrudApiChallenge.Infraestructure.Data
{
    public class ConnectionFactory: IConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public IDbConnection GetConnection
        //{
        //    //get
        //    //{
        //    //    //var sqlConnection = new SqlConnection();
        //    //    //if (sqlConnection == null) return null;
                
        //    //    //sqlConnection.ConnectionString = _configuration.GetConnectionString("GVDBEngineConnection");
        //    //    //sqlConnection.Open();
        //    //    //return sqlConnection;
        //    //}
        //}
    }
}
