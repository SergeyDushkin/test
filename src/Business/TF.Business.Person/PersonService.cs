using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF.Business
{
    public class PersonService : IPersonService
    {
        private readonly string _connectionString;

        public void Create(PERSON Person)
        {

            if (Person == null)
                throw new ArgumentNullException("category");

            if (Person.GUID_RECORD == Guid.Empty)
            {
                Person.GUID_RECORD = Guid.NewGuid();
            }

            if (string.IsNullOrEmpty(Person.FIRSTNAME))
            {
                throw new ArgumentNullException("Firstname");
            }

            if (string.IsNullOrEmpty(Person.LASTNAME))
            {
                throw new ArgumentNullException("Lastname");
            }

         

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        /// Check id
                        command.CommandText = string.Format("select 1 from [BUSINESS.PERSON] where [GUID_RECORD] = '{0}'", Person.GUID_RECORD);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                throw new Exception("Record already exists");
                        }

                        command.CommandText = @"insert into [BUSINESS.PERSON] (
                        [GUID_RECORD], 
                        [FIRSTNAME], 
                        [LASTNAME], 
                        [MIDNAME], 
                        [BIRTHDATE], 
                        [USER_GUID], 
                        [BATCH_GUID], 
                        [HIDDEN], 
                        [DELETED]) 
                        values (@GUID_RECORD, @FIRSTNAME, @LASTNAME, @MIDNAME, @BIRTHDATE, @USER_GUID, @BATCH_GUID, @HIDDEN, @DELETED)";

                        command.Parameters.AddWithValue("@GUID_RECORD", Person.GUID_RECORD);
                        command.Parameters.AddWithValue("@FIRSTNAME", Person.FIRSTNAME);
                        command.Parameters.AddWithValue("@LASTNAME", Person.LASTNAME);
                        command.Parameters.AddWithValue("@MIDNAME", string.IsNullOrEmpty(Person.MIDNAME)? string.Empty : Person.MIDNAME );
                        command.Parameters.AddWithValue("@BIRTHDATE", Person.BIRTHDATE.HasValue? Person.BIRTHDATE.Value : (object)DBNull.Value );
                        command.Parameters.AddWithValue("@USER_GUID", DBNull.Value);
                        command.Parameters.AddWithValue("@BATCH_GUID", DBNull.Value);
                        command.Parameters.AddWithValue("@HIDDEN", 0);
                        command.Parameters.AddWithValue("@DELETED", 0);


                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
                catch (Exception e)
                {
                    throw new Exception("Exception");
                }
                finally
                {
                    connection.Close();
                }
            }
        }               
       
        public void Update(PERSON Person)
        {
            if (Person == null)
                throw new ArgumentNullException("category");

            if (Person.GUID_RECORD == Guid.Empty)
                throw new ArgumentNullException("GUID_RECORD");

            if (string.IsNullOrEmpty(Person.FIRSTNAME))
                throw new ArgumentNullException("Firstname");

            if (string.IsNullOrEmpty(Person.LASTNAME))
                throw new ArgumentNullException("Lastname");

            var record = GetById(Person.GUID_RECORD);

            var alterColumns = new List<string>();
            var alterValues = new List<SqlParameter>();

            if (!String.Equals(Person.LASTNAME, record.LASTNAME))
            {
                alterColumns.Add("[LASTNAME] = @LASTNAME");
                alterValues.Add(new SqlParameter("@LASTNAME", Person.LASTNAME));
            }

            if (!String.Equals(Person.FIRSTNAME, record.FIRSTNAME))
            {
                alterColumns.Add("[FIRSTNAME] = @FIRSTNAME");
                alterValues.Add(new SqlParameter("@FIRSTNAME", Person.FIRSTNAME));
            }

            Person.MIDNAME = string.IsNullOrEmpty(Person.MIDNAME) ? string.Empty : Person.MIDNAME;
            if (!String.Equals(Person.MIDNAME.ToString(), record.MIDNAME))
            {
                alterColumns.Add("[MIDNAME] = @MIDNAME");
                alterValues.Add(new SqlParameter("@MIDNAME", Person.MIDNAME));
            }
            if (!String.Equals(Person.BIRTHDATE, record.BIRTHDATE))
            {
                alterColumns.Add("[BIRTHDATE] = @BIRTHDATE");
                alterValues.Add(new SqlParameter("@BIRTHDATE", Person.BIRTHDATE));
            }
            if (!String.Equals(Person.USER_GUID, record.USER_GUID))
            {
                alterColumns.Add("[USER_GUID] = @USER_GUID");
                alterValues.Add(new SqlParameter("@USER_GUID", Person.USER_GUID));
            }
            if (!String.Equals(Person.HIDDEN, record.HIDDEN))
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (var command = connection.CreateCommand())
                        {
                            /// Checking user
                            command.CommandText = string.Format("select 1 from [SYSTEM.SECURITY.USER] where [GUID_RECORD] = '{0}'", Person.USER_GUID);
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    throw new Exception("user is");
                                }
                                else
                                {
                                    /// Check for Person user
                                    command.CommandText = string.Format("select 1 from [BUSINESS.PERSON] where [USER_GUID] = '{0}'", Person.USER_GUID);
                                    using (var readerUser = command.ExecuteReader())
                                    {
                                        if (reader.HasRows)
                                        {
                                            throw new Exception("Recording for the user already exists");
                                        }
                                        else
                                        {
                                            alterColumns.Add("[HIDDEN] = @HIDDEN");
                                            alterValues.Add(new SqlParameter("@HIDDEN", Person.HIDDEN));
                                        }

                                    }
                                }
                            }
                        }
                        connection.Close();
                    }
                    catch (Exception e)
                    { }
                    finally
                    {
                        connection.Close();
                    }
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
                            "update [BUSINESS.PERSON] set {0} where [GUID_RECORD] = @GUID_RECORD",
                            String.Join(", ", alterColumns));

                        command.Parameters.AddWithValue("@GUID_RECORD", Person.GUID_RECORD);
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
                    command.CommandText = "update[BUSINESS.PERSON] set [DELETED] = 1  where[GUID_RECORD] = @GUID_RECORD";
                    command.Parameters.AddWithValue("@GUID_RECORD", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public PERSON GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from [BUSINESS.PERSON] where [GUID_RECORD] = @GUID_RECORD";
                    command.Parameters.AddWithValue("@GUID_RECORD", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            return new PERSON
                            {
                                GUID_RECORD = reader.GetGuid(reader.GetOrdinal("GUID_RECORD")),
                                FIRSTNAME = reader.GetString(reader.GetOrdinal("FIRSTNAME")),
                                LASTNAME = reader.GetString(reader.GetOrdinal("LASTNAME")),
                                MIDNAME = reader.GetString(reader.GetOrdinal("MIDNAME")),
                                BIRTHDATE = reader.IsDBNull(reader.GetOrdinal("BIRTHDATE"))
                                    ? default(DateTime?)
                                    : reader.GetDateTime(reader.GetOrdinal("BIRTHDATE")),
                                USER_GUID = reader.IsDBNull(reader.GetOrdinal("USER_GUID"))
                                    ? default(Guid?)
                                    : reader.GetGuid(reader.GetOrdinal("USER_GUID")),
                                BATCH_GUID = reader.IsDBNull(reader.GetOrdinal("BATCH_GUID"))
                                    ? default(Guid?)
                                    : reader.GetGuid(reader.GetOrdinal("BATCH_GUID")),
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

        public IEnumerable<PERSON> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from [BUSINESS.PERSON]";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new PERSON
                            {
                                GUID_RECORD = reader.GetGuid(reader.GetOrdinal("GUID_RECORD")),
                                FIRSTNAME = reader.GetString(reader.GetOrdinal("FIRSTNAME")),
                                LASTNAME = reader.GetString(reader.GetOrdinal("LASTNAME")),
                                MIDNAME = reader.GetString(reader.GetOrdinal("MIDNAME")),
                                BIRTHDATE = reader.IsDBNull(reader.GetOrdinal("BIRTHDATE"))
                                    ? default(DateTime?)
                                    : reader.GetDateTime(reader.GetOrdinal("BIRTHDATE")),
                                USER_GUID = reader.IsDBNull(reader.GetOrdinal("USER_GUID"))
                                    ? default(Guid?)
                                    : reader.GetGuid(reader.GetOrdinal("USER_GUID")),
                                BATCH_GUID = reader.IsDBNull(reader.GetOrdinal("BATCH_GUID"))
                                    ? default(Guid?)
                                    : reader.GetGuid(reader.GetOrdinal("BATCH_GUID")),
                                HIDDEN = reader.GetBoolean(reader.GetOrdinal("HIDDEN")),
                                DELETED = reader.GetBoolean(reader.GetOrdinal("DELETED"))
                            };
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

        public PersonService()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PersonDb"].ConnectionString;
        }

        public PersonService(string connectionString)
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
            if object_id('[BUSINESS.PERSON]', 'U') is null begin
	            CREATE TABLE [dbo].[BUSINESS.PERSON](
	                [GUID_RECORD] [uniqueidentifier] NOT NULL,
	                [FIRSTNAME] [nvarchar](50) NOT NULL,
	                [LASTNAME] [nvarchar](50) NOT NULL,
	                [MIDNAME] [nvarchar](50) NULL,
	                [BIRTHDATE] [smalldatetime] NULL,
	                [USER_GUID] [uniqueidentifier] NULL,
	                [BATCH_GUID] [uniqueidentifier] NULL,
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
