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
    
    public partial class proCoreBankingTran
    {
        public int CoreBankingId { get; set; }
        public string TerminalId { get; set; }
        public string Stan { get; set; }
        public string Pan { get; set; }
        public string AcctNo { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string TransType { get; set; }
        public string TransCategory { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        public string ReversalIndicator { get; set; }
        public string UniqueField { get; set; }
        public string TransCompleted { get; set; }
    }
}
