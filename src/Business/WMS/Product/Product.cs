﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF.Business.WMS
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

    public class Product
    {
        /// <summary>
        /// Product Identity
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// REGULAR, KIT, DYNAKIT, NONSTOCK, INGREDIENT
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Product> Childs { get; set; }
    }
}
