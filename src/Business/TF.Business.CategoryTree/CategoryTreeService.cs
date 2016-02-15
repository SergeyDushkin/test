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

        public void Create(CategoryTree category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            if (category.Id == Guid.Empty)
                category.Id = Guid.NewGuid();

            if (string.IsNullOrEmpty(category.Key))
                throw new ArgumentNullException("Key");

            if (string.IsNullOrEmpty(category.Name))
                throw new ArgumentNullException("Name");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    /// Check id
                    command.CommandText = string.Format("select 1 from [BUSINESS.CATEGORY_TREE] where [GUID_RECORD] = '{0}'", category.Id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            throw new Exception("Record already exists");
                    }

                    /// Check key
                    command.CommandText = string.Format("select 1 from [BUSINESS.CATEGORY_TREE] where [KEY] = '{0}'", category.Key);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            throw new Exception("Record already exists");
                    }

                    /// Check parent id
                    if (category.ParentId.HasValue && category.ParentId.Value != Guid.Empty)
                    {
                        command.CommandText = string.Format("select 1 from [BUSINESS.CATEGORY_TREE] where [GUID_RECORD] = '{0}'", category.ParentId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                                throw new Exception("Record does't exists");
                        }
                    }

                    command.CommandText = @"insert into [BUSINESS.CATEGORY_TREE] (
                        [GUID_RECORD], 
                        [KEY], 
                        [NAME], 
                        [PARENT_GUID], 
                        [BATCH_GUID], 
                        [HIDDEN], 
                        [DELETED]) 
                    values (@GUID_RECORD, @KEY, @NAME, @PARENT_GUID, @BATCH_GUID, @HIDDEN, @DELETED)";

                    command.Parameters.AddWithValue("@GUID_RECORD", category.Id);
                    command.Parameters.AddWithValue("@KEY", category.Key);
                    command.Parameters.AddWithValue("@NAME", category.Name);
                    command.Parameters.AddWithValue("@PARENT_GUID", category.ParentId.HasValue ? category.ParentId.Value : (object)DBNull.Value);
                    command.Parameters.AddWithValue("@BATCH_GUID", DBNull.Value);
                    command.Parameters.AddWithValue("@HIDDEN", 0);
                    command.Parameters.AddWithValue("@DELETED", 0);

                    command.ExecuteNonQuery();
                }
            }
        }


        public void Update(CategoryTree category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            if (category.Id == Guid.Empty)
                throw new ArgumentNullException("Id");

            if (string.IsNullOrEmpty(category.Key))
                throw new ArgumentNullException("Key");

            if (string.IsNullOrEmpty(category.Name))
                throw new ArgumentNullException("Name");

            var record = GetById(category.Id);

            var alterColumns = new List<string>();
            var alterValues = new List<SqlParameter>();

            if (!String.Equals(record.Key, category.Key))
            {
                alterColumns.Add("[KEY] = @KEY");
                alterValues.Add(new SqlParameter("@KEY", category.Key));
                /// add check constraint
            }

            if (!String.Equals(record.Name, category.Name))
            {
                alterColumns.Add("[NAME] = @NAME");
                alterValues.Add(new SqlParameter("@NAME", category.Name));
            }

            if (!Nullable<Guid>.Equals(record.ParentId, category.ParentId))
            {
                alterColumns.Add("[PARENT_GUID] = @PARENT_GUID");
                alterValues.Add(new SqlParameter("@PARENT_GUID", category.ParentId));

                /// add check constraint
            }

            if (alterColumns.Any())
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = String.Format(
                            "update [BUSINESS.CATEGORY_TREE] set {0} where [GUID_RECORD] = @GUID_RECORD",
                            String.Join(", ", alterColumns));

                        command.Parameters.AddWithValue("@GUID_RECORD", category.Id);
                        command.Parameters.AddRange(alterValues.ToArray());
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from [BUSINESS.CATEGORY_TREE] where [GUID_RECORD] = @GUID_RECORD";
                    command.Parameters.AddWithValue("@GUID_RECORD", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddChild(Guid productId, Guid childId) { throw new NotImplementedException(); }
        public void DeleteChild(Guid productId, Guid childId) { throw new NotImplementedException(); }

        public CategoryTree GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from [BUSINESS.CATEGORY_TREE] where [GUID_RECORD] = @GUID_RECORD";
                    command.Parameters.AddWithValue("@GUID_RECORD", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            return new CategoryTree
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("GUID_RECORD")),
                                Key = reader.GetString(reader.GetOrdinal("KEY")),
                                Name = reader.GetString(reader.GetOrdinal("NAME")),
                                ParentId = reader.IsDBNull(reader.GetOrdinal("PARENT_GUID"))
                                    ? default(Guid?)
                                    : reader.GetGuid(reader.GetOrdinal("PARENT_GUID"))
                            };
                        }
                        else
                        {
                            throw new Exception("Record does't exists");
                        }
                    }
                }
            }
        }

        private bool CheckId(Guid id)
        {
            return true;
        }

        public bool CheckKey(string key)
        {
            return true;
        }

        public bool CheckName(string name)
        {
            return true;
        }

        public bool CheckParentId(Guid? parentId)
        {
            return true;
        }

        public CategoryTreeService()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CategoryTreeDb"].ConnectionString;
        }

        public CategoryTreeService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Init()
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
