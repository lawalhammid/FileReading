
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
    public class ConsortiumLibrary
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmReconDataSourcesRepository repoadmReconDataSourcesRepository;
        private readonly IadmSourceAccountRepository repoadmSourceAccountRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly IWUMTTransRepository repoWUMTTransRepository;
        private readonly IadmFailledFileRepository repoadmFailledFileRepository;
        private readonly IadmFileProcessedRepository repoadmFileProcessedRepository;
        private readonly IConsortiumTransactionRepository repoConsortiumTransactionRepository;
        private readonly ICBSConsortiumTransactionRepository repoCBSConsortiumTransactionRepository;
        private readonly ICBSConsortiumTransactionErrorRepository repoCBSConsortiumTransactionErrorRepository;
        private readonly IConsortiumTransactionErrorRepository repoConsortiumTransactionErrorRepository;
        private readonly ICBSConsortiumTransactionHistoryRepository repoCBSConsortiumTransactionHistoryRepository;
        private readonly IConsortiumTransactionHistoryRepository repoConsortiumTransactionHistoryRepository;

        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/ConsortiumLibrary.cs";    
        public ConsortiumLibrary()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmReconDataSourcesRepository = new admReconDataSourcesRepository(idbfactory);
            repoadmSourceAccountRepository = new admSourceAccountRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoWUMTTransRepository = new WUMTTransRepository(idbfactory);
            repoadmFailledFileRepository = new admFailledFileRepository(idbfactory);
            repoadmFileProcessedRepository = new admFileProcessedRepository(idbfactory);
            repoCBSConsortiumTransactionRepository = new CBSConsortiumTransactionRepository(idbfactory);
            repoConsortiumTransactionRepository = new ConsortiumTransactionRepository(idbfactory);
            repoCBSConsortiumTransactionErrorRepository = new CBSConsortiumTransactionErrorRepository(idbfactory);
            repoConsortiumTransactionErrorRepository = new ConsortiumTransactionErrorRepository(idbfactory);
            repoCBSConsortiumTransactionHistoryRepository = new CBSConsortiumTransactionHistoryRepository(idbfactory);
            repoConsortiumTransactionHistoryRepository = new ConsortiumTransactionHistoryRepository(idbfactory);
        }

        //source 1
        public async Task<string> PullConsortiumData()  
        {
            Library library = new Library();
            try
            {
                LogManager.SaveLog("Task Start in ConsortiumLibrary");
                var ReconType = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "Consortium");
                int ReconTypeId = ReconType.ReconTypeId;
                var AccountSources = await repoadmSourceAccountRepository.GetManyAsync(c => c.ReconTypeId == ReconTypeId);
                var listofAccountsource1 = AccountSources.Where(c => c.SourceName == "Source 1");
                var listofAccountsource2 = AccountSources.Where(c => c.SourceName == "Source 2");
                string acctlistSource1 = string.Empty;
                string acctlistSource2 = string.Empty;
                int dt1 = 0;
                int dt2 = 0;
                foreach (var li in listofAccountsource1)
                {
                    acctlistSource1 += "'" + li.AcctNo + "'" + ", ";
                    dt1 = (int)li.DataSourceId;
                }

                int index = acctlistSource1.LastIndexOf(',');
                acctlistSource1 = (acctlistSource1.Remove(index, 1));

                foreach (var li in listofAccountsource2)
                {
                    acctlistSource2 += "'" + li.AcctNo + "'" + ", ";
                    dt2 = (int)li.DataSourceId;
                }

                int index2 = acctlistSource2.LastIndexOf(',');
                acctlistSource2 = (acctlistSource2.Remove(index2, 1));

                LogManager.SaveLog("Account List Source 2: " + acctlistSource2);

                var dtSouceCon1 = await repoadmReconDataSourcesRepository.GetAsync(c => c.ReconDSId == dt1);


                #region Source 1
                #region   // Sybase Server below
                if (dtSouceCon1.DatabaseType == "SYBASE")
                {
                    try
                    {
                        LogManager.SaveLog("PullDataSource Start in Library");
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
                        var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "Consortium");
                        string LastRecordId = string.Empty;
                        var controlTable = await  repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSConsortiumTransaction");

                        if (controlTable != null)
                        {
                            FromDateParam = controlTable.FromDateParam == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.FromDateParam));
                            FromDateParam = "'" + FromDateParam + "'";
                            ToDateParam = controlTable.ToDateParam == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.ToDateParam));
                            ToDateParam = "'" + ToDateParam + "'";

                            ReconDate = controlTable.LastTransDate == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.LastTransDate));
                            ReconDate = "'" + ReconDate + "'";


                            LastRecordId =  controlTable.LastRecordId;
                        }
                        else
                        {
                            LastRecordId = "0";
                            FromDateParam = "'20170901'";
                            ToDateParam = "'20170930'";
                            ReconDate = "'20170930'";
                        }


                        string SqlString = ReconType.Source1Script.Replace("{AcctlistSource1}", acctlistSource1)
                                                                  .Replace("{LastRecordId}", LastRecordId)
                                                                  .Replace("{ReconDate}", ReconDate);

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
                                LogManager.SaveLog("Exception WindowService TechRecon LogManager 29 " + ex.Message == null ? ex.InnerException.Message : ex.Message);

                                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                                var stackTrace = new StackTrace(ex);
                                var thisasm = Assembly.GetExecutingAssembly();
                                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                LogManager.SaveLog("An error occured  ConsortiumLibrary in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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

                                LogManager.SaveLog("Total records pulled in CBSConsortium: " + returndata.Rows.Count);
                                
                                if (returndata.Rows.Count > 0)
                                {
                                    int countTrans = 0;
                                    var CBSConsortiumtbl = new CBSConsortiumTransaction();
                                    var CBSConsorErrTBL = new CBSConsortiumTransactionError();
                                    for (int col = 0; col < returndata.Rows.Count; col++)
                                    {
                                        try
                                        {
                                            decimal Amountad, Amount;
                                            if (decimal.TryParse(returndata.Rows[col][4].ToString(), out Amountad))
                                            {
                                                Amount = returndata.Rows[col][4] == null ? 0 : Convert.ToDecimal(returndata.Rows[col][4]);
                                            }
                                            else
                                            {
                                                Amount = 0;
                                            }
                                                
                                            CBSConsortiumtbl.AcctNo = returndata.Rows[col][0] == null ? null : returndata.Rows[col][0].ToString();
                                            CBSConsortiumtbl.TransDate = returndata.Rows[col][1] == null ? (DateTime?)null : Convert.ToDateTime(returndata.Rows[col][1]);

                                            dateTest = CBSConsortiumtbl.TransDate.ToString();
                                            if (countTrans > 1)
                                            {
                                                MaxTransDate = CBSConsortiumtbl.TransDate.ToString();

                                                if (Convert.ToDateTime(dateTest) > Convert.ToDateTime(MaxTransDate))
                                                {
                                                    MaxTransDate = dateTest;

                                                }
                                                else
                                                {
                                                    MaxTransDate = MaxTransDate;
                                                }
                                            }
                                            
                                            CBSConsortiumtbl.Description = returndata.Rows[col][2] == null ? null : returndata.Rows[col][2].ToString();   
                                            CBSConsortiumtbl.OriginatingBranch = returndata.Rows[col][3] == null ? null : returndata.Rows[col][3].ToString();
                                            CBSConsortiumtbl.Amount = Math.Round(Amount, 2);
                                            CBSConsortiumtbl.DebitCredit = returndata.Rows[col][5] == null ? null : returndata.Rows[col][5].ToString();
                                            CBSConsortiumtbl.PayerName = returndata.Rows[col][6] == null ? null : returndata.Rows[col][6].ToString();
                                            CBSConsortiumtbl.SerialNo = returndata.Rows[col][7] == null ? null : returndata.Rows[col][7].ToString();
                                            CBSConsortiumtbl.OrigRefNo = CBSConsortiumtbl.SerialNo;
                                            CBSConsortiumtbl.PostedBy = returndata.Rows[col][8] == null ? null : returndata.Rows[col][8].ToString();
                                            CBSConsortiumtbl.PtId = returndata.Rows[col][9] == null ? null : returndata.Rows[col][9].ToString();

                                            LastRecordId = CBSConsortiumtbl.PtId;
                                            int revcode;
                                            CBSConsortiumtbl.ReversalCode = returndata.Rows[col][10] == null ? (int?)null : int.TryParse(returndata.Rows[col][10].ToString(), out revcode) ? revcode : (int?)null;
                                            CBSConsortiumtbl.UserId = 1;
                                           
                                            CBSConsortiumtbl.PullDate = DateTime.Now;
                                            
                                            CBSConsortiumtbl.MatchingStatus = "N";
                                           
                                            countTrans += 1;
                                            if (!string.IsNullOrWhiteSpace(CBSConsortiumtbl.SerialNo))
                                            {
                                                var exist = await repoCBSConsortiumTransactionRepository.GetAsync(c => c.SerialNo == CBSConsortiumtbl.SerialNo && (c.DebitCredit == CBSConsortiumtbl.DebitCredit) && (c.Amount == CBSConsortiumtbl.Amount) && (c.AcctNo == CBSConsortiumtbl.AcctNo));

                                                var existHis = await repoCBSConsortiumTransactionHistoryRepository.GetAsync(c => c.SerialNo == CBSConsortiumtbl.SerialNo && (c.DebitCredit == CBSConsortiumtbl.DebitCredit) && (c.Amount == CBSConsortiumtbl.Amount) && (c.AcctNo == CBSConsortiumtbl.AcctNo));


                                                if (exist != null || existHis != null)
                                                {
                                                    CBSConsorErrTBL.PtId = CBSConsortiumtbl.PtId;
                                                    CBSConsorErrTBL.AcctNo = CBSConsortiumtbl.AcctNo;
                                                    CBSConsorErrTBL.TransDate = CBSConsortiumtbl.TransDate;
                                                    CBSConsorErrTBL.Description = CBSConsortiumtbl.Description;
                                                    CBSConsorErrTBL.OriginatingBranch = CBSConsortiumtbl.OriginatingBranch;
                                                    CBSConsorErrTBL.Amount = CBSConsortiumtbl.Amount;
                                                    CBSConsorErrTBL.DebitCredit = CBSConsortiumtbl.DebitCredit;
                                                    CBSConsorErrTBL.PayerName = CBSConsortiumtbl.PayerName;
                                                    CBSConsorErrTBL.SerialNo = CBSConsortiumtbl.SerialNo;
                                                    CBSConsorErrTBL.OrigRefNo = CBSConsortiumtbl.OrigRefNo;
                                                    CBSConsorErrTBL.ReversalCode = CBSConsortiumtbl.ReversalCode;
                                                    CBSConsorErrTBL.PostedBy = CBSConsortiumtbl.PostedBy;
                                                    CBSConsorErrTBL.UserId = 1;
                                                    CBSConsorErrTBL.PullDate = DateTime.Now;
                                                    CBSConsorErrTBL.ErrorMsg = "Duplicate transaction record";

                                                    repoCBSConsortiumTransactionErrorRepository.Add(CBSConsorErrTBL);
                                                    var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret1)
                                                    {
                                                        continue;
                                                    }
                                             
                                                }
                                                else
                                                {
                                                    repoCBSConsortiumTransactionRepository.Add(CBSConsortiumtbl);
                                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                        continue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                CBSConsorErrTBL.PtId = CBSConsortiumtbl.PtId;
                                                CBSConsorErrTBL.AcctNo = CBSConsortiumtbl.AcctNo;
                                                CBSConsorErrTBL.TransDate = CBSConsortiumtbl.TransDate;
                                                CBSConsorErrTBL.Description = CBSConsortiumtbl.Description;
                                                CBSConsorErrTBL.OriginatingBranch = CBSConsortiumtbl.OriginatingBranch;
                                                CBSConsorErrTBL.Amount = CBSConsortiumtbl.Amount;
                                                CBSConsorErrTBL.DebitCredit = CBSConsortiumtbl.DebitCredit;
                                                CBSConsorErrTBL.PayerName = CBSConsortiumtbl.PayerName;
                                                CBSConsorErrTBL.SerialNo = CBSConsortiumtbl.SerialNo;
                                                CBSConsorErrTBL.OrigRefNo = CBSConsortiumtbl.OrigRefNo;
                                                CBSConsorErrTBL.ReversalCode = CBSConsortiumtbl.ReversalCode;
                                                CBSConsorErrTBL.PostedBy = CBSConsortiumtbl.PostedBy;
                                                CBSConsorErrTBL.UserId = 1;
                                                CBSConsorErrTBL.PullDate = DateTime.Now;
                                                CBSConsorErrTBL.ErrorMsg = "No trans ref";

                                                repoCBSConsortiumTransactionErrorRepository.Add(CBSConsorErrTBL);
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
                                            LogManager.SaveLog("An error occured in  Line ConsortiumLibrary: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                                           
                                            try
                                            {
                                             
                                                CBSConsorErrTBL.PtId = CBSConsortiumtbl.PtId;
                                                CBSConsorErrTBL.AcctNo = CBSConsortiumtbl.AcctNo;
                                                CBSConsorErrTBL.TransDate = CBSConsortiumtbl.TransDate;
                                                CBSConsorErrTBL.Description = CBSConsortiumtbl.Description;
                                                CBSConsorErrTBL.OriginatingBranch = CBSConsortiumtbl.OriginatingBranch;
                                                CBSConsorErrTBL.Amount = CBSConsortiumtbl.Amount;
                                                CBSConsorErrTBL.DebitCredit = CBSConsortiumtbl.DebitCredit;
                                                CBSConsorErrTBL.PayerName = CBSConsortiumtbl.PayerName;
                                                CBSConsorErrTBL.SerialNo = CBSConsortiumtbl.SerialNo;
                                                CBSConsorErrTBL.OrigRefNo = CBSConsortiumtbl.OrigRefNo;
                                                CBSConsorErrTBL.PostedBy = CBSConsortiumtbl.PostedBy;
                                                CBSConsorErrTBL.ReversalCode = CBSConsortiumtbl.ReversalCode;
                                                CBSConsorErrTBL.UserId = 1;
                                                CBSConsorErrTBL.PullDate = DateTime.Now;
                                                CBSConsorErrTBL.ErrorMsg = ex.Message == null ? ex.InnerException.Message : ex.Message;

                                                repoCBSConsortiumTransactionErrorRepository.Add(CBSConsorErrTBL);
                                                var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                                if (ret1)
                                                {
                                                    continue;
                                                }
                                            }
                                            catch (Exception ex1)
                                            {
                                              
                                                var exErr1 = ex1 == null ? ex1.InnerException.Message : ex1.Message;
                                                var stackTrace1 = new StackTrace(ex);
                                                var thisasm1 = Assembly.GetExecutingAssembly();
                                                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm1).Name;
                                                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                                                LogManager.SaveLog("An error occured ConsortiumLibrary in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                                        admDataPoollingControlTBL.TableName = "CBSConsortiumTransaction";
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
                                LogManager.SaveLog("An error occured ConsortiumLibrary in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                              
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
                        LogManager.SaveLog("An error occured in ConsortiumLibrary: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                        
                    }
                }

                #endregion
                #endregion
               
                #region Source 2

                #region  //MS SQL SERVER Below
                var dtSouceCon2 = await repoadmReconDataSourcesRepository.GetAsync(c => c.ReconDSId == dt2);
                if (dtSouceCon2.DatabaseType == "MSSQL")
                {
                    try
                    {
                        string MaxTransDate = string.Empty;
                        string dateTest = string.Empty;
                        string connstring = System.Configuration.ConfigurationManager.ConnectionStrings["SQLconnection"].ToString();

                        dtSouceCon2.Password = dtSouceCon2.Password == null ? "" : library.Decrypt(dtSouceCon2.Password);

                        connstring = connstring.Replace("{{Data Source}}", dtSouceCon2.ipAddress = dtSouceCon2.ipAddress == null ? "" : dtSouceCon2.ipAddress.Trim());
                        connstring = connstring.Replace("{{port}}", dtSouceCon2.PortNumber = dtSouceCon2.PortNumber == null ? "" : dtSouceCon2.PortNumber.Trim());
                        connstring = connstring.Replace("{{database}}", dtSouceCon2.DatabaseName = dtSouceCon2.DatabaseName == null ? "" : dtSouceCon2.DatabaseName.Trim());
                        connstring = connstring.Replace("{{uid}}", dtSouceCon2.UserName = dtSouceCon2.UserName == null ? "" : dtSouceCon2.UserName.Trim());
                        connstring = connstring.Replace("{{pwd}}", dtSouceCon2.Password);

                        SqlConnection con = new SqlConnection(connstring);
                        try
                        {
                            con.Open();
                            if (con.State.ToString() == "Open")
                            {
                                LogManager.SaveLog("SQL Connection  open for Ip" + dtSouceCon2.ipAddress);
                            }
                            else
                            {
                                LogManager.SaveLog("SQL Connection not Open for Ip" + dtSouceCon2.ipAddress);
                            }
                        }
                        catch (Exception ex)
                        {
                            var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                            var stackTrace = new StackTrace(ex);
                            var thisasm = Assembly.GetExecutingAssembly();
                            _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                            _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                            LogManager.SaveLog("An error occured in Line Consortium MSSQL Source 1 100:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                        }

                        int SqlExecuteType = 0;
                        SqlExecuteType = 1;
                        string sqlScript = string.Empty;
                        if (SqlExecuteType == 1) // Command type is Script
                        {
                            string FromDateParam = string.Empty;
                            string ToDateParam = string.Empty;
                            var rectype = repoReconTypeRepository.Get(c => c.WSReconName == "Consortium");
                            string LastRecordId = string.Empty;
                            string ReconDate = string.Empty;
                            
                            var controlTable = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "ConsortiumTransaction");
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

                            var ConsortiumTBL = new ConsortiumTransaction();
                            var ConsortErrTBL = new ConsortiumTransactionError();

                            sqlScript = ReconType.Source2Script.Replace("{FromDateParam}", FromDateParam)
                                                               .Replace("{ToDateParam}", ToDateParam)
                                                               .Replace("{LastRecordId}", LastRecordId);

                            var returndata = GetConnect(sqlScript, con, SqlExecuteType);

                            LogManager.SaveLog("Total records pulled Consortium SQL Server: " + returndata.Rows.Count);
                            int countTrans = 0;
                            for (int col = 0; col < returndata.Rows.Count; col++)
                            {
                                try
                                {
                                    decimal Amountad, Amount;
                                    if (decimal.TryParse(returndata.Rows[col][2].ToString(), out Amountad))
                                    {
                                        Amount = returndata.Rows[col][2] == null ? 0 : Convert.ToDecimal(returndata.Rows[col][2]);
                                    }
                                    else
                                    {
                                        Amount = 0;
                                    }
                                    ConsortiumTBL.Institution = returndata.Rows[col][0].ToString() == string.Empty.Trim() ? null : returndata.Rows[col][0].ToString();
                                    ConsortiumTBL.FormName = returndata.Rows[col][1].ToString() == string.Empty.Trim() ? null : returndata.Rows[col][1].ToString();
                                    ConsortiumTBL.Amount = Math.Round(Amount, 2);
                                    DateTime dtf = new DateTime();
                                    ConsortiumTBL.TransDate = returndata.Rows[col][3] == null ? (DateTime?)null : DateTime.TryParse(returndata.Rows[col][3].ToString(), out dtf)   ? Convert.ToDateTime(returndata.Rows[col][3]) : (DateTime?)null;
                                    dateTest = ConsortiumTBL.TransDate.ToString();
                                    if (countTrans > 1)
                                    {
                                        MaxTransDate = ConsortiumTBL.TransDate.ToString();

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

                                    
                                    
                                    ConsortiumTBL.BuyVouchersID = returndata.Rows[col][4].ToString() == string.Empty.Trim() ? null : returndata.Rows[col][4].ToString();
                                    LastRecordId = returndata.Rows[col][4].ToString();
                                    ConsortiumTBL.MobileNo = returndata.Rows[col][5].ToString() == string.Empty.Trim() ? null : returndata.Rows[col][5].ToString();
                                    ConsortiumTBL.OriginatingBranch = returndata.Rows[col][6].ToString() == string.Empty.Trim() ? null : returndata.Rows[col][6].ToString();
                                    ConsortiumTBL.TellerName = returndata.Rows[col][7].ToString() == string.Empty.Trim() ? null : returndata.Rows[col][7].ToString();
                                    ConsortiumTBL.SerialNo = returndata.Rows[col][8].ToString() == string.Empty.Trim() ? null : returndata.Rows[col][8].ToString();
                                    ConsortiumTBL.OrigRefNo = ConsortiumTBL.SerialNo;
                                    ConsortiumTBL.PostedBy = returndata.Rows[col][9].ToString() == string.Empty.Trim() ? null : returndata.Rows[col][9].ToString();
                                    ConsortiumTBL.AcctNo = returndata.Rows[col][10].ToString() == string.Empty.Trim() ? null : returndata.Rows[col][10].ToString();
                                    ConsortiumTBL.UserId = 1;
                                    ConsortiumTBL.PullDate = DateTime.Now;
                                    ConsortiumTBL.MatchingStatus = "N";

                                    if (!string.IsNullOrWhiteSpace(ConsortiumTBL.SerialNo))
                                    {
                                        var exist = await repoConsortiumTransactionRepository.GetAsync(c => c.SerialNo == ConsortiumTBL.SerialNo);
                                        var existHis = await repoConsortiumTransactionHistoryRepository.GetAsync(c => c.SerialNo == ConsortiumTBL.SerialNo);

                                        if (exist != null || existHis != null)
                                        {
                                            ConsortErrTBL.Institution = ConsortiumTBL.Institution;
                                            ConsortErrTBL.FormName =  ConsortiumTBL.FormName;
                                            ConsortErrTBL.Amount = ConsortiumTBL.Amount;
                                            ConsortErrTBL.TransDate = ConsortiumTBL.TransDate;
                                            ConsortErrTBL.BuyVouchersID =  ConsortiumTBL.BuyVouchersID;
                                            ConsortErrTBL.MobileNo = ConsortiumTBL.MobileNo;
                                            ConsortErrTBL.OriginatingBranch =  ConsortiumTBL.OriginatingBranch;
                                            ConsortErrTBL.TellerName =  ConsortiumTBL.TellerName;
                                            ConsortErrTBL.SerialNo =  ConsortiumTBL.SerialNo;
                                            ConsortErrTBL.OrigRefNo =  ConsortiumTBL.OrigRefNo;
                                            ConsortErrTBL.PostedBy =  ConsortiumTBL.PostedBy;
                                            ConsortErrTBL.AcctNo =  ConsortiumTBL.AcctNo;
                                            ConsortErrTBL.UserId = 1;
                                            ConsortErrTBL.PullDate = DateTime.Now;
                                            ConsortErrTBL.ErrorMsg = "Duplicate transaction record";

                                            repoConsortiumTransactionErrorRepository.Add(ConsortErrTBL);
                                            var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                            if (ret1)
                                            {
                                                continue;
                                            }
                                            continue;
                                        }

                                        else
                                        {
                                            repoConsortiumTransactionRepository.Add(ConsortiumTBL);
                                            var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                            if (ret)
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ConsortErrTBL.Institution = ConsortiumTBL.Institution;
                                        ConsortErrTBL.FormName = ConsortiumTBL.FormName;
                                        ConsortErrTBL.Amount = ConsortiumTBL.Amount;
                                        ConsortErrTBL.TransDate = ConsortiumTBL.TransDate;
                                        ConsortErrTBL.BuyVouchersID = ConsortiumTBL.BuyVouchersID;
                                        ConsortErrTBL.MobileNo = ConsortiumTBL.MobileNo;
                                        ConsortErrTBL.OriginatingBranch = ConsortiumTBL.OriginatingBranch;
                                        ConsortErrTBL.TellerName = ConsortiumTBL.TellerName;
                                        ConsortErrTBL.SerialNo = ConsortiumTBL.SerialNo;
                                        ConsortErrTBL.OrigRefNo = ConsortiumTBL.OrigRefNo;
                                        ConsortErrTBL.PostedBy = ConsortiumTBL.PostedBy;
                                        ConsortErrTBL.AcctNo = ConsortiumTBL.AcctNo;
                                        ConsortErrTBL.UserId = 1;
                                        ConsortErrTBL.PullDate = DateTime.Now;
                                        ConsortErrTBL.ErrorMsg = "No trans ref";

                                        repoConsortiumTransactionErrorRepository.Add(ConsortErrTBL);
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
                                    LogManager.SaveLog("An error occured in ConsortiumLogManager Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);


                                    ConsortErrTBL.Institution = ConsortiumTBL.Institution;
                                    ConsortErrTBL.FormName = ConsortiumTBL.FormName;
                                    ConsortErrTBL.Amount = ConsortiumTBL.Amount;
                                    ConsortErrTBL.TransDate = ConsortiumTBL.TransDate;
                                    ConsortErrTBL.BuyVouchersID = ConsortiumTBL.BuyVouchersID;
                                    ConsortErrTBL.MobileNo = ConsortiumTBL.MobileNo;
                                    ConsortErrTBL.OriginatingBranch = ConsortiumTBL.OriginatingBranch;
                                    ConsortErrTBL.TellerName = ConsortiumTBL.TellerName;
                                    ConsortErrTBL.SerialNo = ConsortiumTBL.SerialNo;
                                    ConsortErrTBL.OrigRefNo = ConsortiumTBL.OrigRefNo;
                                    ConsortErrTBL.PostedBy = ConsortiumTBL.PostedBy;
                                    ConsortErrTBL.AcctNo = ConsortiumTBL.AcctNo;
                                    ConsortErrTBL.UserId = 1;
                                    ConsortErrTBL.PullDate = DateTime.Now;
                                    ConsortErrTBL.ErrorMsg = ex.Message == null ? ex.InnerException.Message : ex.Message;

                                    repoConsortiumTransactionErrorRepository.Add(ConsortErrTBL);
                                    var ret1 = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                    if (ret1)
                                    {
                                        continue;
                                    }
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
                                admDataPoollingControlTBL.TableName = "ConsortiumTransaction";
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
                        con.Close();
                        con.Dispose();
                    }
                    catch (Exception ex)
                    {
                       
                        var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                        var stackTrace = new StackTrace(ex);
                        var thisasm = Assembly.GetExecutingAssembly();
                        _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                        _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                        LogManager.SaveLog("An error occured in  ConsortiumLibrary: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                LogManager.SaveLog("An error occured in Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }

            return string.Empty;
        }
        private static DataTable GetConnect(string SqlScript, SqlConnection con, int ScriptType)
        {
            DataTable dt = new DataTable();
            try
            {
                
                SqlCommand cmd = new SqlCommand(SqlScript, con);
                cmd.CommandType = ScriptType == 0 ? CommandType.StoredProcedure : CommandType.Text;

                SqlDataReader dr = cmd.ExecuteReader();

                DataSet ds = new DataSet();
                ds.Load(dr, LoadOption.OverwriteChanges, "Results");
                dt = ds.Tables[0];
                dr.Close();

                return dt;
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                // _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                // _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                //SaveLog("An error occured in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
            }

            return dt;
        }
    }


}