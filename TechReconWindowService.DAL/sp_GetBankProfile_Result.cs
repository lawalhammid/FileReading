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
    
    public partial class sp_GetBankProfile_Result
    {
        public int ItbId { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public Nullable<System.DateTime> CurrProcessingDate { get; set; }
        public Nullable<bool> EnableActiveDirectory { get; set; }
        public Nullable<bool> EnableEncriptedLogin { get; set; }
        public string Country { get; set; }
        public string BankingSystem { get; set; }
        public Nullable<short> AdminLoginAttempts { get; set; }
        public string Status { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<bool> RetentionPeriodForMatchedRecord { get; set; }
        public Nullable<int> AdminSessionTimeOutMinutes { get; set; }
        public string PasswordChangeType { get; set; }
        public Nullable<int> PasswordChangeValue { get; set; }
    }
}
