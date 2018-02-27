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
    
    public partial class CompanyProfile
    {
        public int IbtId { get; set; }
        public string AccountNo { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<short> SessionIdleTime { get; set; }
        public Nullable<short> LoginAttempts { get; set; }
        public string BankingSystem { get; set; }
        public string Status { get; set; }
        public string EmailAddress { get; set; }
        public string AllowAuth { get; set; }
        public string ImageCss { get; set; }
        public string VerifierMailFormat { get; set; }
        public string ApproverMailFormat { get; set; }
        public Nullable<int> BankUserId { get; set; }
        public Nullable<int> BankAuthId { get; set; }
        public string AccountStatus { get; set; }
    }
}
