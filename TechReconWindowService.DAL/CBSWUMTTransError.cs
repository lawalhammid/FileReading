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
    
    public partial class CBSWUMTTransError
    {
        public long ItbId { get; set; }
        public string AcctNo { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string DrCr { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public string OriginBranch { get; set; }
        public string Postedby { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string Reference { get; set; }
        public string PtId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string ErrorMsg { get; set; }
        public string Status { get; set; }
        public Nullable<int> ReversalCode { get; set; }
        public string OrigRefNo { get; set; }
        public Nullable<System.DateTime> DismissedDate { get; set; }
        public Nullable<int> DismissedBy { get; set; }
        public Nullable<System.DateTime> ReconDate { get; set; }
        public Nullable<System.DateTime> PullDate { get; set; }
        public Nullable<System.DateTime> ReinstatedDate { get; set; }
        public Nullable<int> ReinstatedBy { get; set; }
    }
}
