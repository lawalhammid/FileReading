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
    
    public partial class Journallog
    {
        public long ItbId { get; set; }
        public string terminalId { get; set; }
        public string atmip { get; set; }
        public string brand { get; set; }
        public Nullable<System.DateTime> trxn_date { get; set; }
        public string trxn_time { get; set; }
        public string tsn { get; set; }
        public string pan { get; set; }
        public Nullable<double> eventType_id { get; set; }
        public string currencyCode { get; set; }
        public Nullable<double> amount { get; set; }
        public Nullable<double> availBal { get; set; }
        public Nullable<double> ledger { get; set; }
        public Nullable<double> surcharge { get; set; }
        public string accountfrm { get; set; }
        public string accountTo { get; set; }
        public string trxn_status { get; set; }
        public string comments { get; set; }
        public string detail { get; set; }
        public string cassette { get; set; }
        public string errcode { get; set; }
        public string rejectcount { get; set; }
        public string dispensecount { get; set; }
        public string requestcount { get; set; }
        public string remaincount { get; set; }
        public string pickupcount { get; set; }
        public string denomination { get; set; }
        public string jrn_filename { get; set; }
        public Nullable<double> mstate_id { get; set; }
        public Nullable<double> device_stateId { get; set; }
        public Nullable<double> file_down_load_id { get; set; }
        public string TransTimeRange { get; set; }
        public string opCode { get; set; }
        public string takenAmount { get; set; }
        public string MoveStatus { get; set; }
    }
}