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
    
    public partial class admMatchingCriteria
    {
        public int ItbId { get; set; }
        public decimal MatchingTypeId { get; set; }
        public Nullable<int> ReconTypeId { get; set; }
        public Nullable<int> ItemNo { get; set; }
        public string Source1Table { get; set; }
        public string Source1ColumnName { get; set; }
        public string Source2Table { get; set; }
        public string Source2ColumnName { get; set; }
        public Nullable<bool> MachingBasisTable1 { get; set; }
        public Nullable<bool> MachingBasisTable2 { get; set; }
        public Nullable<bool> SumValueTable1 { get; set; }
        public Nullable<bool> SumValueTable2 { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Status { get; set; }
        public string ReconciliationBatchNo { get; set; }
        public Nullable<System.DateTime> ReconciliationStartTime { get; set; }
        public Nullable<System.DateTime> ReconciliationEndTime { get; set; }
        public string ReconcilledBy { get; set; }
        public string ReconciliationStatus { get; set; }
        public string Stage { get; set; }
    }
}