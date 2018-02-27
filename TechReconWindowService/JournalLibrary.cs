
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.ServiceProcess;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Threading;
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
//using org.apache.pdfbox.pdmodel;
//using org.apache.pdfbox.util;
using JournalProcessingLIBGH;
using Models;
using TechReconWindowService.BAL.Helpers;

namespace TechReconWindowService
{
    public class JournalLibrary
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmReconDataSourcesRepository repoadmReconDataSourcesRepository;
        private readonly IadmSourceAccountRepository repoadmSourceAccountRepository;
        private readonly IPostillionTransationTestRepository repoPostillionTransationTestRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly IConsortiumTransactionRepository repoConsortiumTransactionRepository;
        private readonly IadmTerminalRepository repoadmTerminalRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly IJournallogRepository repoJournallogRepository;
        private readonly IJournalTransErrorRepository repoJournalTransErrorRepository;
        private readonly IJournalTransRepository repoJournalTransRepository;
        private readonly IJournalTransHistoryRepository repoJournalTransHistoryRepository;
        //
        private ParseJournalFile gh;
        private static int cbsNostroRec;

        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/JournalLibrary.cs";
        private static string agentcodestatic;
        private static string AcctNoStatic;
        private static string LegacyStatic;
        private static string SettleCurrStatic;
        private static string TransCurStatic;
        private static string BranchNameStatic;

