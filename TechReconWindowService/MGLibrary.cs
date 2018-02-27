
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
    public class MGLibrary
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmReconDataSourcesRepository repoadmReconDataSourcesRepository;
        private readonly IadmSourceAccountRepository repoadmSourceAccountRepository;
        private readonly ICBSTransationTestRepository repoCBSTransationTestRepository;
        private readonly IPostillionTransationTestRepository repoPostillionTransationTestRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly ICBSMGTransRepository repoCBSMGTransRepository;
        private readonly ICBSMGTransErrorRepository repoCBSMGTransErrorRepository;
        private readonly IMGTransErrorRepository repoMGTransErrorRepository;
        private readonly IadmFileProcessedRepository repoadmFileProcessedRepository;
        private readonly IMGTransRepository repoMGTransRepository;
        private readonly ICBSMGHistoryRepository repoCBSMGHistoryRepository;
        private readonly IMGTRansHistoryRepository repoMGTRansHistoryRepository;
        private readonly IadmFailledFileRepository repoadmFailledFileRepository;

        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/MGLibrary.cs";
        private static string AcctNoStatic;
        private static string LegacyStatic;
        private static string SettleCurrStatic;
        private static string TransCurStatic;
        public MGLibrary()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmReconDataSourcesRepository = new admReconDataSourcesRepository(idbfactory);
            repoadmSourceAccountRepository = new admSourceAccountRepository(idbfactory);
            repoCBSTransationTestRepository = new CBSTransationTestRepository(idbfactory);
            repoPostillionTransationTestRepository = new PostillionTransationTestRepository(idbfactory);
            repoadmFileProcessedRepository = new admFileProcessedRepository(idbfactory);

            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoCBSMGTransRepository = new CBSMGTransRepository(idbfactory);
            repoCBSMGTransErrorRepository = new CBSMGTransErrorRepository(idbfactory);
            repoMGTransErrorRepository = new MGTransErrorRepository(idbfactory);
            repoMGTransRepository = new MGTransRepository(idbfactory);
            repoCBSMGHistoryRepository = new CBSMGHistoryRepository(idbfactory);
            repoMGTRansHistoryRepository = new MGTRansHistoryRepository(idbfactory);
            repoadmFailledFileRepository = new admFailledFileRepository(idbfactory);
        }

        public async Task<string> PullMoneyGramData()
        {
            Library library = new Library();
            try
            {
                LogManager.SaveLog("Task Start in MGLibrary");
                var ReconType = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "MoneyGram");
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
                        LogManager.SaveLog("Money Gram for Sybase Start in LogManager");
                        var value1 = string.Empty;
                        value1 = string.Empty;

                        var value2 = string.Empty;
                        value2 = string.Empty;
                        int scriptExecTtype = 0;

                        string FromDateParam = string.Empty;
                        string ToDateParam = string.Empty;
                        string ReconDate = string.Empty;
                        string MaxTransDate = string.Empty;
                        string dateTest = string.Empty;
                        var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "MoneyGram");
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSMGTrans");
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
                        if (controlTable != null)
                        {
                            LastRecordId = controlTable.LastRecordId; 
                        }
                        else
                        {
                            LastRecordId = "0";
                            ReconDate = "20170831";
                        }
                        string SqlString = ReconType.Source1Script.Replace("{AcctlistSource1}", acctlistSource1)
                                                                  .Replace("{ReconDate}", ReconDate)
                                                                  .Replace("{LastRecordId}", LastRecordId);

                        string scriptTest = SqlString;

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
                                    LogManager.SaveLog("Sybase Connection  open for Ip: " + dtSouceCon1.ipAddress);
                                }
                                else
                                { 
                                    LogManager.SaveLog("Sybase Connection not Open for Ip: " + dtSouceCon1.ipAddress);
                                }
                            }
                            catch (Exception ex)
                            {

                                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                                var stackTrace = new StackTrace(ex);
                                var thisasm = Assembly.GetExecutingAssembly();
                                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                LogManager.SaveLog("An error occured MGLibrary Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                                if (returndata.Rows.Count > 0)
                                {
                                    int countTrans = 0;
                                    var CBSMGTBL = new CBSMGTran();
                                    LogManager.SaveLog("Total records pulled record CBSMG: " + returndata.Rows.Count + "for the period from day: " + FromDateParam + "till :" + ToDateParam);
                                    for (int col = 0; col < returndata.Rows.Count; col++)
                                    {
                                        try
                                        {
                                            CBSMGTBL.AcctNo = returndata.Rows[col][0] == null ? null : returndata.Rows[col][0].ToString();
                                            CBSMGTBL.Amount = returndata.Rows[col][1] == null ? 0 : Math.Round(Convert.ToDecimal(returndata.Rows[col][1]), 2);
                                            CBSMGTBL.DebitCredit = returndata.Rows[col][2] == null ? null : returndata.Rows[col][2].ToString();
                                            CBSMGTBL.Description = returndata.Rows[col][3] == null ? null : returndata.Rows[col][3].ToString();
                                            CBSMGTBL.TransDate = returndata.Rows[col][4] == null ? (DateTime?)null : Convert.ToDateTime(returndata.Rows[col][4]);
                                            dateTest = CBSMGTBL.TransDate.ToString();
                                            if (countTrans > 1)
                                            {
                                                MaxTransDate = CBSMGTBL.TransDate.ToString();

                                                if (Convert.ToDateTime(dateTest) > Convert.ToDateTime(MaxTransDate))
                                                {
                                                    MaxTransDate = dateTest;

                                                }
                                                else
                                                {
                                                    MaxTransDate = MaxTransDate;
                                                }
                                            }
                                            CBSMGTBL.OriginBranch = returndata.Rows[col][5] == null ? null : returndata.Rows[col][5].ToString();
                                            CBSMGTBL.PostedBy = returndata.Rows[col][6] == null ? null : returndata.Rows[col][6].ToString();
                                            CBSMGTBL.Balance = returndata.Rows[col][7] == null ? 0 : Math.Round(Convert.ToDecimal(returndata.Rows[col][7]), 2);
                                            CBSMGTBL.Reference = returndata.Rows[col][8] == null ? null : returndata.Rows[col][8].ToString();
                                            CBSMGTBL.OrigRefNo = CBSMGTBL.Reference;
                                            CBSMGTBL.PtId = returndata.Rows[col][9] == null ? null : returndata.Rows[col][9].ToString();
                                            LastRecordId = CBSMGTBL.PtId;
                                            int revcode;
                                            CBSMGTBL.ReversalCode = returndata.Rows[col][10] == null ? (int?)null : int.TryParse(returndata.Rows[col][10].ToString(), out revcode) ? revcode : (int?) null ;
                                            CBSMGTBL.PullDate = DateTime.Now;
                                            CBSMGTBL.MatchingStatus = "N";
                                            CBSMGTBL.UserId = 1;
                                            countTrans += 1;
                                            if (!string.IsNullOrWhiteSpace(CBSMGTBL.Reference))
                                            {
                                                var exist = await repoCBSMGTransRepository.GetAsync(c => c.Reference == CBSMGTBL.Reference && (c.DebitCredit == CBSMGTBL.DebitCredit) && (c.Amount == CBSMGTBL.Amount) && (c.AcctNo == CBSMGTBL.AcctNo));
                                                var existHis = await repoCBSMGHistoryRepository.GetAsync(c => c.Reference == CBSMGTBL.Reference && (c.DebitCredit == CBSMGTBL.DebitCredit) && (c.Amount == CBSMGTBL.Amount) && (c.AcctNo == CBSMGTBL.AcctNo));
                                                if (exist != null || existHis != null)
                                                {
                                                    var CBSMGTransErrorTBL = new CBSMGTransError();

                                                    CBSMGTransErrorTBL.AcctNo = CBSMGTBL.AcctNo;
                                                    CBSMGTransErrorTBL.Amount = CBSMGTBL.Amount;
                                                    CBSMGTransErrorTBL.DebitCredit = CBSMGTBL.DebitCredit;
                                                    CBSMGTransErrorTBL.Description = CBSMGTBL.Description;
                                                    CBSMGTransErrorTBL.TransDate = CBSMGTBL.TransDate;
                                                    CBSMGTransErrorTBL.OriginBranch = CBSMGTBL.OriginBranch;
                                                    CBSMGTransErrorTBL.PostedBy = CBSMGTBL.PostedBy;
                                                    CBSMGTransErrorTBL.Balance = CBSMGTBL.Balance;
                                                    CBSMGTransErrorTBL.Reference = CBSMGTBL.Reference;
                                                    CBSMGTransErrorTBL.OrigRefNo = CBSMGTBL.Reference;
                                                    CBSMGTransErrorTBL.ReversalCode = CBSMGTBL.ReversalCode;
                                                    CBSMGTransErrorTBL.PtId = CBSMGTBL.PtId;
                                                    LastRecordId = CBSMGTBL.PtId;
                                                    CBSMGTransErrorTBL.PullDate = DateTime.Now;
                                                    CBSMGTransErrorTBL.UserId = 1;
                                                    CBSMGTransErrorTBL.ErrorMsg = "Duplicate transaction record";

                                                    repoCBSMGTransErrorRepository.Add(CBSMGTransErrorTBL);
                                                    var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret1)
                                                    {
                                                        continue;
                                                    }
                                                    continue;
                                                }

                                                repoCBSMGTransRepository.Add(CBSMGTBL);
                                                var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret)
                                                {
                                                }
                                            }
                                            else
                                            {
                                                var CBSMGTransErrorTBL = new CBSMGTransError();

                                                CBSMGTransErrorTBL.AcctNo = CBSMGTBL.AcctNo;
                                                CBSMGTransErrorTBL.Amount = CBSMGTBL.Amount;
                                                CBSMGTransErrorTBL.DebitCredit = CBSMGTBL.DebitCredit;
                                                CBSMGTransErrorTBL.Description = CBSMGTBL.Description;
                                                CBSMGTransErrorTBL.TransDate = CBSMGTBL.TransDate;
                                                CBSMGTransErrorTBL.OriginBranch = CBSMGTBL.OriginBranch;
                                                CBSMGTransErrorTBL.PostedBy = CBSMGTBL.PostedBy;
                                                CBSMGTransErrorTBL.Balance = CBSMGTBL.Balance;
                                                CBSMGTransErrorTBL.Reference = CBSMGTBL.Reference;
                                                CBSMGTransErrorTBL.OrigRefNo = CBSMGTBL.Reference;
                                                CBSMGTransErrorTBL.PtId = CBSMGTBL.PtId;
                                                CBSMGTransErrorTBL.ReversalCode = CBSMGTBL.ReversalCode;
                                                LastRecordId = CBSMGTBL.PtId;
                                                CBSMGTransErrorTBL.PullDate = DateTime.Now;
                                                CBSMGTransErrorTBL.UserId = 1;
                                                CBSMGTransErrorTBL.ErrorMsg = "No transaction reference";

                                                repoCBSMGTransErrorRepository.Add(CBSMGTransErrorTBL);
                                                var ret1 = await  unitOfWork.Commit(0, null) > 0 ? true : false;
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
                                            LogManager.SaveLog("An error occured Library Money Gram Sybase 2 in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                                            try
                                            {
                                                var CBSMGTransErrorTBL = new CBSMGTransError();

                                                CBSMGTransErrorTBL.AcctNo = CBSMGTBL.AcctNo;
                                                CBSMGTransErrorTBL.Amount = CBSMGTBL.Amount;
                                                CBSMGTransErrorTBL.DebitCredit = CBSMGTBL.DebitCredit;
                                                CBSMGTransErrorTBL.Description = CBSMGTBL.Description;
                                                CBSMGTransErrorTBL.TransDate = CBSMGTBL.TransDate;
                                                CBSMGTransErrorTBL.OriginBranch = CBSMGTBL.OriginBranch;
                                                CBSMGTransErrorTBL.PostedBy = CBSMGTBL.PostedBy;
                                                CBSMGTransErrorTBL.Balance = CBSMGTBL.Balance;
                                                CBSMGTransErrorTBL.Reference = CBSMGTBL.Reference;
                                                CBSMGTransErrorTBL.OrigRefNo = CBSMGTBL.Reference;
                                                CBSMGTransErrorTBL.PtId = CBSMGTBL.PtId;
                                                CBSMGTransErrorTBL.ReversalCode = CBSMGTBL.ReversalCode;
                                                LastRecordId = CBSMGTBL.PtId;
                                                CBSMGTransErrorTBL.PullDate = DateTime.Now;
                                                LastRecordId = CBSMGTBL.PtId;
                                                CBSMGTransErrorTBL.UserId = 1;
                                                CBSMGTransErrorTBL.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;

                                                repoCBSMGTransErrorRepository.Add(CBSMGTransErrorTBL);
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
                                                LogManager.SaveLog("An error occured Library Money Gram Sybase 2 in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

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
                                        admDataPoollingControlTBL.TableName = "CBSMGTrans";
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
                                LogManager.SaveLog("An error occured MGLibrary Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                        LogManager.SaveLog("An error occured in Line MGLibrary : " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                        throw;
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
                        string MaxTransDate = string.Empty;
                        string dateTest = string.Empty;
                        var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "MoneyGram");
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "MGTrans");

                        if (controlTable != null)
                        {
                            LastRecordId = controlTable.LastRecordId;
                        }
                        else
                        {
                            LastRecordId = "0";
                        }

                        var MoneyGramTransTBL = new MGTran();
                        DataTable dTable = new DataTable();
                        string FileDirectory = dtSouceCon2.FileDirectory;

                        DirectoryInfo d = new DirectoryInfo(FileDirectory);
                        List<FileInfo> DLIST = null;
                        DLIST = d.GetFiles("*" + ".xlsx").ToList();
                        DLIST.AddRange(d.GetFiles("*" + ".xls").ToList());
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
                        string recordAddFile = string.Empty;
                        foreach (var kl in ggg)
                        {
                            fileNamenAndType = (from f in DLIST orderby f.LastWriteTime ascending select kl).First().Name;
                            FileLastTime = (from f in DLIST orderby f.LastAccessTimeUtc descending select kl).First().LastWriteTime.ToString();

                            var dtTable = await ReadExcelMG(FileDirectory, ReconTypeId, fileNamenAndType);

                            if (dtTable.Rows.Count > 0)
                            {
                                //foreach (var column in dTable.Columns.Cast<DataColumn>().ToArray())
                                //{
                                //    string colname = column.ColumnName;
                                //    int colindex;
                                //    string col = colname.Length == 2 ? colname.Substring(1, 1) : colname.Length > 2 ? colname.Substring(1, 2) : "";
                                //    if (int.TryParse(col, out colindex))
                                //    {
                                //        dTable.Columns.Remove(column);
                                //    }
                                //}

                                var MGTBL = new MGTran();
                                var MGTransErrorErrTBL = new MGTransError();
                                string RowRecord = string.Empty;

                                int LastColum = dtTable.Columns.Count;
                                int insercount = 0;
                                int countTrans = 0;
                                for (int row = 0; row < dtTable.Rows.Count; row++)
                                {
                                    int count = 0;
                                    try
                                    {
                                        DateTime dt = new DateTime();
                                        for (int col = 0; col < LastColum; col++)
                                        {
                                            RowRecord += dtTable.Rows[row][col] == null ? string.Empty : dtTable.Rows[row][col].ToString() + ";";

                                            if (col == LastColum)
                                            {
                                                break;
                                            }
                                        }

                                        string[] breakReco = RowRecord.Split(';');

                                        var dtList = breakReco.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                                        var con = dtList.Take(1);

                                        if (dtList.Count < 15)
                                        {
                                            int getcountforme = 15 - dtList.Count;

                                            for (int h = 0; h < getcountforme; h++)
                                            {
                                                var concat = dtList.Concat(con);
                                                dtList = concat.ToList();
                                                if (dtList.Count() == 15)
                                                {
                                                    break;
                                                }
                                            }
                                        }

                                        decimal decVal = 0;

                                        for (var b = 0; b < 1; b++)
                                        {
                                            if (dtList.Count() == 15)
                                            {
                                                if (dtList[b].Contains("Settlement Currency".Trim()))
                                                {
                                                    MGTBL.SettlementCurrency = dtList[b + 1];
                                                    MGTBL.TransactionCurrency = dtList[b + 3];
                                                }
                                                if (dtList[b].Contains("Account Number".Trim()))
                                                {
                                                    string[] acctNo = dtList[b + 1].Split(' ');
                                                    MGTBL.AccountNo = acctNo[0];
                                                }
                                                if (dtList[b].Contains("Legacy ID".Trim()))
                                                {
                                                    MGTBL.LegacyID = dtList[b + 1];
                                                }
                                            }

                                            if (dtList.Count() == 15)
                                            {
                                                if (DateTime.TryParse(dtList[b], out dt))
                                                {
                                                    MGTBL.TransactionDate = dt;
                                                    dateTest = MGTBL.TransactionDate.ToString();
                                                    if (countTrans > 1)
                                                    {
                                                        MaxTransDate = MGTBL.TransactionDate.ToString();

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
                                                    MGTBL.TransactionId = dtList[b + 1];
                                                    MGTBL.RefNo = dtList[b + 2];
                                                    MGTBL.OrigRefNo = MGTBL.RefNo;
                                                    MGTBL.Prod = dtList[b + 3];
                                                    MGTBL.TransactionType = dtList[b + 4];
                                                    MGTBL.OrigCountry = dtList[b + 5];
                                                    MGTBL.RevCountry = dtList[b + 6];
                                                    MGTBL.FXRate = dtList[b + 7] == null ? Math.Round(Convert.ToDecimal("0"), 11) : decimal.TryParse(dtList[b + 7], out decVal) ? Math.Round(Convert.ToDecimal(dtList[b + 7]), 11) : Math.Round(Convert.ToDecimal("0"), 11);

                                                    MGTBL.FXDate = dtList[b + 8] == null ? (DateTime?)null : DateTime.TryParse(dtList[b + 8], out dt) ? dt : MGTBL.TransactionDate;
                                                    MGTBL.FXMargin = dtList[b + 9] == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(dtList[b + 9], out decVal) ? Math.Round(Convert.ToDecimal(dtList[b + 9]), 2) : Math.Round(Convert.ToDecimal("0"), 2);
                                                    MGTBL.Direction = "trn";
                                                    if (dtList[b + 11].Contains("("))
                                                    {
                                                        string[] minusSignbolReplace = dtList[b + 11].Split('(', ')');
                                                        dtList[b + 11] = "-" + minusSignbolReplace[1];
                                                    }
                                                    MGTBL.BaseAmount = dtList[b + 11] == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(dtList[b + 11], out decVal) ? Math.Round(Convert.ToDecimal(dtList[b + 11]), 2) : Math.Round(Convert.ToDecimal("0"), 2);
                                                    MGTBL.FeeAmount = dtList[b + 12] == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(dtList[b + 12], out decVal) ? Math.Round(Convert.ToDecimal(dtList[b + 12]), 2) : Math.Round(Convert.ToDecimal("0"), 2);

                                                    if (dtList[b + 13].Contains("("))
                                                    {
                                                        string[] minusSignbolReplace = dtList[b + 13].Split('(', ')');
                                                        dtList[b + 13] = "-" + minusSignbolReplace[1];
                                                    }

                                                    MGTBL.ShareAmount = dtList[b + 13] == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(dtList[b + 13], out decVal) ? Math.Round(Convert.ToDecimal(dtList[b + 13]), 2) : Math.Round(Convert.ToDecimal("0"), 2);

                                                    if (recordAddFile == "8")
                                                    {
                                                    }
                                                    if (dtList[b + 14].Contains("("))
                                                    {
                                                        string[] minusSignbolReplace = dtList[b + 14].Split('(', ')');
                                                        dtList[b + 14] = "-" + minusSignbolReplace[1];
                                                    }

                                                    MGTBL.CommissionAmount = dtList[b + 14] == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(dtList[b + 14], out decVal) ? Math.Round(Convert.ToDecimal(dtList[b + 14]), 2) : Math.Round(Convert.ToDecimal("0"), 2);
                                                    insercount += 1;
                                                    continue;
                                                }
                                            }
                                            if (dtList.Count() == 15)
                                            {
                                                if (insercount == 1)
                                                {
                                                    if (decimal.TryParse(dtList[b], out decVal))
                                                    {
                                                        MGTBL.FXExchngRate = dtList[b] == null ? Math.Round(Convert.ToDecimal("0"), 11) : decimal.TryParse(dtList[b], out decVal) ? Math.Round(Convert.ToDecimal(dtList[b]), 11) : Math.Round(Convert.ToDecimal("0"), 11);
                                                        MGTBL.FxExchangeDate = dtList[b + 1] == null ? (DateTime?)null : DateTime.TryParse(dtList[b + 1], out dt) ? dt : (DateTime?)null;
                                                        MGTBL.XchangeDirection = dtList[b + 2] == null ? null : dtList[b + 2];
                                                        if (dtList[b + 3].Contains("("))
                                                        {
                                                            string[] minusSignbolReplace = dtList[b + 3].Split('(', ')');
                                                            dtList[b + 3] = "-" + minusSignbolReplace[1];
                                                        }

                                                        MGTBL.ExchangeAmount = dtList[b + 3] == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(dtList[b + 3], out decVal) ? Math.Round(Convert.ToDecimal(dtList[b + 3]), 2) : Math.Round(Convert.ToDecimal("0"), 2);
                                                        MGTBL.ExchangeFeeAmount = dtList[b + 4] == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(dtList[b + 4], out decVal) ? Math.Round(Convert.ToDecimal(dtList[b + 4]), 2) : Math.Round(Convert.ToDecimal("0"), 2);

                                                        if (dtList[b + 5].Contains("("))
                                                        {
                                                            string[] minusSignbolReplace = dtList[b + 5].Split('(', ')');
                                                            dtList[b + 5] = "-" + minusSignbolReplace[1];
                                                        }

                                                        MGTBL.ExchangeShareAmount = dtList[b + 5] == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(dtList[b + 5], out decVal) ? Math.Round(Convert.ToDecimal(dtList[b + 5]), 2) : Math.Round(Convert.ToDecimal("0"), 2);

                                                        if (dtList[b + 6].Contains("("))
                                                        {
                                                            string[] minusSignbolReplace = dtList[b + 6].Split('(', ')');
                                                            dtList[b + 6] = "-" + minusSignbolReplace[1];
                                                        }

                                                        MGTBL.ExchangeComAmount = dtList[b + 6] == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(dtList[b + 6], out decVal) ? Math.Round(Convert.ToDecimal(dtList[b + 6]), 2) : Math.Round(Convert.ToDecimal("0"), 2);

                                                        MGTBL.OrigRefNo = MGTBL.TransactionId;
                                                    }

                                                    if (!string.IsNullOrWhiteSpace(MGTBL.TransactionId))
                                                    {
                                                        var exist = await repoMGTransRepository.GetAsync(c => c.TransactionId == MGTBL.TransactionId);
                                                        var existHis = await repoMGTRansHistoryRepository.GetAsync(c => c.TransactionId == MGTBL.TransactionId);

                                                        if (exist != null || existHis != null)
                                                        {
                                                            MGTransErrorErrTBL.SettlementCurrency = MGTBL.SettlementCurrency;
                                                            MGTransErrorErrTBL.TransactionCurrency = MGTBL.TransactionCurrency;
                                                            MGTransErrorErrTBL.AccountNo = MGTBL.AccountNo;
                                                            MGTransErrorErrTBL.LegacyID = MGTBL.LegacyID;
                                                            MGTransErrorErrTBL.TransactionDate = MGTBL.TransactionDate;
                                                            MGTransErrorErrTBL.TransactionId = MGTBL.TransactionId;
                                                            MGTransErrorErrTBL.OrigRefNo = MGTBL.OrigRefNo;
                                                            MGTransErrorErrTBL.RefNo = MGTBL.RefNo;
                                                            MGTransErrorErrTBL.Prod = MGTBL.Prod;
                                                            MGTransErrorErrTBL.TransactionType = MGTBL.TransactionType;
                                                            MGTransErrorErrTBL.OrigCountry = MGTBL.OrigCountry;
                                                            MGTransErrorErrTBL.RevCountry = MGTBL.RevCountry;
                                                            MGTransErrorErrTBL.FXRate = MGTBL.FXRate;
                                                            MGTransErrorErrTBL.FXExchngRate = MGTBL.FXExchngRate;
                                                            MGTransErrorErrTBL.FXDate = MGTBL.FXDate;
                                                            MGTransErrorErrTBL.FxExchangeDate = MGTBL.FxExchangeDate;
                                                            MGTransErrorErrTBL.FXMargin = MGTBL.FXMargin;
                                                            MGTransErrorErrTBL.Direction = MGTBL.Direction;
                                                            MGTransErrorErrTBL.XchangeDirection = MGTBL.XchangeDirection;
                                                            MGTransErrorErrTBL.BaseAmount = MGTBL.BaseAmount;
                                                            MGTransErrorErrTBL.ExchangeAmount = MGTBL.ExchangeAmount;
                                                            MGTransErrorErrTBL.FeeAmount = MGTBL.FeeAmount;
                                                            MGTransErrorErrTBL.ExchangeFeeAmount = MGTBL.ExchangeFeeAmount;
                                                            MGTransErrorErrTBL.ShareAmount = MGTBL.ShareAmount;
                                                            MGTransErrorErrTBL.ExchangeShareAmount = MGTBL.ExchangeShareAmount;
                                                            MGTransErrorErrTBL.CommissionAmount = MGTBL.CommissionAmount;
                                                            MGTransErrorErrTBL.ExchangeComAmount = MGTBL.ExchangeComAmount;
                                                            MGTransErrorErrTBL.PullDate = DateTime.Now;
                                                            MGTransErrorErrTBL.UserId = 1;
                                                            MGTransErrorErrTBL.ErrorMsg = "Duplicate transaction record";
                                                            MGTransErrorErrTBL.FileName = MGTBL.FileName;
                                                            repoMGTransErrorRepository.Add(MGTransErrorErrTBL);
                                                            var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                            if (ret1)
                                                            {
                                                                insercount = 0;
                                                                continue;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MGTBL.FileName = fileNamenAndType;
                                                            MGTBL.PullDate = DateTime.Now;
                                                            MGTBL.UserId = 1;
                                                            MGTBL.MatchingStatus = "N";
                                                            repoMGTransRepository.Add(MGTBL);
                                                            var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                            if (ret)
                                                            {
                                                                insercount = 0;
                                                                count += 1;
                                                                recordAddFile = count.ToString();
                                                                continue;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MGTransErrorErrTBL.SettlementCurrency = MGTBL.SettlementCurrency;
                                                        MGTransErrorErrTBL.TransactionCurrency = MGTBL.TransactionCurrency;
                                                        MGTransErrorErrTBL.AccountNo = MGTBL.AccountNo;
                                                        MGTransErrorErrTBL.LegacyID = MGTBL.LegacyID;
                                                        MGTransErrorErrTBL.TransactionDate = MGTBL.TransactionDate;
                                                        MGTransErrorErrTBL.TransactionId = MGTBL.TransactionId;
                                                        MGTransErrorErrTBL.OrigRefNo = MGTBL.OrigRefNo;
                                                        MGTransErrorErrTBL.RefNo = MGTBL.RefNo;
                                                        MGTransErrorErrTBL.Prod = MGTBL.Prod;
                                                        MGTransErrorErrTBL.TransactionType = MGTBL.TransactionType;
                                                        MGTransErrorErrTBL.OrigCountry = MGTBL.OrigCountry;
                                                        MGTransErrorErrTBL.RevCountry = MGTBL.RevCountry;
                                                        MGTransErrorErrTBL.FXRate = MGTBL.FXRate;
                                                        MGTransErrorErrTBL.FXExchngRate = MGTBL.FXExchngRate;
                                                        MGTransErrorErrTBL.FXDate = MGTBL.FXDate;
                                                        MGTransErrorErrTBL.FxExchangeDate = MGTBL.FxExchangeDate;
                                                        MGTransErrorErrTBL.FXMargin = MGTBL.FXMargin;
                                                        MGTransErrorErrTBL.Direction = MGTBL.Direction;
                                                        MGTransErrorErrTBL.XchangeDirection = MGTBL.XchangeDirection;
                                                        MGTransErrorErrTBL.BaseAmount = MGTBL.BaseAmount;
                                                        MGTransErrorErrTBL.ExchangeAmount = MGTBL.ExchangeAmount;
                                                        MGTransErrorErrTBL.FeeAmount = MGTBL.FeeAmount;
                                                        MGTransErrorErrTBL.ExchangeFeeAmount = MGTBL.ExchangeFeeAmount;
                                                        MGTransErrorErrTBL.ShareAmount = MGTBL.ShareAmount;
                                                        MGTransErrorErrTBL.ExchangeShareAmount = MGTBL.ExchangeShareAmount;
                                                        MGTransErrorErrTBL.CommissionAmount = MGTBL.CommissionAmount;
                                                        MGTransErrorErrTBL.ExchangeComAmount = MGTBL.ExchangeComAmount;
                                                        MGTransErrorErrTBL.PullDate = DateTime.Now;
                                                        MGTransErrorErrTBL.UserId = 1;
                                                        MGTransErrorErrTBL.ErrorMsg = "No trans Ref";
                                                        MGTransErrorErrTBL.FileName = MGTBL.FileName;
                                                        repoMGTransErrorRepository.Add(MGTransErrorErrTBL);
                                                        var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                        if (ret1)
                                                        {
                                                            count = 0;
                                                            continue;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        string ggg1 = RowRecord;
                                        RowRecord = string.Empty;
                                        continue;

                                    }
                                    catch (Exception ex)
                                    {
                                        var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                                        var stackTrace = new StackTrace(ex);
                                        var thisasm = Assembly.GetExecutingAssembly();
                                        _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                                        _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                        LogManager.SaveLog("An error occured in Line MGLibrary: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                                        MGTransErrorErrTBL.SettlementCurrency = MGTBL.SettlementCurrency;
                                        MGTransErrorErrTBL.TransactionCurrency = MGTBL.TransactionCurrency;
                                        MGTransErrorErrTBL.AccountNo = MGTBL.AccountNo;
                                        MGTransErrorErrTBL.LegacyID = MGTBL.LegacyID;
                                        MGTransErrorErrTBL.TransactionDate = MGTBL.TransactionDate;
                                        MGTransErrorErrTBL.TransactionId = MGTBL.TransactionId;
                                        MGTransErrorErrTBL.OrigRefNo = MGTBL.OrigRefNo;
                                        MGTransErrorErrTBL.RefNo = MGTBL.RefNo;
                                        MGTransErrorErrTBL.Prod = MGTBL.Prod;
                                        MGTransErrorErrTBL.TransactionType = MGTBL.TransactionType;
                                        MGTransErrorErrTBL.OrigCountry = MGTBL.OrigCountry;
                                        MGTransErrorErrTBL.RevCountry = MGTBL.RevCountry;
                                        MGTransErrorErrTBL.FXRate = MGTBL.FXRate;
                                        MGTransErrorErrTBL.FXExchngRate = MGTBL.FXExchngRate;
                                        MGTransErrorErrTBL.FXDate = MGTBL.FXDate;
                                        MGTransErrorErrTBL.FxExchangeDate = MGTBL.FxExchangeDate;
                                        MGTransErrorErrTBL.FXMargin = MGTBL.FXMargin;
                                        MGTransErrorErrTBL.Direction = MGTBL.Direction;
                                        MGTransErrorErrTBL.XchangeDirection = MGTBL.XchangeDirection;
                                        MGTransErrorErrTBL.BaseAmount = MGTBL.BaseAmount;
                                        MGTransErrorErrTBL.ExchangeAmount = MGTBL.ExchangeAmount;
                                        MGTransErrorErrTBL.FeeAmount = MGTBL.FeeAmount;
                                        MGTransErrorErrTBL.ExchangeFeeAmount = MGTBL.ExchangeFeeAmount;
                                        MGTransErrorErrTBL.ShareAmount = MGTBL.ShareAmount;
                                        MGTransErrorErrTBL.ExchangeShareAmount = MGTBL.ExchangeShareAmount;
                                        MGTransErrorErrTBL.CommissionAmount = MGTBL.CommissionAmount;
                                        MGTransErrorErrTBL.ExchangeComAmount = MGTBL.ExchangeComAmount;
                                        MGTransErrorErrTBL.PullDate = DateTime.Now;
                                        MGTransErrorErrTBL.UserId = 1;
                                        MGTransErrorErrTBL.ErrorMsg = exErr;
                                        MGTransErrorErrTBL.FileName = MGTBL.FileName;
                                        repoMGTransErrorRepository.Add(MGTransErrorErrTBL);
                                        var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                        if (ret1)
                                        {
                                            count = 0;
                                            continue;
                                        }
                                    }
                                }
                            }

                            var proceTBL = new FileProcessControl();

                            proceTBL.ReconTypeId = ReconTypeId;
                            proceTBL.FileDirectory = FileDirectory;
                            proceTBL.FileName = fileNamenAndType;
                            proceTBL.NameConvention = rectype.FileNamingConvention;
                            proceTBL.DateProcessed = DateTime.Now;
                            proceTBL.Status = "Processed";
                            proceTBL.DateCreated = DateTime.Now;
                            proceTBL.UserId = 1;

                            repoadmFileProcessedRepository.Add(proceTBL);
                            var ret2 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                            if (ret2)
                            {
                                LogManager.SaveLog("Move  MGTrans File Start");
                                string MvFile = library.MoveFile(FileDirectory + "\\" + fileNamenAndType, ReconType.ProcessedFileDirectory, fileNamenAndType);
                                LogManager.SaveLog("Move File in MGTrans status: " + MvFile);
                            }
                        }
                        if (controlTable != null)
                        {
                            int Intchk;
                            controlTable.RecordsCount = recordAddFile == null ? 0 : int.TryParse(recordAddFile, out Intchk) ? Convert.ToInt32(recordAddFile) : 0;
                            controlTable.LastRecordId = LastRecordId;
                            controlTable.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                            DateTime dtf = new DateTime();
                            string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                            controlTable.LastTransDate = Convert.ToDateTime(dateee);
                            controlTable.PullDate = DateTime.Now;
                            controlTable.RecordsCount = dTable.Rows.Count;
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
                            admDataPoollingControlTBL.TableName = "MGTrans";
                            admDataPoollingControlTBL.DateCreated = DateTime.Now;
                            admDataPoollingControlTBL.UserId = 1;
                            admDataPoollingControlTBL.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                            DateTime dtf = new DateTime();
                            string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                            admDataPoollingControlTBL.LastTransDate = Convert.ToDateTime(dateee);
                            admDataPoollingControlTBL.PullDate = DateTime.Now;
                            admDataPoollingControlTBL.RecordsCount = Convert.ToInt32(recordAddFile);
                            repoadmDataPoollingControlRepository.Add(admDataPoollingControlTBL);
                            var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                            if (ret)
                            {
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
                        LogManager.SaveLog("An error occured in Line MGLibrary: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

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
                LogManager.SaveLog("An error occured in Line MGLibrary: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                throw;
            }
           
        }
        public async Task<DataTable> ReadExcelMG(string FileDirectory, int ReconTypeId, string fileNamenAndType)
        {
            Library library = new Library();
            DataTable dt = new DataTable();
            try
            {
                var getReconTypeInfo = await repoReconTypeRepository.GetAsync(c => c.ReconTypeId == ReconTypeId);
                if (!fileNamenAndType.Contains(getReconTypeInfo.FileNamingConvention))
                {
                    var failFileTBL = new FailledFile();
                    failFileTBL.ReconTypeId = ReconTypeId;
                    failFileTBL.FileDirectory = FileDirectory;
                    failFileTBL.FileName = fileNamenAndType;
                    failFileTBL.NameConvention = getReconTypeInfo.FileNamingConvention;
                    failFileTBL.DateProcessed = DateTime.Now;
                    failFileTBL.ErrCode = -1;
                    failFileTBL.ErrText = "Unrecognized file name";
                    failFileTBL.DateCreated = DateTime.Now;
                    failFileTBL.UserId = 1;
                    repoadmFailledFileRepository.Add(failFileTBL);

                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                        string MvFile = library.MoveFile(FileDirectory + "\\" + fileNamenAndType, getReconTypeInfo.RejectedFileDirectory, fileNamenAndType);
                        LogManager.SaveLog("Move File in MGLibrary  ReadExcelMG: " + MvFile);
                        return dt;
                    }
                }
                else
                {
                    var chkfile = await repoadmFileProcessedRepository.GetAsync(c => c.FileName == fileNamenAndType);
                    if (chkfile != null)
                    {
                        var failFileTBL = new FailledFile();
                        failFileTBL.ReconTypeId = ReconTypeId;
                        failFileTBL.FileDirectory = FileDirectory;
                        failFileTBL.FileName = fileNamenAndType;
                        failFileTBL.NameConvention = getReconTypeInfo.FileNamingConvention;
                        failFileTBL.DateProcessed = DateTime.Now;
                        failFileTBL.ErrCode = -1;
                        failFileTBL.ErrText = "Couldn't processed the same file more than once";
                        failFileTBL.DateCreated = DateTime.Now;
                        failFileTBL.UserId = 1;
                        repoadmFailledFileRepository.Add(failFileTBL);

                        var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                            string MvFile = library.MoveFile(FileDirectory + "\\" + fileNamenAndType, getReconTypeInfo.RejectedFileDirectory, fileNamenAndType);
                            LogManager.SaveLog("Move File in MGLibrary  ReadExcelMG: " + MvFile);

                            return dt;
                        }
                    }
                }

                OleDbConnection excelConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileDirectory + "\\" + fileNamenAndType + ";Extended Properties='Excel 12.0 xml;HDR=YES;'");

                excelConnection.Open();

                DataTable Sheets = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string sht = "";
                foreach (DataRow dr in Sheets.Rows)
                {
                    sht = dr[2].ToString().Replace("'", "");
                    // if (sht == "Sheet1$")
                    // {
                    OleDbCommand cmd = new OleDbCommand("select * from [" + sht + "]", excelConnection);
                   

                    SqlDataAdapter da = new SqlDataAdapter();

                    //excelConnection.Open();
                    OleDbDataReader dReader;
                    dReader = cmd.ExecuteReader();

                    //dt = dReader.GetSchemaTable(); We can use this one to get the column table name
                    dt.Load(dReader);
                    //int count = 0;


                    return dt;
                }

                return dt;
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in  MGLibrary ReadExcel Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                throw;
            }


            return dt;

        }
    }
    
}