using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReconWindowService
{
   public class MT940ApplicationHeaderBlock
    {
        public string InputDirection { get; set; }
        public string MessageType { get; set; }
        public string ReceiversAddress  { get; set; }
        public string MessagePriority  { get; set; }
        public string DeliveryMonitoring  { get; set; } 
        public string ObsolescencePeriod   { get; set; } 
        public string InputTime   { get; set; } 
        public string MIRWithSendersAddress   { get; set; } 
        public string OutputDate  { get; set; }
        public string MT940SenderSwiftCode { get; set; } 
    }
}
