using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReconWindowService
{
    public class MTBasicHeaderBlock
    {
        public string Bic { get; set; }
        public string SessionNumber { get; set; }
        public string SequenceNumber { get; set; }
        public string ServiceID { get; set; }
        public string ApplicationID { get; set; }

        public string MT940ReceiverSwiftCode { get; set; }

    }
}
