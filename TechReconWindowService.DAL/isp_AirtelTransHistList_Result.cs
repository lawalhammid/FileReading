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
    
    public partial class isp_AirtelTransHistList_Result
    {
        public long ItbId { get; set; }
        public string RecordNo { get; set; }
        public string TransactionId { get; set; }
        public Nullable<int> TransactionAmount { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public Nullable<decimal> FeeServiceChargeTax1 { get; set; }
        public Nullable<decimal> PayeePreBalance { get; set; }
        public string PayeeBankAcctNo { get; set; }
        public string PayerWallet { get; set; }
        public string MatchingStatus { get; set; }
        public Nullable<System.DateTime> ReconDate { get; set; }
        public string ReferenceNo { get; set; }
        public string PayerMFSProvider { get; set; }
        public string PayerPaymentInstrument { get; set; }
        public Nullable<decimal> FeeServiceChargeTax2 { get; set; }
        public string PayerBankAcctNo { get; set; }
        public string UserCategory { get; set; }
        public string UserGrade { get; set; }
        public string PayeeMFSProvider { get; set; }
        public string PayeePaymentInstrument { get; set; }
        public string PayeWalletType { get; set; }
        public string ReceiverCategory { get; set; }
        public string ReceiverGrade { get; set; }
        public string ServiceType { get; set; }
        public string TransactionType { get; set; }
        public Nullable<decimal> PayerPreviousBalance { get; set; }
        public Nullable<decimal> PayerPostBalance { get; set; }
        public Nullable<decimal> PayeePostBalance { get; set; }
        public string ExternalRefNo { get; set; }
        public Nullable<int> UserId { get; set; }
        public string MatchingType { get; set; }
        public Nullable<System.DateTime> DateMoved { get; set; }
    }
}