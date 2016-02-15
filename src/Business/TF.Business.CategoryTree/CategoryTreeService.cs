using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF.Business
{
    public class CategoryTreeService : ICategoryTreeService
    {
        private readonly string _connectionString;

        public void Create(CategoryTree category) { throw new NotImplementedException(); }
        public void Update(CategoryTree category) { throw new NotImplementedException(); }
        public void Delete(Guid id) { throw new NotImplementedException(); }

        public void AddChild(Guid productId, Guid childId) { throw new NotImplementedException(); }
        public void DeleteChild(Guid productId, Guid childId) { throw new NotImplementedException(); }

        public CategoryTree GetById(Guid id) { throw new NotImplementedException(); }

        public CategoryTreeService()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CategoryTreeDb"].ConnectionString;

            Init();
        }

        public CategoryTreeService(string connectionString)
        {
            _connectionString = connectionString;

            Init();
        }

        private void Init()
        {
            if (String.IsNullOrEmpty(_connectionString))
                throw new Exception("Connectionstring is missing");

            CreateDBIfNotExists(_connectionString);
            CreateSchema(_connectionString);
        }

        private void CreateDBIfNotExists(string connectionString)
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            var databaseName = connectionStringBuilder.InitialCatalog;

            connectionStringBuilder.InitialCatalog = "master";

            using (var connection = new SqlConnection(connectionStringBuilder.ToString()))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("select * from master.dbo.sysdatabases where name='{0}'", databaseName);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // exists
                            return;
                    }

                    command.CommandText = string.Format("CREATE DATABASE {0}", databaseName);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void CreateSchema(string connectionString)
        {
            var sql = @"
            if object_id('[BUSINESS.CATEGORY_TREE]', 'U') is null begin
	            CREATE TABLE [dbo].[BUSINESS.CATEGORY_TREE] (
		            [GUID_RECORD] [uniqueidentifier] NOT NULL,
		            [KEY] [nvarchar](50) NOT NULL,
		            [NAME] [nvarchar](200),
		            [PARENT_GUID] [uniqueidentifier],
		            [BATCH_GUID] [uniqueidentifier],
		            [HIDDEN] [bit] NOT NULL,
		            [DELETED] [bit] NOT NULL
	            ) ON [PRIMARY]
            end";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
