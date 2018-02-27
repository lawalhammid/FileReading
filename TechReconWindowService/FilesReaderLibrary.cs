using System.Drawing;
using System.Configuration;
using System.Reflection;
using System.Globalization;
using Sybase.Data.AseClient;
using Excel;
using System.Data.OleDb;
//using Microsoft.VisualBasic.FileIO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using iTextSharp.text;
//using TechReconWindowService.Models;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using TechReconWindowService.DAL.Interfaces;
using TechReconWindowService.DAL.Implementation;
using TechReconWindowService.Repository.Repositories;
using System.Diagnostics;
using TechReconWindowService.DAL;
using System.Threading.Tasks;
using Models;
using System.Collections.Generic;
using JournalProcessingLIBGH;
using System.IO;
using System.Text;

namespace TechReconWindowService
{
    public class FilesReaderLibrary
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmReconDataSourcesRepository repoadmReconDataSourcesRepository;
        private readonly IadmSourceAccountRepository repoadmSourceAccountRepository;
        private readonly ICBSTransationTestRepository repoCBSTransationTestRepository;
        private readonly IPostillionTransationTestRepository repoPostillionTransationTestRepository;
        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWindowService/JournalReadingLibrary.cs";
        public FilesReaderLibrary()
        {

            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmReconDataSourcesRepository = new admReconDataSourcesRepository(idbfactory);
            repoadmSourceAccountRepository = new admSourceAccountRepository(idbfactory);
            repoCBSTransationTestRepository = new CBSTransationTestRepository(idbfactory);
            repoPostillionTransationTestRepository = new PostillionTransationTestRepository(idbfactory);

        }


        //public async Task<int> ReadATMJournal()
        //{

        //    string ip = System.Configuration.ConfigurationManager.AppSettings["ipAddressJournal"].ToString();
        //    //string file = @"C:\Users\Opeyemi\Documents\Projects\TechRecon\ZenithBank Ghana\Journal Files\By Yemi Mr Tola\20170520.jrn";// full path to the file;
        //    string file = @"C:\Users\Opeyemi\Documents\Projects\TechRecon\ZenithBank Ghana\Journal Files\By E Bed Ghana\20170825_JOURNAL.jrn";// full path to the file;
        //    string brandName = "WINCOR";
        //    int brandid = 1;
        //    //C:\Users\Opeyemi\Documents\Projects\TechRecon\ZenithBank Ghana\Settlement Files
        //    ParseJournalFile gh = new ParseJournalFile();

        //    int x = gh.ProcessJournal(file, "", ip, ip, brandid, brandName);

        //    List<p_transaction> tranx = gh.ptranxs;

        //    //var journalTBL = new journal_log();
        //    foreach (var b in tranx)
        //    {
        //        //    journalTBL.terminalId = b.terminalId;
        //        //    journalTBL.atmip = b.atmip;
        //        //    journalTBL.brand = b.brand;
        //        //    journalTBL.trxn_date = b.trxn_date;
        //        //    journalTBL.trxn_time = b.trxn_time;
        //        //    journalTBL.tsn = b.tsn;
        //        //    journalTBL.pan = b.pan;
        //        //    journalTBL.eventType_id = b.eventType_id;
        //        //    journalTBL.currencyCode = b.currencyCode;
        //        //    journalTBL.amount = b.amount;
        //        //    journalTBL.availBal = b.availBal;
        //        //    journalTBL.ledger = b.ledger;
        //        //    journalTBL.surcharge = b.surcharge;
        //        //    journalTBL.accountfrm = b.accountfrm;
        //        //    journalTBL.accountTo = b.accountTo;
        //        //    journalTBL.trxn_status = b.trxn_status;
        //        //    journalTBL.comments = b.comments;
        //        //    journalTBL.detail = b.detail;
        //        //    journalTBL.cassette = b.cassette;
        //        //    journalTBL.errcode = b.errcode;
        //        //    journalTBL.rejectcount = b.rejectcount;
        //        //    journalTBL.dispensecount = b.dispensecount;
        //        //    journalTBL.requestcount = b.requestcount;
        //        //    journalTBL.remaincount = b.remaincount;
        //        //    journalTBL.pickupcount = b.pickupcount;
        //        //    journalTBL.denomination = b.denomination;
        //        //    journalTBL.jrn_filename = b.jrn_filename;
        //        //    journalTBL.mstate_id = b.mstate_id;
        //        //    journalTBL.device_stateId = b.device_stateId;
        //        //    journalTBL.file_down_load_id = b.file_down_load_id;
        //        //    journalTBL.TransTimeRange = b.TransTimeRange;
        //        //    journalTBL.opCode = b.opCode;
        //        //    journalTBL.takenAmount = b.takenAmount;

        //        //    techReconContext.journal_log.Add(journalTBL);

        //        //    int retValue = techReconContext.SaveChanges();

        //    }


        //    return 1;
        //}



        //public async Task<int> ReadtxtFile()
        //{
        //    string filepath = @"C:\Users\Opeyemi\Documents\Projects\TechRecon\ZenithBank Ghana\Settlement Files\230817_EMP.txt";
        //    // string text = File.ReadAllText(filepath, Encoding.UTF8);



        //    //string text1 = string.Empty;
        //    //var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        //    //using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        //    //{l
        //    //    text1 = streamReader.ReadToEnd();
        //    //}


        //    // string[] lines = File.ReadAllLines(filepath, Encoding.UTF8);

        //    string[] lines;
        //    var list = new List<string>();
        //    var fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        //    using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        //    {
        //        string line;
        //        while ((line = streamReader.ReadLine()) != null)
        //        {
        //            list.Add(line);
        //        }
        //    }
        //    lines = list.ToArray();

        //    return 1;
        //}




    }
}