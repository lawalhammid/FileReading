//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TechReconWindowService.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class admBank
    {
        public long ItbId { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string CBNCode { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Createdate { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> AuthId { get; set; }
        public string AccountTrimFormat { get; set; }
    }
}
