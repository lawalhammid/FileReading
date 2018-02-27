
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
    public class ULLibrary
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
        private readonly IUnityLinkTransRepository repoUnityLinkTransRepository;
        private readonly ICBSUnityLinkTransRepository repoCBSUnityLinkTransRepository;
        private readonly ICBSUnityLinkTransErrorRepository repoCBSUnityLinkTransErrorRepository;
        private readonly IUnityLinkTransErrorRepository repoUnityLinkTransErrorRepository;
        private readonly ICBSUnityLinkTransHistoryRepository repoCBSUnityLinkTransHistoryRepository;
        private readonly IUnityLinkTransHistoryRepository repoUnityLinkTransHistoryRepository;

        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/ULLibrary.cs";
      
        public ULLibrary()
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
            repoUnityLinkTransRepository = new UnityLinkTransRepository(idbfactory);
            repoCBSUnityLinkTransHistoryRepository = new CBSUnityLinkTransHistoryRepository(idbfactory);
            repoCBSUnityLinkTransRepository = new CBSUnityLinkTransRepository(idbfactory);
            repoCBSUnityLinkTransErrorRepository = new CBSUnityLinkTransErrorRepository(idbfactory);
            repoUnityLinkTransErrorRepository = new UnityLinkTransErrorRepository(idbfactory);
            repoUnityLinkTransHistoryRepository = new UnityLinkTransHistoryRepository(idbfactory);
           
        }
  
        public async Task<string> PullUnityLinkData()
        {
            Library library = new Library();
            try
            {
                LogManager.SaveLog("Task Start in UlLibrary");
                var ReconType = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "UnityLink");
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
                        LogManager.SaveLog("UnityLink  for Sybase Start in ULLibrary");
                        var value1 = string.Empty;
                        value1 = string.Empty;

                        var value2 = string.Empty;
                        value2 = string.Empty;
                        int scriptExecTtype = 0;

                        string FromDateParam = string.Empty;
                        string ToDateParam = string.Empty;
                        string MaxTransDate = string.Empty;
                        string dateTest = string.Empty;
                        string ReconDate = string.Empty;
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == ReconType.ReconTypeId && c.TableName == "CBSUnityLinkTrans");

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
                            ToDateParam = "'20170908'";

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
                                LogManager.SaveLog("An error occured in ULLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                            }

                            try
                            {

                                AseCommand cmd = new AseCommand(SqlString, theCons);
                                cmd.Connection = theCons;
                                cmd.CommandText = SqlString;
                                cmd.CommandTimeout = 0;

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

                                LogManager.SaveLog("Unity Links dataset ");
                                var returndata = ds.Tables["Results"];

                                LogManager.SaveLog("Total records pulled for CBSUnitylink: " + returndata.Rows.Count);
                                int countTrans = 0;
                                if (returndata.Rows.Count > 0)
                                {
                                    var CBSUnityLink = new CBSUnityLinkTran();
                                    var CBSUnitytransErr = new CBSUnityLinkTransError();
                                    for (int col = 0; col < returndata.Rows.Count; col++)
                                    {
                                        try
                                        {
                                            CBSUnityLink.AcctNo = returndata.Rows[col][0] == null ? null : returndata.Rows[col][0].ToString();
                                            CBSUnityLink.Amount = returndata.Rows[col][1] == null ? 0 : Math.Round(Convert.ToDecimal(returndata.Rows[col][1]), 2);
                                            CBSUnityLink.DebitCredit = returndata.Rows[col][2] == null ? null : returndata.Rows[col][2].ToString();
                                            CBSUnityLink.Description = returndata.Rows[col][3] == null ? null : returndata.Rows[col][3].ToString();
                                            CBSUnityLink.TransDate = returndata.Rows[col][4] == null ? (DateTime?)null : Convert.ToDateTime(returndata.Rows[col][4]);
                                            dateTest = CBSUnityLink.TransDate.ToString();
                                            if (countTrans > 1)
                                            {
                                                MaxTransDate = CBSUnityLink.TransDate.ToString();
                                                if (Convert.ToDateTime(dateTest) > Convert.ToDateTime(MaxTransDate))
                                                {
                                                    MaxTransDate = dateTest;
                                                }
                                                else
                                                {
                                                    MaxTransDate = MaxTransDate;
                                                }
                                            }
                                            CBSUnityLink.OriginBranch = returndata.Rows[col][5] == null ? null : returndata.Rows[col][5].ToString();
                                            CBSUnityLink.PostedBy = returndata.Rows[col][6] == null ? null : returndata.Rows[col][6].ToString();
                                            CBSUnityLink.Balance = returndata.Rows[col][7] == null ? 0 : Math.Round(Convert.ToDecimal(returndata.Rows[col][7]), 2);
                                            CBSUnityLink.Reference = returndata.Rows[col][8] == null ? null : returndata.Rows[col][8].ToString();
                                            CBSUnityLink.OrigRefNo = CBSUnityLink.Reference;
                                            CBSUnityLink.PtId = returndata.Rows[col][9] == null ? null : returndata.Rows[col][9].ToString();
                                            int revcode;
                                            CBSUnityLink.ReversalCode = returndata.Rows[col][10] == null ? 0 : int.TryParse(returndata.Rows[col][10].ToString(), out revcode) ? revcode : (int?)null;
                                            LastRecordId = CBSUnityLink.PtId;
                                            CBSUnityLink.MatchingStatus = "N";
                                            CBSUnityLink.UserId = 1;
                                            CBSUnityLink.PullDate = DateTime.Now;
                                            countTrans += 1;
                                            if (!string.IsNullOrWhiteSpace(CBSUnityLink.Reference))
                                            {
                                                var exist = await repoCBSUnityLinkTransRepository.GetAsync(c => c.Reference == CBSUnityLink.Reference && (c.DebitCredit == CBSUnityLink.DebitCredit) && (c.Amount == CBSUnityLink.Amount) && (c.AcctNo == CBSUnityLink.AcctNo));
                                                var existHis = await repoCBSUnityLinkTransHistoryRepository.GetAsync(c => c.Reference == CBSUnityLink.Reference && (c.DebitCredit == CBSUnityLink.DebitCredit) && (c.Amount == CBSUnityLink.Amount) && (c.AcctNo == CBSUnityLink.AcctNo));
                                                if (exist != null || existHis != null)
                                                {
                                                    CBSUnitytransErr.AcctNo = CBSUnityLink.AcctNo;
                                                    CBSUnitytransErr.Amount = CBSUnityLink.Amount;
                                                    CBSUnitytransErr.DebitCredit = CBSUnityLink.DebitCredit;
                                                    CBSUnitytransErr.Description = CBSUnityLink.Description;
                                                    CBSUnitytransErr.TransDate = CBSUnityLink.TransDate;
                                                    CBSUnitytransErr.OriginBranch = CBSUnityLink.OriginBranch;
                                                    CBSUnitytransErr.PostedBy = CBSUnityLink.PostedBy;
                                                    CBSUnitytransErr.Balance = CBSUnityLink.Balance;
                                                    CBSUnitytransErr.PtId = CBSUnityLink.PtId;
                                                    LastRecordId = CBSUnitytransErr.PtId;
                                                    CBSUnitytransErr.Reference = CBSUnityLink.Reference;
                                                    CBSUnitytransErr.OrigRefNo = CBSUnityLink.Reference;
                                                    CBSUnitytransErr.ReversalCode = CBSUnityLink.ReversalCode;
                                                    CBSUnitytransErr.UserId = 1;
                                                    CBSUnitytransErr.PullDate = DateTime.Now;
                                                    CBSUnitytransErr.ErrorMsg = "Duplicate transaction record";

                                                    repoCBSUnityLinkTransErrorRepository.Add(CBSUnitytransErr);
                                                    var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret1)
                                                    {
                                                        continue;
                                                    }
                                                    continue;
                                                }

                                                repoCBSUnityLinkTransRepository.Add(CBSUnityLink);
                                                var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret)
                                                {
                                                }
                                            }
                                            else
                                            {
                                                CBSUnitytransErr.AcctNo = CBSUnityLink.AcctNo;
                                                CBSUnitytransErr.Amount = CBSUnityLink.Amount;
                                                CBSUnitytransErr.DebitCredit = CBSUnityLink.DebitCredit;
                                                CBSUnitytransErr.Description = CBSUnityLink.Description;
                                                CBSUnitytransErr.TransDate = CBSUnityLink.TransDate;
                                                CBSUnitytransErr.OriginBranch = CBSUnityLink.OriginBranch;
                                                CBSUnitytransErr.PostedBy = CBSUnityLink.PostedBy;
                                                CBSUnitytransErr.Balance = CBSUnityLink.Balance;
                                                CBSUnitytransErr.PtId = CBSUnityLink.PtId;
                                                LastRecordId = CBSUnitytransErr.PtId;
                                                CBSUnitytransErr.Reference = CBSUnityLink.Reference;
                                                CBSUnitytransErr.OrigRefNo = CBSUnityLink.Reference;
                                                CBSUnitytransErr.ReversalCode = CBSUnityLink.ReversalCode;
                                                CBSUnitytransErr.UserId = 1;
                                                CBSUnitytransErr.PullDate = DateTime.Now;
                                                CBSUnitytransErr.ErrorMsg = "No reference No.";

                                                repoCBSUnityLinkTransErrorRepository.Add(CBSUnitytransErr);
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
                                            LogManager.SaveLog("An error occured ULLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                                            try
                                            {
                                                CBSUnitytransErr.AcctNo = CBSUnityLink.AcctNo;
                                                CBSUnitytransErr.Amount = CBSUnityLink.Amount;
                                                CBSUnitytransErr.DebitCredit = CBSUnityLink.DebitCredit;
                                                CBSUnitytransErr.Description = CBSUnityLink.Description;
                                                CBSUnitytransErr.TransDate = CBSUnityLink.TransDate;
                                                CBSUnitytransErr.OriginBranch = CBSUnityLink.OriginBranch;
                                                CBSUnitytransErr.PostedBy = CBSUnityLink.PostedBy;
                                                CBSUnitytransErr.Balance = CBSUnityLink.Balance;
                                                CBSUnitytransErr.PtId = CBSUnityLink.PtId;
                                                LastRecordId = CBSUnitytransErr.PtId;
                                                CBSUnitytransErr.Reference = CBSUnityLink.Reference;
                                                CBSUnitytransErr.ReversalCode = CBSUnityLink.ReversalCode;
                                                CBSUnitytransErr.OrigRefNo = CBSUnityLink.Reference;
                                                CBSUnitytransErr.UserId = 1;
                                                CBSUnitytransErr.PullDate = DateTime.Now;
                                                CBSUnitytransErr.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;

                                                repoCBSUnityLinkTransErrorRepository.Add(CBSUnitytransErr);
                                                var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                                if (ret1)
                                                {
                                                    continue;
                                                }
                                            }
                                            catch (Exception ex1)
                                            {

                                                exErr = ex1 == null ? ex1.InnerException.Message : ex1.Message;
                                                stackTrace = new StackTrace(ex);
                                                thisasm = Assembly.GetExecutingAssembly();
                                                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                                                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                                LogManager.SaveLog("An error occured in ULLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                                        admDataPoollingControlTBL.ReconTypeId = ReconType.ReconTypeId;
                                        admDataPoollingControlTBL.FileType = "Table";
                                        admDataPoollingControlTBL.TableName = "CBSUnityLinkTrans";
                                        admDataPoollingControlTBL.DateCreated = DateTime.Now;
                                        admDataPoollingControlTBL.UserId = 1;
                                        admDataPoollingControlTBL.LastRecordTimeStamp = DateTime.Now;
                                        admDataPoollingControlTBL.LastRecordId = LastRecordId;
                                        DateTime dtf = new DateTime();
                                        string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                                        admDataPoollingControlTBL.LastTransDate = Convert.ToDateTime(dateee);
                                        admDataPoollingControlTBL.PullDate = DateTime.Now;
                                        admDataPoollingControlTBL.ReconLevel = 1;
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
                                LogManager.SaveLog("An error occured in ULLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);


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
                        LogManager.SaveLog("An error occured in ULLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

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
                        var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "UnityLink");
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "UnityLink");

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
                        foreach (var kl in ggg)
                        {

                            fileNamenAndType = (from f in DLIST orderby f.LastWriteTime ascending select kl).First().Name;

                            FileLastTime = (from f in DLIST orderby f.LastAccessTimeUtc descending select kl).First().LastWriteTime.ToString();

                            dTable = await library.ReadUnityLink(FileDirectory, ReconTypeId, fileNamenAndType);

                            if (dTable.Rows.Count > 0)
                            {

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

                                var UnityLinkTBL = new UnityLinkTran();
                                int countTrans = 0;
                                foreach (DataRow col in dTable.Rows.Cast<DataRow>())
                                {
                                    try
                                    {
                                        string col0 = col[0] == null ? string.Empty : col[0].ToString();
                                        string col1 = col[1] == null ? string.Empty : col[1].ToString();
                                        string col2 = col[2] == null ? string.Empty : col[2].ToString();
                                        string col3 = col[3] == null ? string.Empty : col[3].ToString();
                                        string col4 = col[4] == null ? string.Empty : col[4].ToString();
                                        string col5 = col[5] == null ? string.Empty : col[5].ToString();
                                        string col6 = col[6] == null ? string.Empty : col[6].ToString();
                                        string col7 = col[7] == null ? string.Empty : col[7].ToString();
                                        string col8 = col[8] == null ? string.Empty : col[8].ToString();

                                        if (col0.Contains("\n"))
                                        {
                                            col0 = col0.Replace("\n", " ");
                                            if (col0.Contains(","))
                                            {
                                                col0 = col0.Replace(",", string.Empty);
                                            }
                                        }
                                        if (col1.Contains("\n"))
                                        {
                                            col1 = col1.Replace("\n", " ");
                                        }
                                        if (col2.Contains("\n"))
                                        {
                                            col2 = col2.Replace("\n", " ");
                                        }
                                        if (col3.Contains("\n"))
                                        {
                                            col3 = col3.Replace("\n", " ");
                                        }
                                        if (col4.Contains("\n"))
                                        {
                                            col4 = col4.Replace("\n", " ");
                                        }
                                        if (col5.Contains("\n"))
                                        {
                                            col5 = col5.Replace("\n", " ");
                                        }
                                        if (col6.Contains("\n"))
                                        {
                                            col6 = col6.Replace("\n", " ");
                                        }
                                        if (col7.Contains("\n"))
                                        {
                                            col7 = col7.Replace("\n", " ");
                                        }
                                        if (col8.Contains("\n"))
                                        {
                                            col8 = col8.Replace("\n", " ");
                                            if (col8.Contains(","))
                                            {
                                                col8 = col8.Replace(",", " ");
                                            }
                                        }

                                        DateTime date = new DateTime();
                                        if (DateTime.TryParse(col0, out date))
                                        {
                                            DateTime transDate = Convert.ToDateTime(string.Format("{0:MM/dd/yyyy hh:mm:ss}", Convert.ToDateTime(col0)));
                                            dateTest = UnityLinkTBL.TransDate.ToString();
                                            if (countTrans > 1)
                                            {
                                                MaxTransDate = UnityLinkTBL.TransDate.ToString();

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


                                            if (col4.Contains("\n"))
                                            {
                                                col4 = col4.Replace("\n", " ");
                                            }
                                            if (col5.Contains("\n"))
                                            {
                                                col5 = col5.Replace("\n", " ");
                                            }
                                            if (col6.Contains("\n"))
                                            {
                                                col6 = col6.Replace("\n", " ");
                                            }
                                            decimal decCheck;
                                            UnityLinkTBL.TransDate = transDate;
                                            
                                            
                                            UnityLinkTBL.TransRef = col1;
                                            UnityLinkTBL.OrigRefNo = UnityLinkTBL.TransRef;
                                            UnityLinkTBL.TransType = col2;
                                            UnityLinkTBL.Remitter = col3;
                                            UnityLinkTBL.Beneficiary = col4;
                                            string hhh = col5.Trim();
                                            UnityLinkTBL.SourceCurrency = col5.Split(' ')[0];
                                            string takeSrcAmt = col5.Split(' ')[1];
                                            UnityLinkTBL.SourceAmount = takeSrcAmt == null ? Convert.ToDecimal("0.00") : decimal.TryParse(takeSrcAmt, out decCheck) ? Math.Round(Convert.ToDecimal(takeSrcAmt), 2) : Math.Round(Convert.ToDecimal("0"), 2);

                                            UnityLinkTBL.ExchangeRate = col6 == null ? Convert.ToDecimal("0.0000") : decimal.TryParse(col6, out decCheck) ? Math.Round(Convert.ToDecimal(col6), 4) : Math.Round(Convert.ToDecimal("0"), 4);
                                            string takeDesAmt = col7.Split(' ')[1];
                                            UnityLinkTBL.DestAmount = takeDesAmt == null ? Convert.ToDecimal("0.00") : decimal.TryParse(takeDesAmt, out decCheck) ? Math.Round(Convert.ToDecimal(takeDesAmt), 2) : Math.Round(Convert.ToDecimal("0"), 2);
                                            UnityLinkTBL.DestCurrency = col7.Split(' ')[0];
                                            UnityLinkTBL.ProcessedbyAndDate = col8;
                                            UnityLinkTBL.MatchingStatus = "N";
                                            UnityLinkTBL.PullDate = DateTime.Now;
                                            UnityLinkTBL.UserId = 1;
                                            UnityLinkTBL.FileName = fileNamenAndType;

                                            if (!string.IsNullOrWhiteSpace(UnityLinkTBL.TransRef))
                                            {
                                                var exist = await repoUnityLinkTransRepository.GetAsync(c => c.TransRef == UnityLinkTBL.TransRef);
                                                var existHis = await repoUnityLinkTransHistoryRepository.GetAsync(c => c.TransRef == UnityLinkTBL.TransRef);
                                                if (exist != null || existHis != null)
                                                {
                                                    var UnityLinkTransError = new UnityLinkTransError();

                                                    UnityLinkTransError.TransDate = UnityLinkTBL.TransDate;
                                                    UnityLinkTransError.TransRef = UnityLinkTBL.TransRef;
                                                    UnityLinkTransError.OrigRefNo = UnityLinkTransError.TransRef;
                                                    UnityLinkTransError.TransType = UnityLinkTBL.TransType;
                                                    UnityLinkTransError.Remitter = UnityLinkTBL.Remitter;
                                                    UnityLinkTransError.Beneficiary = UnityLinkTBL.Beneficiary;
                                                    UnityLinkTransError.SourceAmount = UnityLinkTBL.SourceAmount;
                                                    UnityLinkTransError.ExchangeRate = UnityLinkTBL.ExchangeRate;
                                                    UnityLinkTransError.DestAmount = UnityLinkTBL.DestAmount;
                                                    UnityLinkTransError.ProcessedbyAndDate = UnityLinkTBL.ProcessedbyAndDate;
                                                    UnityLinkTransError.MatchingStatus = UnityLinkTBL.MatchingStatus;
                                                    UnityLinkTransError.PullDate = DateTime.Now;
                                                    UnityLinkTransError.UserId = UnityLinkTBL.UserId;
                                                    UnityLinkTransError.ErrorMsg = "Duplicate transaction record";
                                                    UnityLinkTransError.FileName = UnityLinkTBL.FileName;

                                                    repoUnityLinkTransErrorRepository.Add(UnityLinkTransError);
                                                    var hh = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (hh)
                                                    {
                                                        continue;
                                                    }

                                                    continue;
                                                }
                                                else
                                                {
                                                    repoUnityLinkTransRepository.Add(UnityLinkTBL);
                                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                        count += 1;
                                                        continue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var UnityLinkTransError = new UnityLinkTransError();

                                                UnityLinkTransError.TransDate = UnityLinkTBL.TransDate;
                                                UnityLinkTransError.TransRef = UnityLinkTBL.TransRef;
                                                UnityLinkTransError.OrigRefNo = UnityLinkTransError.TransRef;
                                                UnityLinkTransError.TransType = UnityLinkTBL.TransType;
                                                UnityLinkTransError.Remitter = UnityLinkTBL.Remitter;
                                                UnityLinkTransError.Beneficiary = UnityLinkTBL.Beneficiary;
                                                UnityLinkTransError.SourceAmount = UnityLinkTBL.SourceAmount;
                                                UnityLinkTransError.ExchangeRate = UnityLinkTBL.ExchangeRate;
                                                UnityLinkTransError.DestAmount = UnityLinkTBL.DestAmount;
                                                UnityLinkTransError.ProcessedbyAndDate = UnityLinkTBL.ProcessedbyAndDate;
                                                UnityLinkTransError.MatchingStatus = UnityLinkTBL.MatchingStatus;
                                                UnityLinkTransError.PullDate = DateTime.Now;
                                                UnityLinkTransError.UserId = UnityLinkTBL.UserId;
                                                UnityLinkTransError.ErrorMsg = "No transaction  ref";
                                                UnityLinkTransError.FileName = UnityLinkTBL.FileName;

                                                repoUnityLinkTransErrorRepository.Add(UnityLinkTransError);
                                                var hh = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (hh)
                                                {
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
                                        LogManager.SaveLog("An error occured in Unity Link in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr + " FileName: " + fileNamenAndType);

                                        var UnityLinkTransError = new UnityLinkTransError();
                                        UnityLinkTransError.OrigRefNo = UnityLinkTransError.TransRef;
                                        UnityLinkTransError.TransDate = UnityLinkTBL.TransDate;
                                        UnityLinkTransError.TransRef = UnityLinkTBL.TransRef;
                                        UnityLinkTransError.TransType = UnityLinkTBL.TransType;
                                        UnityLinkTransError.Remitter = UnityLinkTBL.Remitter;
                                        UnityLinkTransError.Beneficiary = UnityLinkTBL.Beneficiary;
                                        UnityLinkTransError.SourceAmount = UnityLinkTBL.SourceAmount;
                                        UnityLinkTransError.ExchangeRate = UnityLinkTBL.ExchangeRate;
                                        UnityLinkTransError.DestAmount = UnityLinkTBL.DestAmount;
                                        UnityLinkTransError.ProcessedbyAndDate = UnityLinkTBL.ProcessedbyAndDate;
                                        UnityLinkTransError.MatchingStatus = UnityLinkTBL.MatchingStatus;
                                        UnityLinkTransError.PullDate = DateTime.Now;
                                        UnityLinkTransError.UserId = UnityLinkTBL.UserId;
                                        UnityLinkTransError.ErrorMsg = exErr;
                                        UnityLinkTransError.FileName = UnityLinkTBL.FileName;

                                        repoUnityLinkTransErrorRepository.Add(UnityLinkTransError);
                                        var hh = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                        if (hh)
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }

                            //Update and Move here

                            var proceTBL = new FileProcessControl();

                            proceTBL.ReconTypeId = ReconTypeId;
                            proceTBL.FileDirectory = FileDirectory;
                            proceTBL.FileName = fileNamenAndType;
                            proceTBL.NameConvention = rectype.FileNamingConvention;
                            proceTBL.DateProcessed = DateTime.Now;
                            proceTBL.Status = "Processed";
                            proceTBL.DateCreated = DateTime.Now;
                            proceTBL.UserId = 1;

                            LogManager.SaveLog("Log File reading ReconTye :" + rectype.ReconName + " FileDirectory: " + FileDirectory + "fileNamenAndType: " + fileNamenAndType + "getReconTypeInfo.FileNamingConvention: " + rectype.FileNamingConvention);

                            repoadmFileProcessedRepository.Add(proceTBL);
                            var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                            if (ret1)
                            {
                                LogManager.SaveLog("Move  Unity Link  Start");
                                string MvFile = library.MoveFile(FileDirectory + "\\" + fileNamenAndType, ReconType.ProcessedFileDirectory, fileNamenAndType);
                                LogManager.SaveLog("Move File in Unity Link status: " + MvFile);
                            }
                            //Update control table
                            if (controlTable != null)
                            {
                                controlTable.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                                DateTime dtf = new DateTime();
                                string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                                controlTable.LastTransDate = Convert.ToDateTime(dateee);
                                controlTable.PullDate = DateTime.Now;
                                controlTable.RecordsCount = count;
                                repoadmDataPoollingControlRepository.Update(controlTable);
                                var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                if (ret)
                                {

                                }
                            }
                            else
                            {
                                //var admDataPoollingControlTBL = new admDataPoollingControl();
                                //admDataPoollingControlTBL.ReconTypeId = rectype.ReconTypeId;
                                //admDataPoollingControlTBL.FileType = "File";
                                //admDataPoollingControlTBL.TableName = "UnityLinkTrans";
                                //admDataPoollingControlTBL.DateCreated = DateTime.Now;
                                //admDataPoollingControlTBL.UserId = 1;
                                //admDataPoollingControlTBL.ReconLevel = 1;
                                //admDataPoollingControlTBL.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                                //admDataPoollingControlTBL.ReconDate = DateTime.Now;
                                //admDataPoollingControlTBL.RecordsCount = count;
                                //repoadmDataPoollingControlRepository.Add(admDataPoollingControlTBL);
                                //var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                //if (ret)
                                //{

                                //}
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
                        LogManager.SaveLog("An error occured in  Line Unity Link Library File Source:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

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
                LogManager.SaveLog("An error occured in ULLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
            }

            return string.Empty;
        }    
      
    }
}