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
    
    public partial class admReconType
    {
        public int ReconTypeId { get; set; }
        public string ReconName { get; set; }
        public int MatchingTypeId { get; set; }
        public Nullable<int> MachingCriteriaId { get; set; }
        public int Level { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Status { get; set; }
        public string WSReconName { get; set; }
        public string FileNamingConvention { get; set; }
        public string RejectedFileDirectory { get; set; }
        public string ProcessedFileDirectory { get; set; }
        public string Source1Script { get; set; }
        public string Source2Script { get; set; }
    }
}