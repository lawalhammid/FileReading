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
    
    public partial class isp_XpressMoneyTransHist_Result
    {
        public long ItbId { get; set; }
        public Nullable<System.DateTime> PayoutDate { get; set; }
        public string Xpin { get; set; }
        public string RcvAgentTxnRefNo { get; set; }
        public Nullable<decimal> Commission { get; set; }
        public Nullable<decimal> PayoutAmount { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> AmountInUSD { get; set; }
        public Nullable<int> NoofTxn { get; set; }
        public Nullable<System.DateTime> ReconDate { get; set; }
        public string MatchingStatus { get; set; }
        public Nullable<int> UserId { get; set; }
    }
}
