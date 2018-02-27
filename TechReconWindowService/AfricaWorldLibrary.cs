
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
    public class AfricaWorldLibrary
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmReconDataSourcesRepository repoadmReconDataSourcesRepository;
        private readonly IadmSourceAccountRepository repoadmSourceAccountRepository;
        private readonly IPostillionTransationTestRepository repoPostillionTransationTestRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly IadmTerminalRepository repoadmTerminalRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly IAfricaWorldTransactionRepository repoAfricaWorldTransactionRepository;
        private readonly ICBSAfricaWorldTransactionRepository repoCBSAfricaWorldTransactionRepository;
        private readonly IadmFailledFileRepository repoadmFailledFileRepository;
        private readonly IadmFileProcessedRepository repoadmFileProcessedRepository;
        private readonly ICBSAfricaWorldTransErrorRepository repoCBSAfricaWorldTransErrorRepository;
        private readonly ICBSAfricaWorldTransHistoryRepository repoCBSAfricaWorldTransHistoryRepository;
        private readonly IAfricaWorldTransErrorRepository repoAfricaWorldTransErrorRepository;
        private readonly IAfricaWorldTransHistoryRepository repoAfricaWorldTransHistoryRepository;

        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/AfricaWorldLibrary.cs";
        public AfricaWorldLibrary()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmReconDataSourcesRepository = new admReconDataSourcesRepository(idbfactory);
            repoadmSourceAccountRepository = new admSourceAccountRepository(idbfactory);
            repoPostillionTransationTestRepository = new PostillionTransationTestRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoadmTerminalRepository = new admTerminalRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoAfricaWorldTransactionRepository = new AfricaWorldTransactionRepository(idbfactory);
            repoCBSAfricaWorldTransactionRepository = new CBSAfricaWorldTransactionRepository(idbfactory);
            repoadmFailledFileRepository = new admFailledFileRepository(idbfactory);
            repoadmFileProcessedRepository = new admFileProcessedRepository(idbfactory);
            repoCBSAfricaWorldTransErrorRepository = new CBSAfricaWorldTransErrorRepository(idbfactory);
            repoCBSAfricaWorldTransHistoryRepository = new CBSAfricaWorldTransHistoryRepository(idbfactory);
            repoAfricaWorldTransErrorRepository = new AfricaWorldTransErrorRepository(idbfactory);
            repoAfricaWorldTransHistoryRepository = new AfricaWorldTransHistoryRepository(idbfactory);
        }

        public async Task<string> PullAfricaWorldData()
        {
            Library library = new Library();
           
            try
            {
                LogManager.SaveLog("Task Start in AfricaWorldLibrary");
                var ReconType = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "AfricaWorld");
                int ReconTypeId = ReconType.ReconTypeId;
                var AccountSources = await repoadmSourceAccountRepository.GetManyAsync(c => c.ReconTypeId == ReconTypeId);
                var listofAccountsource1 = AccountSources.Where(c => c.SourceName == "Source 1");
                string acctlistSource1 = string.Empty;
                string acctlistSource2 = string.Empty;
                int dt1 = 0;
                //int dt2 = 0;
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
                        LogManager.SaveLog("PullDataforAfricaWorld Start in AfricaWorldLibrary");
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

                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == ReconType.ReconTypeId && c.TableName == "CBSAfricaWorldTrans");

                        if (controlTable != null)
                        {
                            LastRecordId = controlTable.LastRecordId;
                            FromDateParam = controlTable.FromDateParam == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.FromDateParam));
                            FromDateParam = "'" + FromDateParam + "'";
                            ToDateParam = controlTable.ToDateParam == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.ToDateParam));
                            ToDateParam = "'" + ToDateParam + "'";

                            ReconDate = controlTable.LastTransDate == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.LastTransDate));
                            ReconDate = "'" + ReconDate + "'";
                        }
                        else
                        {
                            FromDateParam = "'20170801'";
                            ToDateParam = "'20170831'";
                            ReconDate = "'20170831'";
                            LastRecordId = "0";
                        }

                        string SqlString = ReconType.Source1Script.Replace("{FromDateParam} ", FromDateParam)
                                                                    .Replace("{ToDateParam}", ToDateParam);
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
                                LogManager.SaveLog("An error occured AfricaWorldLogManager PullDataforAfricaWorld Sybase 1 in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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

                                LogManager.SaveLog("Total records pulled CBSAfriTWorld: " + returndata.Rows.Count);
                                if (returndata.Rows.Count > 0)
                                {
                                    int countTrans = 0;
                                    var CBSAfriTBL = new CBSAfricaWorldTran();
                                    var CBSAfricaWorldTransErrorTBL = new CBSAfricaWorldTransError();

                                    for (int col = 0; col < returndata.Rows.Count; col++)
                                    {
                                       // 'AccountNo' = a.acct_no     0
                                        //,'AccountType' = a.acct_type  1
                                        //,'Amount' = amount  2
                                        //,'Description' = a.description  3
                                        //,'DebitCredit' = case when h.tran_code > 149 then 'DR' else 'CR' end   4
                                        //,'CreateDate' = convert(date,a.create_dt)  5
                                        //,'CheckNo' = isnull(convert(varchar,a.check_no),'')   6
                                        //,'PNR' = field1    7
                                        //,'AgentsPaxName' = field2    8
                                        //,'ContactNo' = field3    9
                                        //,'Location' = field4    10
                                        //,'TravelDate' = field5    11
                                        //,'DepositorsName' = field6   12
                                        //,'OriginTracerNo' = h.origin_tracer_no,    13
                                        //h.ptid,     14
                                        //h.reversal    15

                                        try
                                        {
                                            DateTime dt = new DateTime();
                                            CBSAfriTBL.AcctNo = returndata.Rows[col][0] == null ? null : returndata.Rows[col][0].ToString();
                                            CBSAfriTBL.AcctType = returndata.Rows[col][1] == null ? string.Empty : returndata.Rows[col][1].ToString();
                                            decimal decAmount = 0;
                                            string unt = returndata.Rows[col][2].ToString();
                                            CBSAfriTBL.Amount = returndata.Rows[col][2] == null ? Math.Round(Convert.ToDecimal("0"), 2) : decimal.TryParse(returndata.Rows[col][2].ToString(), out decAmount) ? Math.Round(decAmount, 2) : Math.Round(Convert.ToDecimal("0"), 2);
                                            CBSAfriTBL.Description = returndata.Rows[col][3] == null ? string.Empty : returndata.Rows[col][3].ToString();
                                            CBSAfriTBL.DebitCredit = returndata.Rows[col][4] == null ? string.Empty : returndata.Rows[col][4].ToString();
                                            CBSAfriTBL.TransDate = returndata.Rows[col][5] == null ? (DateTime?)null : DateTime.TryParse(returndata.Rows[col][5].ToString(), out dt) ? dt : (DateTime?)null;
                                            dateTest = CBSAfriTBL.TransDate.ToString();
                                            MaxTransDate = CBSAfriTBL.TransDate.ToString();
                                            if (countTrans > 1)
                                            {
                                                MaxTransDate = CBSAfriTBL.TransDate.ToString();
                                                if (Convert.ToDateTime(dateTest) > Convert.ToDateTime(MaxTransDate))
                                                {
                                                    MaxTransDate = dateTest;
                                                }
                                                else
                                                {
                                                    MaxTransDate = MaxTransDate;
                                                }
                                            }
                                            CBSAfriTBL.CheckNo = returndata.Rows[col][6] == null ? string.Empty : returndata.Rows[col][6].ToString();
                                            CBSAfriTBL.Reference = returndata.Rows[col][7] == null ? string.Empty : returndata.Rows[col][7].ToString();
                                            CBSAfriTBL.OrigRefNo = CBSAfriTBL.Reference;
                                            CBSAfriTBL.AgentName = returndata.Rows[col][8] == null ? string.Empty : returndata.Rows[col][8].ToString();
                                            CBSAfriTBL.ContactNo = returndata.Rows[col][9] == null ? string.Empty : returndata.Rows[col][9].ToString();
                                            CBSAfriTBL.Location = returndata.Rows[col][10] == null ? string.Empty : returndata.Rows[col][10].ToString();
                                            CBSAfriTBL.TravelDate = returndata.Rows[col][11] == null ? (DateTime?)null : DateTime.TryParse(returndata.Rows[col][11].ToString(), out dt) ? dt : (DateTime?)null;
                                            CBSAfriTBL.DepositorName = returndata.Rows[col][12] == null ? string.Empty : returndata.Rows[col][12].ToString();
                                            CBSAfriTBL.OriginTracerNo = returndata.Rows[col][13] == null ? string.Empty : returndata.Rows[col][13].ToString();
                                            CBSAfriTBL.ptId = returndata.Rows[col][14] == null ? string.Empty : returndata.Rows[col][14].ToString();
                                            int intChk = 0;
                                            CBSAfriTBL.ReversalCode = returndata.Rows[col][15] == null ? 0 : int.TryParse(returndata.Rows[col][15].ToString(), out intChk) ? intChk : 0; ;
                                            CBSAfriTBL.PullDate = DateTime.Now;
                                            CBSAfriTBL.MatchingStatus = "N";
                                            CBSAfriTBL.UserId = 1;

                                            if (!string.IsNullOrWhiteSpace(CBSAfriTBL.Reference))
                                            {
                                                var exist = await repoCBSAfricaWorldTransactionRepository.GetAsync(c => c.Reference == CBSAfriTBL.Reference && (c.DebitCredit == CBSAfriTBL.DebitCredit) && (c.Amount == CBSAfriTBL.Amount) && (c.AcctNo == CBSAfriTBL.AcctNo));
                                                var existHis = await repoCBSAfricaWorldTransHistoryRepository.GetAsync(c => c.Reference == CBSAfriTBL.Reference && (c.DebitCredit == CBSAfriTBL.DebitCredit) && (c.Amount == CBSAfriTBL.Amount) && (c.AcctNo == CBSAfriTBL.AcctNo));
                                                if (exist != null || existHis != null)
                                                {
                                                    CBSAfricaWorldTransErrorTBL.ptId = CBSAfriTBL.ptId;
                                                    CBSAfricaWorldTransErrorTBL.AcctNo = CBSAfriTBL.AcctNo;
                                                    CBSAfricaWorldTransErrorTBL.AcctType = CBSAfriTBL.AcctType;
                                                    CBSAfricaWorldTransErrorTBL.TransDate = CBSAfriTBL.TransDate;
                                                    CBSAfricaWorldTransErrorTBL.EffectiveDate = CBSAfriTBL.EffectiveDate;
                                                    CBSAfricaWorldTransErrorTBL.Amount = CBSAfriTBL.Amount;
                                                    CBSAfricaWorldTransErrorTBL.Description = CBSAfriTBL.Description;
                                                    CBSAfricaWorldTransErrorTBL.Reference = CBSAfriTBL.Reference;
                                                    CBSAfricaWorldTransErrorTBL.CheckNo = CBSAfriTBL.CheckNo;
                                                    CBSAfricaWorldTransErrorTBL.PNR = CBSAfriTBL.PNR;
                                                    CBSAfricaWorldTransErrorTBL.AgentName = CBSAfriTBL.AgentName;
                                                    CBSAfricaWorldTransErrorTBL.ContactNo = CBSAfriTBL.ContactNo;
                                                    CBSAfricaWorldTransErrorTBL.Location = CBSAfriTBL.Location;
                                                    CBSAfricaWorldTransErrorTBL.TravelDate = CBSAfriTBL.TravelDate;
                                                    CBSAfricaWorldTransErrorTBL.DepositorName = CBSAfriTBL.DepositorName;
                                                    CBSAfricaWorldTransErrorTBL.ErrorMsg = "Duplicate transaction reference";
                                                    CBSAfricaWorldTransErrorTBL.PullDate = CBSAfriTBL.PullDate;
                                                    CBSAfricaWorldTransErrorTBL.MatchingStatus = CBSAfriTBL.MatchingStatus;
                                                    CBSAfricaWorldTransErrorTBL.UserId = CBSAfriTBL.UserId;
                                                    CBSAfricaWorldTransErrorTBL.OrigRefNo = CBSAfriTBL.OrigRefNo;
                                                    CBSAfricaWorldTransErrorTBL.ReversalCode = CBSAfriTBL.ReversalCode;
                                                    CBSAfricaWorldTransErrorTBL.OriginTracerNo = CBSAfriTBL.OriginTracerNo;


                                                    repoCBSAfricaWorldTransErrorRepository.Add(CBSAfricaWorldTransErrorTBL);
                                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                    }
                                                }
                                                else
                                                {
                                                    repoCBSAfricaWorldTransactionRepository.Add(CBSAfriTBL);
                                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                CBSAfricaWorldTransErrorTBL.ptId = CBSAfriTBL.ptId;
                                                CBSAfricaWorldTransErrorTBL.AcctNo = CBSAfriTBL.AcctNo;
                                                CBSAfricaWorldTransErrorTBL.AcctType = CBSAfriTBL.AcctType;
                                                CBSAfricaWorldTransErrorTBL.TransDate = CBSAfriTBL.TransDate;
                                                CBSAfricaWorldTransErrorTBL.EffectiveDate = CBSAfriTBL.EffectiveDate;
                                                CBSAfricaWorldTransErrorTBL.Amount = CBSAfriTBL.Amount;
                                                CBSAfricaWorldTransErrorTBL.Description = CBSAfriTBL.Description;
                                                CBSAfricaWorldTransErrorTBL.Reference = CBSAfriTBL.Reference;
                                                CBSAfricaWorldTransErrorTBL.CheckNo = CBSAfriTBL.CheckNo;
                                                CBSAfricaWorldTransErrorTBL.PNR = CBSAfriTBL.PNR;
                                                CBSAfricaWorldTransErrorTBL.AgentName = CBSAfriTBL.AgentName;
                                                CBSAfricaWorldTransErrorTBL.ContactNo = CBSAfriTBL.ContactNo;
                                                CBSAfricaWorldTransErrorTBL.Location = CBSAfriTBL.Location;
                                                CBSAfricaWorldTransErrorTBL.TravelDate = CBSAfriTBL.TravelDate;
                                                CBSAfricaWorldTransErrorTBL.DepositorName = CBSAfriTBL.DepositorName;
                                                CBSAfricaWorldTransErrorTBL.ErrorMsg = "No trans ref";
                                                CBSAfricaWorldTransErrorTBL.PullDate = CBSAfriTBL.PullDate;
                                                CBSAfricaWorldTransErrorTBL.MatchingStatus = CBSAfriTBL.MatchingStatus;
                                                CBSAfricaWorldTransErrorTBL.UserId = CBSAfriTBL.UserId;
                                                CBSAfricaWorldTransErrorTBL.OrigRefNo = CBSAfriTBL.OrigRefNo;
                                                CBSAfricaWorldTransErrorTBL.ReversalCode = CBSAfriTBL.ReversalCode;
                                                CBSAfricaWorldTransErrorTBL.OriginTracerNo = CBSAfriTBL.OriginTracerNo;

                                                repoCBSAfricaWorldTransErrorRepository.Add(CBSAfricaWorldTransErrorTBL);
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
                                            LogManager.SaveLog("An error occured in Line AfricaWorldLogManager: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                                            CBSAfricaWorldTransErrorTBL.ptId = CBSAfriTBL.ptId;
                                            CBSAfricaWorldTransErrorTBL.AcctNo = CBSAfriTBL.AcctNo;
                                            CBSAfricaWorldTransErrorTBL.AcctType = CBSAfriTBL.AcctType;
                                            CBSAfricaWorldTransErrorTBL.TransDate = CBSAfriTBL.TransDate;
                                            CBSAfricaWorldTransErrorTBL.EffectiveDate = CBSAfriTBL.EffectiveDate;
                                            CBSAfricaWorldTransErrorTBL.Amount = CBSAfriTBL.Amount;
                                            CBSAfricaWorldTransErrorTBL.Description = CBSAfriTBL.Description;
                                            CBSAfricaWorldTransErrorTBL.Reference = CBSAfriTBL.Reference;
                                            CBSAfricaWorldTransErrorTBL.CheckNo = CBSAfriTBL.CheckNo;
                                            CBSAfricaWorldTransErrorTBL.PNR = CBSAfriTBL.PNR;
                                            CBSAfricaWorldTransErrorTBL.AgentName = CBSAfriTBL.AgentName;
                                            CBSAfricaWorldTransErrorTBL.ContactNo = CBSAfriTBL.ContactNo;
                                            CBSAfricaWorldTransErrorTBL.Location = CBSAfriTBL.Location;
                                            CBSAfricaWorldTransErrorTBL.TravelDate = CBSAfriTBL.TravelDate;
                                            CBSAfricaWorldTransErrorTBL.DepositorName = CBSAfriTBL.DepositorName;
                                            CBSAfricaWorldTransErrorTBL.PullDate = CBSAfriTBL.PullDate;
                                            CBSAfricaWorldTransErrorTBL.MatchingStatus = CBSAfriTBL.MatchingStatus;
                                            CBSAfricaWorldTransErrorTBL.UserId = CBSAfriTBL.UserId;
                                            CBSAfricaWorldTransErrorTBL.OrigRefNo = CBSAfriTBL.OrigRefNo;
                                            CBSAfricaWorldTransErrorTBL.ReversalCode = CBSAfriTBL.ReversalCode;
                                            CBSAfricaWorldTransErrorTBL.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;
                                            CBSAfricaWorldTransErrorTBL.OriginTracerNo = CBSAfriTBL.OriginTracerNo;


                                            repoCBSAfricaWorldTransErrorRepository.Add(CBSAfricaWorldTransErrorTBL);
                                            var ret = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                            if (ret)
                                            {
                                            }
                                        }
                                    }

                                    //Update Control table
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
                                        admDataPoollingControlTBL.TableName = "CBSAfricaWorld";
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
                                LogManager.SaveLog("An error occured AfricaWorldLogManager  Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                        LogManager.SaveLog("An error occured in Line AfricaWorldLibrary: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == ReconType.ReconTypeId && c.TableName == "AfricaWorldTrans");

                        if (controlTable != null)
                        {
                            LastRecordId = controlTable.LastRecordId;
                        }
                        else
                        {
                            LastRecordId = "0";
                        }


                        DataTable dTable = new DataTable();


                        string FileDirectory = dtSouceCon2.FileDirectory;

                        DirectoryInfo d = new DirectoryInfo(FileDirectory);
                        List<FileInfo> DLIST = null;
                        DLIST = d.GetFiles("*" + ".CSV").ToList();
                        DLIST.AddRange(d.GetFiles("*" + ".CSV").ToList());
                        List<FileInfo> ggg = null;

                        if (controlTable != null)
                        {
                            if (controlTable.LastRecordTimeStamp != null)
                            {
                                ggg = DLIST;//.Where(c => c.LastAccessTime > controlTable.LastRecordTimeStamp).ToList();
                            }
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

                            if (fileNamenAndType.Contains(".CSV"))
                                dTable = library.CSVFile(FileDirectory + "\\" + fileNamenAndType);
                            else
                                continue;

                            var afriWorldTBL = new AfricaWorldTran();
                            var AfricaWorldTransErrorTBL = new AfricaWorldTransError();
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

                                foreach (DataRow row in dTable.Rows.Cast<DataRow>())
                                {
                                    try
                                    {
                                        afriWorldTBL.ConfirmationNo = row[0] == null ? string.Empty : row[0].ToString();
                                        afriWorldTBL.BookingAgent = row[1] == null ? string.Empty : row[1].ToString();
                                        afriWorldTBL.IATANo = row[2] == null ? string.Empty : row[2].ToString();
                                        afriWorldTBL.AfricaWorldUserId = row[3] == null ? string.Empty : row[3].ToString();
                                        DateTime dt = new DateTime();
                                        afriWorldTBL.TransDate = row[4] == null ? (DateTime?)null : DateTime.TryParse(row[4].ToString(), out dt) ? dt : (DateTime?)null;
                                        dateTest = afriWorldTBL.TransDate.ToString();
                                        if (countTrans > 1)
                                        {
                                            MaxTransDate = afriWorldTBL.TransDate.ToString();

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

                                        afriWorldTBL.Currency = row[5] == null ? string.Empty : row[5].ToString();
                                        afriWorldTBL.FirstName = row[6] == null ? string.Empty : row[6].ToString();
                                        afriWorldTBL.LastName = row[7] == null ? string.Empty : row[7].ToString();

                                        decimal DepositAmount, DepositAmt;
                                        if (decimal.TryParse(row[8].ToString(), out DepositAmount))
                                        {
                                            DepositAmt = Convert.ToDecimal(row[8].ToString()) != null ? Convert.ToDecimal(row[8].ToString()) : 0;
                                        }
                                        else
                                        {
                                            DepositAmt = 0;
                                        }
                                        afriWorldTBL.Deposit = DepositAmt;
                                        decimal BookedAmount, BookedAmt;
                                        if (decimal.TryParse(row[9].ToString(), out BookedAmount))
                                        {
                                            BookedAmt = Convert.ToDecimal(row[9].ToString()) != null ? Convert.ToDecimal(row[9].ToString()) : 0;
                                        }
                                        else
                                        {
                                            BookedAmt = 0;
                                        }
                                        afriWorldTBL.BookedAmount = BookedAmt;
                                        decimal CancelledAmount, CancelledAmt;
                                        if (decimal.TryParse(row[10].ToString(), out CancelledAmount))
                                        {
                                            CancelledAmt = Convert.ToDecimal(row[10].ToString()) != null ? Convert.ToDecimal(row[10].ToString()) : 0;
                                        }
                                        else
                                        {
                                            CancelledAmt = 0;
                                        }
                                        afriWorldTBL.CancelledAmount = CancelledAmt;
                                        afriWorldTBL.BookingDate = Convert.ToDateTime(row[11]);
                                        afriWorldTBL.PullDate = DateTime.Now;
                                        afriWorldTBL.MatchingStatus = "N";
                                        afriWorldTBL.UserId = 1;
                                        afriWorldTBL.OrigRefNo = afriWorldTBL.ConfirmationNo;

                                        if (!string.IsNullOrWhiteSpace(afriWorldTBL.ConfirmationNo))
                                        {
                                            var exist = await repoAfricaWorldTransactionRepository.GetAsync(c => c.ConfirmationNo == afriWorldTBL.ConfirmationNo);
                                            var existHis = await repoAfricaWorldTransHistoryRepository.GetAsync(c => c.ConfirmationNo == afriWorldTBL.ConfirmationNo);

                                            if (exist != null || existHis != null)
                                            {
                                                AfricaWorldTransErrorTBL.ConfirmationNo = afriWorldTBL.ConfirmationNo;
                                                AfricaWorldTransErrorTBL.BookingAgent = afriWorldTBL.BookingAgent;
                                                AfricaWorldTransErrorTBL.IATANo = afriWorldTBL.IATANo;
                                                AfricaWorldTransErrorTBL.AfricaWorldUserId = afriWorldTBL.AfricaWorldUserId;
                                                AfricaWorldTransErrorTBL.TransDate = afriWorldTBL.TransDate;
                                                AfricaWorldTransErrorTBL.Currency = afriWorldTBL.Currency;
                                                AfricaWorldTransErrorTBL.FirstName = afriWorldTBL.FirstName;
                                                AfricaWorldTransErrorTBL.LastName = afriWorldTBL.LastName;
                                                AfricaWorldTransErrorTBL.Deposit = afriWorldTBL.Deposit;
                                                AfricaWorldTransErrorTBL.BookedAmount = afriWorldTBL.BookedAmount;
                                                AfricaWorldTransErrorTBL.CancelledAmount = afriWorldTBL.CancelledAmount;
                                                AfricaWorldTransErrorTBL.BookingDate = afriWorldTBL.BookingDate;
                                                AfricaWorldTransErrorTBL.PullDate = afriWorldTBL.PullDate;
                                                AfricaWorldTransErrorTBL.MatchingStatus = afriWorldTBL.MatchingStatus;
                                                AfricaWorldTransErrorTBL.UserId = afriWorldTBL.UserId;
                                                AfricaWorldTransErrorTBL.OrigRefNo = afriWorldTBL.OrigRefNo;
                                                AfricaWorldTransErrorTBL.ErrorMsg = "Duplicate transaction reference";

                                                repoAfricaWorldTransErrorRepository.Add(AfricaWorldTransErrorTBL);
                                                var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret)
                                                {
                                                    continue;
                                                }

                                            }
                                            else
                                            {
                                                repoAfricaWorldTransactionRepository.Add(afriWorldTBL);
                                                var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret)
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            AfricaWorldTransErrorTBL.ConfirmationNo = afriWorldTBL.ConfirmationNo;
                                            AfricaWorldTransErrorTBL.BookingAgent = afriWorldTBL.BookingAgent;
                                            AfricaWorldTransErrorTBL.IATANo = afriWorldTBL.IATANo;
                                            AfricaWorldTransErrorTBL.AfricaWorldUserId = afriWorldTBL.AfricaWorldUserId;
                                            AfricaWorldTransErrorTBL.TransDate = afriWorldTBL.TransDate;
                                            AfricaWorldTransErrorTBL.Currency = afriWorldTBL.Currency;
                                            AfricaWorldTransErrorTBL.FirstName = afriWorldTBL.FirstName;
                                            AfricaWorldTransErrorTBL.LastName = afriWorldTBL.LastName;
                                            AfricaWorldTransErrorTBL.Deposit = afriWorldTBL.Deposit;
                                            AfricaWorldTransErrorTBL.BookedAmount = afriWorldTBL.BookedAmount;
                                            AfricaWorldTransErrorTBL.CancelledAmount = afriWorldTBL.CancelledAmount;
                                            AfricaWorldTransErrorTBL.BookingDate = afriWorldTBL.BookingDate;
                                            AfricaWorldTransErrorTBL.PullDate = afriWorldTBL.PullDate;
                                            AfricaWorldTransErrorTBL.MatchingStatus = afriWorldTBL.MatchingStatus;
                                            AfricaWorldTransErrorTBL.UserId = afriWorldTBL.UserId;
                                            AfricaWorldTransErrorTBL.OrigRefNo = afriWorldTBL.OrigRefNo;
                                            AfricaWorldTransErrorTBL.ErrorMsg = "No trans  reference";

                                            repoAfricaWorldTransErrorRepository.Add(AfricaWorldTransErrorTBL);
                                            var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                            if (ret)
                                            {
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
                                        LogManager.SaveLog("An error occured in AfricaWorldlibrary Line :" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                                        AfricaWorldTransErrorTBL.ConfirmationNo = afriWorldTBL.ConfirmationNo;
                                        AfricaWorldTransErrorTBL.BookingAgent = afriWorldTBL.BookingAgent;
                                        AfricaWorldTransErrorTBL.IATANo = afriWorldTBL.IATANo;
                                        AfricaWorldTransErrorTBL.AfricaWorldUserId = afriWorldTBL.AfricaWorldUserId;
                                        AfricaWorldTransErrorTBL.TransDate = afriWorldTBL.TransDate;
                                        AfricaWorldTransErrorTBL.Currency = afriWorldTBL.Currency;
                                        AfricaWorldTransErrorTBL.FirstName = afriWorldTBL.FirstName;
                                        AfricaWorldTransErrorTBL.LastName = afriWorldTBL.LastName;
                                        AfricaWorldTransErrorTBL.Deposit = afriWorldTBL.Deposit;
                                        AfricaWorldTransErrorTBL.BookedAmount = afriWorldTBL.BookedAmount;
                                        AfricaWorldTransErrorTBL.CancelledAmount = afriWorldTBL.CancelledAmount;
                                        AfricaWorldTransErrorTBL.BookingDate = afriWorldTBL.BookingDate;
                                        AfricaWorldTransErrorTBL.PullDate = afriWorldTBL.PullDate;
                                        AfricaWorldTransErrorTBL.MatchingStatus = afriWorldTBL.MatchingStatus;
                                        AfricaWorldTransErrorTBL.UserId = afriWorldTBL.UserId;
                                        AfricaWorldTransErrorTBL.OrigRefNo = afriWorldTBL.OrigRefNo;
                                        AfricaWorldTransErrorTBL.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;

                                        repoAfricaWorldTransErrorRepository.Add(AfricaWorldTransErrorTBL);
                                        var ret = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }
                                        continue;

                                    }
                                }
                            }

                            //Move File
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
                                LogManager.SaveLog("Move File in AfricaWorldLibrary");
                                string MvFile = library.MoveFile(FileDirectory + "\\" + fileNamenAndType, ReconType.ProcessedFileDirectory, fileNamenAndType);
                                LogManager.SaveLog("Move File in AfricaWorldLibrary status: " + MvFile);
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
                            admDataPoollingControlTBL.ReconTypeId = ReconType.ReconTypeId;
                            admDataPoollingControlTBL.FileType = "Table";
                            admDataPoollingControlTBL.TableName = "AfricaWorldTransaction";
                            admDataPoollingControlTBL.DateCreated = DateTime.Now;
                            admDataPoollingControlTBL.UserId = 1;
                            admDataPoollingControlTBL.LastRecordTimeStamp = DateTime.Now;
                            admDataPoollingControlTBL.LastRecordId = LastRecordId;
                            DateTime dtf = new DateTime();
                            string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                            admDataPoollingControlTBL.LastTransDate = Convert.ToDateTime(dateee);
                            admDataPoollingControlTBL.PullDate = DateTime.Now;
                            admDataPoollingControlTBL.RecordsCount = dTable.Rows.Count;
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
                        LogManager.SaveLog("An error occured in AfricaWorldlibrary Line :" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                LogManager.SaveLog("An error occured in AfricaWorldLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
            }
            return string.Empty;
        }
        
    }

    
}