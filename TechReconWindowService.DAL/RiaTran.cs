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
    
    public partial class RiaTran
    {
        public long ItbId { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public string TransactionNo { get; set; }
        public string TransPin { get; set; }
        public Nullable<decimal> PayAbleAmount { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> CommissionUSD { get; set; }
        public string BranchName { get; set; }
        public string BranchNo { get; set; }
        public string PaidBy { get; set; }
        public Nullable<System.DateTime> EnteredTime { get; set; }
        public Nullable<System.DateTime> PaidTime { get; set; }
        public string PaidCurrency { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<decimal> Commission_2 { get; set; }
        public Nullable<decimal> FxShare { get; set; }
        public Nullable<decimal> Commission_2AndFxShare { get; set; }
        public string BeneficiaryFirstName { get; set; }
        public string BeneficiaryLastName1 { get; set; }
        public string BeneficiaryLastName2 { get; set; }
        public string BeneficiaryIdType { get; set; }
        public string BeneficiaryIdNo { get; set; }
        public Nullable<decimal> PaymentAmountUSD { get; set; }
        public Nullable<System.DateTime> ReconDate { get; set; }
        public string MatchingStatus { get; set; }
        public Nullable<int> UserId { get; set; }
        public string MatchingType { get; set; }
        public string FileName { get; set; }
        public string OrigRefNo { get; set; }
        public Nullable<System.DateTime> DismissedDate { get; set; }
        public Nullable<int> DismissedBy { get; set; }
        public Nullable<System.DateTime> PullDate { get; set; }
        public Nullable<System.DateTime> ReinstatedDate { get; set; }
        public Nullable<int> ReinstatedBy { get; set; }
    }
}
