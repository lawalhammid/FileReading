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
    
    public partial class admDBAudit
    {
        public System.Guid auditlogid { get; set; }
        public string userid { get; set; }
        public System.DateTime eventdateutc { get; set; }
        public string eventtype { get; set; }
        public string tablename { get; set; }
        public string recordid { get; set; }
        public string columnname { get; set; }
        public string originalvalue { get; set; }
        public string newvalue { get; set; }
    }
}
