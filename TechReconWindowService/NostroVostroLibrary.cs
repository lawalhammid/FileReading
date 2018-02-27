
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
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using TechReconWindowService.BAL.Helpers;

namespace TechReconWindowService
{
    public class NostroVostroLibrary
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmReconDataSourcesRepository repoadmReconDataSourcesRepository;
        private readonly IadmSourceAccountRepository repoadmSourceAccountRepository;
        private readonly ICBSTransationTestRepository repoCBSTransationTestRepository;
        private readonly IPostillionTransationTestRepository repoPostillionTransationTestRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly IadmTerminalRepository repoadmTerminalRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly IadmFailledFileRepository repoadmFailledFileRepository;
        private readonly IadmFileProcessedRepository repoadmFileProcessedRepository;
        private readonly ICBSNostroVostroTransRepository repoCBSNostroVostroTransRepository;
        private readonly ICBSNostroVostroTransErrorRepository repoCBSNostroVostroTransErrorRepository;
        private readonly ICBSNostroVostroTransHistoryRepository repoCBSNostroVostroTransHistoryRepository;
        private readonly IVostroMT940950TransHistoryRepository repoVostroMT940950TransHistoryRepository;
        private ReadMT940 readMT940;
        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/NostroVostroLibrary.cs";

        public NostroVostroLibrary()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmReconDataSourcesRepository = new admReconDataSourcesRepository(idbfactory);
            repoadmSourceAccountRepository = new admSourceAccountRepository(idbfactory);
            repoCBSTransationTestRepository = new CBSTransationTestRepository(idbfactory);
            repoPostillionTransationTestRepository = new PostillionTransationTestRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoadmTerminalRepository = new admTerminalRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoadmFailledFileRepository = new admFailledFileRepository(idbfactory);
            repoadmFileProcessedRepository = new admFileProcessedRepository(idbfactory);
            repoCBSNostroVostroTransRepository = new CBSNostroVostroTransRepository(idbfactory);
            repoCBSNostroVostroTransErrorRepository = new CBSNostroVostroTransErrorRepository(idbfactory);
            repoCBSNostroVostroTransHistoryRepository = new CBSNostroVostroTransHistoryRepository(idbfactory);
            repoVostroMT940950TransHistoryRepository = new VostroMT940950TransHistoryRepository(idbfactory);
            readMT940 = new ReadMT940();
        }
        public async Task<string> PullNostroVostroData()
        {
            Library library = new Library();
            try
            {
                LogManager.SaveLog("Task Start in NostroVostroLibrary");
                var ReconType = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "NostroVostro");
                int ReconTypeId = ReconType.ReconTypeId;
                var AccountSources = await repoadmSourceAccountRepository.GetManyAsync(c => c.ReconTypeId == ReconTypeId);
                var listofAccountsource1 = AccountSources.Where(c => c.SourceName == "Source 1" && c.AcctNo != null);
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

                #region Source 1
                #region   // Sybase Server below

                if (dtSouceCon1.DatabaseType == "SYBASE")
                {
                    try
                    {
                        LogManager.SaveLog("PullDataNostro  for Sybase Start in NostroVostroLibrary");
                        var value1 = string.Empty;
                        value1 = string.Empty;

                        var value2 = string.Empty;
                        value2 = string.Empty;
                        int scriptExecTtype = 0;

                        string FromDateParam = string.Empty;
                        string ToDateParam = string.Empty;
                        string LastRecordId = string.Empty;
                        string CBSRecordCount = string.Empty;
                        string MaxTransDate = string.Empty;
                        string dateTest = string.Empty;
                        string ReconDate = string.Empty;
                        
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == ReconType.ReconTypeId && c.TableName == "CBSNostroTrans");

                        if (controlTable != null)
                        {
                            FromDateParam = controlTable.FromDateParam == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.FromDateParam));
                            FromDateParam = "'" + FromDateParam + "'";
                            ToDateParam = controlTable.ToDateParam == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.ToDateParam));
                            ToDateParam = "'" + ToDateParam + "'";

                            ReconDate = controlTable.LastTransDate == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.LastTransDate));
                            ReconDate = "'" + ReconDate + "'"; 
                            
                            LastRecordId = controlTable.LastRecordId;

                        }
                        else
                        {
                           
                            LastRecordId = "0";
                            FromDateParam = "'20170901'";
                            ToDateParam = "'20170923'";
                            ReconDate = "'20170923'";
                        }
                        string pdt = "";// parametername.Split(' ')[1];
                        List<AseParameter> parameterPasses = new List<AseParameter>()
                            {              
                                new AseParameter() {ParameterName = pdt, AseDbType = AseDbType.VarChar, Value = value1},  
                                new AseParameter() {ParameterName = pdt, AseDbType = AseDbType.VarChar, Value = value2}
                            };

                        string connstring = System.Configuration.ConfigurationManager.AppSettings["sybconnection"].ToString();

                        connstring = connstring.Replace("{{Data Source}}", dtSouceCon1.ipAddress);
                        connstring = connstring.Replace("{{port}}", dtSouceCon1.PortNumber);
                        connstring = connstring.Replace("{{database}}", dtSouceCon1.DatabaseName);
                        connstring = connstring.Replace("{{uid}}", dtSouceCon1.UserName);
                        connstring = connstring.Replace("{{pwd}}", library.Decrypt(dtSouceCon1.Password));

                        DataSet dsGetData = new DataSet();

                        using (AseConnection theCons = new AseConnection(connstring))
                        {
                            DataSet ds = new DataSet();
                            try
                            {
                                theCons.Open();
                                if (theCons.State.ToString() == "Open")
                                {
                                    LogManager.SaveLog("Sybase Connection  open for Ip " + dtSouceCon1.ipAddress);
                                }
                                else
                                {
                                    LogManager.SaveLog("Sybase Connection not Open for Ip " + dtSouceCon1.ipAddress);
                                }
                            }
                            catch (Exception ex)
                            {

                                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                                var stackTrace = new StackTrace(ex);
                                var thisasm = Assembly.GetExecutingAssembly();
                                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                LogManager.SaveLog("An error occured NostroVostroLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                            }

                            try
                            {
                                var CurrencyList = AccountSources.Where(c => c.SourceName == "Source 1" && c.Currency != null && c.Currency != string.Empty);
                                int gocount = 0;
                                foreach (var cur in CurrencyList)
                                {
                                    //87572420
                                    string currency = "'" + cur.Currency + "'"; ;
                                    string Account = "'" + cur.AcctNo + "'"; ;
                                    string SqlString = ReconType.Source1Script.Replace("{Account}", Account)
                                                                                 .Replace("{Currency}", currency)
                                                                                    .Replace("{LastRecordId}", LastRecordId)
                                                                                    .Replace("{ReconDate}", ReconDate);

                                    AseCommand cmd = new AseCommand(SqlString, theCons);
                                    cmd.Connection = theCons;
                                    cmd.CommandText = SqlString;
                                    cmd.CommandTimeout = 0;
                                    //i.e check if the parameters are not null, if the are null, that means scriptExecTtype = 1, 
                                    // would be using CommandText not store procdure
                                    if (!string.IsNullOrWhiteSpace(value1) && !string.IsNullOrWhiteSpace(value1))
                                    {
                                        if (parameterPasses != null)
                                        {
                                            cmd.Parameters.AddRange(parameterPasses.ToArray());
                                            var gggg = (parameterPasses.ToArray());
                                        }
                                    }
                                    else
                                    {
                                        scriptExecTtype = 1;
                                    }

                                    cmd.CommandType = scriptExecTtype == 0 ? CommandType.StoredProcedure : CommandType.Text;
                               
                                    AseDataReader reader = cmd.ExecuteReader();
                                    ds.EnforceConstraints = false;
                                    ds.Load(reader, LoadOption.OverwriteChanges, "Results");
                                    var returndata = ds.Tables["Results"];

                                    LogManager.SaveLog("PullDataCBSNostroTrans  for Sybase returndata.Rows.Count " + returndata.Rows.Count);

                                    if (returndata.Rows.Count > 0)
                                    {

                                        var CBSNostroTransTBL = new CBSNostroTran();
                                        var CBSNostroTransTBLErr = new CBSNostroTransError();

                                        int count = 0;
                                        int countTrans = 0;
                                        for (int col = 0; col < returndata.Rows.Count; col++)
                                        {
                                            try
                                            {
                                                CBSNostroTransTBL.AcctNo = returndata.Rows[col][0] == null ? null : returndata.Rows[col][0].ToString();
                                                CBSNostroTransTBL.AcctType = returndata.Rows[col][1] == null ? null : returndata.Rows[col][1].ToString();
                                                CBSNostroTransTBL.TransDate = returndata.Rows[col][2] == null ? (DateTime?)null : Convert.ToDateTime(returndata.Rows[col][2]); ;

                                                dateTest = CBSNostroTransTBL.TransDate.ToString();
                                                if (countTrans > 1)
                                                {
                                                    MaxTransDate = CBSNostroTransTBL.TransDate.ToString();

                                                    if (Convert.ToDateTime(dateTest) > Convert.ToDateTime(MaxTransDate))
                                                    {
                                                        MaxTransDate = dateTest;
                                                    }
                                                    else
                                                    {
                                                        MaxTransDate = MaxTransDate;
                                                    }
                                                }
                                                CBSNostroTransTBL.Amount = returndata.Rows[col][3] == null ? 0 : Math.Round(Convert.ToDecimal(returndata.Rows[col][3]), 2);
                                                CBSNostroTransTBL.Description = returndata.Rows[col][4] == null ? null : returndata.Rows[col][4].ToString();
                                                CBSNostroTransTBL.Reference = returndata.Rows[col][5] == null ? null : returndata.Rows[col][5].ToString();
                                                CBSNostroTransTBL.OrigRefNo = CBSNostroTransTBL.Reference;
                                                CBSNostroTransTBL.DebitCredit = returndata.Rows[col][6] == null ? null : returndata.Rows[col][6].ToString();
                                                CBSNostroTransTBL.OriginatingBranch = returndata.Rows[col][7] == null ? null : returndata.Rows[col][7].ToString();
                                                CBSNostroTransTBL.PostedBy = returndata.Rows[col][8] == null ? null : returndata.Rows[col][8].ToString();
                                                CBSNostroTransTBL.Currency = returndata.Rows[col][9] == null ? null : returndata.Rows[col][9].ToString();
                                                CBSNostroTransTBL.PtId = returndata.Rows[col][10] == null ? null : returndata.Rows[col][10].ToString();
                                                int revcode;
                                                CBSNostroTransTBL.ReversalCode = returndata.Rows[col][11] == null ? (int?) null : int.TryParse(returndata.Rows[col][11].ToString(), out revcode) ? revcode : (int?)null;
                                                LastRecordId = CBSNostroTransTBL.PtId;
                                                CBSNostroTransTBL.MatchingStatus = "N";
                                                CBSNostroTransTBL.PullDate = DateTime.Now;
                                                CBSNostroTransTBL.UserId = 1;

                                                countTrans += 1;

                                                if (!string.IsNullOrWhiteSpace(CBSNostroTransTBL.Reference))
                                                {
                                                    var exist = await repoCBSNostroVostroTransRepository.GetAsync(c => (c.Reference == CBSNostroTransTBL.Reference && c.Amount == CBSNostroTransTBL.Amount));
                                                    var existHis = await repoCBSNostroVostroTransHistoryRepository.GetAsync(c => c.Reference == CBSNostroTransTBL.Reference && c.Amount == CBSNostroTransTBL.Amount);
                                                    if (exist != null || existHis != null)
                                                    {
                                                        CBSNostroTransTBLErr.AcctNo = CBSNostroTransTBL.AcctNo;
                                                        CBSNostroTransTBLErr.AcctType = CBSNostroTransTBL.AcctType;
                                                        CBSNostroTransTBLErr.TransDate = CBSNostroTransTBL.TransDate;
                                                        CBSNostroTransTBLErr.Amount = CBSNostroTransTBL.Amount;
                                                        CBSNostroTransTBLErr.Description = CBSNostroTransTBL.Description;
                                                        CBSNostroTransTBLErr.Reference = CBSNostroTransTBL.Reference;
                                                        CBSNostroTransTBLErr.OrigRefNo = CBSNostroTransTBL.Reference;
                                                        CBSNostroTransTBLErr.DebitCredit = CBSNostroTransTBL.DebitCredit;
                                                        CBSNostroTransTBLErr.OriginatingBranch = CBSNostroTransTBL.OriginatingBranch;
                                                        CBSNostroTransTBLErr.PostedBy = CBSNostroTransTBL.PostedBy;
                                                        CBSNostroTransTBLErr.Currency = CBSNostroTransTBL.Currency;
                                                        CBSNostroTransTBLErr.PtId = CBSNostroTransTBL.PtId;
                                                        LastRecordId = CBSNostroTransTBLErr.PtId;
                                                        CBSNostroTransTBLErr.MatchingStatus = CBSNostroTransTBL.MatchingStatus;
                                                        CBSNostroTransTBLErr.PullDate = CBSNostroTransTBL.PullDate;
                                                        CBSNostroTransTBLErr.ReversalCode = CBSNostroTransTBL.ReversalCode;
                                                        CBSNostroTransTBLErr.UserId = 1;
                                                        CBSNostroTransTBLErr.ErrorMsg = "Duplicate transaction record";

                                                        repoCBSNostroVostroTransErrorRepository.Add(CBSNostroTransTBLErr);

                                                        var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                        if (ret1)
                                                        {
                                                            continue;
                                                        }
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        repoCBSNostroVostroTransRepository.Add(CBSNostroTransTBL);
                                                        var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                        if (ret)
                                                        {
                                                            count += 1;
                                                            CBSRecordCount = count.ToString();
                                                            continue;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    CBSNostroTransTBLErr.AcctNo = CBSNostroTransTBL.AcctNo;
                                                    CBSNostroTransTBLErr.AcctType = CBSNostroTransTBL.AcctType;
                                                    CBSNostroTransTBLErr.TransDate = CBSNostroTransTBL.TransDate;
                                                    CBSNostroTransTBLErr.Amount = CBSNostroTransTBL.Amount;
                                                    CBSNostroTransTBLErr.Description = CBSNostroTransTBL.Description;
                                                    CBSNostroTransTBLErr.Reference = CBSNostroTransTBL.Reference;
                                                    CBSNostroTransTBLErr.OrigRefNo = CBSNostroTransTBL.Reference;
                                                    CBSNostroTransTBLErr.DebitCredit = CBSNostroTransTBL.DebitCredit;
                                                    CBSNostroTransTBLErr.OriginatingBranch = CBSNostroTransTBL.OriginatingBranch;
                                                    CBSNostroTransTBLErr.PostedBy = CBSNostroTransTBL.PostedBy;
                                                    CBSNostroTransTBLErr.Currency = CBSNostroTransTBL.Currency;
                                                    CBSNostroTransTBLErr.PtId = CBSNostroTransTBL.PtId;
                                                    LastRecordId = CBSNostroTransTBLErr.PtId;
                                                    CBSNostroTransTBLErr.MatchingStatus = CBSNostroTransTBL.MatchingStatus;
                                                    CBSNostroTransTBLErr.PullDate = CBSNostroTransTBL.PullDate;
                                                    CBSNostroTransTBLErr.ReversalCode = CBSNostroTransTBL.ReversalCode;
                                                    CBSNostroTransTBLErr.UserId = 1;
                                                    CBSNostroTransTBLErr.ErrorMsg = "No ref No.";

                                                    repoCBSNostroVostroTransErrorRepository.Add(CBSNostroTransTBLErr);

                                                    var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret1)
                                                    {
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
                                                LogManager.SaveLog("An error occured NostroVostroLibrary in Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                                                try
                                                {
                                                    CBSNostroTransTBLErr.AcctNo = CBSNostroTransTBL.AcctNo;
                                                    CBSNostroTransTBLErr.AcctType = CBSNostroTransTBL.AcctType;
                                                    CBSNostroTransTBLErr.TransDate = CBSNostroTransTBL.TransDate;
                                                    CBSNostroTransTBLErr.Amount = CBSNostroTransTBL.Amount;
                                                    CBSNostroTransTBLErr.Description = CBSNostroTransTBL.Description;
                                                    CBSNostroTransTBLErr.Reference = CBSNostroTransTBL.Reference;
                                                    CBSNostroTransTBLErr.OrigRefNo = CBSNostroTransTBL.Reference;
                                                    CBSNostroTransTBLErr.DebitCredit = CBSNostroTransTBL.DebitCredit;
                                                    CBSNostroTransTBLErr.OriginatingBranch = CBSNostroTransTBL.OriginatingBranch;
                                                    CBSNostroTransTBLErr.PostedBy = CBSNostroTransTBL.PostedBy;
                                                    CBSNostroTransTBLErr.Currency = CBSNostroTransTBL.Currency;
                                                    CBSNostroTransTBLErr.PtId = CBSNostroTransTBL.PtId;
                                                    LastRecordId = CBSNostroTransTBLErr.PtId;
                                                    CBSNostroTransTBLErr.MatchingStatus = CBSNostroTransTBL.MatchingStatus;
                                                    CBSNostroTransTBLErr.PullDate = CBSNostroTransTBL.PullDate;
                                                    CBSNostroTransTBLErr.UserId = 1;
                                                    CBSNostroTransTBLErr.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;

                                                    repoCBSNostroVostroTransErrorRepository.Add(CBSNostroTransTBLErr);

                                                    var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                                    if (ret1)
                                                    {
                                                        continue;
                                                    }
                                                }
                                                catch (Exception ex1)
                                                {
                                                    var exErr2 = ex1 == null ? ex.InnerException.Message : ex.Message;
                                                    var stackTrace2 = new StackTrace(ex);
                                                    var thisas = Assembly.GetExecutingAssembly();
                                                    _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisas).Name;
                                                    _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                                    LogManager.SaveLog("An error occured NostroVostroLibrary in Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                                                    continue;
                                                }
                                                continue;
                                            }
                                        }
                                        if (controlTable != null)
                                        {
                                            //Update with below
                                           
                                            string updatScript = "update CBSNostroTrans set VostroAcctNo = b.VostroAcctNo "
                                                           + " from CBSNostroTrans a,admSourceAccount b "
                                                           + " where a.AcctNo = b.AcctNo "
                                                           + " and b.ReconTypeId = " + ReconType.ReconTypeId;

                                            string connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                                            SqlConnection con = new SqlConnection(connectionstring);

                                            con.Open();
                                            SqlCommand commd = new SqlCommand(updatScript, con);
                                            commd.CommandType = CommandType.Text;
                                            commd.ExecuteNonQuery();
                                            con.Close();

                                            controlTable.LastRecordId = LastRecordId;
                                            controlTable.LastRecordTimeStamp = DateTime.Now;
                                            DateTime dt = new DateTime();
                                            controlTable.LastTransDate = MaxTransDate == null ? DateTime.Now : DateTime.TryParse(MaxTransDate, out dt) ? dt : DateTime.Now;
                                            controlTable.PullDate = DateTime.Now;
                                            int recTrack;
                                            controlTable.RecordsCount = CBSRecordCount == null ? (int?)null : int.TryParse(CBSRecordCount, out recTrack) ? recTrack : (int?)null;
                                            repoadmDataPoollingControlRepository.Update(controlTable);
                                            var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                            if (ret)
                                            {
                                            }
                                        }
                                        else
                                        {
                                            
                                            string updatScript = "update CBSNostroTrans set VostroAcctNo = b.VostroAcctNo "
                                                           + " from CBSNostroTrans a,admSourceAccount b "
                                                           + " where a.AcctNo = b.AcctNo "
                                                           + " and b.ReconTypeId = " + ReconType.ReconTypeId;

                                            string connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                                            SqlConnection con = new SqlConnection(connectionstring);

                                            con.Open();
                                            SqlCommand commd = new SqlCommand(updatScript, con);
                                            commd.CommandType = CommandType.Text;
                                            commd.ExecuteNonQuery();
                                            con.Close();

                                            var admDataPoollingControlTBL = new admDataPullingControl();
                                            admDataPoollingControlTBL.ReconTypeId = ReconType.ReconTypeId;
                                            admDataPoollingControlTBL.FileType = "Table";
                                            admDataPoollingControlTBL.TableName = "CBSNostroTrans";
                                            admDataPoollingControlTBL.DateCreated = DateTime.Now;
                                            admDataPoollingControlTBL.UserId = 1;
                                            admDataPoollingControlTBL.LastRecordTimeStamp = DateTime.Now;
                                            admDataPoollingControlTBL.LastRecordId = LastRecordId;
                                            admDataPoollingControlTBL.ReconLevel = 1;
                                            DateTime dt = new DateTime();
                                            admDataPoollingControlTBL.LastTransDate = MaxTransDate == null ? DateTime.Now : DateTime.TryParse(MaxTransDate, out dt) ? dt : DateTime.Now;
                                            admDataPoollingControlTBL.PullDate = DateTime.Now;
                                            int recTrack;
                                            admDataPoollingControlTBL.RecordsCount = CBSRecordCount == null ? (int?)null : int.TryParse(CBSRecordCount, out recTrack) ? recTrack : (int?)null;
                                            repoadmDataPoollingControlRepository.Add(admDataPoollingControlTBL);
                                            var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                            if (ret)
                                            {
                                            }
                                        }
                                    }

                                    var lstRec = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == ReconType.ReconTypeId && c.TableName == "CBSNostroTrans");
                                    LastRecordId = lstRec.LastRecordId;

                                }
                               // reader.Close();
                                theCons.Close();
                            }
                            catch (Exception ex)
                            {
                                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                                var stackTrace = new StackTrace(ex);
                                var thisasm = Assembly.GetExecutingAssembly();
                                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                LogManager.SaveLog("An error occured NostroVostroLibrary in Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                        LogManager.SaveLog("An error occured in CBSNostroVostroTrans Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                       
                    }
                }

                #endregion
                #endregion

                #region Source 2

                var dtSouceCon2 = await repoadmSourceAccountRepository.GetAsync(c => c.ReconTypeId == ReconTypeId && c.SourceName == "Source 2");

                #region //File Directory Below

                if (!string.IsNullOrWhiteSpace(dtSouceCon2.FileDirectory))
                {
                    try
                    {
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == ReconType.ReconTypeId && c.TableName == "VostroMT940950Trans");

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
                        DLIST = d.GetFiles("*" + ".out").ToList();
                        List<FileInfo> ggg = null;
                        if (controlTable != null)
                        {
                            ggg = DLIST;//.Where(c => c.LastWriteTime > controlTable.LastRecordTimeStamp).ToList();
                        }
                        else
                        {
                            ggg = DLIST;
                        }
                        string fileNamenAndType = string.Empty;
                        string FileLastTime = string.Empty;
                        int count = 0;
                        string returnValue = string.Empty;
                        foreach (var kl in ggg)
                        {
                            fileNamenAndType = (from f in DLIST orderby f.LastWriteTime ascending select kl).First().Name;

                            FileLastTime = (from f in DLIST orderby f.LastAccessTimeUtc descending select kl).First().LastWriteTime.ToString();

                            int read = await readMT940.Generate940950Message(dtSouceCon2.FileDirectory + "\\" + fileNamenAndType, ReconTypeId, dtSouceCon2.FileDirectory, fileNamenAndType, FileLastTime);
                            returnValue = read.ToString();

                            if (!string.IsNullOrWhiteSpace(returnValue))
                            {
                                // Move here
                                var proceTBL = new FileProcessControl();

                                proceTBL.ReconTypeId = ReconTypeId;
                                proceTBL.FileDirectory = FileDirectory;
                                proceTBL.FileName = fileNamenAndType;
                                proceTBL.NameConvention = ReconType.FileNamingConvention;
                                proceTBL.DateProcessed = DateTime.Now;
                                proceTBL.Status = "Processed";
                                proceTBL.DateCreated = DateTime.Now;
                                proceTBL.UserId = 1;
                                repoadmFileProcessedRepository.Add(proceTBL);
                                var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                if (ret1)
                                {
                                    LogManager.SaveLog("Move File Start Nostro Vostro");
                                    string MvFile = library.MoveFile(FileDirectory + "\\" + fileNamenAndType, ReconType.ProcessedFileDirectory, fileNamenAndType);
                                    LogManager.SaveLog("Move File in Nostro Vostro status: " + MvFile);
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
                        LogManager.SaveLog("An error occured in NostroVostroLibrary Line : " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                       
                    }
                }
                #endregion
                #endregion
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in  NostroVostroLogManager Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
           
            return string.Empty;
        }
    }
}