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
    
    public partial class ConsortiumTransaction
    {
        public long ItbId { get; set; }
        public string Institution { get; set; }
        public string FormName { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public string BuyVouchersID { get; set; }
        public string MobileNo { get; set; }
        public string OriginatingBranch { get; set; }
        public string TellerName { get; set; }
        public string SerialNo { get; set; }
        public string PostedBy { get; set; }
        public string AcctNo { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> ReconDate { get; set; }
        public string MatchingStatus { get; set; }
        public string MatchingType { get; set; }
        public string OrigRefNo { get; set; }
        public Nullable<int> DismissedBy { get; set; }
        public Nullable<System.DateTime> DismissedDate { get; set; }
        public Nullable<System.DateTime> PullDate { get; set; }
        public Nullable<System.DateTime> ReinstatedDate { get; set; }
        public Nullable<int> ReinstatedBy { get; set; }
    }
}
