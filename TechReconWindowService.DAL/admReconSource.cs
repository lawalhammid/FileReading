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
    
    public partial class admReconSource
    {
        public int ReconSId { get; set; }
        public Nullable<int> ReconTypeId { get; set; }
        public string DataSource1 { get; set; }
        public string DataSource2 { get; set; }
        public string AccountNoDataSource1 { get; set; }
        public string AccountNoDataSource2 { get; set; }
        public string AccountTypeDataSource1 { get; set; }
        public string AccountTypeDataSource2 { get; set; }
        public string ObjectType1 { get; set; }
        public string ObjectType2 { get; set; }
        public string TableName1 { get; set; }
        public string TableName2 { get; set; }
        public string FileDirectory1 { get; set; }
        public string FileDirectory2 { get; set; }
        public string ExecutionType1 { get; set; }
        public string ExecutionType2 { get; set; }
        public string FileFormat1 { get; set; }
        public string FileFormat2 { get; set; }
        public string Status { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<int> ProcessBatchRef { get; set; }
        public Nullable<System.DateTime> ProcessBatchDate { get; set; }
        public string ProcessBatchStatus { get; set; }
    }
}
