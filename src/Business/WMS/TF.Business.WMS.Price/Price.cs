using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF.Business.WMS
{
    /*
    CREATE TABLE [dbo].[BUSINESS.WMS.PRICE](
	    [GUID_RECORD] [uniqueidentifier] NOT NULL,
	    [ITEM_GUID] [uniqueidentifier] NOT NULL,
	    [CURRENCY_GUID] [uniqueidentifier] NOT NULL,
	    [LOCATION_GUID] [uniqueidentifier],					-- NULL means that price applying for all locations
	    [PRICE] FLOAT NOT NULL,
	    [BATCH_GUID] [uniqueidentifier],
	    [HIDDEN] [bit] NOT NULL,
	    [DELETED] [bit] NOT NULL
    ) ON [PRIMARY]
    */

    /// <summary>
    /// Price for product
    /// </summary>
    public class Price
    {
        /// <summary>
        /// Price identity
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Product identity
        /// </summary>
        public Guid ProductId { get; set; }
        
        /// <summary>
        /// Product price
        /// </summary>
        public double ProductPrice { get; set; }
    }
}
