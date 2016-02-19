using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TF.SystemSecurity
{

    public class USER
    {
        public Guid GUID_RECORD { get; set; }
        public string KEY { get; set; }
        public DateTime LAST_LOGIN { get; set; }
        public Int16 LOGIN_ATTEMPT_COUNT { get; set; }
        public Guid? BATCH_GUID { get; set; }
        public Boolean HIDDEN { get; set; }
        public Boolean DELETED { get; set; }

        public string PROVIDER { get; set; }
        public string KEY_IDENTITY { get; set; }
    }

    public class USER_IDENTITY
    {
        public Guid USER_GUID { get; set; }
        public string PROVIDER { get; set; }
        public string KEY { get; set; }
        public Guid? BATCH_GUID { get; set; }
        public Boolean HIDDEN { get; set; }
        public Boolean DELETED { get; set; }
    }
}
