
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
    public class AirtelLibrary
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
        private readonly IAIRTELTransRepository repoAIRTELTransRepository;
        private readonly IAIRTELTransErrorRepository repoAIRTELTransErrorRepository;
        private readonly ICBSAirtelTransErrorRepository repoCBSAirtelTransErrorRepository;
        private readonly ICBSAirtelTransRepository repoCBSAirtelTransRepository;
        private readonly ICBSAirtelTransHistoryRepository repoCBSAirtelTransHistoryRepository;
        private readonly IAIRTELTransHistoryRepository repoAIRTELTransHistoryRepository;
        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/AritelLibrary.cs";
        private static string agentcodestatic;
        private static string AcctNoStatic;
        private static string LegacyStatic;
        private static string SettleCurrStatic;
        private static string TransCurStatic;
        private static string BranchNameStatic;
        public AirtelLibrary()
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
            repoCBSAirtelTransErrorRepository = new CBSAirtelTransErrorRepository(idbfactory);
            repoCBSAirtelTransRepository = new CBSAirtelTransRepository(idbfactory);
            repoAIRTELTransRepository = new AIRTELTransRepository(idbfactory);
            repoAIRTELTransErrorRepository = new AIRTELTransErrorRepository(idbfactory);
            repoCBSAirtelTransHistoryRepository = new CBSAirtelTransHistoryRepository(idbfactory);
            repoAIRTELTransHistoryRepository = new AIRTELTransHistoryRepository(idbfactory);
 
        }
        public async Task<string> PullAirtelData()
        {
            Library library = new Library();
            try
            {
                LogManager.SaveLog("Task Start in AirtelLibrary");
                var ReconType = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "AIRTEL");
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

                string AcctNoErrolog = string.Empty;
                string AmountErrolog = string.Empty;
                string DebitCreditErrolog = string.Empty;
                string DescriptionErrolog = string.Empty;
                string TransDateErrolog = string.Empty;
                string OriginBranchErrolog = string.Empty;
                string PostedByErrolog = string.Empty;
                string BalanceErrolog = string.Empty;
                string ReferenceErrolog = string.Empty;
                string PtIdErrolog = string.Empty;
                string ReconDateErrolog = string.Empty;
                string MatchingStatusErrolog = string.Empty;
                string UserIdErrolog = string.Empty;
          
                #region Source 1
                #region   // Sybase Server below

                if (dtSouceCon1.DatabaseType == "SYBASE")
                {
                    try
                    {
                        LogManager.SaveLog("PullDataAirtel for Sybase Start in AirtelLibrary");
                        var value1 = string.Empty;
                        value1 = string.Empty;

                        var value2 = string.Empty;
                        value2 = string.Empty;
                        int scriptExecTtype = 0;

                        string FromDateParam = string.Empty;
                        string ToDateParam = string.Empty;

                        string MaxTransDate = string.Empty;
                        string dateTest = string.Empty;
                        string RecordCount = string.Empty;

                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == ReconType.ReconTypeId && c.TableName == "CBSAirtelTrans");
                        string ReconDate = string.Empty;
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
                            LastRecordId = "0";
                            FromDateParam = "'20170901'";
                            ToDateParam = "'20170924'";
                        }

                        string SqlString = string.Empty;
                        if (ReconType.Source1Script != null)
                        {
                             SqlString = ReconType.Source1Script.Replace("{ActlistSource1}", acctlistSource1)
                                                                .Replace("{ReconDate}", ReconDate)
                                                                .Replace("{LastRecordId}", LastRecordId);
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
                                LogManager.SaveLog("An error occured AirtelLibrary Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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

                                ds.EnforceConstraints = false;
                                ds.Load(reader, LoadOption.OverwriteChanges, "Results");
                                var returndata = ds.Tables["Results"];

                                LogManager.SaveLog("PullDataAirtel for Sybase returndata.Rows.Count " + returndata.Rows.Count);
                                
                                int count = 0;
                                if (returndata.Rows.Count > 0)
                                {
                                    
                                    var CBSUnityLink = new CBSUnityLinkTran();
                                    var CBSAirtelErr = new CBSAirtelTransError();

                                    var CBSAirtel = new CBSAirtelTran();
                                    int countTrans = 0;
                                   
                                    for (int col = 0; col < returndata.Rows.Count; col++)
                                    {
                                        try
                                        {
                                            CBSAirtel.AcctNo = returndata.Rows[col][0] == null ? null : returndata.Rows[col][0].ToString();
                                            CBSAirtel.Amount = returndata.Rows[col][1] == null ? 0 : Math.Round(Convert.ToDecimal(returndata.Rows[col][1]), 2);
                                            CBSAirtel.DebitCredit = returndata.Rows[col][2] == null ? null : returndata.Rows[col][2].ToString();
                                            CBSAirtel.Description = returndata.Rows[col][3] == null ? null : returndata.Rows[col][3].ToString();
                                            CBSAirtel.TransDate = returndata.Rows[col][4] == null ? (DateTime?)null : Convert.ToDateTime(returndata.Rows[col][4]);
                                            dateTest = CBSAirtel.TransDate.ToString();
                                            if (countTrans > 1)
                                            {
                                                MaxTransDate = CBSAirtel.TransDate.ToString();

                                                if (Convert.ToDateTime(dateTest) > Convert.ToDateTime(MaxTransDate))
                                                {
                                                    MaxTransDate = dateTest;
                                                }
                                                else
                                                {
                                                    MaxTransDate = MaxTransDate;
                                                }
                                            }
                                            CBSAirtel.OriginBranch = returndata.Rows[col][5] == null ? null : returndata.Rows[col][5].ToString();
                                            CBSAirtel.PostedBy = returndata.Rows[col][6] == null ? null : returndata.Rows[col][6].ToString();
                                            CBSAirtel.Balance = returndata.Rows[col][7] == null ? 0 : Math.Round(Convert.ToDecimal(returndata.Rows[col][7]), 2);
                                            CBSAirtel.Reference = returndata.Rows[col][8] == null ? null : returndata.Rows[col][8].ToString();
                                            CBSAirtel.OrigRefNo = CBSAirtel.Reference;
                                            CBSAirtel.TrfAcctType = returndata.Rows[col][9] == null ? null : returndata.Rows[col][9].ToString();
                                            CBSAirtel.TrfAcctNo = returndata.Rows[col][10] == null ? null : returndata.Rows[col][10].ToString();
                                            CBSAirtel.PtId = returndata.Rows[col][11] == null ? null : returndata.Rows[col][11].ToString();
                                            LastRecordId = CBSAirtel.PtId;
                                            int revcode;
                                            CBSAirtel.ReversalCode = returndata.Rows[col][12] == null ? (int?)null : int.TryParse(returndata.Rows[col][12].ToString(), out revcode) ? revcode : (int?)revcode; 
                                            CBSAirtel.ReconDate = DateTime.Now;
                                            CBSAirtel.MatchingStatus = "N";
                                            CBSAirtel.UserId = 1;

                                            countTrans += 1;

                                            if (!string.IsNullOrWhiteSpace(CBSAirtel.Reference))
                                            {
                                                var exist = await repoCBSAirtelTransRepository.GetAsync(c => c.Reference == CBSAirtel.Reference);
                                                var existHis = await repoCBSAirtelTransHistoryRepository.GetAsync(c => c.Reference == CBSAirtel.Reference);
                                                if (exist != null || existHis != null)
                                                {
                                                   
                                                    CBSAirtelErr.AcctNo = CBSAirtel.AcctNo;
                                                    CBSAirtelErr.Amount = CBSAirtel.Amount;
                                                    CBSAirtelErr.DebitCredit = CBSAirtel.DebitCredit;
                                                    CBSAirtelErr.Description = CBSAirtel.Description;
                                                    CBSAirtelErr.TransDate = CBSAirtel.TransDate;
                                                    CBSAirtelErr.OriginBranch = CBSAirtel.OriginBranch;
                                                    CBSAirtelErr.PostedBy = CBSAirtel.PostedBy;
                                                    CBSAirtelErr.Balance = CBSAirtel.Balance;
                                                    CBSAirtelErr.Reference = CBSAirtel.Reference;
                                                    CBSAirtelErr.OrigRefNo = CBSAirtel.Reference; 
                                                    CBSAirtelErr.TrfAcctType = CBSAirtel.TrfAcctType;
                                                    CBSAirtelErr.TrfAcctNo = CBSAirtel.TrfAcctNo;
                                                    CBSAirtelErr.PtId = CBSAirtel.PtId;
                                                    LastRecordId = CBSAirtel.PtId;
                                                    CBSAirtelErr.MatchingStatus = CBSAirtel.MatchingStatus;
                                                    CBSAirtelErr.PullDate = CBSAirtel.PullDate;
                                                    CBSAirtelErr.UserId = CBSAirtel.UserId;
                                                    
                                                    CBSAirtelErr.ReversalCode = CBSAirtel.ReversalCode;
                                                    CBSAirtelErr.ErrorMsg = "Duplicate transaction record";

                                                    repoCBSAirtelTransErrorRepository.Add(CBSAirtelErr);

                                                    var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret1)
                                                    {
                                                        continue;
                                                    }
                                                    continue;
                                                }
                                                repoCBSAirtelTransRepository.Add(CBSAirtel);
                                                var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret)
                                                {
                                                    count += 1;
                                                    RecordCount = count.ToString();
                                                    continue;
                                                }
                                            }
                                            else
                                            {

                                                CBSAirtelErr.AcctNo = CBSAirtel.AcctNo;
                                                CBSAirtelErr.Amount = CBSAirtel.Amount;
                                                CBSAirtelErr.DebitCredit = CBSAirtel.DebitCredit;
                                                CBSAirtelErr.Description = CBSAirtel.Description;
                                                CBSAirtelErr.TransDate = CBSAirtel.TransDate;
                                                CBSAirtelErr.OriginBranch = CBSAirtel.OriginBranch;
                                                CBSAirtelErr.PostedBy = CBSAirtel.PostedBy;
                                                CBSAirtelErr.Balance = CBSAirtel.Balance;
                                                CBSAirtelErr.Reference = CBSAirtel.Reference;
                                                CBSAirtelErr.OrigRefNo = CBSAirtel.Reference; 
                                                CBSAirtelErr.TrfAcctType = CBSAirtel.TrfAcctType;
                                                CBSAirtelErr.TrfAcctNo = CBSAirtel.TrfAcctNo;
                                                CBSAirtelErr.PtId = CBSAirtel.PtId;
                                                LastRecordId = CBSAirtel.PtId;
                                                CBSAirtelErr.MatchingStatus = CBSAirtel.MatchingStatus;
                                                CBSAirtelErr.PullDate = CBSAirtel.PullDate;
                                                CBSAirtelErr.UserId = CBSAirtel.UserId;
                                                CBSAirtelErr.MatchingType = CBSAirtel.MatchingType;
                                                CBSAirtelErr.ReversalCode = CBSAirtel.ReversalCode;
                                                CBSAirtelErr.ErrorMsg = "No reference No.";

                                                repoCBSAirtelTransErrorRepository.Add(CBSAirtelErr);

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
                                            LogManager.SaveLog("An error occured AirtelLibrary in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                                            try
                                            {
                                                CBSAirtelErr.AcctNo = CBSAirtel.AcctNo;
                                                CBSAirtelErr.Amount = CBSAirtel.Amount;
                                                CBSAirtelErr.DebitCredit = CBSAirtel.DebitCredit;
                                                CBSAirtelErr.Description = CBSAirtel.Description;
                                                CBSAirtelErr.TransDate = CBSAirtel.TransDate;
                                                CBSAirtelErr.OriginBranch = CBSAirtel.OriginBranch;
                                                CBSAirtelErr.PostedBy = CBSAirtel.PostedBy;
                                                CBSAirtelErr.Balance = CBSAirtel.Balance;
                                                CBSAirtelErr.Reference = CBSAirtel.Reference;
                                                CBSAirtelErr.OrigRefNo = CBSAirtel.Reference; 
                                                CBSAirtelErr.TrfAcctType = CBSAirtel.TrfAcctType;
                                                CBSAirtelErr.TrfAcctNo = CBSAirtel.TrfAcctNo;
                                                CBSAirtelErr.PtId = CBSAirtel.PtId;
                                                LastRecordId = CBSAirtel.PtId;
                                                CBSAirtelErr.MatchingStatus = CBSAirtel.MatchingStatus;
                                                CBSAirtelErr.PullDate = CBSAirtel.PullDate;
                                                CBSAirtelErr.UserId = CBSAirtel.UserId;
                                                CBSAirtelErr.MatchingType = CBSAirtel.MatchingType;
                                                CBSAirtelErr.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;

                                                repoCBSAirtelTransErrorRepository.Add(CBSAirtelErr);
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
                                                LogManager.SaveLog("An error occured in AirtelLibrary Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                                                continue;
                                            }
                                            continue;
                                        }
                                    }
                                    if (controlTable != null)
                                    {
                                        controlTable.LastRecordId = LastRecordId;
                                        controlTable.LastRecordTimeStamp = DateTime.Now;
                                        string dateee = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate));
                                        controlTable.LastTransDate = Convert.ToDateTime(dateee);
                                        controlTable.PullDate = DateTime.Now;
                                        controlTable.RecordsCount = Convert.ToInt32(RecordCount);
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
                                        admDataPoollingControlTBL.TableName = "CBSAirtelTrans";
                                        admDataPoollingControlTBL.DateCreated = DateTime.Now;
                                        admDataPoollingControlTBL.UserId = 1;
                                        admDataPoollingControlTBL.LastRecordTimeStamp = DateTime.Now;
                                        admDataPoollingControlTBL.LastRecordId = LastRecordId;
                                        admDataPoollingControlTBL.ReconLevel = 1;
                                        string dateee = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate));
                                        admDataPoollingControlTBL.LastTransDate = Convert.ToDateTime(dateee);
                                        admDataPoollingControlTBL.PullDate = DateTime.Now;
                                        admDataPoollingControlTBL.RecordsCount = Convert.ToInt32(RecordCount);
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
                                LogManager.SaveLog("An error occured in AirtelLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                        LogManager.SaveLog("An error occured in AirtelLogManager Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);  
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
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == ReconType.ReconTypeId && c.TableName == "AirtelTrans");

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

                            dTable = await library.ReadExcel(FileDirectory, ReconTypeId);

                            if (dTable.Rows.Count > 0)
                            {
                                var AIRTELTBL = new AirtelTran();
                                var AIRTELTBLErr = new AirtelTransError();

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

                                int countTrans = 0;
                                foreach (DataRow row in dTable.Rows.Cast<DataRow>().Skip(6))
                                {
                                    try
                                    {
                                        string col0 = row[0] == null ? string.Empty : row[0].ToString();
                                        string col1 = row[1] == null ? string.Empty : row[1].ToString();
                                        string col2 = row[2] == null ? string.Empty : row[2].ToString();
                                        string col3 = row[3] == null ? string.Empty : row[3].ToString();
                                        string col4 = row[4] == null ? string.Empty : row[4].ToString();
                                        string col5 = row[5] == null ? string.Empty : row[5].ToString();
                                        string col6 = row[6] == null ? string.Empty : row[6].ToString();
                                        string col7 = row[7] == null ? string.Empty : row[7].ToString();
                                        string col8 = row[8] == null ? string.Empty : row[8].ToString();
                                        string col9 = row[9] == null ? string.Empty : row[9].ToString();
                                        string col10 = row[10] == null ? string.Empty : row[10].ToString();
                                        string col11 = row[11] == null ? string.Empty : row[11].ToString();
                                        string col12 = row[12] == null ? string.Empty : row[12].ToString();
                                        string col13 = row[13] == null ? string.Empty : row[13].ToString();
                                        string col14 = row[14] == null ? string.Empty : row[14].ToString();
                                        string col15 = row[15] == null ? string.Empty : row[15].ToString();
                                        string col16 = row[16] == null ? string.Empty : row[16].ToString();
                                        string col17 = row[17] == null ? string.Empty : row[17].ToString();
                                        string col18 = row[18] == null ? string.Empty : row[18].ToString();
                                        string col19 = row[19] == null ? string.Empty : row[19].ToString();
                                        string col20 = row[20] == null ? string.Empty : row[20].ToString();
                                        string col21 = row[21] == null ? string.Empty : row[21].ToString();
                                        string col22 = row[22] == null ? string.Empty : row[22].ToString();
                                        string col23 = row[23] == null ? string.Empty : row[23].ToString();
                                        string col24 = row[24] == null ? string.Empty : row[24].ToString();
                                        string col25 = row[25] == null ? string.Empty : row[25].ToString();

                                        int col0check = 0;
                                        if (int.TryParse(col0, out col0check))
                                        {
                                            AIRTELTBL.RecordNo = col0;
                                            AIRTELTBL.TransactionId = col1;
                                            AIRTELTBL.OrigRefNo = AIRTELTBL.TransactionId;
                                            DateTime date = new DateTime();
                                            if (DateTime.TryParse(col2, out date))
                                            {
                                                AIRTELTBL.TransactionDate = col2 == null ? (DateTime?)null : Convert.ToDateTime(col2);
                                                dateTest = AIRTELTBL.TransactionDate.ToString();
                                                if (countTrans > 1)
                                                {
                                                    MaxTransDate = AIRTELTBL.TransactionDate.ToString();

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

                                            }
                                            AIRTELTBL.PayerMFSProvider = col3;
                                            AIRTELTBL.PayerPaymentInstrument = col4;
                                            AIRTELTBL.PayerWallet = col5;

                                            decimal bal = 0;
                                            if (decimal.TryParse(col6, out bal))
                                            {
                                                AIRTELTBL.FeeServiceChargeTax1 = col6 == null ? 0 : Math.Round(Convert.ToDecimal(col6), 2);
                                            }
                                            if (decimal.TryParse(col7, out bal))
                                            {
                                                AIRTELTBL.FeeServiceChargeTax2 = col7 == null ? 0 : Math.Round(Convert.ToDecimal(col7), 2);
                                            }
                                            AIRTELTBL.PayerBankAcctNo = col8;
                                            AIRTELTBL.UserCategory = col9;
                                            AIRTELTBL.UserGrade = col10;
                                            AIRTELTBL.PayeeMFSProvider = col11;
                                            AIRTELTBL.PayeePaymentInstrument = col12;
                                            AIRTELTBL.PayeWalletType = col13;
                                            AIRTELTBL.PayeeBankAcctNo = col4;
                                            AIRTELTBL.ReceiverCategory = col15;
                                            AIRTELTBL.ReceiverGrade = col16;
                                            AIRTELTBL.ServiceType = col17;
                                            AIRTELTBL.TransactionType = col18;
                                            AIRTELTBL.FileName = fileNamenAndType;
                                            int Amt = 0;
                                            if (int.TryParse(col19, out Amt))
                                            {
                                                AIRTELTBL.TransactionAmount = col19 == null ? 0 : Convert.ToInt32(col19);
                                            }
                                            if (decimal.TryParse(col20, out bal))
                                            {
                                                AIRTELTBL.PayerPreviousBalance = col20 == null ? 0 : Math.Round(Convert.ToDecimal(col20), 2);
                                            }

                                            if (decimal.TryParse(col21, out bal))
                                            {
                                                AIRTELTBL.PayerPostBalance = col21 == null ? 0 : Math.Round(Convert.ToDecimal(col21), 2);
                                            }

                                            if (decimal.TryParse(col22, out bal))
                                            {
                                                AIRTELTBL.PayeePreBalance = col22 == null ? 0 : Math.Round(Convert.ToDecimal(col22), 2);
                                            }

                                            if (decimal.TryParse(col23, out bal))
                                            {
                                                AIRTELTBL.PayeePostBalance = col23 == null ? 0 : Math.Round(Convert.ToDecimal(col23), 2);
                                            }
                                            AIRTELTBL.ReferenceNo = col24;
                                            AIRTELTBL.ExternalRefNo = col25;
                                            AIRTELTBL.MatchingStatus = "N";
                                            AIRTELTBL.PullDate = DateTime.Now;
                                            AIRTELTBL.UserId = 1;

                                        }

                                        if (!string.IsNullOrWhiteSpace(AIRTELTBL.TransactionId))
                                        {
                                            var exist = await repoAIRTELTransRepository.GetAsync(c => c.TransactionId == AIRTELTBL.TransactionId);
                                            var existHis = await repoAIRTELTransHistoryRepository.GetAsync(c => c.TransactionId == AIRTELTBL.TransactionId);
                                            if (exist != null || existHis != null)
                                            {
                                                AIRTELTBLErr.RecordNo = AIRTELTBL.RecordNo;
                                                AIRTELTBLErr.TransactionId = AIRTELTBL.TransactionId;
                                                AIRTELTBLErr.OrigRefNo = AIRTELTBLErr.TransactionId;
                                                AIRTELTBLErr.TransactionDate = AIRTELTBL.TransactionDate;
                                                AIRTELTBLErr.PayerMFSProvider = AIRTELTBL.PayerMFSProvider;
                                                AIRTELTBLErr.PayerPaymentInstrument = AIRTELTBL.PayerPaymentInstrument;
                                                AIRTELTBLErr.PayerWallet = AIRTELTBL.PayerWallet;
                                                AIRTELTBLErr.FeeServiceChargeTax1 = AIRTELTBL.FeeServiceChargeTax1;
                                                AIRTELTBLErr.FeeServiceChargeTax2 = AIRTELTBL.FeeServiceChargeTax2;
                                                AIRTELTBLErr.PayerBankAcctNo = AIRTELTBL.PayerBankAcctNo;
                                                AIRTELTBLErr.UserCategory = AIRTELTBL.UserCategory;
                                                AIRTELTBLErr.UserGrade = AIRTELTBL.UserGrade;
                                                AIRTELTBLErr.PayeeMFSProvider = AIRTELTBL.PayeeMFSProvider;
                                                AIRTELTBLErr.PayeePaymentInstrument = AIRTELTBL.PayeePaymentInstrument;
                                                AIRTELTBLErr.PayeWalletType = AIRTELTBL.PayeWalletType;
                                                AIRTELTBLErr.PayeeBankAcctNo = AIRTELTBL.PayeeBankAcctNo;
                                                AIRTELTBLErr.ReceiverCategory = AIRTELTBL.ReceiverCategory;
                                                AIRTELTBLErr.ReceiverGrade = AIRTELTBL.ReceiverGrade;
                                                AIRTELTBLErr.ServiceType = AIRTELTBL.ServiceType;
                                                AIRTELTBLErr.TransactionType = AIRTELTBL.TransactionType;
                                                AIRTELTBLErr.TransactionAmount = AIRTELTBL.TransactionAmount;
                                                AIRTELTBLErr.PayerPreviousBalance = AIRTELTBL.PayerPreviousBalance;
                                                AIRTELTBLErr.PayerPostBalance = AIRTELTBL.PayerPostBalance;
                                                AIRTELTBLErr.PayeePreBalance = AIRTELTBL.PayeePreBalance;
                                                AIRTELTBLErr.PayeePostBalance = AIRTELTBL.PayeePostBalance;
                                                AIRTELTBLErr.ReferenceNo = AIRTELTBL.ReferenceNo;
                                                AIRTELTBLErr.ExternalRefNo = AIRTELTBL.ExternalRefNo;
                                                AIRTELTBLErr.MatchingStatus = "N";
                                                AIRTELTBLErr.PullDate = DateTime.Now;
                                                AIRTELTBLErr.UserId = 1;
                                                AIRTELTBLErr.ErrorMsg = "Duplicate transaction record";
                                                AIRTELTBLErr.FileName = fileNamenAndType;
                                                repoAIRTELTransErrorRepository.Add(AIRTELTBLErr);
                                                var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret)
                                                {
                                                    continue;
                                                }
                                                continue;
                                            }
                                            else
                                            {
                                                repoAIRTELTransRepository.Add(AIRTELTBL);
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
                                            AIRTELTBLErr.RecordNo = AIRTELTBL.RecordNo;
                                            AIRTELTBLErr.TransactionId = AIRTELTBL.TransactionId;
                                            AIRTELTBLErr.OrigRefNo = AIRTELTBLErr.TransactionId;
                                            AIRTELTBLErr.TransactionDate = AIRTELTBL.TransactionDate;
                                            AIRTELTBLErr.PayerMFSProvider = AIRTELTBL.PayerMFSProvider;
                                            AIRTELTBLErr.PayerPaymentInstrument = AIRTELTBL.PayerPaymentInstrument;
                                            AIRTELTBLErr.PayerWallet = AIRTELTBL.PayerWallet;
                                            AIRTELTBLErr.FeeServiceChargeTax1 = AIRTELTBL.FeeServiceChargeTax1;
                                            AIRTELTBLErr.FeeServiceChargeTax2 = AIRTELTBL.FeeServiceChargeTax2;
                                            AIRTELTBLErr.PayerBankAcctNo = AIRTELTBL.PayerBankAcctNo;
                                            AIRTELTBLErr.UserCategory = AIRTELTBL.UserCategory;
                                            AIRTELTBLErr.UserGrade = AIRTELTBL.UserGrade;
                                            AIRTELTBLErr.PayeeMFSProvider = AIRTELTBL.PayeeMFSProvider;
                                            AIRTELTBLErr.PayeePaymentInstrument = AIRTELTBL.PayeePaymentInstrument;
                                            AIRTELTBLErr.PayeWalletType = AIRTELTBL.PayeWalletType;
                                            AIRTELTBLErr.PayeeBankAcctNo = AIRTELTBL.PayeeBankAcctNo;
                                            AIRTELTBLErr.ReceiverCategory = AIRTELTBL.ReceiverCategory;
                                            AIRTELTBLErr.ReceiverGrade = AIRTELTBL.ReceiverGrade;
                                            AIRTELTBLErr.ServiceType = AIRTELTBL.ServiceType;
                                            AIRTELTBLErr.TransactionType = AIRTELTBL.TransactionType;
                                            AIRTELTBLErr.TransactionAmount = AIRTELTBL.TransactionAmount;
                                            AIRTELTBLErr.PayerPreviousBalance = AIRTELTBL.PayerPreviousBalance;
                                            AIRTELTBLErr.PayerPostBalance = AIRTELTBL.PayerPostBalance;
                                            AIRTELTBLErr.PayeePreBalance = AIRTELTBL.PayeePreBalance;
                                            AIRTELTBLErr.PayeePostBalance = AIRTELTBL.PayeePostBalance;
                                            AIRTELTBLErr.ReferenceNo = AIRTELTBL.ReferenceNo;
                                            AIRTELTBLErr.ExternalRefNo = AIRTELTBL.ExternalRefNo;
                                            AIRTELTBLErr.MatchingStatus = "N";
                                            AIRTELTBLErr.PullDate = DateTime.Now;
                                            AIRTELTBLErr.UserId = 1;
                                            AIRTELTBLErr.ErrorMsg = "No reference No.";
                                            AIRTELTBLErr.FileName = fileNamenAndType;

                                            repoAIRTELTransErrorRepository.Add(AIRTELTBLErr);
                                            var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                            if (ret)
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
                                        LogManager.SaveLog("An error occured in AirtelLibrary Line  :" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);


                                        AIRTELTBLErr.RecordNo = AIRTELTBL.RecordNo;
                                        AIRTELTBLErr.TransactionId = AIRTELTBL.TransactionId;
                                        AIRTELTBLErr.OrigRefNo = AIRTELTBLErr.TransactionId;
                                        AIRTELTBLErr.TransactionDate = AIRTELTBL.TransactionDate;
                                        AIRTELTBLErr.PayerMFSProvider = AIRTELTBL.PayerMFSProvider;
                                        AIRTELTBLErr.PayerPaymentInstrument = AIRTELTBL.PayerPaymentInstrument;
                                        AIRTELTBLErr.PayerWallet = AIRTELTBL.PayerWallet;
                                        AIRTELTBLErr.FeeServiceChargeTax1 = AIRTELTBL.FeeServiceChargeTax1;
                                        AIRTELTBLErr.FeeServiceChargeTax2 = AIRTELTBL.FeeServiceChargeTax2;
                                        AIRTELTBLErr.PayerBankAcctNo = AIRTELTBL.PayerBankAcctNo;
                                        AIRTELTBLErr.UserCategory = AIRTELTBL.UserCategory;
                                        AIRTELTBLErr.UserGrade = AIRTELTBL.UserGrade;
                                        AIRTELTBLErr.PayeeMFSProvider = AIRTELTBL.PayeeMFSProvider;
                                        AIRTELTBLErr.PayeePaymentInstrument = AIRTELTBL.PayeePaymentInstrument;
                                        AIRTELTBLErr.PayeWalletType = AIRTELTBL.PayeWalletType;
                                        AIRTELTBLErr.PayeeBankAcctNo = AIRTELTBL.PayeeBankAcctNo;
                                        AIRTELTBLErr.ReceiverCategory = AIRTELTBL.ReceiverCategory;
                                        AIRTELTBLErr.ReceiverGrade = AIRTELTBL.ReceiverGrade;
                                        AIRTELTBLErr.ServiceType = AIRTELTBL.ServiceType;
                                        AIRTELTBLErr.TransactionType = AIRTELTBL.TransactionType;
                                        AIRTELTBLErr.TransactionAmount = AIRTELTBL.TransactionAmount;
                                        AIRTELTBLErr.PayerPreviousBalance = AIRTELTBL.PayerPreviousBalance;
                                        AIRTELTBLErr.PayerPostBalance = AIRTELTBL.PayerPostBalance;
                                        AIRTELTBLErr.PayeePreBalance = AIRTELTBL.PayeePreBalance;
                                        AIRTELTBLErr.PayeePostBalance = AIRTELTBL.PayeePostBalance;
                                        AIRTELTBLErr.ReferenceNo = AIRTELTBL.ReferenceNo;
                                        AIRTELTBLErr.ExternalRefNo = AIRTELTBL.ExternalRefNo;
                                        AIRTELTBLErr.MatchingStatus = "N";
                                        AIRTELTBLErr.PullDate = DateTime.Now;
                                        AIRTELTBLErr.UserId = 1;
                                        AIRTELTBLErr.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;
                                        AIRTELTBLErr.FileName = fileNamenAndType;

                                        repoAIRTELTransErrorRepository.Add(AIRTELTBLErr);
                                        var ret = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                            continue;
                                        }
                                    }
                                }
                                // Move file after reading below 
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
                                    LogManager.SaveLog("Move  Airtel File Start");
                                    string MvFile = library.MoveFile(FileDirectory + "\\" + fileNamenAndType, ReconType.ProcessedFileDirectory, fileNamenAndType);
                                    LogManager.SaveLog("Move File in Airtel status: " + MvFile);
                                }
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
                                var admDataPoollingControlTBL = new admDataPullingControl();
                                
                                admDataPoollingControlTBL.ReconTypeId = ReconType.ReconTypeId;
                                admDataPoollingControlTBL.FileType = "File";
                                admDataPoollingControlTBL.TableName = "AirtelTrans";
                                admDataPoollingControlTBL.DateCreated = DateTime.Now;
                                admDataPoollingControlTBL.UserId = 1;
                                admDataPoollingControlTBL.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                                admDataPoollingControlTBL.ReconLevel = 1;
                                DateTime dtf = new DateTime();
                                string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                                admDataPoollingControlTBL.LastTransDate = Convert.ToDateTime(dateee);
                                admDataPoollingControlTBL.PullDate = DateTime.Now;
                                admDataPoollingControlTBL.RecordsCount = count;
                                repoadmDataPoollingControlRepository.Add(admDataPoollingControlTBL);
                                var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                if (ret)
                                {

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
                        LogManager.SaveLog("An error occured in AirtelLibrary Line  :" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                        // throw;
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
                LogManager.SaveLog("An error occured in AirtelLibrary Line : " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }

            return string.Empty;
        }
        

    }
 
}