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
    
    public partial class isp_WesternUnionHistList_Result
    {
        public long ItbId { get; set; }
        public string AgentCode { get; set; }
        public Nullable<System.DateTime> SentDate { get; set; }
        public string TransReference { get; set; }
        public Nullable<decimal> Spread { get; set; }
        public Nullable<decimal> SettleRate { get; set; }
        public Nullable<decimal> PayoutRate { get; set; }
        public string SentCurrencyCode { get; set; }
        public Nullable<decimal> SentPrincipal { get; set; }
        public Nullable<decimal> SentCharges { get; set; }
        public Nullable<decimal> SettledPrincipal { get; set; }
        public Nullable<decimal> SettledCharges { get; set; }
        public Nullable<decimal> SettledFx { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public string SentCountry { get; set; }
        public string LcySettleRate { get; set; }
        public string LcyPayoutRate { get; set; }
        public string LcyCurrencyCode { get; set; }
        public string LcySettledPrin { get; set; }
        public string LcyCharges { get; set; }
        public string LcySettledPrincipal { get; set; }
        public string LcySettledCharges { get; set; }
        public string lcySettledFx { get; set; }
        public Nullable<decimal> lcyTotal { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> ReconDate { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> DateMoved { get; set; }
    }
}
