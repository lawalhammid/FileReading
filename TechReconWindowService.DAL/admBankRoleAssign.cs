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
    
    public partial class admBankRoleAssign
    {
        public decimal AssignId { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<int> MenuId { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Status { get; set; }
        public Nullable<int> CanView { get; set; }
        public Nullable<int> CanAdd { get; set; }
        public Nullable<int> CanEdit { get; set; }
        public Nullable<int> CanDelete { get; set; }
        public Nullable<int> CanAuth { get; set; }
        public Nullable<int> BankUserId { get; set; }
    }
}
