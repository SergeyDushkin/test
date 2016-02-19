using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF.Business
{

    public class PERSON
    {
        public Guid GUID_RECORD { get; set; }
        public string FIRSTNAME { get; set; }    
        public string LASTNAME { get; set; }    
        public string MIDNAME { get; set; }      
        public DateTime? BIRTHDATE { get; set; } 
        public Guid? USER_GUID { get; set; }
        
        public Guid? BATCH_GUID { get; set; }
        public Boolean HIDDEN { get; set; }
        public Boolean DELETED { get; set; }
    }
}