        public JournalLibrary()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmReconDataSourcesRepository = new admReconDataSourcesRepository(idbfactory);
            repoadmSourceAccountRepository = new admSourceAccountRepository(idbfactory);
            repoPostillionTransationTestRepository = new PostillionTransationTestRepository(idbfactory);
            repoConsortiumTransactionRepository = new ConsortiumTransactionRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoadmTerminalRepository = new admTerminalRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoJournallogRepository = new JournallogRepository(idbfactory);
            gh = new ParseJournalFile();
            repoJournalTransErrorRepository = new JournalTransErrorRepository(idbfactory);
            repoJournalTransRepository = new JournalTransRepository(idbfactory);
            repoJournalTransHistoryRepository = new JournalTransHistoryRepository(idbfactory);
        }

        public async Task<string> PullJournalData()
        {
            Library library = new Library();
            try
            {
                LogManager.SaveLog("PullDataMTN Start in JournalLibrary");
                var ReconType = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "ATM");
                int ReconTypeId =  ReconType.ReconTypeId;
                var AccountSources = await repoadmSourceAccountRepository.GetManyAsync(c => c.ReconTypeId == ReconTypeId);
                var listofAccountsource1 = AccountSources.Where(c => c.SourceName == "Source 1");
                string acctlistSource1 = string.Empty;
                string acctlistSource2 = string.Empty;
                int dt1 = 0;
                // int dt2 = 0;
                foreach (var li in listofAccountsource1)
                {
                    acctlistSource1 += "'" + li.AcctNo + "'" + ", ";
                    dt1 = (int)li.DataSourceId;
                }
                int index = acctlistSource1.LastIndexOf(',');
                if (!string.IsNullOrEmpty(acctlistSource1))
                {
                    acctlistSource1 = (acctlistSource1.Remove(index, 1));
                }
                var dtSouceCon1 = await repoadmReconDataSourcesRepository.GetAsync(c => c.ReconDSId == dt1);

                #region Source 2

                var dtSouceCon2 = await repoadmSourceAccountRepository.GetAsync(c => c.ReconTypeId == ReconTypeId && c.SourceName == "Source 2");

                #region //File Directory Below

                if (!string.IsNullOrWhiteSpace(dtSouceCon2.FileDirectory))
                {
                    try
                    {

                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == ReconType.ReconTypeId && c.TableName == "ATMTrans");

                        if (controlTable != null)
                        {
                            LastRecordId = controlTable.LastRecordId;
                        }
                        else
                        {
                            LastRecordId = "0";
                        }

                        string FileDirectory = dtSouceCon2.FileDirectory;

                        DataTable dTable = new DataTable();

                        DirectoryInfo d = new DirectoryInfo(FileDirectory);
                        List<FileInfo> DLIST = null;
                        DLIST = d.GetFiles("*" + ".jrn").ToList();
                        List<FileInfo> ggg = null;
                        if (controlTable != null)
                        {
                            ggg = DLIST;//.Where(c => c.LastAccessTime > controlTable.LastRecordTimeStamp).ToList();
                        }
                        else
                        {
                            ggg = DLIST;
                        }
                        string fileNamenAndType = string.Empty;
                        string FileLastTime = string.Empty;
                        int count = 0;
                        foreach (var kl in ggg)
                        {
                            fileNamenAndType = (from f in DLIST orderby f.LastWriteTime ascending select kl).First().Name;

                            FileLastTime = (from f in DLIST orderby f.LastAccessTimeUtc descending select kl).First().LastWriteTime.ToString();

                            string ipAddress = System.Configuration.ConfigurationManager.AppSettings["ipAddressJournal"].ToString();
                            string file = FileDirectory + "\\" + fileNamenAndType;
                            string brandName = "WINCOR";
                            int brandid = 1;
                            int x = gh.ProcessJournal(file, "", ipAddress, ipAddress, brandid, brandName);

                            List<p_transaction> tranx = gh.ptranxs;
                            var journal = new Journallog();
                            if (tranx.Count > 0)
                            {
                                var journalLogTBL = new Journallog();
                                foreach (var b in tranx)
                                {
                                    try
                                    {
                                        journalLogTBL.terminalId = b.terminalId;
                                        journalLogTBL.atmip = b.atmip;
                                        journalLogTBL.brand = b.brand;
                                        journalLogTBL.trxn_date = b.trxn_date;
                                        journalLogTBL.trxn_time = b.trxn_time;
                                        journalLogTBL.tsn = b.tsn;
                                        journalLogTBL.pan = b.pan;
                                        journalLogTBL.eventType_id = b.eventType_id;
                                        journalLogTBL.currencyCode = b.currencyCode;
                                        journalLogTBL.amount = b.amount;
                                        journalLogTBL.availBal = b.availBal;
                                        journalLogTBL.ledger = b.ledger;
                                        journalLogTBL.surcharge = b.surcharge;
                                        journalLogTBL.accountfrm = b.accountfrm;
                                        journalLogTBL.accountTo = b.accountTo;
                                        journalLogTBL.trxn_status = b.trxn_status;
                                        journalLogTBL.comments = b.comments;
                                        journalLogTBL.detail = b.detail;
                                        journalLogTBL.cassette = b.cassette;
                                        journalLogTBL.errcode = b.errcode;
                                        journalLogTBL.rejectcount = b.rejectcount;
                                        journalLogTBL.dispensecount = b.dispensecount;
                                        journalLogTBL.requestcount = b.requestcount;
                                        journalLogTBL.remaincount = b.remaincount;
                                        journalLogTBL.pickupcount = b.pickupcount;
                                        journalLogTBL.denomination = b.denomination;
                                        journalLogTBL.jrn_filename = fileNamenAndType;
                                        journalLogTBL.mstate_id = b.mstate_id;
                                        journalLogTBL.device_stateId = b.device_stateId;
                                        journalLogTBL.file_down_load_id = b.file_down_load_id;
                                        journalLogTBL.TransTimeRange = b.TransTimeRange;
                                        journalLogTBL.opCode = b.opCode;
                                        journalLogTBL.takenAmount = b.takenAmount;

                                        var exist = await repoJournallogRepository.GetAsync(c => c.tsn == journalLogTBL.tsn);
                                        if (exist != null)
                                        {
                                            //Error Inserted here
                                            continue;
                                        }
                                        else
                                        {
                                            repoJournallogRepository.Add(journalLogTBL);
                                            var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                            if (ret)
                                            {
                                                count += 1;
                                                continue;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                                        var stackTrace = new StackTrace(ex);
                                        var thisasm = Assembly.GetExecutingAssembly();
                                        _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                                        _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                        // library.SaveLog("An error occured in Journal in Line  :" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);


                                    }
                                }


                                //Move from Temporal Table to JournalTrans Table below
                                //var JourLogList = await repoJournallogRepository.GetManyAsync(c => c.MoveStatus != null);
                                string currcy = string.Empty;
                                var JourLogList = await repoJournallogRepository.GetManyAsync(c => c.ItbId > 0);
                                if (JourLogList.Count() > 0)
                                {
                                    var JournalTransTBL = new JournalTran();
                                    var JournalTBLErr = new JournalTransError();
                                    string appendtring = string.Empty;
                                    foreach (var b in JourLogList)
                                    {
                                        var gg = b.detail;
                                        if (gg != null)
                                        {
                                            var spldetails = gg.Split('\n');

                                            if (spldetails[0].Contains("-> TRANSACTION START"))
                                            {
                                                foreach (var L in spldetails)
                                                {
                                                    if (L.Contains("CURRENT"))
                                                    {
                                                        var g = string.Join(" ", L.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(w => w.Trim()));

                                                        var cur = g.Split(' ');
                                                        if (cur.Count() > 0)
                                                        {
                                                            currcy = cur[1].Substring(3);
                                                        }
                                                    }
                                                    if (L.Contains("AVAILABL"))
                                                    {
                                                        var g = string.Join(" ", L.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(w => w.Trim()));
                                                        string avail = g.Split(' ')[1];
                                                    }
                                                    appendtring += L + " ";
                                                    //I can be setting what will be entering the Table column from L
                                                    if (L.Contains("<- TRANSACTION END"))
                                                    {
                                                        //Insert into the DB the break below
                                                        break; //break as you find <- TRANSACTION END
                                                    }
                                                }
                                            }


                                            JournalTransTBL.TerminalId = b.terminalId;
                                            JournalTransTBL.ATMIp = b.atmip;
                                            JournalTransTBL.Brand = b.brand;
                                            JournalTransTBL.TransDate = b.trxn_date;
                                            JournalTransTBL.TransTime = b.trxn_time;
                                            JournalTransTBL.TransRef = b.tsn;
                                            JournalTransTBL.Pan = b.pan;
                                            JournalTransTBL.EventTypeId = b.eventType_id;
                                            JournalTransTBL.CurrencyCode = b.currencyCode;
                                            JournalTransTBL.Amount = b.amount;
                                            decimal decValue = 0;
                                            JournalTransTBL.AvailBal = b.availBal == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(b.availBal.ToString(), out decValue) ? decValue : Math.Round(Convert.ToDecimal("0"), 2);
                                            JournalTransTBL.Ledger = b.ledger == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(b.ledger.ToString(), out decValue) ? decValue : Math.Round(Convert.ToDecimal("0"), 2);
                                            JournalTransTBL.CurrentAmount = currcy == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(currcy, out decValue) ? decValue : Math.Round(Convert.ToDecimal("0"), 2);
                                            JournalTransTBL.Surcharge = b.surcharge;
                                            JournalTransTBL.AcctFrom = b.accountfrm;
                                            JournalTransTBL.AcctTo = b.accountTo;
                                            JournalTransTBL.TransStatus = b.trxn_status;
                                            JournalTransTBL.Cassette = b.cassette;
                                            JournalTransTBL.ErrorCode = b.errcode;
                                            JournalTransTBL.RejectCount = b.rejectcount;
                                            JournalTransTBL.DispenseCount = b.dispensecount;
                                            JournalTransTBL.RequestCount = b.requestcount;
                                            JournalTransTBL.RemainCount = b.remaincount;
                                            JournalTransTBL.PickUpCount = b.pickupcount;
                                            JournalTransTBL.Denomination = b.denomination;
                                            JournalTransTBL.JournalfileDirectory = fileNamenAndType;
                                            JournalTransTBL.MstateId = b.mstate_id;
                                            JournalTransTBL.DeviceStateId = b.device_stateId;
                                            JournalTransTBL.FileDownloadId = b.file_down_load_id;
                                            JournalTransTBL.TransTimeRange = b.TransTimeRange;
                                            JournalTransTBL.OpCode = b.opCode;
                                            JournalTransTBL.TakenAmount = b.takenAmount;
                                            JournalTransTBL.MatchingStatus = "N";
                                            JournalTransTBL.ReconDate = DateTime.Now;
                                            JournalTransTBL.UserId = 1;
                                            JournalTransTBL.OrigRefNo = b.tsn;

                                            if (!string.IsNullOrWhiteSpace(b.tsn))
                                            {
                                                var exist = await repoJournalTransRepository.GetAsync(c => c.TransRef == b.tsn);

                                                var existHis = await repoJournalTransHistoryRepository.GetAsync(c => c.TransRef == b.tsn);
                                                if (exist != null || existHis != null)
                                                {
                                                    JournalTBLErr.TerminalId = JournalTransTBL.TerminalId;
                                                    JournalTBLErr.ATMIp = JournalTransTBL.ATMIp;
                                                    JournalTBLErr.Brand = JournalTransTBL.Brand;
                                                    JournalTBLErr.TransDate = JournalTransTBL.TransDate;
                                                    JournalTBLErr.TransTime = JournalTransTBL.TransTime;
                                                    JournalTBLErr.TransRef = JournalTransTBL.TransRef;
                                                    JournalTBLErr.Pan = JournalTransTBL.Pan;
                                                    JournalTBLErr.EventTypeId = JournalTransTBL.EventTypeId;
                                                    JournalTBLErr.CurrencyCode = JournalTransTBL.CurrencyCode;
                                                    JournalTBLErr.Amount = JournalTransTBL.Amount;
                                                    JournalTBLErr.AvailBal = JournalTransTBL.AvailBal;
                                                    JournalTBLErr.Ledger = JournalTransTBL.Ledger;
                                                    JournalTBLErr.Surcharge = JournalTransTBL.Surcharge;
                                                    JournalTBLErr.AcctFrom = JournalTransTBL.AcctFrom;
                                                    JournalTBLErr.AcctTo = JournalTransTBL.AcctTo;
                                                    JournalTBLErr.TransStatus = JournalTransTBL.TransStatus;
                                                    JournalTBLErr.Cassette = JournalTransTBL.Cassette;
                                                    JournalTBLErr.ErrorCode = JournalTransTBL.ErrorCode;
                                                    JournalTBLErr.RejectCount = JournalTransTBL.RejectCount;
                                                    JournalTBLErr.DispenseCount = JournalTransTBL.DispenseCount;
                                                    JournalTBLErr.RequestCount = JournalTransTBL.RequestCount;
                                                    JournalTBLErr.RemainCount = JournalTransTBL.RemainCount;
                                                    JournalTBLErr.PickUpCount = JournalTransTBL.PickUpCount;
                                                    JournalTBLErr.Denomination = JournalTransTBL.Denomination;
                                                    JournalTBLErr.JournalfileDirectory = fileNamenAndType;
                                                    JournalTBLErr.MstateId = JournalTransTBL.MstateId;
                                                    JournalTBLErr.DeviceStateId = JournalTransTBL.DeviceStateId;
                                                    JournalTBLErr.FileDownloadId = JournalTransTBL.FileDownloadId;
                                                    JournalTBLErr.TransTimeRange = JournalTransTBL.TransTimeRange;
                                                    JournalTBLErr.OpCode = JournalTransTBL.OpCode;
                                                    JournalTBLErr.TakenAmount = JournalTransTBL.TakenAmount;
                                                    JournalTBLErr.MatchingStatus = JournalTransTBL.MatchingStatus;
                                                    JournalTBLErr.MatchingType = JournalTransTBL.MatchingType;
                                                    JournalTBLErr.ReconDate = JournalTransTBL.ReconDate;
                                                    JournalTBLErr.UserId = JournalTransTBL.UserId;
                                                    JournalTBLErr.ErrorMsg = "Duplicate transaction record";
                                                    JournalTBLErr.OrigRefNo = JournalTransTBL.OrigRefNo;

                                                    repoJournalTransErrorRepository.Add(JournalTBLErr);
                                                    var ret2 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret2)
                                                    {

                                                    }
                                                }
                                                repoJournalTransRepository.Add(JournalTransTBL);
                                                var rey = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (rey)
                                                {
                                                }

                                            }
                                            else
                                            {
                                                JournalTBLErr.TerminalId = JournalTransTBL.TerminalId;
                                                JournalTBLErr.ATMIp = JournalTransTBL.ATMIp;
                                                JournalTBLErr.Brand = JournalTransTBL.Brand;
                                                JournalTBLErr.TransDate = JournalTransTBL.TransDate;
                                                JournalTBLErr.TransTime = JournalTransTBL.TransTime;
                                                JournalTBLErr.TransRef = JournalTransTBL.TransRef;
                                                JournalTBLErr.Pan = JournalTransTBL.Pan;
                                                JournalTBLErr.EventTypeId = JournalTransTBL.EventTypeId;
                                                JournalTBLErr.CurrencyCode = JournalTransTBL.CurrencyCode;
                                                JournalTBLErr.Amount = JournalTransTBL.Amount;
                                                JournalTBLErr.AvailBal = JournalTransTBL.AvailBal;
                                                JournalTBLErr.Ledger = JournalTransTBL.Ledger;
                                                JournalTBLErr.Surcharge = JournalTransTBL.Surcharge;
                                                JournalTBLErr.AcctFrom = JournalTransTBL.AcctFrom;
                                                JournalTBLErr.AcctTo = JournalTransTBL.AcctTo;
                                                JournalTBLErr.TransStatus = JournalTransTBL.TransStatus;
                                                JournalTBLErr.Cassette = JournalTransTBL.Cassette;
                                                JournalTBLErr.ErrorCode = JournalTransTBL.ErrorCode;
                                                JournalTBLErr.RejectCount = JournalTransTBL.RejectCount;
                                                JournalTBLErr.DispenseCount = JournalTransTBL.DispenseCount;
                                                JournalTBLErr.RequestCount = JournalTransTBL.RequestCount;
                                                JournalTBLErr.RemainCount = JournalTransTBL.RemainCount;
                                                JournalTBLErr.PickUpCount = JournalTransTBL.PickUpCount;
                                                JournalTBLErr.Denomination = JournalTransTBL.Denomination;
                                                JournalTBLErr.JournalfileDirectory = fileNamenAndType;
                                                JournalTBLErr.MstateId = JournalTransTBL.MstateId;
                                                JournalTBLErr.DeviceStateId = JournalTransTBL.DeviceStateId;
                                                JournalTBLErr.FileDownloadId = JournalTransTBL.FileDownloadId;
                                                JournalTBLErr.TransTimeRange = JournalTransTBL.TransTimeRange;
                                                JournalTBLErr.OpCode = JournalTransTBL.OpCode;
                                                JournalTBLErr.TakenAmount = JournalTransTBL.TakenAmount;
                                                JournalTBLErr.MatchingStatus = JournalTransTBL.MatchingStatus;
                                                JournalTBLErr.MatchingType = JournalTransTBL.MatchingType;
                                                JournalTBLErr.ReconDate = JournalTransTBL.ReconDate;
                                                JournalTBLErr.UserId = JournalTransTBL.UserId;
                                                JournalTBLErr.ErrorMsg = "No  Ref No";
                                                JournalTBLErr.OrigRefNo = JournalTransTBL.OrigRefNo;

                                                repoJournalTransErrorRepository.Add(JournalTBLErr);
                                                var ret2 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret2)
                                                {

                                                }
                                            }
                                        }

                                    }
                                    //Truncate journal table after this as below
                                    string updatScript = "truncate table dbo.Journallog";

                                    string connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                                    SqlConnection con = new SqlConnection(connectionstring);

                                    con.Open();
                                    SqlCommand commd = new SqlCommand(updatScript, con);
                                    commd.CommandType = CommandType.Text;
                                    commd.ExecuteNonQuery();
                                    con.Close();

                                    //Update and Move here

                                    var proceTBL = new FileProcessControl();

                                    proceTBL.ReconTypeId = ReconTypeId;
                                    proceTBL.FileDirectory = FileDirectory;
                                    proceTBL.FileName = fileNamenAndType;
                                    proceTBL.NameConvention = ReconType.FileNamingConvention;
                                    proceTBL.DateProcessed = DateTime.Now;
                                    proceTBL.Status = "Processed";
                                    proceTBL.DateCreated = DateTime.Now;
                                    proceTBL.UserId = 1;

                                    LogManager.SaveLog("Log File reading ReconTye :" + ReconType.ReconName + " FileDirectory: " + FileDirectory + "fileNamenAndType: " + fileNamenAndType + "getReconTypeInfo.FileNamingConvention: " + ReconType.FileNamingConvention);

                                    //repoadmFileProcessedRepository.Add(proceTBL);
                                    var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                    if (ret1)
                                    {
                                        string fnn = ReconType.ProcessedFileDirectory;
                                        string fn = @fnn + "\\" + fileNamenAndType;
                                        File.Move((FileDirectory + "\\" + fileNamenAndType), fn);
                                    }
                                    //Update control table

                                    if (controlTable != null)
                                    {

                                        controlTable.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                                        controlTable.LastTransDate = DateTime.Now;
                                        controlTable.RecordsCount = count;
                                        repoadmDataPoollingControlRepository.Update(controlTable);
                                        var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {

                                        }
                                    }
                                    else
                                    {
                                        var admDataPoollingControlTBL = new admDataPullingControl();
                                        admDataPoollingControlTBL.ReconTypeId = ReconType.ReconTypeId;
                                        admDataPoollingControlTBL.FileType = "File";
                                        admDataPoollingControlTBL.TableName = "ATMTrans";
                                        admDataPoollingControlTBL.DateCreated = DateTime.Now;
                                        admDataPoollingControlTBL.UserId = 1;
                                        admDataPoollingControlTBL.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                                        admDataPoollingControlTBL.ReconLevel = 1;
                                        admDataPoollingControlTBL.LastTransDate = DateTime.Now;
                                        admDataPoollingControlTBL.RecordsCount = count;
                                        repoadmDataPoollingControlRepository.Add(admDataPoollingControlTBL);
                                        var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {

                                        }
                                    }

                                }
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                        var stackTrace = new StackTrace(ex);
                        var thisasm = Assembly.GetExecutingAssembly();
                        _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                        _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                        LogManager.SaveLog("An error occured in Journal in Line  :" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                        // throw;
                    }
                }
                #endregion
                #endregion


                #region Source 1

                #endregion

           


            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in  Line JournalLibrary MTN sybase 6 File Source:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }

            return string.Empty;
        }

    }

}