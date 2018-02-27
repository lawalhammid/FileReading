
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
    public class RiaTransLibrary
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmReconDataSourcesRepository repoadmReconDataSourcesRepository;
        private readonly IadmSourceAccountRepository repoadmSourceAccountRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly ICBSRiaRepository repoCBSRiaRepository;
        private readonly IRiaRepository repoRiaRepository;
        private readonly IadmFailledFileRepository repoadmFailledFileRepository;
        private readonly IadmFileProcessedRepository repoadmFileProcessedRepository;
        private readonly ICBSRiaTransErrorRepository repoCBSRiaTransErrorRepository;
        private readonly IRiaTransErrorRepository repoRiaTransErrorRepository;
        private readonly ICBSRiaHistoryRepository repoCBSRiaHistoryRepository;
        private readonly IRiaHistoryRepository repoRiaHistoryRepository;
        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/RiaTransLibrary.cs";
        private static string agentcodestatic;
        private static string AcctNoStatic;
        private static string LegacyStatic;
        private static string SettleCurrStatic;
        private static string TransCurStatic;
        private static string BranchNameStatic;
        public RiaTransLibrary()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmReconDataSourcesRepository = new admReconDataSourcesRepository(idbfactory);
            repoadmSourceAccountRepository = new admSourceAccountRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoCBSRiaRepository = new CBSRiaRepository(idbfactory);
            repoRiaRepository = new RiaRepository(idbfactory);
            repoadmFailledFileRepository = new admFailledFileRepository(idbfactory);
            repoadmFileProcessedRepository = new admFileProcessedRepository(idbfactory);
            repoCBSRiaTransErrorRepository = new CBSRiaTransErrorRepository(idbfactory);
            repoRiaTransErrorRepository = new RiaTransErrorRepository(idbfactory);
            repoCBSRiaHistoryRepository = new CBSRiaHistoryRepository(idbfactory);
            repoRiaHistoryRepository = new RiaHistoryRepository(idbfactory);
           
        }    
        public async Task<string> PullRiaData()
        {
            Library library = new Library();
           
            try
            {
                LogManager.SaveLog("Task Start in RiaTransLibrary");
                var ReconType = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "Ria");
                int ReconTypeId = ReconType.ReconTypeId;
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
                acctlistSource1 = (acctlistSource1.Remove(index, 1));

                var dtSouceCon1 = await repoadmReconDataSourcesRepository.GetAsync(c => c.ReconDSId == dt1);
               
                #region Source 1
                #region   // Sybase Server below

                if (dtSouceCon1.DatabaseType == "SYBASE")
                {
                    try
                    {
                        LogManager.SaveLog("PullRiaData Start in RiaTransLogManager for Sybase");
                        var value1 = string.Empty;
                        value1 = string.Empty;

                        var value2 = string.Empty;
                        value2 = string.Empty;
                        int scriptExecTtype = 0;

                        var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "Ria");
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSRiaTrans");
                        string FromDateParam = string.Empty;
                        string ToDateParam = string.Empty;
                        string ReconDate = string.Empty;
                        string CBSRecordCount = string.Empty;
                        string MaxTransDate = string.Empty;
                        string dateTest = string.Empty;

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
                            FromDateParam = "'20170828'";
                            ToDateParam = "'20170831'";
                            ReconDate = "'20170831'";
                        }


                        string SqlString = ReconType.Source1Script.Replace("{AcctlistSource1}", acctlistSource1)
                                                                   .Replace("{ReconDate}", ReconDate)
                                                                   .Replace("{LastRecordId}", LastRecordId);
                       
                        
                        
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
                                    LogManager.SaveLog("Sybase Connection  open for Ip" + dtSouceCon1.ipAddress);
                                }
                                else
                                {
                                    LogManager.SaveLog("Sybase Connection not Open for Ip" + dtSouceCon1.ipAddress);
                                }
                            }
                            catch (Exception ex)
                            {

                                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                                var stackTrace = new StackTrace(ex);
                                var thisasm = Assembly.GetExecutingAssembly();
                                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                LogManager.SaveLog("An error occured RiaTransLibrary Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                            }

                            try
                            {

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

                                ds.Load(reader, LoadOption.OverwriteChanges, "Results");

                                var returndata = ds.Tables["Results"];

                                LogManager.SaveLog("Total records pulled in CBSRiaTrans: " + returndata.Rows.Count);

                                if (returndata.Rows.Count > 0)
                                {
                                    var CBRiaTBL = new CBSRiaTran();
                                     var CBSRiaErrLogTBL = new CBSRiaTransError();
                                    int countTrans = 0;

                                    for (int col = 0; col < returndata.Rows.Count; col++)
                                    {
                                        try
                                        {

                                            CBRiaTBL.AcctNo = returndata.Rows[col][0] == null ? null : returndata.Rows[col][0].ToString();
                                            decimal Amountad, Amount;
                                            if (decimal.TryParse(returndata.Rows[col][1].ToString(), out Amountad))
                                            {
                                                Amount = Convert.ToDecimal(returndata.Rows[col][1]) != null ? Convert.ToDecimal(returndata.Rows[col][1]) : 0;
                                            }
                                            else
                                            {
                                                Amount = 0;
                                            }
                                            CBRiaTBL.Amount = Math.Round(Amount, 2);
                                            CBRiaTBL.DebitCredit = returndata.Rows[col][2] == null ? null : returndata.Rows[col][2].ToString();
                                            CBRiaTBL.Description = returndata.Rows[col][3].ToString();
                                            CBRiaTBL.TransDate = returndata.Rows[col][4] == null ? (DateTime?)null : Convert.ToDateTime(returndata.Rows[col][4]);
                                            dateTest = CBRiaTBL.TransDate.ToString();
                                            if (countTrans > 1)
                                            {
                                                MaxTransDate = CBRiaTBL.TransDate.ToString();

                                                if (Convert.ToDateTime(dateTest) > Convert.ToDateTime(MaxTransDate))
                                                {
                                                    MaxTransDate = dateTest;

                                                }
                                                else
                                                {
                                                    MaxTransDate = MaxTransDate;
                                                }
                                            }
                                            countTrans += 1;
                                            CBRiaTBL.OriginBranch = returndata.Rows[col][5] == null ? null : returndata.Rows[col][5].ToString();
                                            CBRiaTBL.PostedBy = returndata.Rows[col][6] == null ? null : returndata.Rows[col][6].ToString();
                                            decimal Balancead, Balance;
                                            if (decimal.TryParse(returndata.Rows[col][7].ToString(), out Balancead))
                                            {
                                                Balance = Convert.ToDecimal(returndata.Rows[col][7]) != null ? Convert.ToDecimal(returndata.Rows[col][7]) : 0;
                                            }
                                            else
                                            {
                                                Balance = 0;
                                            }

                                            CBRiaTBL.Balance = Math.Round(Balance, 2);
                                            CBRiaTBL.Refference = returndata.Rows[col][8] == null ? null : returndata.Rows[col][8].ToString();
                                            CBRiaTBL.OrigRefNo = CBRiaTBL.Refference;
                                            CBRiaTBL.PtId = returndata.Rows[col][9] == null ? null : returndata.Rows[col][9].ToString();
                                            LastRecordId = CBRiaTBL.PtId;
                                            int revcode;
                                            CBRiaTBL.ReversalCode = returndata.Rows[col][10] == null ? (int?)null : int.TryParse(returndata.Rows[col][10].ToString(), out revcode) ? revcode : (int?)null;
                                            CBRiaTBL.PullDate = DateTime.Now;
                                            CBRiaTBL.MatchingStatus = "N";
                                            CBRiaTBL.UserId = 1;

                                            if (!string.IsNullOrWhiteSpace(CBRiaTBL.Refference))
                                            {

                                                var exist = await repoCBSRiaRepository.GetAsync(c => c.Refference == CBRiaTBL.Refference && (c.DebitCredit == CBRiaTBL.DebitCredit) && (c.Amount == CBRiaTBL.Amount) && (c.AcctNo == CBRiaTBL.AcctNo));
                                                var existHis = await repoCBSRiaHistoryRepository.GetAsync(c => c.Refference == CBRiaTBL.Refference && (c.DebitCredit == CBRiaTBL.DebitCredit) && (c.Amount == CBRiaTBL.Amount) && (c.AcctNo == CBRiaTBL.AcctNo));
                                                if (exist != null || existHis != null)
                                                {
                                                    CBSRiaErrLogTBL.AcctNo = CBRiaTBL.AcctNo;
                                                    CBSRiaErrLogTBL.Amount = CBRiaTBL.Amount;
                                                    CBSRiaErrLogTBL.DebitCredit = CBRiaTBL.DebitCredit;
                                                    CBSRiaErrLogTBL.Description = CBRiaTBL.Description;
                                                    CBSRiaErrLogTBL.TransDate = CBRiaTBL.TransDate;
                                                    CBSRiaErrLogTBL.OriginBranch = CBRiaTBL.OriginBranch;
                                                    CBSRiaErrLogTBL.PostedBy = CBRiaTBL.PostedBy;
                                                    CBSRiaErrLogTBL.ReversalCode = CBRiaTBL.ReversalCode;
                                                    CBSRiaErrLogTBL.Balance = CBRiaTBL.Balance;
                                                    CBSRiaErrLogTBL.PullDate = DateTime.Now;
                                                    CBSRiaErrLogTBL.UserId = 1;
                                                    CBSRiaErrLogTBL.Refference = CBRiaTBL.Refference;
                                                    CBSRiaErrLogTBL.OrigRefNo = CBRiaTBL.Refference;
                                                    CBSRiaErrLogTBL.PtId = CBRiaTBL.PtId;
                                                    CBSRiaErrLogTBL.ErrorMsg = "Duplicate transaction record";

                                                    repoCBSRiaTransErrorRepository.Add(CBSRiaErrLogTBL);

                                                    var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret1)
                                                    {
                                                        continue;
                                                    }
                                                    continue;
                                                }
                                                else
                                                {
                                                    repoCBSRiaRepository.Add(CBRiaTBL);
                                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                        continue;
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                CBSRiaErrLogTBL.AcctNo = CBRiaTBL.AcctNo;
                                                CBSRiaErrLogTBL.Amount = CBRiaTBL.Amount;
                                                CBSRiaErrLogTBL.DebitCredit = CBRiaTBL.DebitCredit;
                                                CBSRiaErrLogTBL.Description = CBRiaTBL.Description;
                                                CBSRiaErrLogTBL.TransDate = CBRiaTBL.TransDate;
                                                CBSRiaErrLogTBL.OriginBranch = CBRiaTBL.OriginBranch;
                                                CBSRiaErrLogTBL.PostedBy = CBRiaTBL.PostedBy;
                                                CBSRiaErrLogTBL.ReversalCode = CBRiaTBL.ReversalCode;
                                                CBSRiaErrLogTBL.Balance = CBRiaTBL.Balance;
                                                CBSRiaErrLogTBL.PullDate = DateTime.Now;
                                                CBSRiaErrLogTBL.UserId = 1;
                                                CBSRiaErrLogTBL.Refference = CBRiaTBL.Refference;
                                                CBSRiaErrLogTBL.OrigRefNo = CBRiaTBL.Refference;
                                                CBSRiaErrLogTBL.PtId = CBRiaTBL.PtId;
                                                CBSRiaErrLogTBL.ErrorMsg = "No ref No.";

                                                repoCBSRiaTransErrorRepository.Add(CBSRiaErrLogTBL);

                                                var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret1)
                                                {
                                                    continue;
                                                }
                                                continue;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                                            var stackTrace = new StackTrace(ex);
                                            var thisasm = Assembly.GetExecutingAssembly();
                                            _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                                            _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                            LogManager.SaveLog("An error occured RiaTransLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                                            
                                            try
                                            {
                                                CBSRiaErrLogTBL.AcctNo = CBRiaTBL.AcctNo;
                                                CBSRiaErrLogTBL.Amount = CBRiaTBL.Amount;
                                                CBSRiaErrLogTBL.DebitCredit = CBRiaTBL.DebitCredit;
                                                CBSRiaErrLogTBL.Description = CBRiaTBL.Description;
                                                CBSRiaErrLogTBL.TransDate = CBRiaTBL.TransDate;
                                                CBSRiaErrLogTBL.OriginBranch = CBRiaTBL.OriginBranch;
                                                CBSRiaErrLogTBL.PostedBy = CBRiaTBL.PostedBy;
                                                CBSRiaErrLogTBL.Balance = CBRiaTBL.Balance;
                                                CBSRiaErrLogTBL.PullDate = DateTime.Now;
                                                CBSRiaErrLogTBL.UserId = 1;
                                                CBSRiaErrLogTBL.ReversalCode = CBRiaTBL.ReversalCode;
                                                CBSRiaErrLogTBL.Refference = CBRiaTBL.Refference;
                                                CBSRiaErrLogTBL.OrigRefNo = CBRiaTBL.Refference;
                                                CBSRiaErrLogTBL.PtId = CBRiaTBL.PtId;
                                                CBSRiaErrLogTBL.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;

                                                repoCBSRiaTransErrorRepository.Add(CBSRiaErrLogTBL);

                                                var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                                if (ret1)
                                                {
                                                    continue;
                                                }
                                            }
                                            catch (Exception ex1)
                                            {
                                                 exErr = ex1 == null ? ex1.InnerException.Message : ex1.Message;
                                                 stackTrace = new StackTrace(ex1);
                                                 thisasm = Assembly.GetExecutingAssembly();
                                                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                                                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                                LogManager.SaveLog("An error occured RiaTransLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                                                continue;
                                            }
                                            continue;
                                        }
                                    }
                                    if (controlTable != null)
                                    {
                                        controlTable.LastRecordId = LastRecordId;
                                        controlTable.LastRecordTimeStamp = DateTime.Now;
                                        DateTime dtf = new DateTime();
                                        string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                                        controlTable.LastTransDate = Convert.ToDateTime(dateee);
                                        controlTable.PullDate = DateTime.Now;
                                        controlTable.RecordsCount = returndata.Rows.Count;
                                        repoadmDataPoollingControlRepository.Update(controlTable);
                                        var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }
                                    }
                                    else
                                    {
                                        var admDataPoollingControlTBL = new admDataPullingControl();
                                        admDataPoollingControlTBL.ReconTypeId = rectype.ReconTypeId;
                                        admDataPoollingControlTBL.FileType = "Table";
                                        admDataPoollingControlTBL.TableName = "CBSRiaTrans";
                                        admDataPoollingControlTBL.DateCreated = DateTime.Now;
                                        admDataPoollingControlTBL.UserId = 1;
                                        admDataPoollingControlTBL.LastRecordTimeStamp = DateTime.Now;
                                        admDataPoollingControlTBL.LastRecordId = LastRecordId;
                                        DateTime dtf = new DateTime();
                                        string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                                        admDataPoollingControlTBL.LastTransDate = Convert.ToDateTime(dateee);
                                        admDataPoollingControlTBL.PullDate = DateTime.Now;
                                        admDataPoollingControlTBL.RecordsCount = returndata.Rows.Count;
                                        repoadmDataPoollingControlRepository.Add(admDataPoollingControlTBL);
                                        var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }
                                    }
                                }
                              
                                reader.Close();
                                theCons.Close();

                            }
                            catch (Exception ex)
                            {
                                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                                var stackTrace = new StackTrace(ex);
                                var thisasm = Assembly.GetExecutingAssembly();
                                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                LogManager.SaveLog("An error occured RiaTransLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                        LogManager.SaveLog("An error occured in Line RiaTransLibrary: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                    }
                }

                #endregion
                #endregion
                #region Source 2

                var dtSouceCon2 = await repoadmSourceAccountRepository.GetAsync(c => c.ReconTypeId == ReconTypeId && c.SourceName == "Source 2");

                #region //File Directory Below
                string TrandId = string.Empty;

                if (!string.IsNullOrWhiteSpace(dtSouceCon2.FileDirectory))
                {
                    try
                    {
                        string MaxTransDate = string.Empty;
                        string dateTest = string.Empty;

                        var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "Ria");
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "RiaTrans");

                        if (controlTable != null)
                        {
                            LastRecordId = controlTable.LastRecordId;
                        }
                        else
                        {
                            LastRecordId = "0";
                        }

                        var RiaTBL = new RiaTran();
                        DataTable dTable = new DataTable();
                        string FileDirectory = dtSouceCon2.FileDirectory;

                        DirectoryInfo d = new DirectoryInfo(FileDirectory);
                        List<FileInfo> DLIST = null;
                        DLIST = d.GetFiles("*" + ".xlsx").ToList();
                        DLIST.AddRange(d.GetFiles("*" + ".xls").ToList());
                        DLIST.AddRange(d.GetFiles("*" + ".XLSX").ToList());
                        List<FileInfo> ggg = null;// new List<FileInfo>();
                        if (controlTable != null)
                        {
                            ggg = DLIST;//.Where(c => c.LastAccessTime > controlTable.LastRecordTimeStamp).ToList();
                        }
                        else
                        {
                            ggg = DLIST;//.ToList();
                        }
                        string fileNamenAndType = string.Empty;
                        string FileLastTime = string.Empty;
                        int count = 0;
                        string countNoInserted = string.Empty;
                        foreach (var kl in ggg)
                        {

                            fileNamenAndType = (from f in DLIST orderby f.LastWriteTime ascending select kl).First().Name;

                            FileLastTime = (from f in DLIST orderby f.LastAccessTimeUtc descending select kl).First().LastWriteTime.ToString();

                            dTable = await library.ReadExcel(FileDirectory, ReconTypeId);

                            LogManager.SaveLog("No colum return :" + dTable.Columns.Count);

                            if (dTable.Rows.Count > 0)
                            {
                                int countTrans = 0;
                                foreach (var column in dTable.Columns.Cast<DataColumn>().ToArray())
                                {
                                    string colname = column.ColumnName;
                                    int colindex;
                                    string col = colname.Length == 2 ? colname.Substring(1, 1) : colname.Length > 2 ? colname.Substring(1, 2) : "";
                                    if (int.TryParse(col, out colindex))
                                    {
                                        dTable.Columns.Remove(column);
                                    }
                                }

                                foreach (DataRow row in dTable.Rows.Cast<DataRow>().Skip(2))
                                {
                                    try
                                    {
                                        decimal checDec = 0;
                                        DateTime dt = new DateTime();

                                        RiaTBL.TransDate = row[0] == null ? (DateTime?)null : DateTime.TryParse(row[0].ToString(), out dt) ? dt : (DateTime?)null;

                                        dateTest = RiaTBL.TransDate.ToString();
                                        if (countTrans > 1)
                                        {
                                            MaxTransDate = RiaTBL.TransDate.ToString();

                                            if (Convert.ToDateTime(dateTest) > Convert.ToDateTime(MaxTransDate))
                                            {
                                                MaxTransDate = dateTest;
                                            }
                                            else
                                            {
                                                MaxTransDate = MaxTransDate;
                                            }
                                        }

                                        countTrans += 1;

                                        if (DateTime.TryParse(RiaTBL.TransDate.ToString(), out dt))
                                        {

                                        }
                                        else
                                        {
                                            continue;
                                        }

                                        RiaTBL.TransactionNo = row[1] == null ? "" : row[1].ToString();
                                        RiaTBL.OrigRefNo = RiaTBL.TransactionNo;

                                        RiaTBL.TransPin = row[2] == null ? "" : row[2].ToString();
                                        RiaTBL.PayAbleAmount = row[3] == null ? (decimal?)null : Decimal.TryParse((row[3]).ToString(), out checDec) ? Math.Round(checDec, 2) : (decimal?)null;
                                        RiaTBL.Currency = row[4] == null ? "" : row[4].ToString();

                                        RiaTBL.CommissionUSD = row[5] == null ? (decimal?)null : Decimal.TryParse((row[5]).ToString(), out checDec) ? Math.Round(checDec, 2) : (decimal?)null;

                                        RiaTBL.BranchName = row[6] == null ? "" : row[6].ToString();
                                        RiaTBL.BranchNo = row[7] == null ? "" : row[7].ToString();
                                        RiaTBL.PaidBy = row[8] == null ? "" : row[8].ToString();
                                        RiaTBL.EnteredTime = row[9] == null ? (DateTime?)null : DateTime.TryParse(row[9].ToString(), out dt) ? dt : (DateTime?)null;
                                        RiaTBL.PaidTime = row[10] == null ? (DateTime?)null : DateTime.TryParse(row[10].ToString(), out dt) ? dt : (DateTime?)null;

                                        LogManager.SaveLog("PaidCurrency: " + row[11].ToString());

                                        RiaTBL.PaidCurrency = row[11] == null ? "" : row[11].ToString();
                                        RiaTBL.PaidAmount = row[12] == null ? (decimal?)null : Decimal.TryParse((row[12]).ToString(), out checDec) ? Math.Round(checDec, 2) : (decimal?)null;
                                        RiaTBL.Commission_2 = row[13] == null ? (decimal?)null : Decimal.TryParse((row[13]).ToString(), out checDec) ? Math.Round(checDec, 2) : (decimal?)null;
                                        RiaTBL.FxShare = row[14] == null ? (decimal?)null : Decimal.TryParse((row[14]).ToString(), out checDec) ? Math.Round(checDec, 2) : (decimal?)null;
                                        RiaTBL.Commission_2AndFxShare = row[15] == null ? (decimal?)null : Decimal.TryParse((row[15]).ToString(), out checDec) ? Math.Round(checDec, 2) : (decimal?)null;
                                        RiaTBL.BeneficiaryFirstName = row[16] == null ? "" : row[16].ToString();
                                        RiaTBL.BeneficiaryLastName1 = row[17] == null ? "" : row[17].ToString();
                                        RiaTBL.BeneficiaryLastName2 = row[18] == null ? "" : row[18].ToString();
                                        RiaTBL.BeneficiaryIdType = row[19] == null ? "" : row[19].ToString();
                                        RiaTBL.BeneficiaryIdNo = row[20] == null ? "" : row[20].ToString();
                                        RiaTBL.PaymentAmountUSD = row[21] == null ? (decimal?)null : Decimal.TryParse((row[21]).ToString(), out checDec) ? Math.Round(checDec, 2) : (decimal?)null;
                                        RiaTBL.PullDate = DateTime.Now;
                                        RiaTBL.MatchingStatus = "N";
                                        RiaTBL.UserId = 1;
                                        RiaTBL.FileName = fileNamenAndType;

                                        if (!string.IsNullOrWhiteSpace(RiaTBL.TransactionNo))
                                        {
                                            var exist = await repoRiaRepository.GetAsync(c => c.TransactionNo == RiaTBL.TransactionNo);
                                            var existHis = await repoRiaHistoryRepository.GetAsync(c => c.TransactionNo == RiaTBL.TransactionNo);
                                            if (exist != null || existHis != null)
                                            {
                                                var RiaErrTBL = new RiaTransError();

                                                RiaErrTBL.TransDate = RiaTBL.TransDate;
                                                RiaErrTBL.TransactionNo = RiaTBL.TransactionNo;
                                                RiaErrTBL.OrigRefNo = RiaTBL.TransactionNo;
                                                RiaErrTBL.TransPin = RiaTBL.TransPin;
                                                RiaErrTBL.PayAbleAmount = RiaTBL.PayAbleAmount;
                                                RiaErrTBL.Currency = RiaTBL.Currency;
                                                RiaErrTBL.CommissionUSD = RiaTBL.CommissionUSD;
                                                RiaErrTBL.BranchName = RiaTBL.BranchName;
                                                RiaErrTBL.BranchNo = RiaTBL.BranchNo;
                                                RiaErrTBL.PaidBy = RiaTBL.PaidBy;
                                                RiaErrTBL.EnteredTime = RiaTBL.EnteredTime;
                                                RiaErrTBL.PaidTime = RiaTBL.PaidTime;
                                                RiaErrTBL.PaidCurrency = RiaTBL.PaidCurrency;
                                                RiaErrTBL.PaidAmount = RiaTBL.PaidAmount;
                                                RiaErrTBL.Commission_2 = RiaTBL.Commission_2;
                                                RiaErrTBL.FxShare = RiaTBL.FxShare;
                                                RiaErrTBL.Commission_2AndFxShare = RiaTBL.Commission_2AndFxShare;
                                                RiaErrTBL.BeneficiaryFirstName = RiaTBL.BeneficiaryFirstName;
                                                RiaErrTBL.BeneficiaryLastName1 = RiaTBL.BeneficiaryLastName1;
                                                RiaErrTBL.BeneficiaryLastName2 = RiaTBL.BeneficiaryLastName2;
                                                RiaErrTBL.BeneficiaryIdType = RiaTBL.BeneficiaryIdType;
                                                RiaErrTBL.BeneficiaryIdNo = RiaTBL.BeneficiaryIdNo;
                                                RiaErrTBL.PaymentAmountUSD = RiaTBL.PaymentAmountUSD;
                                                RiaErrTBL.PullDate = DateTime.Now; ;
                                                RiaErrTBL.ErrorMsg = "Duplicate transaction record";
                                                RiaErrTBL.UserId = 1;
                                                RiaErrTBL.FileName = RiaTBL.FileName;

                                                repoRiaTransErrorRepository.Add(RiaErrTBL);
                                                var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                                if (ret1)
                                                {
                                                    continue;
                                                }
                                                continue;
                                            }
                                            else
                                            {
                                                repoRiaRepository.Add(RiaTBL);
                                                var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret)
                                                {
                                                    count += 1;
                                                    countNoInserted = count.ToString();
                                                    continue;
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
                                        LogManager.SaveLog("An error occured in RiaTransLogManager  Line  : " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr + "TransId : " + TrandId);

                                        try
                                        {
                                            var RiaErrTBL = new RiaTransError();

                                            RiaErrTBL.TransDate = RiaTBL.TransDate;
                                            RiaErrTBL.TransactionNo = RiaTBL.TransactionNo;
                                            RiaErrTBL.OrigRefNo = RiaTBL.TransactionNo;
                                            RiaErrTBL.TransPin = RiaTBL.TransPin;
                                            RiaErrTBL.PayAbleAmount = RiaTBL.PayAbleAmount;
                                            RiaErrTBL.Currency = RiaTBL.Currency;
                                            RiaErrTBL.CommissionUSD = RiaTBL.CommissionUSD;
                                            RiaErrTBL.BranchName = RiaTBL.BranchName;
                                            RiaErrTBL.BranchNo = RiaTBL.BranchNo;
                                            RiaErrTBL.PaidBy = RiaTBL.PaidBy;
                                            RiaErrTBL.EnteredTime = RiaTBL.EnteredTime;
                                            RiaErrTBL.PaidTime = RiaTBL.PaidTime;
                                            RiaErrTBL.PaidCurrency = RiaTBL.PaidCurrency;
                                            RiaErrTBL.PaidAmount = RiaTBL.PaidAmount;
                                            RiaErrTBL.Commission_2 = RiaTBL.Commission_2;
                                            RiaErrTBL.FxShare = RiaTBL.FxShare;
                                            RiaErrTBL.Commission_2AndFxShare = RiaTBL.Commission_2AndFxShare;
                                            RiaErrTBL.BeneficiaryFirstName = RiaTBL.BeneficiaryFirstName;
                                            RiaErrTBL.BeneficiaryLastName1 = RiaTBL.BeneficiaryLastName1;
                                            RiaErrTBL.BeneficiaryLastName2 = RiaTBL.BeneficiaryLastName2;
                                            RiaErrTBL.BeneficiaryIdType = RiaTBL.BeneficiaryIdType;
                                            RiaErrTBL.BeneficiaryIdNo = RiaTBL.BeneficiaryIdNo;
                                            RiaErrTBL.PaymentAmountUSD = RiaTBL.PaymentAmountUSD;
                                            RiaErrTBL.PullDate = DateTime.Now;
                                            RiaErrTBL.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;
                                            RiaErrTBL.UserId = 1;
                                            RiaErrTBL.FileName = RiaTBL.FileName;
                                            repoRiaTransErrorRepository.Add(RiaErrTBL);
                                            var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                            if (ret1)
                                            {
                                                continue;
                                            }
                                        }
                                        catch (Exception ex1)
                                        {
                                            var exErr1 = ex1 == null ? ex1.InnerException.Message : ex.Message;
                                            var stackTrace1 = new StackTrace(ex);
                                            var thisasm1 = Assembly.GetExecutingAssembly();
                                            _methodname = stackTrace1.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm1).Name;
                                            _lineErrorNumber = ex1.StackTrace.Substring(ex1.StackTrace.Length - 7, 7);
                                            LogManager.SaveLog("An error occured in RiaTransLibrary  Line  : " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr1 + "TransId : " + RiaTBL.TransactionNo);

                                            continue;
                                        }
                                        continue;
                                    }
                                }
                                //Moved file after reading below
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
                                var ret2 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                if (ret2)
                                {
                                    LogManager.SaveLog("Move Ria RiaTransLibrary File Start");
                                    string MvFile = library.MoveFile(FileDirectory + "\\" + fileNamenAndType, ReconType.ProcessedFileDirectory, fileNamenAndType);
                                    LogManager.SaveLog("Move File in Ria RiaTransLibrary status: " + MvFile);
                                }

                                if (controlTable != null)
                                {
                                    controlTable.LastRecordId = LastRecordId;
                                    controlTable.LastRecordTimeStamp = DateTime.Now;
                                    DateTime dtf = new DateTime();
                                    string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                                    controlTable.LastTransDate = Convert.ToDateTime(dateee);
                                    controlTable.PullDate = DateTime.Now;
                                    controlTable.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                                    int recTrack;
                                    controlTable.RecordsCount = countNoInserted == null ? 0 : int.TryParse(countNoInserted, out recTrack) ? recTrack : (int?)null;
                                    repoadmDataPoollingControlRepository.Update(controlTable);
                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                    if (ret)
                                    {
                                    }
                                }
                                else
                                {
                                    var admDataPoollingControlTBL = new admDataPullingControl();
                                    admDataPoollingControlTBL.ReconTypeId = rectype.ReconTypeId;
                                    admDataPoollingControlTBL.FileType = "File";
                                    admDataPoollingControlTBL.TableName = "RiaTrans";
                                    admDataPoollingControlTBL.DateCreated = DateTime.Now;
                                    admDataPoollingControlTBL.UserId = 1;
                                    admDataPoollingControlTBL.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                                    DateTime dtf = new DateTime();
                                    string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                                    admDataPoollingControlTBL.LastTransDate = Convert.ToDateTime(dateee);
                                    admDataPoollingControlTBL.PullDate = DateTime.Now;
                                    int recTrack;
                                    admDataPoollingControlTBL.RecordsCount = countNoInserted == null ? (int?)null : int.TryParse(countNoInserted, out recTrack) ? recTrack : (int?)null;
                                    repoadmDataPoollingControlRepository.Add(admDataPoollingControlTBL);
                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                    if (ret)
                                    {
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
                        LogManager.SaveLog("An error occured in RiaTransLibrary  Line  : " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr + "TransId : " + TrandId);
                    }
                }

                #endregion


                #endregion
              


                return string.Empty;
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in RiaTransLibrary in Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
            }

            return string.Empty;
        }

    }
}