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
    
    public partial class proGLBalance
    {
        public int GLId { get; set; }
        public string AcctNo { get; set; }
        public Nullable<decimal> GlBalance { get; set; }
        public string CurrencyCode { get; set; }
        public string AcctName { get; set; }
        public Nullable<System.DateTime> TransDate { get; set; }
        public string CardAcceptor { get; set; }
    }
}
