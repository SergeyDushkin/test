using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF.SystemSecurity
{
    public class UserService : IUserService
    {
        private readonly string _connectionString;

        public UserService()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["UserDb"].ConnectionString;
        }

        public UserService(string connectionString)
        {
            _connectionString = connectionString;

        }

        public void Create (USER User)
        {
            if (User == null)
                throw new ArgumentNullException("User");

            if (User.GUID_RECORD == Guid.Empty)
            {
                User.GUID_RECORD = Guid.NewGuid();
            }

            if (string.IsNullOrEmpty(User.KEY))
                throw new ArgumentNullException("KEY");

            if (string.IsNullOrEmpty(User.KEY_IDENTITY) && !string.IsNullOrEmpty(User.PROVIDER))
            {
                throw new ArgumentNullException("KEY_IDENTITY");
            }
            if (!string.IsNullOrEmpty(User.KEY_IDENTITY) && string.IsNullOrEmpty(User.PROVIDER))
            {
                User.PROVIDER = "PASSWORD";
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    /// Check GUID
                    command.CommandText = string.Format("select 1 from [SYSTEM.SECURITY.USER] where [GUID_RECORD] = '{0}'", User.GUID_RECORD);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            throw new Exception("Record already exists");
                    }

                    /// Check key
                    command.CommandText = string.Format("select 1 from [SYSTEM.SECURITY.USER] where [KEY] = '{0}'", User.KEY);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            throw new Exception("Record already exists");
                    }

                    command.CommandText = @"insert into [SYSTEM.SECURITY.USER] (
                        [GUID_RECORD], 
                        [KEY], 
                        [LAST_LOGIN],
                        [LOGIN_ATTEMPT_COUNT],
                        [BATCH_GUID],
                        [HIDDEN],
                        [DELETED]) 
                    values (@GUID_RECORD, @KEY, @LAST_LOGIN, @LOGIN_ATTEMPT_COUNT, @BATCH_GUID, @HIDDEN, @DELETED)";

                    command.Parameters.AddWithValue("@GUID_RECORD", User.GUID_RECORD);
                    command.Parameters.AddWithValue("@KEY", User.KEY);
                    command.Parameters.AddWithValue("@LAST_LOGIN", DateTime.Today);
                    command.Parameters.AddWithValue("@LOGIN_ATTEMPT_COUNT", 0);
                    command.Parameters.AddWithValue("@BATCH_GUID", DBNull.Value);
                    command.Parameters.AddWithValue("@HIDDEN", 0);
                    command.Parameters.AddWithValue("@DELETED", 0);

                    command.ExecuteNonQuery();

                    /// INSERT USER_IDENTITY
                    if (!string.IsNullOrEmpty(User.KEY_IDENTITY) && !string.IsNullOrEmpty(User.PROVIDER))
                    {
                        command.CommandText = string.Format("select 1 from [SYSTEM.SECURITY.USER_IDENTITY] where [USER_GUID] = '{0}' AND PROVIDER = '{1}'", User.GUID_RECORD, User.PROVIDER);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                throw new Exception("Record already exists");
                        }

                        command.CommandText = @"insert into [SYSTEM.SECURITY.USER_IDENTITY] (
                        [USER_GUID], 
                        [PROVIDER],
                        [KEY],
                        [BATCH_GUID],
                        [HIDDEN],
                        [DELETED]) 
                        values (@GUID_RECORD, @USER_GUID, @PROVIDER, @KEY, @BATCH_GUID, @HIDDEN, @DELETED)";

                        command.Parameters.AddWithValue("@USER_GUID", User.GUID_RECORD);
                        command.Parameters.AddWithValue("@PROVIDER", User.PROVIDER);
                        command.Parameters.AddWithValue("@KEY", User.KEY_IDENTITY); /// Добавить преобразование в пароль
                        command.Parameters.AddWithValue("@BATCH_GUID", DBNull.Value);
                        command.Parameters.AddWithValue("@HIDDEN", 0);
                        command.Parameters.AddWithValue("@DELETED", 0);

                        command.ExecuteNonQuery();
                    }



                }
            }
        }


        public void Update (USER User)
        {
            if (User == null)
                throw new ArgumentNullException("User");

            if (User.GUID_RECORD == Guid.Empty)
                throw new ArgumentNullException("GUID_RECORD");

            if (string.IsNullOrEmpty(User.KEY))
                throw new ArgumentNullException("Key");

            var record = GetById(User.GUID_RECORD);

            var alterColumns = new List<string>();
            var alterValues = new List<SqlParameter>();

            if (!String.Equals(record.KEY, User.KEY))
            {
                alterColumns.Add("[KEY] = @KEY");
                alterValues.Add(new SqlParameter("@KEY", User.KEY));
                /// add check constraint
            }

            if (!String.Equals(record.DELETED, User.DELETED))
            {
                alterColumns.Add("[DELETED] = @DELETED");
                alterValues.Add(new SqlParameter("@DELETED", User.DELETED));
            }

            if (!Nullable<Guid>.Equals(record.HIDDEN, User.HIDDEN))
            {
                alterColumns.Add("[HIDDEN] = @HIDDEN");
                alterValues.Add(new SqlParameter("@HIDDEN", User.HIDDEN));
                if (record.HIDDEN == true && User.HIDDEN == false)
                {
                    alterColumns.Add("[LOGIN_ATTEMPT_COUNT] = @LOGIN_ATTEMPT_COUNT");
                    alterValues.Add(new SqlParameter("@LOGIN_ATTEMPT_COUNT", 0));
                }

            }

            if (alterColumns.Any())
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = String.Format(
                            "update [SYSTEM.SECURITY.USER] set {0} where [GUID_RECORD] = @GUID_RECORD",
                            String.Join(", ", alterColumns));

                        command.Parameters.AddWithValue("@GUID_RECORD", User.GUID_RECORD);
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
                    /// DELETED USER
                    command.CommandText = "update [SYSTEM.SECURITY.USER] set [DELETED] = 1 where [GUID_RECORD] = @GUID_RECORD";
                    command.Parameters.AddWithValue("@GUID_RECORD", id);

                    command.ExecuteNonQuery();

                    /// DELETED USER_IDENTITY
                    command.CommandText = "update [SYSTEM.SECURITY.USER_IDENTITY] set [DELETED] = 1 where [USER_GUID] = @USER_GUID";
                    command.Parameters.AddWithValue("@USER_GUID", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public USER GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from [SYSTEM.SECURITY.USER] where [GUID_RECORD] = @GUID_RECORD";
                    command.Parameters.AddWithValue("@GUID_RECORD", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            return new USER
                            {
                                GUID_RECORD = reader.GetGuid(reader.GetOrdinal("GUID_RECORD")),
                                KEY = reader.GetString(reader.GetOrdinal("KEY")),
                                LOGIN_ATTEMPT_COUNT = reader.GetInt16(reader.GetOrdinal("LOGIN_ATTEMPT_COUNT")),
                                HIDDEN = reader.GetBoolean(reader.GetOrdinal("HIDDEN")),
                                DELETED = reader.GetBoolean(reader.GetOrdinal("DELETED"))
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

        
        public IEnumerable<USER> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from [SYSTEM.SECURITY.USER]";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new USER
                            {
                                GUID_RECORD = reader.GetGuid(reader.GetOrdinal("GUID_RECORD")),
                                KEY = reader.GetString(reader.GetOrdinal("KEY")),
                                LAST_LOGIN = reader.GetDateTime(reader.GetOrdinal("LAST_LOGIN")),
                                LOGIN_ATTEMPT_COUNT = reader.GetInt16(reader.GetOrdinal("LOGIN_ATTEMPT_COUNT")),
                                HIDDEN = reader.GetBoolean(reader.GetOrdinal("HIDDEN")),
                                DELETED = reader.GetBoolean(reader.GetOrdinal("DELETED"))
                            };
                        }
                    }
                }
            }
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
            if object_id('[SYSTEM.SECURITY.USER]', 'U') is null begin
	            CREATE TABLE [dbo].[SYSTEM.SECURITY.USER] (
		            [GUID_RECORD] [uniqueidentifier] NOT NULL,
		            [KEY] [nvarchar](50) NOT NULL,
		            [LAST_LOGIN] [smalldatetime] NOT NULL,
		            [LOGIN_ATTEMPT_COUNT] [smallint] NOT NULL,
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
