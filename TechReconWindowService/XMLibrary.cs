
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
    public class XMLibrary
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmReconDataSourcesRepository repoadmReconDataSourcesRepository;
        private readonly IadmSourceAccountRepository repoadmSourceAccountRepository;
        private readonly ICBSTransationTestRepository repoCBSTransationTestRepository;
        private readonly IPostillionTransationTestRepository repoPostillionTransationTestRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly ICBSConsortiumTransactionRepository repoCBSConsortiumTransactionRepository;
        private readonly IConsortiumTransactionRepository repoConsortiumTransactionRepository;
        private readonly IadmTerminalRepository repoadmTerminalRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly IAfricaWorldTransactionRepository repoAfricaWorldTransactionRepository;
        private readonly ICBSAfricaWorldTransactionRepository repoCBSAfricaWorldTransactionRepository;
        private readonly ICBSRiaRepository repoCBSRiaRepository;
        private readonly ICBSXPressMoneyRepository repoCBSXPressMoneyRepository;
        private readonly IRiaRepository repoRiaRepository;
        private readonly IXPressMoneyRepository repoXPressMoneyRepository;
        private readonly IadmFailledFileRepository repoadmFailledFileRepository;
        private readonly IadmFileProcessedRepository repoadmFileProcessedRepository;
        private readonly ICBSWUMTTransErrorRepository repoCBSWUMTTransErrorRepository;
        private readonly ICBSXpressMoneyTransErrorRepository repoCBSXpressMoneyTransErrorRepository;
        private readonly IConsortiumTransactionErrorRepository repoConsortiumTransactionErrorRepository;
        private readonly IXpressMoneyTransErrorRepository repoXpressMoneyTransErrorRepository;
        private readonly IXPressMoneyTransHistoryRepository repoXPressMoneyTransHistoryRepository;
        private readonly ICBSXPressMoneyHistoryRepository repoCBSXPressMoneyHistoryRepository;
        
        
        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/XMLibrary.cs";
       
        public XMLibrary()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmReconDataSourcesRepository = new admReconDataSourcesRepository(idbfactory);
            repoadmSourceAccountRepository = new admSourceAccountRepository(idbfactory);
            repoCBSTransationTestRepository = new CBSTransationTestRepository(idbfactory);
            repoPostillionTransationTestRepository = new PostillionTransationTestRepository(idbfactory);
            repoCBSConsortiumTransactionRepository = new CBSConsortiumTransactionRepository(idbfactory);
            repoConsortiumTransactionRepository = new ConsortiumTransactionRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoadmTerminalRepository = new admTerminalRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoAfricaWorldTransactionRepository = new AfricaWorldTransactionRepository(idbfactory);
            repoCBSAfricaWorldTransactionRepository = new CBSAfricaWorldTransactionRepository(idbfactory);
            repoCBSRiaRepository = new CBSRiaRepository(idbfactory);
            repoCBSXPressMoneyRepository = new CBSXPressMoneyRepository(idbfactory);
            repoRiaRepository = new RiaRepository(idbfactory);
            repoXPressMoneyRepository = new XPressMoneyRepository(idbfactory);
            repoadmFailledFileRepository = new admFailledFileRepository(idbfactory);
            repoadmFileProcessedRepository = new admFileProcessedRepository(idbfactory);
            repoCBSWUMTTransErrorRepository = new CBSWUMTTransErrorRepository(idbfactory);
            repoCBSXpressMoneyTransErrorRepository = new CBSXpressMoneyTransErrorRepository(idbfactory);
            repoConsortiumTransactionErrorRepository = new ConsortiumTransactionErrorRepository(idbfactory);
            repoXpressMoneyTransErrorRepository = new XpressMoneyTransErrorRepository(idbfactory);
            repoXPressMoneyTransHistoryRepository = new XPressMoneyTransHistoryRepository(idbfactory);
            repoCBSXPressMoneyHistoryRepository = new CBSXPressMoneyHistoryRepository(idbfactory);
  
        }

        public async Task<string> PullXpressMoneyData()
        {
            Library library = new Library();
            try
            {
                LogManager.SaveLog("Task Start in XMLibrary");
                var ReconType = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "XpressMoney");
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
                        LogManager.SaveLog("PullDataXpressMoney Start in XMLibrary");
                        var value1 = string.Empty;
                        value1 = string.Empty;

                        var value2 = string.Empty;
                        value2 = string.Empty;
                        int scriptExecTtype = 0;
                        var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "XpressMoney");
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSXpressMoneyTrans");
                        string FromDateParam = string.Empty;
                        string ToDateParam = string.Empty;
                        string ReconDate = string.Empty;
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
                            FromDateParam = "'20170901'";
                            ToDateParam = "'20170931'";
                            ReconDate = "'20170901'";
                            LastRecordId = "0";

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
                                LogManager.SaveLog("An error occured XMLibrary Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                            }
                            string RefNo = string.Empty;
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

                                LogManager.SaveLog("Total records pulled in CBSXpress: " + returndata.Rows.Count);

                                if (returndata.Rows.Count > 0)
                                {
                                    var CBSExpressMoneyTBL = new CBSXpressMoneyTran();
                                    var CBSXPressLogTBLErr = new CBSXpressMoneyTransError();
                                    int countTrans = 0;
                                    
                                    for (int col = 0; col < returndata.Rows.Count; col++)
                                    {
                                        try
                                        {
                                            CBSExpressMoneyTBL.AcctNo = returndata.Rows[col][0] == null ? null : returndata.Rows[col][0].ToString();
                                            decimal Amountad, Amount;
                                            if (decimal.TryParse(returndata.Rows[col][1].ToString(), out Amountad))
                                            {
                                                Amount = Convert.ToDecimal(returndata.Rows[col][1]) != null ? Convert.ToDecimal(returndata.Rows[col][1]) : 0;
                                            }
                                            else
                                            {
                                                Amount = 0;
                                            }
                                            CBSExpressMoneyTBL.Amount = Math.Round(Amount, 2);
                                            CBSExpressMoneyTBL.DebitCredit = returndata.Rows[col][2] == null ? null : returndata.Rows[col][2].ToString();
                                            CBSExpressMoneyTBL.Description = returndata.Rows[col][3] == null ? null : returndata.Rows[col][3].ToString();
                                            CBSExpressMoneyTBL.TransDate = returndata.Rows[col][4] == null ? (DateTime?)null : Convert.ToDateTime(returndata.Rows[col][4]);
                                            dateTest = CBSExpressMoneyTBL.TransDate.ToString();
                                            if (countTrans > 1)
                                            {
                                                MaxTransDate = CBSExpressMoneyTBL.TransDate.ToString();

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
                                            CBSExpressMoneyTBL.OriginBranch = returndata.Rows[col][5] == null ? null : returndata.Rows[col][5].ToString();
                                            CBSExpressMoneyTBL.PostedBy = returndata.Rows[col][6] == null ? null : returndata.Rows[col][6].ToString();
                                            decimal Balancead, Balance;
                                            if (decimal.TryParse(returndata.Rows[col][7].ToString(), out Balancead))
                                            {
                                                Balance = Convert.ToDecimal(returndata.Rows[col][7]) != null ? Convert.ToDecimal(returndata.Rows[col][7]) : 0;
                                            }
                                            else
                                            {
                                                Balance = 0;
                                            }

                                            CBSExpressMoneyTBL.Balance = Math.Round(Balance);
                                            CBSExpressMoneyTBL.Reference = returndata.Rows[col][8] == null ? null : returndata.Rows[col][8].ToString();
                                            CBSExpressMoneyTBL.OrigRefNo = CBSExpressMoneyTBL.Reference;
                                            CBSExpressMoneyTBL.PtId = returndata.Rows[col][9] == null ? null : returndata.Rows[col][9].ToString();
                                            LastRecordId = returndata.Rows[col][9] == null ? "0" : returndata.Rows[col][9].ToString();
                                            int revcode;
                                            CBSExpressMoneyTBL.ReversalCode = returndata.Rows[col][10] == null ? (int?)null : int.TryParse(returndata.Rows[col][10].ToString(), out revcode) ? revcode : (int?)null ;
                                            CBSExpressMoneyTBL.PullDate = DateTime.Now;
                                            CBSExpressMoneyTBL.MatchingStatus = "N";
                                            CBSExpressMoneyTBL.UserId = 1;

                                            if (!string.IsNullOrWhiteSpace(CBSExpressMoneyTBL.Reference))
                                            {
                                                var exist = await repoCBSXPressMoneyRepository.GetAsync(c => c.Reference == CBSExpressMoneyTBL.Reference && (c.DebitCredit == CBSExpressMoneyTBL.DebitCredit) && (c.Amount == CBSExpressMoneyTBL.Amount) && (c.AcctNo == CBSExpressMoneyTBL.AcctNo));
                                                var existHis = await repoCBSXPressMoneyHistoryRepository.GetAsync(c => c.Refference == CBSExpressMoneyTBL.Reference && (c.DebitCredit == CBSExpressMoneyTBL.DebitCredit) && (c.Amount == CBSExpressMoneyTBL.Amount) && (c.AcctNo == CBSExpressMoneyTBL.AcctNo));
                                                if (exist != null || existHis != null)
                                                {
                                                    CBSXPressLogTBLErr.AcctNo = CBSExpressMoneyTBL.AcctNo;
                                                    CBSXPressLogTBLErr.Amount = CBSExpressMoneyTBL.Amount;
                                                    CBSXPressLogTBLErr.DebitCredit = CBSExpressMoneyTBL.DebitCredit;
                                                    CBSXPressLogTBLErr.Description = CBSExpressMoneyTBL.Description;
                                                    CBSXPressLogTBLErr.TransDate = CBSExpressMoneyTBL.TransDate;
                                                    CBSXPressLogTBLErr.OriginBranch = CBSExpressMoneyTBL.OriginBranch;
                                                    CBSXPressLogTBLErr.PostedBy = CBSExpressMoneyTBL.PostedBy;
                                                    CBSXPressLogTBLErr.Balance = CBSExpressMoneyTBL.Balance;
                                                    CBSXPressLogTBLErr.PullDate = DateTime.Now;
                                                    CBSXPressLogTBLErr.UserId = CBSExpressMoneyTBL.UserId; ;
                                                    CBSXPressLogTBLErr.Reference = CBSExpressMoneyTBL.Reference;
                                                    CBSXPressLogTBLErr.OrigRefNo = CBSExpressMoneyTBL.Reference;
                                                    CBSXPressLogTBLErr.ReversalCode = CBSExpressMoneyTBL.ReversalCode;
                                                    CBSXPressLogTBLErr.PtId = CBSExpressMoneyTBL.PtId;
                                                    CBSXPressLogTBLErr.ErrorMsg = "Duplicate transaction record";

                                                    repoCBSXpressMoneyTransErrorRepository.Add(CBSXPressLogTBLErr);
                                                    var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret1)
                                                    {
                                                        continue;
                                                    }
                                                    continue;
                                                }
                                                else
                                                {
                                                    repoCBSXPressMoneyRepository.Add(CBSExpressMoneyTBL);
                                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                CBSXPressLogTBLErr.AcctNo = CBSExpressMoneyTBL.AcctNo;
                                                CBSXPressLogTBLErr.Amount = CBSExpressMoneyTBL.Amount;
                                                CBSXPressLogTBLErr.DebitCredit = CBSExpressMoneyTBL.DebitCredit;
                                                CBSXPressLogTBLErr.Description = CBSExpressMoneyTBL.Description;
                                                CBSXPressLogTBLErr.TransDate = CBSExpressMoneyTBL.TransDate;
                                                CBSXPressLogTBLErr.OriginBranch = CBSExpressMoneyTBL.OriginBranch;
                                                CBSXPressLogTBLErr.PostedBy = CBSExpressMoneyTBL.PostedBy;
                                                CBSXPressLogTBLErr.Balance = CBSExpressMoneyTBL.Balance;
                                                CBSXPressLogTBLErr.PullDate = DateTime.Now;
                                                CBSXPressLogTBLErr.UserId = CBSExpressMoneyTBL.UserId; ;
                                                CBSXPressLogTBLErr.Reference = CBSExpressMoneyTBL.Reference;
                                                CBSXPressLogTBLErr.OrigRefNo = CBSExpressMoneyTBL.Reference;
                                                CBSXPressLogTBLErr.ReversalCode = CBSExpressMoneyTBL.ReversalCode;
                                                CBSXPressLogTBLErr.PtId = CBSExpressMoneyTBL.PtId;
                                                CBSXPressLogTBLErr.ErrorMsg = "No trans ref";

                                                repoCBSXpressMoneyTransErrorRepository.Add(CBSXPressLogTBLErr);
                                                var ret1 = await  unitOfWork.Commit(0, null) > 0 ? true : false;
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
                                            LogManager.SaveLog("An error occured XMLogManager Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr + "Tran Id: " + RefNo);

                                            try
                                            {

                                                CBSXPressLogTBLErr.AcctNo = CBSExpressMoneyTBL.AcctNo;
                                                CBSXPressLogTBLErr.Amount = CBSExpressMoneyTBL.Amount;
                                                CBSXPressLogTBLErr.DebitCredit = CBSExpressMoneyTBL.DebitCredit;
                                                CBSXPressLogTBLErr.Description = CBSExpressMoneyTBL.Description;
                                                CBSXPressLogTBLErr.TransDate = CBSExpressMoneyTBL.TransDate;
                                                CBSXPressLogTBLErr.OriginBranch = CBSExpressMoneyTBL.OriginBranch;
                                                CBSXPressLogTBLErr.PostedBy = CBSExpressMoneyTBL.PostedBy;
                                                CBSXPressLogTBLErr.Balance = CBSExpressMoneyTBL.Balance;
                                                CBSXPressLogTBLErr.PullDate = DateTime.Now;
                                                CBSXPressLogTBLErr.UserId = CBSExpressMoneyTBL.UserId; ;
                                                CBSXPressLogTBLErr.Reference = CBSExpressMoneyTBL.Reference;
                                                CBSXPressLogTBLErr.OrigRefNo = CBSExpressMoneyTBL.Reference;
                                                CBSXPressLogTBLErr.ReversalCode = CBSExpressMoneyTBL.ReversalCode;
                                                CBSXPressLogTBLErr.PtId = CBSExpressMoneyTBL.PtId;
                                                CBSXPressLogTBLErr.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message; ;

                                                repoCBSXpressMoneyTransErrorRepository.Add(CBSXPressLogTBLErr);
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
                                                LogManager.SaveLog("An error occured XMLogManager Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr + "Tran Id: " + RefNo);
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
                                        var admDataPoollingControlTBL = new  admDataPullingControl();
                                        admDataPoollingControlTBL.ReconTypeId = rectype.ReconTypeId;
                                        admDataPoollingControlTBL.FileType = "Table";
                                        admDataPoollingControlTBL.TableName = "CBSXpressMoneyTrans";
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
                                LogManager.SaveLog("An error occured XMLogManager Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr + "Tran Id: " + RefNo);
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
                        LogManager.SaveLog("An error occured in Line XMLogManager: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                    }
                }

                #endregion
                #endregion

                #region Source 2

                var dtSouceCon2 = await repoadmSourceAccountRepository.GetAsync(c => c.ReconTypeId == ReconTypeId && c.SourceName == "Source 2");

                #region //File Directory Below

                string Xpin = string.Empty;
                if (!string.IsNullOrWhiteSpace(dtSouceCon2.FileDirectory))
                {
                    try
                    {
                        string MaxTransDate = string.Empty;
                        string dateTest = string.Empty;

                        var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "XpressMoney");
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "XpressMoneyTrans");

                        if (controlTable != null)
                        {
                            LastRecordId = controlTable.LastRecordId;
                        }
                        else
                        {
                            LastRecordId = "0";
                        }


                        var XpressMoney = new XpressMoneyTran();

                        string FileDirectory = dtSouceCon2.FileDirectory;

                        string PayoutDateErrolog = string.Empty;
                        string XpinErrolog = string.Empty;
                        string RcvAgentTxnRefNoErrolog = string.Empty;
                        string CommissionErrolog = string.Empty;
                        string PayoutAmountErrolog = string.Empty;
                        string AmountPaidErrolog = string.Empty;
                        string CurrencyErrolog = string.Empty;
                        string AmountInUSDErrolog = string.Empty;
                        string NoofTxnErrolog = string.Empty;



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
                            ggg = DLIST;
                        }
                        string fileNamenAndType = string.Empty;
                        string FileLastTime = string.Empty;
                        int count = 0;
                        string FileRecInserted = string.Empty;
                        foreach (var kl in ggg)
                        {
                            fileNamenAndType = (from f in DLIST orderby f.LastWriteTime ascending select kl).First().Name;

                            FileLastTime = (from f in DLIST orderby f.LastAccessTimeUtc descending select kl).First().LastWriteTime.ToString();

                            dTable = await library.ReadExcel(FileDirectory, ReconTypeId);


                            LogManager.SaveLog("No of Record return for XM File  :" + dTable.Rows.Count);

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

                                int countTrans = 0;
                                foreach (DataRow row in dTable.Rows.Cast<DataRow>().Skip(3))
                                {
                                    try
                                    {
                                        string hh = row[0].ToString();
                                        DateTime dt = new DateTime();

                                       // library.SaveLog("row[0] of XM File: " + row[0]);

                                        if (DateTime.TryParse(row[0].ToString(), out dt))
                                        {
                                            XpressMoney.PayoutDate = row[0] == null ? (DateTime?)null : Convert.ToDateTime(row[0]);
                                            dateTest = XpressMoney.PayoutDate.ToString();
                                            if (countTrans > 1)
                                            {
                                                MaxTransDate = XpressMoney.PayoutDate.ToString();

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
                                            
                                            PayoutDateErrolog = row[0] == null ? null : row[0].ToString();
                                            XpressMoney.Xpin = row[1] == null ? "" : row[1].ToString();
                                            XpressMoney.OrigRefNo = XpressMoney.Xpin;
                                            XpinErrolog = row[1] == null ? "" : row[1].ToString();
                                            XpressMoney.RcvAgentTxnRefNo = row[2] == null ? "" : row[2].ToString();
                                            RcvAgentTxnRefNoErrolog = row[2] == null ? "" : row[2].ToString();
                                            XpressMoney.Commission = row[3] == null ? (decimal?)null : Math.Round(Convert.ToDecimal(row[3]), 3);
                                            CommissionErrolog = row[3] == null ? null : row[3].ToString();
                                            XpressMoney.PayoutAmount = row[4] == null ? (decimal?)null : Math.Round(Convert.ToDecimal(row[4]), 3);
                                            PayoutAmountErrolog = row[4] == null ? null : row[4].ToString();
                                            XpressMoney.AmountPaid = row[5] == null ? (decimal?)null : Math.Round(Convert.ToDecimal(row[5]), 3);
                                            AmountPaidErrolog = row[5] == null ? null : row[5].ToString();
                                            XpressMoney.Currency = row[6] == null ? null : row[6].ToString();
                                            CurrencyErrolog = row[6] == null ? null : row[6].ToString();
                                            XpressMoney.AmountInUSD = row[7] == null ? (decimal?)null : Math.Round(Convert.ToDecimal(row[7]), 2);
                                            AmountInUSDErrolog = row[7] == null ? null : row[7].ToString();
                                            string hh8 = row[8].ToString();
                                            XpressMoney.NoofTxn = hh8 == "" ? 0 : Convert.ToInt32(row[8]);
                                            NoofTxnErrolog = hh8 == "" ? "0" : row[8].ToString();
                                            XpressMoney.PullDate = DateTime.Now;

                                            XpressMoney.MatchingStatus = "N";
                                            XpressMoney.MatchingType = "SYSTEM";
                                            XpressMoney.UserId = 1;
                                            XpressMoney.FileName = fileNamenAndType;

                                            Xpin = XpressMoney.Xpin;

                                            if (!string.IsNullOrWhiteSpace(XpressMoney.Xpin))
                                            {
                                                var exist = await repoXPressMoneyRepository.GetAsync(c => c.Xpin == XpressMoney.Xpin);
                                                var existHis = await repoXPressMoneyTransHistoryRepository.GetAsync(c => c.Xpin == XpressMoney.Xpin);
                                                if (exist != null || existHis != null)
                                                {
                                                    var errXpressMoneyTransErrorTBL = new XpressMoneyTransError();

                                                    errXpressMoneyTransErrorTBL.PayoutDate = PayoutDateErrolog == null ? DateTime.Now : Convert.ToDateTime(PayoutDateErrolog);
                                                    errXpressMoneyTransErrorTBL.Xpin = XpinErrolog;
                                                    errXpressMoneyTransErrorTBL.OrigRefNo = errXpressMoneyTransErrorTBL.Xpin;
                                                    errXpressMoneyTransErrorTBL.RcvAgentTxnRefNo = RcvAgentTxnRefNoErrolog;
                                                    errXpressMoneyTransErrorTBL.Commission = CommissionErrolog == null ? 0 : Math.Round(Convert.ToDecimal(CommissionErrolog), 2);
                                                    errXpressMoneyTransErrorTBL.PayoutAmount = PayoutAmountErrolog == null ? 0 : Math.Round(Convert.ToDecimal(PayoutAmountErrolog), 2);
                                                    errXpressMoneyTransErrorTBL.AmountPaid = AmountPaidErrolog == null ? 0 : Math.Round(Convert.ToDecimal(AmountPaidErrolog), 2);
                                                    errXpressMoneyTransErrorTBL.Currency = CurrencyErrolog;
                                                    errXpressMoneyTransErrorTBL.AmountInUSD = AmountInUSDErrolog == null ? 0 : Math.Round(Convert.ToDecimal(AmountInUSDErrolog), 2);
                                                    errXpressMoneyTransErrorTBL.NoofTxn = NoofTxnErrolog == null ? 0 : Convert.ToInt32(NoofTxnErrolog);
                                                    errXpressMoneyTransErrorTBL.PullDate = DateTime.Now;
                                                    errXpressMoneyTransErrorTBL.UserId = 1;
                                                    errXpressMoneyTransErrorTBL.ErrorMsg = "Duplicate transaction record";
                                                    errXpressMoneyTransErrorTBL.FileName = XpressMoney.FileName;

                                                    repoXpressMoneyTransErrorRepository.Add(errXpressMoneyTransErrorTBL);

                                                    var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret1)
                                                    {
                                                        continue;
                                                    }
                                                    continue;
                                                }
                                                else
                                                {
                                                    repoXPressMoneyRepository.Add(XpressMoney);
                                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                        count += 1;
                                                        FileRecInserted = count.ToString();
                                                        //library.SaveLog("No of Record Inserted XM for File :" + dTable.Rows.Count);
                                                        continue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        try
                                        {
                                            var errXpressMoneyTransErrorTBL = new XpressMoneyTransError();

                                            errXpressMoneyTransErrorTBL.PayoutDate = PayoutDateErrolog == null ? DateTime.Now : Convert.ToDateTime(PayoutDateErrolog);
                                            errXpressMoneyTransErrorTBL.Xpin = XpinErrolog;
                                            errXpressMoneyTransErrorTBL.OrigRefNo = errXpressMoneyTransErrorTBL.Xpin;
                                            errXpressMoneyTransErrorTBL.RcvAgentTxnRefNo = RcvAgentTxnRefNoErrolog;
                                            errXpressMoneyTransErrorTBL.Commission = CommissionErrolog == null ? 0 : Math.Round(Convert.ToDecimal(CommissionErrolog), 2);
                                            errXpressMoneyTransErrorTBL.PayoutAmount = PayoutAmountErrolog == null ? 0 : Math.Round(Convert.ToDecimal(PayoutAmountErrolog), 2);
                                            errXpressMoneyTransErrorTBL.AmountPaid = AmountPaidErrolog == null ? 0 : Math.Round(Convert.ToDecimal(AmountPaidErrolog), 2);
                                            errXpressMoneyTransErrorTBL.Currency = CurrencyErrolog;
                                            errXpressMoneyTransErrorTBL.AmountInUSD = AmountInUSDErrolog == null ? 0 : Math.Round(Convert.ToDecimal(AmountInUSDErrolog), 2);
                                            errXpressMoneyTransErrorTBL.NoofTxn = NoofTxnErrolog == null ? 0 : Convert.ToInt32(NoofTxnErrolog);
                                            errXpressMoneyTransErrorTBL.PullDate = DateTime.Now;
                                            errXpressMoneyTransErrorTBL.UserId = 1;
                                            errXpressMoneyTransErrorTBL.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;
                                            errXpressMoneyTransErrorTBL.FileName = XpressMoney.FileName;
                                            repoXpressMoneyTransErrorRepository.Add(errXpressMoneyTransErrorTBL);

                                            var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                            if (ret1)
                                            {
                                                continue;
                                            }
                                        }
                                        catch (Exception ex1)
                                        {
                                            var exErr = ex1 == null ? ex1.InnerException.Message : ex1.Message;
                                            var stackTrace = new StackTrace(ex1);
                                            var thisasm = Assembly.GetExecutingAssembly();
                                            _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                                            _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                            LogManager.SaveLog("An error occured in XMLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr + " Xpin: " + Xpin);
                                            continue;
                                        }
                                        continue;
                                    }
                                }

                                //Move file here 
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

                                    LogManager.SaveLog("Move XMLogManager XMoney File Start");
                                    string MvFile = library.MoveFile(FileDirectory + "\\" + fileNamenAndType, ReconType.ProcessedFileDirectory, fileNamenAndType);
                                    LogManager.SaveLog("Move File in XMoney status: " + MvFile);
                                }
                                if (controlTable != null)
                                {
                                    controlTable.LastRecordId = LastRecordId;
                                    controlTable.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                                    DateTime dtf = new DateTime();
                                    string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                                    controlTable.LastTransDate = Convert.ToDateTime(dateee);
                                    controlTable.PullDate = DateTime.Now;
                                    controlTable.RecordsCount = Convert.ToInt32(FileRecInserted);
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
                                    admDataPoollingControlTBL.TableName = "XpressMoneyTrans";
                                    admDataPoollingControlTBL.DateCreated = DateTime.Now;
                                    admDataPoollingControlTBL.UserId = 1;
                                    admDataPoollingControlTBL.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
                                    DateTime dtf = new DateTime();
                                    string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                                    admDataPoollingControlTBL.LastTransDate = Convert.ToDateTime(dateee);
                                    admDataPoollingControlTBL.PullDate = DateTime.Now;
                                    admDataPoollingControlTBL.RecordsCount = Convert.ToInt32(FileRecInserted);
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
                        LogManager.SaveLog("An error occured in  Line XMLogManager: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr + " Xpin: " + Xpin);
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
                LogManager.SaveLog("An error occured in XMLibrary in Line 300:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }

            return string.Empty; ;
        }
    }
}