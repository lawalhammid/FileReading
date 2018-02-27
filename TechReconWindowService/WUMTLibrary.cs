
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
    public class WUMTLibrary
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmReconDataSourcesRepository repoadmReconDataSourcesRepository;
        private readonly IadmSourceAccountRepository repoadmSourceAccountRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly IAfricaWorldTransactionRepository repoAfricaWorldTransactionRepository;
        private readonly ICBSWUMTTransRepository repoCBSWUMTTransRepository;
        private readonly IWUMTTransRepository repoWUMTTransRepository;
        private readonly IadmFailledFileRepository repoadmFailledFileRepository;
        private readonly IadmFileProcessedRepository repoadmFileProcessedRepository;
        private readonly ICBSWUMTTransErrorRepository repoCBSWUMTTransErrorRepository;
        private readonly IWUMTTransErrorRepository repoWUMTTransErrorRepository;
        private readonly IWUMTTransHistoryRepository repoWUMTTransHistoryRepository;
        private readonly ICBSWUMTTransHistoryRepository repoCBSWUMTTransHistoryRepository;

        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/WUMTLibrary.cs";
        private static string agentcodestatic;
        private static string AcctNoStatic;
        private static string LegacyStatic;
        private static string SettleCurrStatic;
        private static string TransCurStatic;
        private static string BranchNameStatic;
        public WUMTLibrary()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmReconDataSourcesRepository = new admReconDataSourcesRepository(idbfactory);
            repoadmSourceAccountRepository = new admSourceAccountRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoAfricaWorldTransactionRepository = new AfricaWorldTransactionRepository(idbfactory);
            repoCBSWUMTTransRepository = new CBSWUMTTransRepository(idbfactory);
            repoWUMTTransRepository = new WUMTTransRepository(idbfactory);
            repoadmFailledFileRepository = new admFailledFileRepository(idbfactory);
            repoadmFileProcessedRepository = new admFileProcessedRepository(idbfactory);
            repoWUMTTransErrorRepository = new WUMTTransErrorRepository(idbfactory);
            repoWUMTTransHistoryRepository = new WUMTTransHistoryRepository(idbfactory);
            repoCBSWUMTTransHistoryRepository = new CBSWUMTTransHistoryRepository(idbfactory);
            repoCBSWUMTTransErrorRepository = new CBSWUMTTransErrorRepository(idbfactory);
        }
       
        public async Task<string> PullWUMTData()
        {
            Library library = new Library();
            var dataList = new List<DataList>();
            try
            {
                LogManager.SaveLog("Task Start in WUMTLogManager");
                var ReconType = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "WUMT");
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

                string AcctNoErrolog = string.Empty;
                string AmountErrolog = string.Empty;
                string DrCrErrolog = string.Empty;
                string DescriptionErrolog = string.Empty;
                string TransDateErrolog = string.Empty;
                string OriginBranchErrolog = string.Empty;
                string PostedbyErrolog = string.Empty;
                string BalanceErrolog = string.Empty;
                string ReferenceErrolog = string.Empty;
                string PtIdErrolog = string.Empty;
                string UserIdErrolog = string.Empty;
                string MatchingStatusErrolog = string.Empty;
                string ReconDateErrolog = string.Empty;

                if (dtSouceCon1.DatabaseType == "SYBASE")
                {
                    try
                    {
                        LogManager.SaveLog("PullDataWUMT Start in WUMTLibrary");
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
                        var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "WUMT");
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSWUMTTrans");

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
                            ToDateParam = "'20170930'";
                        }

                        string SqlString = ReconType.Source1Script
                                                                .Replace("{AccountListSource1}", acctlistSource1)
                                                                .Replace("{LastRecordId}", LastRecordId)
                                                                .Replace("{ReconDate}", ReconDate);

                        //{AccountListSource1}  {LastRecordId}  {ReconDate}
                        
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
                                LogManager.SaveLog("An error occured WUMTLibrary Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                                throw;
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

                                LogManager.SaveLog("Total  records  pulled in PullDataWUMT: " + returndata.Rows.Count);
                               
                                if (returndata.Rows.Count > 0)
                                {
                                    int countTrans = 0;
                                    var CBSWUMTTrans = new CBSWUMTTran();
                                    var CBSWUMTTransErrorTBL = new CBSWUMTTransError();
                                    for (int col = 0; col < returndata.Rows.Count; col++)
                                    {
                                        try
                                        {
                                            CBSWUMTTrans.AcctNo = returndata.Rows[col][0] == null ? null : returndata.Rows[col][0].ToString();
                                            decimal Amountad, Amount;
                                            if (decimal.TryParse(returndata.Rows[col][1].ToString(), out Amountad))
                                            {
                                                Amount = Convert.ToDecimal(returndata.Rows[col][1]) != null ? Convert.ToDecimal(returndata.Rows[col][1]) : 0;
                                            }
                                            else
                                            {
                                                Amount = 0;
                                            }
                                            CBSWUMTTrans.Amount = Math.Round(Amount, 2);
                                            CBSWUMTTrans.DrCr = returndata.Rows[col][2] == null ? null : returndata.Rows[col][2].ToString();
                                            CBSWUMTTrans.Description = returndata.Rows[col][3] == null ? null : returndata.Rows[col][3].ToString();
                                            CBSWUMTTrans.TransDate = returndata.Rows[col][4] == null ? (DateTime?)null : Convert.ToDateTime(returndata.Rows[col][4]);
                                            dateTest = CBSWUMTTrans.TransDate.ToString();
                                            if (countTrans > 1)
                                            {
                                                MaxTransDate = CBSWUMTTrans.TransDate.ToString();

                                                if (Convert.ToDateTime(dateTest) > Convert.ToDateTime(MaxTransDate))
                                                {
                                                    MaxTransDate = dateTest;

                                                }
                                                else
                                                {
                                                    MaxTransDate = MaxTransDate;
                                                }
                                            }
                                            CBSWUMTTrans.OriginBranch = returndata.Rows[col][5] == null ? null : returndata.Rows[col][5].ToString();
                                            CBSWUMTTrans.Postedby = returndata.Rows[col][6] == null ? null : returndata.Rows[col][6].ToString();
                                            decimal Balancead, Balance;
                                            if (decimal.TryParse(returndata.Rows[col][7].ToString(), out Balancead))
                                            {
                                                Balance = Convert.ToDecimal(returndata.Rows[col][7]) != null ? Convert.ToDecimal(returndata.Rows[col][7]) : 0;
                                            }
                                            else
                                            {
                                                Balance = 0;
                                            }
                                            CBSWUMTTrans.Balance = Math.Round(Balance, 2);
                                            CBSWUMTTrans.Reference = returndata.Rows[col][8] == null ? null : returndata.Rows[col][8].ToString();
                                            CBSWUMTTrans.OrigRefNo = CBSWUMTTrans.Reference;
                                            CBSWUMTTrans.PtId = returndata.Rows[col][9] == null ? null : returndata.Rows[col][9].ToString();
                                            LastRecordId = returndata.Rows[col][9].ToString();
                                            CBSWUMTTrans.PullDate = DateTime.Now;
                                            CBSWUMTTrans.MatchingStatus = "N";
                                            CBSWUMTTrans.UserId = 1;
                                            int revcode;
                                            CBSWUMTTrans.ReversalCode = returndata.Rows[col][10] == null ? (int?)null : int.TryParse(returndata.Rows[col][10].ToString(), out revcode) ? revcode : (int?)null ;
                                            countTrans += 1;
                                            if (!string.IsNullOrWhiteSpace(CBSWUMTTrans.Reference))
                                            {
                                                var exist = await repoCBSWUMTTransRepository.GetAsync(c => c.Reference == CBSWUMTTrans.Reference && (c.Amount == CBSWUMTTrans.Amount) && (c.AcctNo == CBSWUMTTrans.AcctNo));
                                                var existHis = await repoCBSWUMTTransHistoryRepository.GetAsync(c => c.Reference == CBSWUMTTrans.Reference && (c.Amount == CBSWUMTTrans.Amount) && (c.AcctNo == CBSWUMTTrans.AcctNo));
                                                if (exist != null || existHis != null)
                                                {
                                                    CBSWUMTTransErrorTBL.AcctNo = CBSWUMTTrans.AcctNo ;
                                                    CBSWUMTTransErrorTBL.Amount = CBSWUMTTrans.Amount ;
                                                    CBSWUMTTransErrorTBL.DrCr = CBSWUMTTrans.DrCr ;
                                                    CBSWUMTTransErrorTBL.Description = CBSWUMTTrans.Description ;
                                                    CBSWUMTTransErrorTBL.TransDate = CBSWUMTTrans.TransDate ;
                                                    CBSWUMTTransErrorTBL.OriginBranch = CBSWUMTTrans.OriginBranch ;
                                                    CBSWUMTTransErrorTBL.Postedby = CBSWUMTTrans.Postedby ;
                                                    CBSWUMTTransErrorTBL.Balance = CBSWUMTTrans.Balance ;
                                                    CBSWUMTTransErrorTBL.Reference = CBSWUMTTrans.Reference ;
                                                    CBSWUMTTransErrorTBL.OrigRefNo = CBSWUMTTrans.OrigRefNo ;
                                                    CBSWUMTTransErrorTBL.PtId = CBSWUMTTrans.PtId;
                                                    CBSWUMTTransErrorTBL.UserId = 1;
                                                    CBSWUMTTransErrorTBL.ErrorMsg = "Duplicate transaction record";
                                                    CBSWUMTTransErrorTBL.ReversalCode = CBSWUMTTrans.ReversalCode;
                                                    CBSWUMTTransErrorTBL.PullDate = DateTime.Now;
                                                    repoCBSWUMTTransErrorRepository.Add(CBSWUMTTransErrorTBL);
                                                    var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret1)
                                                    {
                                                        continue;
                                                    }
                                                    continue;
                                                }
                                                else
                                                {
                                                    repoCBSWUMTTransRepository.Add(CBSWUMTTrans);
                                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            else
                                            {

                                                CBSWUMTTransErrorTBL.AcctNo = CBSWUMTTrans.AcctNo;
                                                CBSWUMTTransErrorTBL.Amount = CBSWUMTTrans.Amount;
                                                CBSWUMTTransErrorTBL.DrCr = CBSWUMTTrans.DrCr;
                                                CBSWUMTTransErrorTBL.Description = CBSWUMTTrans.Description;
                                                CBSWUMTTransErrorTBL.TransDate = CBSWUMTTrans.TransDate;
                                                CBSWUMTTransErrorTBL.OriginBranch = CBSWUMTTrans.OriginBranch;
                                                CBSWUMTTransErrorTBL.Postedby = CBSWUMTTrans.Postedby;
                                                CBSWUMTTransErrorTBL.Balance = CBSWUMTTrans.Balance;
                                                CBSWUMTTransErrorTBL.Reference = CBSWUMTTrans.Reference;
                                                CBSWUMTTransErrorTBL.OrigRefNo = CBSWUMTTrans.OrigRefNo;
                                                CBSWUMTTransErrorTBL.PtId = CBSWUMTTrans.PtId;
                                                CBSWUMTTransErrorTBL.UserId = 1;
                                                CBSWUMTTransErrorTBL.ReversalCode = CBSWUMTTrans.ReversalCode;
                                                CBSWUMTTransErrorTBL.ErrorMsg = "No ref No.";
                                                CBSWUMTTransErrorTBL.PullDate = DateTime.Now;
                                                repoCBSWUMTTransErrorRepository.Add(CBSWUMTTransErrorTBL);
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
                                            LogManager.SaveLog("An error occured WUMTLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                                            try
                                            {

                                                CBSWUMTTransErrorTBL.AcctNo = CBSWUMTTrans.AcctNo;
                                                CBSWUMTTransErrorTBL.Amount = CBSWUMTTrans.Amount;
                                                CBSWUMTTransErrorTBL.DrCr = CBSWUMTTrans.DrCr;
                                                CBSWUMTTransErrorTBL.Description = CBSWUMTTrans.Description;
                                                CBSWUMTTransErrorTBL.TransDate = CBSWUMTTrans.TransDate;
                                                CBSWUMTTransErrorTBL.OriginBranch = CBSWUMTTrans.OriginBranch;
                                                CBSWUMTTransErrorTBL.Postedby = CBSWUMTTrans.Postedby;
                                                CBSWUMTTransErrorTBL.Balance = CBSWUMTTrans.Balance;
                                                CBSWUMTTransErrorTBL.Reference = CBSWUMTTrans.Reference;
                                                CBSWUMTTransErrorTBL.OrigRefNo = CBSWUMTTrans.OrigRefNo;
                                                CBSWUMTTransErrorTBL.PtId = CBSWUMTTrans.PtId;
                                                CBSWUMTTransErrorTBL.UserId = 1;
                                                CBSWUMTTransErrorTBL.ReversalCode = CBSWUMTTrans.ReversalCode;
                                                CBSWUMTTransErrorTBL.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message; ;
                                                CBSWUMTTransErrorTBL.PullDate = DateTime.Now;

                                                repoCBSWUMTTransErrorRepository.Add(CBSWUMTTransErrorTBL);
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
                                                LogManager.SaveLog("An error occured in Line WUMTLibrary : " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                                        admDataPoollingControlTBL.TableName = "CBSWUMTTrans";
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
                                LogManager.SaveLog("An error occured in WUMTLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                        LogManager.SaveLog("An error occured in WUMTLibrary Line :" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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

                        var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "WUMT");
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "WUMTTrans");

                        if (controlTable != null)
                        {
                            LastRecordId = controlTable.LastRecordId;
                        }
                        else
                        {
                            LastRecordId = "0";
                        }


                        var WUMTTransTBL = new WUMTTran();
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
                            ggg = DLIST;
                        }
                        string fileNamenAndType = string.Empty;
                        string FileLastTime = string.Empty;
                        string NofOrecFile = string.Empty;
                        int count = 0;
                        foreach (var kl in ggg)
                        {
                            fileNamenAndType = (from f in DLIST orderby f.LastWriteTime ascending select kl).First().Name;
                            FileLastTime = (from f in DLIST orderby f.LastAccessTimeUtc descending select kl).First().LastWriteTime.ToString();

                            dTable = await library.ReadExcel(FileDirectory, ReconTypeId);

                            LogManager.SaveLog("No of Record pull for WUMT File: " + dTable.Rows.Count);

                            if (dTable.Rows.Count > 0)
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
                                var WUMTTransErrorTBL = new WUMTTransError();
                                int countTrans = 0;
                                int NoOfRet = 0;
                                foreach (DataRow row in dTable.Rows.Cast<DataRow>())
                                {
                                    DateTime dt = new DateTime();
                                    decimal decvalue = 0;
                                    try
                                    {

                                        string agentCode = row[0].ToString();
                                        string SentDate = string.Empty;
                                        string[] agentcode = agentCode.Split('(');

                                        if (agentcode.Count() > 1)
                                        {
                                            if (agentcode[1].StartsWith("AI"))
                                            {
                                                agentCode = agentcode[1].Replace(")", "");
                                                agentcodestatic = agentCode;
                                            }
                                        }
                                        double date = 0;
                                        string rowstr = row[1] != null ? row[1].ToString() : "";

                                        if (double.TryParse(rowstr, out date))
                                        {

                                            WUMTTransTBL.AgentCode = agentcodestatic;
                                            WUMTTransTBL.SentDate = DateTime.FromOADate(date);
                                            dateTest = WUMTTransTBL.SentDate.ToString();
                                            if (countTrans > 1)
                                            {
                                                MaxTransDate = WUMTTransTBL.SentDate.ToString();

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
                                            
                                            WUMTTransTBL.TransReference = row[3] == null ? null : row[3].ToString();
                                            WUMTTransTBL.OrigRefNo = WUMTTransTBL.TransReference;
                                            WUMTTransTBL.Spread = row[5] == null ? (Decimal?)null : decimal.TryParse(row[5].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.SettleRate = row[7] == null ? (Decimal?)null : decimal.TryParse(row[7].ToString(), out decvalue) ? Math.Round(decvalue, 9) : (Decimal?)null;
                                            WUMTTransTBL.PayoutRate = row[9] == null ? (Decimal?)null : decimal.TryParse(row[9].ToString(), out decvalue) ? Math.Round(decvalue, 9) : (Decimal?)null;
                                            WUMTTransTBL.SentCurrencyCode = row[11] == null ? null : row[11].ToString();
                                            WUMTTransTBL.SentPrincipal = row[12] == null ? (Decimal?)null : decimal.TryParse(row[12].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.SentCharges = row[15] == null ? (Decimal?)null : decimal.TryParse(row[15].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.SettledPrincipal = row[17] == null ? (Decimal?)null : decimal.TryParse(row[17].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.SettledCharges = row[20] == null ? (Decimal?)null : decimal.TryParse(row[20].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.SettledFx = row[22] == null ? (Decimal?)null : decimal.TryParse(row[22].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.Total = row[24] == null ? (Decimal?)null : decimal.TryParse(row[24].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;

                                            count = 2;
                                            continue;
                                        }


                                        if(rowstr.Contains("/"))
                                        {
                                            var dtw = rowstr.Split('/');
                                            rowstr = dtw[1] + "/" + dtw[0] + "/" + dtw[2];
                                        }

                                        string dt21 = rowstr == null ? string.Empty : DateTime.TryParse(rowstr, out dt) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(rowstr)) : string.Empty;
                                        if (DateTime.TryParse(dt21, out dt) && count > 1)
                                        {
                                            WUMTTransTBL.PaidDate = dt == null ? (DateTime?)null : dt;
                                            WUMTTransTBL.SentCountry = row[3] == null ? null : row[3].ToString();
                                            WUMTTransTBL.LcySettleRate = row[6] == null ? (Decimal?)null : decimal.TryParse(row[6].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.LcyPayoutRate = row[8] == null ? (Decimal?)null : decimal.TryParse(row[8].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.LcyCurrencyCode = row[10] == null ? null : row[10].ToString();
                                            WUMTTransTBL.LcySettledPrin = row[11] == null ? (Decimal?)null : decimal.TryParse(row[11].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.LcyCharges = row[14] == null ? (Decimal?)null : decimal.TryParse(row[14].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.LcySettledPrincipal = row[16] == null ? (Decimal?)null : decimal.TryParse(row[16].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.LcySettledCharges = row[19] == null ? (Decimal?)null : decimal.TryParse(row[19].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.lcySettledFx = row[21] == null ? (Decimal?)null : decimal.TryParse(row[21].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;

                                            WUMTTransTBL.lcyTotal = row[23] == null ? (Decimal?)null : decimal.TryParse(row[23].ToString(), out decvalue) ? Math.Round(decvalue, 2) : (Decimal?)null;
                                            WUMTTransTBL.PullDate = DateTime.Now;
                                            WUMTTransTBL.UserId = 1;
                                            WUMTTransTBL.FileName = fileNamenAndType;
                                            WUMTTransTBL.MatchingStatus = "N";
                                            if (!string.IsNullOrWhiteSpace(WUMTTransTBL.TransReference))
                                            {
                                                var exist = await repoWUMTTransRepository.GetAsync(c => c.TransReference == WUMTTransTBL.TransReference);
                                                var existHis = await repoWUMTTransHistoryRepository.GetAsync(c => c.TransReference == WUMTTransTBL.TransReference);
                                                if ((exist != null) || (existHis != null))
                                                {
                                                    WUMTTransErrorTBL.AgentCode = WUMTTransTBL.AgentCode;
                                                    WUMTTransErrorTBL.SentDate = WUMTTransTBL.SentDate;
                                                    WUMTTransErrorTBL.TransReference = WUMTTransTBL.TransReference;
                                                    WUMTTransErrorTBL.OrigRefNo = WUMTTransTBL.TransReference;
                                                    WUMTTransErrorTBL.Spread = WUMTTransTBL.Spread;
                                                    WUMTTransErrorTBL.SettleRate = WUMTTransTBL.SettleRate;
                                                    WUMTTransErrorTBL.PayoutRate = WUMTTransTBL.PayoutRate;
                                                    WUMTTransErrorTBL.SentCurrencyCode = WUMTTransTBL.SentCurrencyCode;
                                                    WUMTTransErrorTBL.SentPrincipal = WUMTTransTBL.SentPrincipal;
                                                    WUMTTransErrorTBL.SentCharges = WUMTTransTBL.SentCharges;
                                                    WUMTTransErrorTBL.SettledPrincipal = WUMTTransTBL.SettledPrincipal;
                                                    WUMTTransErrorTBL.SettledCharges = WUMTTransTBL.SettledCharges;
                                                    WUMTTransErrorTBL.SettledFx = WUMTTransTBL.SettledFx;
                                                    WUMTTransErrorTBL.Total = WUMTTransTBL.Total;
                                                    WUMTTransErrorTBL.PaidDate = WUMTTransTBL.PaidDate;
                                                    WUMTTransErrorTBL.SentCountry = WUMTTransTBL.SentCountry;
                                                    WUMTTransErrorTBL.LcySettleRate = WUMTTransTBL.LcySettleRate;
                                                    WUMTTransErrorTBL.LcyPayoutRate = WUMTTransTBL.LcyPayoutRate;
                                                    WUMTTransErrorTBL.LcyCurrencyCode = WUMTTransTBL.LcyCurrencyCode;
                                                    WUMTTransErrorTBL.LcySettledPrin = WUMTTransTBL.LcySettledPrin;
                                                    WUMTTransErrorTBL.LcyCharges = WUMTTransTBL.LcyCharges;
                                                    WUMTTransErrorTBL.LcySettledPrincipal = WUMTTransTBL.LcySettledPrincipal;
                                                    WUMTTransErrorTBL.LcySettledCharges = WUMTTransTBL.LcySettledCharges;
                                                    WUMTTransErrorTBL.lcySettledFx = WUMTTransTBL.lcySettledFx;
                                                    WUMTTransErrorTBL.lcyTotal = WUMTTransTBL.lcyTotal;
                                                    WUMTTransErrorTBL.PullDate = DateTime.Now;
                                                    WUMTTransErrorTBL.UserId = 1;
                                                    WUMTTransErrorTBL.ErrorMsg = "Duplicate transaction record";
                                                    WUMTTransErrorTBL.FileName = fileNamenAndType;

                                                    repoWUMTTransErrorRepository.Add(WUMTTransErrorTBL);
                                                    var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret1)
                                                    {
                                                        count = 0;
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    repoWUMTTransRepository.Add(WUMTTransTBL);
                                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                        NoOfRet +=1;
                                                        NofOrecFile = NoOfRet.ToString();
                                                        count = 0;
                                                        continue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                WUMTTransErrorTBL.AgentCode = WUMTTransTBL.AgentCode;
                                                WUMTTransErrorTBL.SentDate = WUMTTransTBL.SentDate;
                                                WUMTTransErrorTBL.TransReference = WUMTTransTBL.TransReference;
                                                WUMTTransErrorTBL.OrigRefNo = WUMTTransTBL.TransReference;
                                                WUMTTransErrorTBL.Spread = WUMTTransTBL.Spread;
                                                WUMTTransErrorTBL.SettleRate = WUMTTransTBL.SettleRate;
                                                WUMTTransErrorTBL.PayoutRate = WUMTTransTBL.PayoutRate;
                                                WUMTTransErrorTBL.SentCurrencyCode = WUMTTransTBL.SentCurrencyCode;
                                                WUMTTransErrorTBL.SentPrincipal = WUMTTransTBL.SentPrincipal;
                                                WUMTTransErrorTBL.SentCharges = WUMTTransTBL.SentCharges;
                                                WUMTTransErrorTBL.SettledPrincipal = WUMTTransTBL.SettledPrincipal;
                                                WUMTTransErrorTBL.SettledCharges = WUMTTransTBL.SettledCharges;
                                                WUMTTransErrorTBL.SettledFx = WUMTTransTBL.SettledFx;
                                                WUMTTransErrorTBL.Total = WUMTTransTBL.Total;
                                                WUMTTransErrorTBL.PaidDate = WUMTTransTBL.PaidDate;
                                                WUMTTransErrorTBL.SentCountry = WUMTTransTBL.SentCountry;
                                                WUMTTransErrorTBL.LcySettleRate = WUMTTransTBL.LcySettleRate;
                                                WUMTTransErrorTBL.LcyPayoutRate = WUMTTransTBL.LcyPayoutRate;
                                                WUMTTransErrorTBL.LcyCurrencyCode = WUMTTransTBL.LcyCurrencyCode;
                                                WUMTTransErrorTBL.LcySettledPrin = WUMTTransTBL.LcySettledPrin;
                                                WUMTTransErrorTBL.LcyCharges = WUMTTransTBL.LcyCharges;
                                                WUMTTransErrorTBL.LcySettledPrincipal = WUMTTransTBL.LcySettledPrincipal;
                                                WUMTTransErrorTBL.LcySettledCharges = WUMTTransTBL.LcySettledCharges;
                                                WUMTTransErrorTBL.lcySettledFx = WUMTTransTBL.lcySettledFx;
                                                WUMTTransErrorTBL.lcyTotal = WUMTTransTBL.lcyTotal;
                                                WUMTTransErrorTBL.UserId = 1;
                                                WUMTTransErrorTBL.ErrorMsg = "No ref No.";
                                                WUMTTransErrorTBL.FileName = fileNamenAndType;
                                                WUMTTransErrorTBL.PullDate = DateTime.Now;

                                                repoWUMTTransErrorRepository.Add(WUMTTransErrorTBL);
                                                var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret1)
                                                {
                                                    count = 0;
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
                                        LogManager.SaveLog("An error occured in WUMTLibrary Line : " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                                        try
                                        {
                                            WUMTTransErrorTBL.AgentCode = WUMTTransTBL.AgentCode;
                                            WUMTTransErrorTBL.SentDate = WUMTTransTBL.SentDate;
                                            WUMTTransErrorTBL.TransReference = WUMTTransTBL.TransReference;
                                            WUMTTransErrorTBL.OrigRefNo = WUMTTransTBL.TransReference;
                                            WUMTTransErrorTBL.Spread = WUMTTransTBL.Spread;
                                            WUMTTransErrorTBL.SettleRate = WUMTTransTBL.SettleRate;
                                            WUMTTransErrorTBL.PayoutRate = WUMTTransTBL.PayoutRate;
                                            WUMTTransErrorTBL.SentCurrencyCode = WUMTTransTBL.SentCurrencyCode;
                                            WUMTTransErrorTBL.SentPrincipal = WUMTTransTBL.SentPrincipal;
                                            WUMTTransErrorTBL.SentCharges = WUMTTransTBL.SentCharges;
                                            WUMTTransErrorTBL.SettledPrincipal = WUMTTransTBL.SettledPrincipal;
                                            WUMTTransErrorTBL.SettledCharges = WUMTTransTBL.SettledCharges;
                                            WUMTTransErrorTBL.SettledFx = WUMTTransTBL.SettledFx;
                                            WUMTTransErrorTBL.Total = WUMTTransTBL.Total;
                                            WUMTTransErrorTBL.PaidDate = WUMTTransTBL.PaidDate;
                                            WUMTTransErrorTBL.SentCountry = WUMTTransTBL.SentCountry;
                                            WUMTTransErrorTBL.LcySettleRate = WUMTTransTBL.LcySettleRate;
                                            WUMTTransErrorTBL.LcyPayoutRate = WUMTTransTBL.LcyPayoutRate;
                                            WUMTTransErrorTBL.LcyCurrencyCode = WUMTTransTBL.LcyCurrencyCode;
                                            WUMTTransErrorTBL.LcySettledPrin = WUMTTransTBL.LcySettledPrin;
                                            WUMTTransErrorTBL.LcyCharges = WUMTTransTBL.LcyCharges;
                                            WUMTTransErrorTBL.LcySettledPrincipal = WUMTTransTBL.LcySettledPrincipal;
                                            WUMTTransErrorTBL.LcySettledCharges = WUMTTransTBL.LcySettledCharges;
                                            WUMTTransErrorTBL.lcySettledFx = WUMTTransTBL.lcySettledFx;
                                            WUMTTransErrorTBL.lcyTotal = WUMTTransTBL.lcyTotal;
                                            WUMTTransErrorTBL.PullDate = DateTime.Now;
                                            WUMTTransErrorTBL.UserId = 1;
                                            WUMTTransErrorTBL.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message; ;

                                            repoWUMTTransErrorRepository.Add(WUMTTransErrorTBL);
                                            var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                            if (ret1)
                                            {
                                                count = 0;
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
                                            LogManager.SaveLog("An error occured in  Line WUMTLibrary :" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                                            count = 0;
                                            continue;
                                        }

                                        continue;
                                    }
                                }
                                //Move File here 
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
                                var ret3 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                if (ret3)
                                {
                                    LogManager.SaveLog("Move  WUMT File Start");
                                    string MvFile = library.MoveFile(FileDirectory + "\\" + fileNamenAndType, ReconType.ProcessedFileDirectory, fileNamenAndType);
                                    LogManager.SaveLog("Move File in WUMT status: " + MvFile);
                                }
                                //Update Control Table
                                if (controlTable != null)
                                {
                                    controlTable.LastRecordId = WUMTTransTBL.OrigRefNo;
                                    controlTable.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                                    DateTime dtf = new DateTime();
                                    string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                                    controlTable.LastTransDate = Convert.ToDateTime(dateee);
                                    controlTable.PullDate = DateTime.Now;
                                    controlTable.RecordsCount = Convert.ToInt32(NofOrecFile);
                                    repoadmDataPoollingControlRepository.Update(controlTable);
                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                    if (ret)
                                    {

                                    }
                                }
                                else
                                {
                                    var admDataPoollingControlTBL = new admDataPullingControl();
                                    admDataPoollingControlTBL.LastRecordId = WUMTTransTBL.OrigRefNo;
                                    admDataPoollingControlTBL.ReconTypeId = rectype.ReconTypeId;
                                    admDataPoollingControlTBL.FileType = "File";
                                    admDataPoollingControlTBL.TableName = "WUMTTrans";
                                    admDataPoollingControlTBL.DateCreated = DateTime.Now;
                                    admDataPoollingControlTBL.UserId = 1;
                                    admDataPoollingControlTBL.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                                    DateTime dtf = new DateTime();
                                    string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                                    admDataPoollingControlTBL.LastTransDate = Convert.ToDateTime(dateee);
                                    admDataPoollingControlTBL.PullDate = DateTime.Now;
                                    admDataPoollingControlTBL.RecordsCount = Convert.ToInt32(NofOrecFile);
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
                        LogManager.SaveLog("An error occured in  Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                LogManager.SaveLog("An error occured in WUMTLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
            return string.Empty;
        }

    }
  
  
}