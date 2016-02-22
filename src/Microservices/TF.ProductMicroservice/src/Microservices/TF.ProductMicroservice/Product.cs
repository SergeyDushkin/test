using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TF.ProductMicroservice
{
    /*
    CREATE TABLE [dbo].[BUSINESS.WMS.PRODUCT_N_SERVICE](
	    [GUID_RECORD] [uniqueidentifier] NOT NULL,
	    [TYPE] [nvarchar](20) NOT NULL,						-- fixed values: REGULAR, KIT, DYNAKIT, NONSTOCK, INGREDIENT
	    [KEY] [nvarchar](50) NOT NULL,
	    [NAME] [nvarchar](200),
	    [UOM_GUID] [uniqueidentifier] NOT NULL,
	    [ALLOW_NEGATIVE_QTY] BIT NOT NULL,					-- ALLOW NEGATIVE ISSUES
	    [BATCH_GUID] [uniqueidentifier],
	    [HIDDEN] [bit] NOT NULL,
	    [DELETED] [bit] NOT NULL
    ) ON [PRIMARY]
    */

    [Table("[PRODUCT_N_SERVICE]")]
    public class Product
    {

        public static List<ProductMessage> CreateMessages(IEnumerable<Product> products)
        {
            List<ProductMessage> message = new List<ProductMessage>();
            foreach (var product in products)
            {
                message.Add(new ProductMessage(product));
            }
            return message;
        }

        public Product()
        {
            this.ChildProducts = new List<Product>();
        }

        /// <summary>
        /// Product Identity
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("GUID_RECORD")] 
        public Guid Id { get; set; }

        /// <summary>
        /// REGULAR, KIT, DYNAKIT, NONSTOCK, INGREDIENT
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        [Column("TYPE")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "name")]
        [Column("NAME")]
        public string Name { get; set; }

        public virtual Product Parent { get; set; }

        [JsonProperty(PropertyName = "children")]
        public ICollection<Product> ChildProducts { get; set; }

        public ProductMessage GetMessage() 
        {
            return new ProductMessage(this);
        }
    }
}
