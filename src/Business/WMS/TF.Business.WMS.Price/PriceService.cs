using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF.Business.WMS
{
    public class PriceService : IPriceService
    {
        private readonly string _connectionString;

        public void Create(Price price)
        {
            if (price == null)
                throw new ArgumentNullException("price");

            if (price.Id == Guid.Empty)
                price.Id = Guid.NewGuid();

            if (price.ProductId == Guid.Empty)
                throw new ArgumentNullException("ProductId");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    /// Check id
                    command.CommandText = string.Format("select 1 from [BUSINESS.WMS.PRICE] where [GUID_RECORD] = '{0}'", price.Id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            throw new Exception("Record already exists");
                    }

                    /// Check product id
                    command.CommandText = string.Format("select 1 from [BUSINESS.WMS.PRICE] where [ITEM_GUID] = '{0}'", price.ProductId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            throw new Exception("Record already exists");
                    }

                    command.CommandText = @"insert into [BUSINESS.WMS.PRICE] (
	                    [GUID_RECORD],
	                    [ITEM_GUID],
	                    [CURRENCY_GUID],
	                    [LOCATION_GUID],
	                    [PRICE],
	                    [BATCH_GUID],
	                    [HIDDEN],
	                    [DELETED]) 
                    values (@GUID_RECORD, @ITEM_GUID, @CURRENCY_GUID, @LOCATION_GUID, @PRICE, @BATCH_GUID, @HIDDEN, @DELETED)";

                    command.Parameters.AddWithValue("@GUID_RECORD", price.Id);
                    command.Parameters.AddWithValue("@ITEM_GUID", price.ProductId);
                    command.Parameters.AddWithValue("@CURRENCY_GUID", DBNull.Value);
                    command.Parameters.AddWithValue("@LOCATION_GUID", DBNull.Value);
                    command.Parameters.AddWithValue("@PRICE", price.ProductPrice);
                    command.Parameters.AddWithValue("@BATCH_GUID", DBNull.Value);
                    command.Parameters.AddWithValue("@HIDDEN", 0);
                    command.Parameters.AddWithValue("@DELETED", 0);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Price price)
        {
            if (price == null)
                throw new ArgumentNullException("price");

            if (price.Id == Guid.Empty)
                throw new ArgumentNullException("Id");

            if (price.ProductId == Guid.Empty)
                throw new ArgumentNullException("ProductId");

            var record = GetById(price.Id);

            var alterColumns = new List<string>();
            var alterValues = new List<SqlParameter>();

            if (!Guid.Equals(record.ProductId, price.ProductId))
            {
                alterColumns.Add("[ITEM_GUID] = @ITEM_GUID");
                alterValues.Add(new SqlParameter("@ITEM_GUID", price.ProductId));
            }

            if (!Guid.Equals(record.ProductPrice, price.ProductPrice))
            {
                alterColumns.Add("[PRICE] = @PRICE");
                alterValues.Add(new SqlParameter("@PRICE", price.ProductPrice));
            }

            if (alterColumns.Any())
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = String.Format(
                            "update [BUSINESS.WMS.PRICE] set {0} where [GUID_RECORD] = @GUID_RECORD",
                            String.Join(", ", alterColumns));

                        command.Parameters.AddWithValue("@GUID_RECORD", price.Id);
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
                    command.CommandText = "delete from [BUSINESS.WMS.PRICE] where [GUID_RECORD] = @GUID_RECORD";
                    command.Parameters.AddWithValue("@GUID_RECORD", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteByProduct(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "delete from [BUSINESS.WMS.PRICE] where [ITEM_GUID] = @ITEM_GUID";
                    command.Parameters.AddWithValue("@ITEM_GUID", id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Price> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from [BUSINESS.WMS.PRICE]";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return ConvertToPrice(reader);
                        }
                    }
                }
            }
        }

        public Price GetByProductId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from [BUSINESS.WMS.PRICE] where [ITEM_GUID] = @ITEM_GUID";
                    command.Parameters.AddWithValue("@ITEM_GUID", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return ConvertToPrice(reader);
                        }
                        else
                        {
                            throw new Exception("Record does't exists");
                        }
                    }
                }
            }
        }

        public Price GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select * from [BUSINESS.WMS.PRICE] where [GUID_RECORD] = @GUID_RECORD";
                    command.Parameters.AddWithValue("@GUID_RECORD", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            return ConvertToPrice(reader);
                        }
                        else
                        {
                            throw new Exception("Record does't exists");
                        }
                    }
                }
            }
        }

        private Price ConvertToPrice(IDataReader reader)
        {
            return new Price
            {
                Id = reader.GetGuid(reader.GetOrdinal("GUID_RECORD")),
                ProductId = reader.GetGuid(reader.GetOrdinal("ITEM_GUID")),
                ProductPrice = reader.GetDouble(reader.GetOrdinal("PRICE")),
            };
        }

        public PriceService()
        {
            _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PriceDb"].ConnectionString;
        }

        public PriceService(string connectionString)
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
            if object_id('[BUSINESS.WMS.PRICE]', 'U') is null begin
                CREATE TABLE [dbo].[BUSINESS.WMS.PRICE](
	                [GUID_RECORD] [uniqueidentifier] NOT NULL,
	                [ITEM_GUID] [uniqueidentifier] NOT NULL,
	                [CURRENCY_GUID] [uniqueidentifier],
	                [LOCATION_GUID] [uniqueidentifier],					-- NULL means that price applying for all locations
	                [PRICE] FLOAT NOT NULL,
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
