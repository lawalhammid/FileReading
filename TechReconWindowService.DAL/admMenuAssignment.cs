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
    
    public partial class admMenuAssignment
    {
        public int ItbId { get; set; }
        public Nullable<int> MenuId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Status { get; set; }
        public Nullable<bool> Can_View { get; set; }
        public Nullable<bool> Can_Insert { get; set; }
        public Nullable<bool> Can_Delete { get; set; }
        public Nullable<bool> Can_Update { get; set; }
    }
}
