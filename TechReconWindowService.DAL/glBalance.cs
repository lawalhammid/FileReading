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
    
    public partial class glBalance
    {
        public int gl_id { get; set; }
        public Nullable<decimal> gl_balance { get; set; }
        public string currCode { get; set; }
        public string acctName { get; set; }
        public Nullable<System.DateTime> trans_date { get; set; }
        public string card_acceptor { get; set; }
    }
}
