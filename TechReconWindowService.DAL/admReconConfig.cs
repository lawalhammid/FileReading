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
    
    public partial class admReconConfig
    {
        public int ItbId { get; set; }
        public string OracleDateFormat { get; set; }
        public Nullable<int> ScheduleHour { get; set; }
        public string SqlDateFormat { get; set; }
        public Nullable<int> DatasourceId { get; set; }
        public Nullable<System.DateTime> CurrentScheduleDatetime { get; set; }
        public Nullable<System.DateTime> NextScheduleDatetime { get; set; }
    }
}
