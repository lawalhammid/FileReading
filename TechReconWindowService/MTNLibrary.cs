
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
    public class MTNLibrary
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmReconDataSourcesRepository repoadmReconDataSourcesRepository;
        private readonly IadmSourceAccountRepository repoadmSourceAccountRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly IMGTransRepository repoMGTransRepository;
        private readonly IadmFailledFileRepository repoadmFailledFileRepository;
        private readonly IadmFileProcessedRepository repoadmFileProcessedRepository;
        private readonly ICBSMTNTransRepository repoCBSMTNTransRepository;
        private readonly IMTNTRansErrorRepository repoMTNTRansErrorRepository;
        private readonly ICBSMTNTransErrorRepository repoCBSMTNTransErrorRepository;
        private readonly IMTNTRansRepository repoMTNTRansRepository;
        private readonly ICBSMTNTransHistoryRepository repoCBSMTNTransHistoryRepository;
        private readonly IMTNTRansHistoryRepository repoMTNTRansHistoryRepository;
        

        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/MTNLibrary.cs";

        public MTNLibrary()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmReconDataSourcesRepository = new admReconDataSourcesRepository(idbfactory);
            repoadmSourceAccountRepository = new admSourceAccountRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoMGTransRepository = new MGTransRepository(idbfactory);
            repoadmFailledFileRepository = new admFailledFileRepository(idbfactory);
            repoadmFileProcessedRepository = new admFileProcessedRepository(idbfactory);
            repoMTNTRansErrorRepository = new MTNTRansErrorRepository(idbfactory);
            repoCBSMTNTransRepository = new CBSMTNTransRepository(idbfactory);
            repoCBSMTNTransErrorRepository = new CBSMTNTransErrorRepository(idbfactory);
            repoMTNTRansRepository = new MTNTRansRepository(idbfactory);
            repoCBSMTNTransHistoryRepository = new CBSMTNTransHistoryRepository(idbfactory);
            repoMTNTRansHistoryRepository = new MTNTRansHistoryRepository(idbfactory);
        }

        public async Task<string> PullMTNData()
        {
            Library library = new Library();
            try
            {
                LogManager.SaveLog("Task Start in MTNLibrary");
                var ReconType = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "MTN");
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

                #region Source 2

                var dtSouceCon2 = await repoadmSourceAccountRepository.GetAsync(c => c.ReconTypeId == ReconTypeId && c.SourceName == "Source 2");

                #region //File Directory Below

                if (!string.IsNullOrWhiteSpace(dtSouceCon2.FileDirectory))
                {
                    try
                    {
                        string MaxTransDate = string.Empty;
                        string dateTest = string.Empty;
                        var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "MTN");
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "MTNTrans");

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
                        DLIST.AddRange(d.GetFiles("*" + ".XLSX").ToList());
                        DLIST = d.GetFiles("*" + ".CSV").ToList();
                        DLIST.AddRange(d.GetFiles("*" + ".CSV").ToList());
                        DLIST.AddRange(d.GetFiles("*" + ".csv").ToList());
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
                            var MTNTBL = new MTNTRan();
                            var MTNErrTBL = new MTNTRansError();

                            int countTrans = 0;
                            fileNamenAndType = (from f in DLIST orderby f.LastWriteTime ascending select kl).First().Name;

                            FileLastTime = (from f in DLIST orderby f.LastAccessTimeUtc descending select kl).First().LastWriteTime.ToString();

                            //Cpndition to read CSV file
                            if (fileNamenAndType.Contains(".CSV") || fileNamenAndType.Contains(".csv"))
                            {
                                var Lines = File.ReadLines(FileDirectory + "\\" + fileNamenAndType).Select(a => a.Split(';'));

                                for (int i = 0; i < Lines.Count(); i++)
                                {
                                    foreach (var b in Lines)
                                    {
                                        string[] name = b[i].Split(',');
                                        if (name.Count() == 26)
                                        {
                                            try
                                            {
                                                Int64 checkTransId;
                                                string  index0 = name[0];

                                                if (Int64.TryParse(index0.ToString(), out checkTransId))
                                                {
                                                    if (!string.IsNullOrEmpty(name[0]))
                                                    {
                                                        MTNTBL.TransId = name[0];
                                                        MTNTBL.OrigRefNo = MTNTBL.TransId;
                                                        MTNTBL.ExternalTransId = string.IsNullOrWhiteSpace(name[1]) ? null : name[1];
                                                        DateTime date = new DateTime();
                                                        string swapdate = name[2];
                                                        if (swapdate.Contains("/"))
                                                        {
                                                            var dtw = swapdate.Split('/');
                                                            swapdate = dtw[1] + "/" + dtw[0] + "/" + dtw[2];
                                                        }
                                                        if (DateTime.TryParse(swapdate, out date))
                                                        {
                                                            MTNTBL.Date = swapdate == null ? (DateTime?)null : Convert.ToDateTime(swapdate);
                                                            dateTest = MTNTBL.Date.ToString();
                                                            MaxTransDate = MTNTBL.Date.ToString();
                                                            if (countTrans > 1)
                                                            {
                                                                MaxTransDate = MTNTBL.Date.ToString();

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
                                                        MTNTBL.Status = name[3];
                                                        MTNTBL.Type = name[4];
                                                        MTNTBL.ProviderCategory = string.IsNullOrWhiteSpace(name[5]) ? null : name[5];

                                                        int ToMsg = 0;
                                                        if (int.TryParse(name[6], out ToMsg))
                                                        {
                                                            MTNTBL.ToMsg = name[6] == null ? 0 : Convert.ToInt32(name[6]);
                                                        }
                                                        MTNTBL.From = name[7];
                                                        MTNTBL.FromName = name[8];
                                                        MTNTBL.To = name[9];
                                                        MTNTBL.ToName = name[10];
                                                        MTNTBL.InitiatedBy = name[11];
                                                        MTNTBL.OnBehalfOf = name[12];
                                                        decimal amtdec = 0;
                                                        int Amt = 0;
                                                        if (int.TryParse(name[13], out Amt))
                                                        {
                                                            decimal hhh = Convert.ToDecimal(name[13]);
                                                            MTNTBL.Amount = Math.Round(hhh,2);
                                                        }
                                                        if (decimal.TryParse(name[13], out amtdec))
                                                        {
                                                            MTNTBL.Amount = Math.Round(Convert.ToDecimal(name[13]), 2);
                                                        }

                                                        MTNTBL.Currency1 = name[14];

                                                        int fee = 0;
                                                        if (int.TryParse(name[15], out fee))
                                                        {
                                                            MTNTBL.Fee = name[15] == null ? 0 : Convert.ToInt32(name[15]); ;
                                                        }
                                                        MTNTBL.Currency2 = string.IsNullOrWhiteSpace(name[16]) ? null : name[16];
                                                        MTNTBL.Discount = string.IsNullOrWhiteSpace(name[17]) ? null : name[17];
                                                        MTNTBL.Currency3 = string.IsNullOrWhiteSpace(name[18]) ? null : name[18];
                                                        MTNTBL.Promotion = string.IsNullOrWhiteSpace(name[19]) ? null : name[19];
                                                        MTNTBL.Currency4 = string.IsNullOrWhiteSpace(name[20]) ? null : name[20];
                                                        MTNTBL.Coupon = string.IsNullOrWhiteSpace(name[21]) ? null : name[21];
                                                        MTNTBL.Currency5 = string.IsNullOrWhiteSpace(name[22]) ? null : name[22];
                                                        decimal bal = 0;
                                                        if (decimal.TryParse(name[23], out bal))
                                                        {
                                                            MTNTBL.Balance = name[23] == null ? 0 : Math.Round(Convert.ToDecimal(name[23]), 1);
                                                        }
                                                        MTNTBL.Currency6 = string.IsNullOrWhiteSpace(name[24]) ? null : name[24];
                                                        MTNTBL.Information = string.IsNullOrWhiteSpace(name[25]) ? null : name[25];
                                                        MTNTBL.PullDate = DateTime.Now;
                                                        MTNTBL.MatchingStatus = "N";
                                                        MTNTBL.UserId = 1;
                                                        MTNTBL.FileName = fileNamenAndType;
                                                    }

                                                    if (!string.IsNullOrWhiteSpace(MTNTBL.TransId))
                                                    {
                                                        var exist = await repoMTNTRansRepository.GetAsync(c => c.TransId == MTNTBL.TransId);
                                                        var existHis = await repoMTNTRansHistoryRepository.GetAsync(c => c.TransId == MTNTBL.TransId);
                                                        if (exist != null || existHis != null)
                                                        {
                                                            MTNErrTBL.TransId = MTNTBL.TransId;
                                                            MTNErrTBL.OrigRefNo = MTNTBL.TransId;
                                                            MTNErrTBL.ExternalTransId = MTNTBL.ExternalTransId;
                                                            MTNErrTBL.Date = MTNTBL.Date;
                                                            MTNErrTBL.Type = MTNTBL.Type;
                                                            MTNErrTBL.ProviderCategory = MTNTBL.ProviderCategory;
                                                            MTNErrTBL.ToMsg = MTNTBL.ToMsg;
                                                            MTNErrTBL.From = MTNTBL.From;
                                                            MTNErrTBL.FromName = MTNTBL.FromName;
                                                            MTNErrTBL.To = MTNTBL.To;
                                                            MTNErrTBL.ToName = MTNTBL.ToName;
                                                            MTNErrTBL.InitiatedBy = MTNTBL.InitiatedBy;
                                                            MTNErrTBL.OnBehalfOf = MTNTBL.OnBehalfOf;
                                                            MTNErrTBL.Amount = MTNTBL.Amount;
                                                            MTNErrTBL.Currency1 = MTNTBL.Currency1;
                                                            MTNErrTBL.Fee = MTNTBL.Fee;
                                                            MTNErrTBL.Currency2 = MTNTBL.Currency2;
                                                            MTNErrTBL.Discount = MTNTBL.Discount;
                                                            MTNErrTBL.Currency3 = MTNTBL.Currency3;
                                                            MTNErrTBL.Promotion = MTNTBL.Promotion;
                                                            MTNErrTBL.Currency4 = MTNTBL.Currency4;
                                                            MTNErrTBL.Coupon = MTNTBL.Coupon;
                                                            MTNErrTBL.Currency5 = MTNTBL.Currency5;
                                                            MTNErrTBL.Balance = MTNTBL.Balance;
                                                            MTNErrTBL.Currency6 = MTNTBL.Currency6;
                                                            MTNErrTBL.Information = MTNTBL.Information;
                                                            MTNErrTBL.PullDate = MTNTBL.PullDate;
                                                            MTNErrTBL.MatchingStatus = MTNTBL.MatchingStatus;
                                                            MTNErrTBL.UserId = MTNTBL.UserId;
                                                            MTNErrTBL.MatchingType = MTNTBL.MatchingType;
                                                            MTNErrTBL.ErrorMsg = "Duplicate transaction record";
                                                            MTNErrTBL.FileName = MTNTBL.FileName;
                                                            repoMTNTRansErrorRepository.Add(MTNErrTBL);
                                                            var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                            if (ret)
                                                            {
                                                                continue;
                                                            }
                                                            continue;
                                                        }
                                                        else
                                                        {
                                                            repoMTNTRansRepository.Add(MTNTBL);
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
                                                        MTNErrTBL.TransId = MTNTBL.TransId;
                                                        MTNErrTBL.OrigRefNo = MTNTBL.TransId;
                                                        MTNErrTBL.ExternalTransId = MTNTBL.ExternalTransId;
                                                        MTNErrTBL.Date = MTNTBL.Date;
                                                        MTNErrTBL.Type = MTNTBL.Type;
                                                        MTNErrTBL.ProviderCategory = MTNTBL.ProviderCategory;
                                                        MTNErrTBL.ToMsg = MTNTBL.ToMsg;
                                                        MTNErrTBL.From = MTNTBL.From;
                                                        MTNErrTBL.FromName = MTNTBL.FromName;
                                                        MTNErrTBL.To = MTNTBL.To;
                                                        MTNErrTBL.ToName = MTNTBL.ToName;
                                                        MTNErrTBL.InitiatedBy = MTNTBL.InitiatedBy;
                                                        MTNErrTBL.OnBehalfOf = MTNTBL.OnBehalfOf;
                                                        MTNErrTBL.Amount = MTNTBL.Amount;
                                                        MTNErrTBL.Currency1 = MTNTBL.Currency1;
                                                        MTNErrTBL.Fee = MTNTBL.Fee;
                                                        MTNErrTBL.Currency2 = MTNTBL.Currency2;
                                                        MTNErrTBL.Discount = MTNTBL.Discount;
                                                        MTNErrTBL.Currency3 = MTNTBL.Currency3;
                                                        MTNErrTBL.Promotion = MTNTBL.Promotion;
                                                        MTNErrTBL.Currency4 = MTNTBL.Currency4;
                                                        MTNErrTBL.Coupon = MTNTBL.Coupon;
                                                        MTNErrTBL.Currency5 = MTNTBL.Currency5;
                                                        MTNErrTBL.Balance = MTNTBL.Balance;
                                                        MTNErrTBL.Currency6 = MTNTBL.Currency6;
                                                        MTNErrTBL.Information = MTNTBL.Information;
                                                        MTNErrTBL.PullDate = MTNTBL.PullDate;
                                                        MTNErrTBL.MatchingStatus = MTNTBL.MatchingStatus;
                                                        MTNErrTBL.UserId = MTNTBL.UserId;
                                                        MTNErrTBL.MatchingType = MTNTBL.MatchingType;
                                                        MTNErrTBL.ErrorMsg = "No trans ref";
                                                        MTNErrTBL.FileName = MTNTBL.FileName;
                                                        repoMTNTRansErrorRepository.Add(MTNErrTBL);
                                                        var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                        if (ret)
                                                        {
                                                            continue;
                                                        }
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
                                                LogManager.SaveLog("An error occured in MTNLibrary Line  :" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                                                MTNErrTBL.TransId = MTNTBL.TransId;
                                                MTNErrTBL.OrigRefNo = MTNTBL.TransId;
                                                MTNErrTBL.ExternalTransId = MTNTBL.ExternalTransId;
                                                MTNErrTBL.Date = MTNTBL.Date;
                                                MTNErrTBL.Type = MTNTBL.Type;
                                                MTNErrTBL.ProviderCategory = MTNTBL.ProviderCategory;
                                                MTNErrTBL.ToMsg = MTNTBL.ToMsg;
                                                MTNErrTBL.From = MTNTBL.From;
                                                MTNErrTBL.FromName = MTNTBL.FromName;
                                                MTNErrTBL.To = MTNTBL.To;
                                                MTNErrTBL.ToName = MTNTBL.ToName;
                                                MTNErrTBL.InitiatedBy = MTNTBL.InitiatedBy;
                                                MTNErrTBL.OnBehalfOf = MTNTBL.OnBehalfOf;
                                                MTNErrTBL.Amount = MTNTBL.Amount;
                                                MTNErrTBL.Currency1 = MTNTBL.Currency1;
                                                MTNErrTBL.Fee = MTNTBL.Fee;
                                                MTNErrTBL.Currency2 = MTNTBL.Currency2;
                                                MTNErrTBL.Discount = MTNTBL.Discount;
                                                MTNErrTBL.Currency3 = MTNTBL.Currency3;
                                                MTNErrTBL.Promotion = MTNTBL.Promotion;
                                                MTNErrTBL.Currency4 = MTNTBL.Currency4;
                                                MTNErrTBL.Coupon = MTNTBL.Coupon;
                                                MTNErrTBL.Currency5 = MTNTBL.Currency5;
                                                MTNErrTBL.Balance = MTNTBL.Balance;
                                                MTNErrTBL.Currency6 = MTNTBL.Currency6;
                                                MTNErrTBL.Information = MTNTBL.Information;
                                                MTNErrTBL.PullDate = MTNTBL.PullDate;
                                                MTNErrTBL.MatchingStatus = MTNTBL.MatchingStatus;
                                                MTNErrTBL.UserId = MTNTBL.UserId;
                                                MTNErrTBL.MatchingType = MTNTBL.MatchingType;
                                                MTNErrTBL.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;
                                                MTNErrTBL.FileName = MTNTBL.FileName;
                                                repoMTNTRansErrorRepository.Add(MTNErrTBL);
                                                var ret = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                                if (ret)
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                    }


                                    break;
                                }

                            }
                                //Condition to read Excel File
                            else
                            {
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

                                    foreach (DataRow row in dTable.Rows.Cast<DataRow>().Skip(2))//.Skip(2))
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



                                            if (!string.IsNullOrEmpty(col0))
                                            {
                                                MTNTBL.TransId = col0;
                                                MTNTBL.OrigRefNo = MTNTBL.TransId;
                                                MTNTBL.ExternalTransId = col1;
                                                DateTime date = new DateTime();
                                                if (DateTime.TryParse(col2, out date))
                                                {
                                                    MTNTBL.Date = col2 == null ? (DateTime?)null : Convert.ToDateTime(col2);
                                                    dateTest = MTNTBL.Date.ToString();
                                                    if (countTrans > 1)
                                                    {
                                                        MaxTransDate = MTNTBL.Date.ToString();

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
                                                MTNTBL.Status = col3;
                                                MTNTBL.Type = col4;
                                                MTNTBL.ProviderCategory = col5;

                                                int ToMsg = 0;
                                                if (int.TryParse(col6, out ToMsg))
                                                {
                                                    MTNTBL.ToMsg = col6 == null ? 0 : Convert.ToInt32(col6);
                                                }
                                                MTNTBL.From = col7;
                                                MTNTBL.FromName = col8;
                                                MTNTBL.To = col9;
                                                MTNTBL.ToName = col10;
                                                MTNTBL.InitiatedBy = col11;
                                                MTNTBL.OnBehalfOf = col12;
                                                int Amt = 0;
                                                if (int.TryParse(col13, out Amt))
                                                {
                                                    MTNTBL.Amount = col13 == null ? 0 : Convert.ToInt32(col13);
                                                }
                                                MTNTBL.Currency1 = col14;

                                                int fee = 0;
                                                if (int.TryParse(col15, out fee))
                                                {
                                                    MTNTBL.Fee = col15 == null ? 0 : Convert.ToInt32(col15); ;
                                                }
                                                MTNTBL.Currency2 = col16;
                                                MTNTBL.Discount = col17;
                                                MTNTBL.Currency3 = col18;
                                                MTNTBL.Promotion = col19;
                                                MTNTBL.Currency4 = col20;
                                                MTNTBL.Coupon = col21;
                                                MTNTBL.Currency5 = col22;
                                                decimal bal = 0;
                                                if (decimal.TryParse(col23, out bal))
                                                {
                                                    MTNTBL.Balance = col23 == null ? 0 : Math.Round(Convert.ToDecimal(col23), 1);
                                                }
                                                MTNTBL.Currency6 = col24;
                                                MTNTBL.Information = col25;
                                                MTNTBL.PullDate = DateTime.Now;
                                                MTNTBL.MatchingStatus = "N";
                                                MTNTBL.UserId = 1;
                                                MTNTBL.FileName = fileNamenAndType;
                                            }

                                            if (!string.IsNullOrWhiteSpace(MTNTBL.TransId))
                                            {
                                                var exist = await repoMTNTRansRepository.GetAsync(c => c.TransId == MTNTBL.TransId);
                                                var existHis = await repoMTNTRansHistoryRepository.GetAsync(c => c.TransId == MTNTBL.TransId);
                                                if (exist != null || existHis != null)
                                                {
                                                    MTNErrTBL.TransId = MTNTBL.TransId;
                                                    MTNErrTBL.OrigRefNo = MTNTBL.TransId;
                                                    MTNErrTBL.ExternalTransId = MTNTBL.ExternalTransId;
                                                    MTNErrTBL.Date = MTNTBL.Date;
                                                    MTNErrTBL.Type = MTNTBL.Type;
                                                    MTNErrTBL.ProviderCategory = MTNTBL.ProviderCategory;
                                                    MTNErrTBL.ToMsg = MTNTBL.ToMsg;
                                                    MTNErrTBL.From = MTNTBL.From;
                                                    MTNErrTBL.FromName = MTNTBL.FromName;
                                                    MTNErrTBL.To = MTNTBL.To;
                                                    MTNErrTBL.ToName = MTNTBL.ToName;
                                                    MTNErrTBL.InitiatedBy = MTNTBL.InitiatedBy;
                                                    MTNErrTBL.OnBehalfOf = MTNTBL.OnBehalfOf;
                                                    MTNErrTBL.Amount = MTNTBL.Amount;
                                                    MTNErrTBL.Currency1 = MTNTBL.Currency1;
                                                    MTNErrTBL.Fee = MTNTBL.Fee;
                                                    MTNErrTBL.Currency2 = MTNTBL.Currency2;
                                                    MTNErrTBL.Discount = MTNTBL.Discount;
                                                    MTNErrTBL.Currency3 = MTNTBL.Currency3;
                                                    MTNErrTBL.Promotion = MTNTBL.Promotion;
                                                    MTNErrTBL.Currency4 = MTNTBL.Currency4;
                                                    MTNErrTBL.Coupon = MTNTBL.Coupon;
                                                    MTNErrTBL.Currency5 = MTNTBL.Currency5;
                                                    MTNErrTBL.Balance = MTNTBL.Balance;
                                                    MTNErrTBL.Currency6 = MTNTBL.Currency6;
                                                    MTNErrTBL.Information = MTNTBL.Information;
                                                    MTNErrTBL.PullDate = MTNTBL.PullDate;
                                                    MTNErrTBL.MatchingStatus = MTNTBL.MatchingStatus;
                                                    MTNErrTBL.UserId = MTNTBL.UserId;
                                                    MTNErrTBL.MatchingType = MTNTBL.MatchingType;
                                                    MTNErrTBL.ErrorMsg = "Duplicate transaction record";
                                                    MTNErrTBL.FileName = MTNTBL.FileName;
                                                    repoMTNTRansErrorRepository.Add(MTNErrTBL);
                                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                        continue;
                                                    }
                                                    continue;
                                                }
                                                else
                                                {
                                                    repoMTNTRansRepository.Add(MTNTBL);
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
                                                MTNErrTBL.TransId = MTNTBL.TransId;
                                                MTNErrTBL.OrigRefNo = MTNTBL.TransId;
                                                MTNErrTBL.ExternalTransId = MTNTBL.ExternalTransId;
                                                MTNErrTBL.Date = MTNTBL.Date;
                                                MTNErrTBL.Type = MTNTBL.Type;
                                                MTNErrTBL.ProviderCategory = MTNTBL.ProviderCategory;
                                                MTNErrTBL.ToMsg = MTNTBL.ToMsg;
                                                MTNErrTBL.From = MTNTBL.From;
                                                MTNErrTBL.FromName = MTNTBL.FromName;
                                                MTNErrTBL.To = MTNTBL.To;
                                                MTNErrTBL.ToName = MTNTBL.ToName;
                                                MTNErrTBL.InitiatedBy = MTNTBL.InitiatedBy;
                                                MTNErrTBL.OnBehalfOf = MTNTBL.OnBehalfOf;
                                                MTNErrTBL.Amount = MTNTBL.Amount;
                                                MTNErrTBL.Currency1 = MTNTBL.Currency1;
                                                MTNErrTBL.Fee = MTNTBL.Fee;
                                                MTNErrTBL.Currency2 = MTNTBL.Currency2;
                                                MTNErrTBL.Discount = MTNTBL.Discount;
                                                MTNErrTBL.Currency3 = MTNTBL.Currency3;
                                                MTNErrTBL.Promotion = MTNTBL.Promotion;
                                                MTNErrTBL.Currency4 = MTNTBL.Currency4;
                                                MTNErrTBL.Coupon = MTNTBL.Coupon;
                                                MTNErrTBL.Currency5 = MTNTBL.Currency5;
                                                MTNErrTBL.Balance = MTNTBL.Balance;
                                                MTNErrTBL.Currency6 = MTNTBL.Currency6;
                                                MTNErrTBL.Information = MTNTBL.Information;
                                                MTNErrTBL.PullDate = MTNTBL.PullDate;
                                                MTNErrTBL.MatchingStatus = MTNTBL.MatchingStatus;
                                                MTNErrTBL.UserId = MTNTBL.UserId;
                                                MTNErrTBL.MatchingType = MTNTBL.MatchingType;
                                                MTNErrTBL.ErrorMsg = "No trans ref";
                                                MTNErrTBL.FileName = MTNTBL.FileName;
                                                repoMTNTRansErrorRepository.Add(MTNErrTBL);
                                                var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret)
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
                                            LogManager.SaveLog("An error occured in MTNLibrary Line  :" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                                            MTNErrTBL.TransId = MTNTBL.TransId;
                                            MTNErrTBL.OrigRefNo = MTNTBL.TransId;
                                            MTNErrTBL.ExternalTransId = MTNTBL.ExternalTransId;
                                            MTNErrTBL.Date = MTNTBL.Date;
                                            MTNErrTBL.Type = MTNTBL.Type;
                                            MTNErrTBL.ProviderCategory = MTNTBL.ProviderCategory;
                                            MTNErrTBL.ToMsg = MTNTBL.ToMsg;
                                            MTNErrTBL.From = MTNTBL.From;
                                            MTNErrTBL.FromName = MTNTBL.FromName;
                                            MTNErrTBL.To = MTNTBL.To;
                                            MTNErrTBL.ToName = MTNTBL.ToName;
                                            MTNErrTBL.InitiatedBy = MTNTBL.InitiatedBy;
                                            MTNErrTBL.OnBehalfOf = MTNTBL.OnBehalfOf;
                                            MTNErrTBL.Amount = MTNTBL.Amount;
                                            MTNErrTBL.Currency1 = MTNTBL.Currency1;
                                            MTNErrTBL.Fee = MTNTBL.Fee;
                                            MTNErrTBL.Currency2 = MTNTBL.Currency2;
                                            MTNErrTBL.Discount = MTNTBL.Discount;
                                            MTNErrTBL.Currency3 = MTNTBL.Currency3;
                                            MTNErrTBL.Promotion = MTNTBL.Promotion;
                                            MTNErrTBL.Currency4 = MTNTBL.Currency4;
                                            MTNErrTBL.Coupon = MTNTBL.Coupon;
                                            MTNErrTBL.Currency5 = MTNTBL.Currency5;
                                            MTNErrTBL.Balance = MTNTBL.Balance;
                                            MTNErrTBL.Currency6 = MTNTBL.Currency6;
                                            MTNErrTBL.Information = MTNTBL.Information;
                                            MTNErrTBL.PullDate = MTNTBL.PullDate;
                                            MTNErrTBL.MatchingStatus = MTNTBL.MatchingStatus;
                                            MTNErrTBL.UserId = MTNTBL.UserId;
                                            MTNErrTBL.MatchingType = MTNTBL.MatchingType;
                                            MTNErrTBL.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;
                                            MTNErrTBL.FileName = MTNTBL.FileName;
                                            repoMTNTRansErrorRepository.Add(MTNErrTBL);
                                            var ret = unitOfWork.CommitNonAsync(0, null) > 0 ? true : false;
                                            if (ret)
                                            {
                                                continue;
                                            }
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
                                LogManager.SaveLog("Move  MTN File Start");
                                string MvFile = library.MoveFile(FileDirectory + "\\" + fileNamenAndType, ReconType.ProcessedFileDirectory, fileNamenAndType);
                                LogManager.SaveLog("Move File in MTN status: " + MvFile);
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
                                admDataPoollingControlTBL.ReconTypeId = rectype.ReconTypeId;
                                admDataPoollingControlTBL.FileType = "File";
                                admDataPoollingControlTBL.TableName = "MTNTrans";
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
                        LogManager.SaveLog("An error occured in MTNLibrary Line  :" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                    }
                }
                #endregion
                #endregion
                
                var dtSouceCon1 = await repoadmReconDataSourcesRepository.GetAsync(c => c.ReconDSId == dt1);

                #region Source 1
                #region   // Sybase Server below

                if (dtSouceCon1.DatabaseType == "SYBASE")
                {
                    try
                    {
                        LogManager.SaveLog("PullMTNData  for Sybase Start in MTNLibrary");
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
                        string ReconDate = string.Empty;
                        string LastRecordId = string.Empty;
                        var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == ReconType.ReconTypeId && c.TableName == "CBSMTNTrans");

                        if (controlTable != null)
                        {
                            FromDateParam = controlTable.FromDateParam == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.FromDateParam));
                            FromDateParam = "'" + FromDateParam + "'";
                            ToDateParam = controlTable.ToDateParam == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.ToDateParam));
                            ToDateParam = "'" + ToDateParam + "'";
                            LastRecordId = controlTable.LastRecordId;

                            ReconDate = controlTable.LastTransDate == null ? DateTime.Now.ToString() : string.Format("{0:yyyyMMdd}", Convert.ToDateTime(controlTable.LastTransDate));
                            ReconDate = "'" + ReconDate + "'";
                        }
                        else
                        {
                            LastRecordId = "0";
                            FromDateParam = "'20170901'";
                            ToDateParam = "'20170908'";

                            ReconDate = "'20170831'";
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
                                LogManager.SaveLog("An error occured MTNLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                                ds.Load(reader, LoadOption.OverwriteChanges, "ResultsMTN");
                                var returndata = ds.Tables["ResultsMTN"];

                                LogManager.SaveLog("PullMTNData  for Sybase returndata.Rows.Count is: " + returndata.Rows.Count);
                                
                                if (returndata.Rows.Count > 0)
                                {
                                    var CBSMTNTBL = new CBSMTNTran();
                                    var CBSMTNErr = new CBSMTNTransError();

                                    int countTrans = 0;
                                    int count = 0;
                                   
                                    for (int col = 0; col < returndata.Rows.Count; col++)
                                    {
                                        try
                                        {
                                            CBSMTNTBL.AcctNo = returndata.Rows[col][0] == null ? null : returndata.Rows[col][0].ToString();
                                            CBSMTNTBL.Amount = returndata.Rows[col][1] == null ? 0 : Math.Round(Convert.ToDecimal(returndata.Rows[col][1]), 2);
                                            CBSMTNTBL.DebitCredit = returndata.Rows[col][2] == null ? null : returndata.Rows[col][2].ToString();
                                            CBSMTNTBL.Description = returndata.Rows[col][3] == null ? null : returndata.Rows[col][3].ToString();
                                            CBSMTNTBL.TransDate = returndata.Rows[col][4] == null ? (DateTime?)null : Convert.ToDateTime(returndata.Rows[col][4]);
                                            dateTest = CBSMTNTBL.TransDate.ToString();
                                            if (countTrans > 1)
                                            {
                                                MaxTransDate = CBSMTNTBL.TransDate.ToString();

                                                if (Convert.ToDateTime(dateTest) > Convert.ToDateTime(MaxTransDate))
                                                {
                                                    MaxTransDate = dateTest;
                                                }
                                                else
                                                {
                                                    MaxTransDate = MaxTransDate;
                                                }
                                            }
                                            CBSMTNTBL.OriginBranch = returndata.Rows[col][5] == null ? null : returndata.Rows[col][5].ToString();
                                            CBSMTNTBL.PostedBy = returndata.Rows[col][6] == null ? null : returndata.Rows[col][6].ToString();
                                            CBSMTNTBL.Balance = returndata.Rows[col][7] == null ? 0 :  Math.Round(Convert.ToDecimal(returndata.Rows[col][7]), 2);
                                            CBSMTNTBL.Reference = returndata.Rows[col][8] == null ? null : returndata.Rows[col][8].ToString();
                                            CBSMTNTBL.OrigRefNo = CBSMTNTBL.Reference;
                                            CBSMTNTBL.TrfAcctType = returndata.Rows[col][9] == null ? null : returndata.Rows[col][9].ToString();
                                            CBSMTNTBL.TrfAcctNo = returndata.Rows[col][10] == null ? null : returndata.Rows[col][10].ToString();
                                            
                                            CBSMTNTBL.PtId = returndata.Rows[col][11] == null ? null : returndata.Rows[col][11].ToString();
                                            LastRecordId = CBSMTNTBL.PtId;
                                               int revcode ;
                                            CBSMTNTBL.ReversalCode = returndata.Rows[col][12] == null ? (int?)null : int.TryParse(returndata.Rows[col][12].ToString(), out revcode) ? revcode : (int?)null ;
                                            LastRecordId = CBSMTNTBL.PtId;
                                            CBSMTNTBL.PullDate = DateTime.Now;
                                            CBSMTNTBL.MatchingStatus = "N";
                                            CBSMTNTBL.UserId = 1;

                                            countTrans += 1;
                                            if (!string.IsNullOrWhiteSpace(CBSMTNTBL.Reference))
                                            {
                                                var exist = await repoCBSMTNTransRepository.GetAsync(c => c.Reference == CBSMTNTBL.Reference);
                                                var existHis = await repoCBSMTNTransHistoryRepository.GetAsync(c => c.Reference == CBSMTNTBL.Reference);
                                                if (exist != null || existHis != null)
                                                {
                                                    CBSMTNErr.AcctNo = CBSMTNTBL.AcctNo;
                                                    CBSMTNErr.Amount = CBSMTNTBL.Amount;
                                                    CBSMTNErr.DebitCredit = CBSMTNTBL.DebitCredit;
                                                    CBSMTNErr.Description = CBSMTNTBL.Description;
                                                    CBSMTNErr.TransDate = CBSMTNTBL.TransDate;
                                                    CBSMTNErr.OriginBranch = CBSMTNTBL.OriginBranch;
                                                    CBSMTNErr.PostedBy = CBSMTNTBL.PostedBy;
                                                    CBSMTNErr.Balance = CBSMTNTBL.Balance;
                                                    CBSMTNErr.Reference = CBSMTNTBL.Reference;
                                                    CBSMTNErr.OrigRefNo = CBSMTNTBL.Reference;
                                                    CBSMTNErr.TrfAcctType = CBSMTNTBL.TrfAcctType;
                                                    CBSMTNErr.TrfAcctNo = CBSMTNTBL.TrfAcctNo;
                                                    CBSMTNErr.PtId = CBSMTNTBL.PtId;
                                                    CBSMTNErr.ReversalCode = CBSMTNTBL.ReversalCode;
                                                    LastRecordId = CBSMTNTBL.PtId;
                                                    CBSMTNErr.PullDate = DateTime.Now;
                                                    CBSMTNErr.MatchingStatus = "N";
                                                    CBSMTNErr.UserId = 1;
                                                    CBSMTNErr.ErrorMsg = "Duplicate transaction record";

                                                    repoCBSMTNTransErrorRepository.Add(CBSMTNErr);
                                                    var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret1)
                                                    {
                                                        continue;
                                                    }
                                                    continue;
                                                }
                                                else
                                                {
                                                    repoCBSMTNTransRepository.Add(CBSMTNTBL);
                                                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                        count += 1;
                                                        RecordCount = count.ToString();
                                                        continue;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                               
                                                CBSMTNErr.AcctNo = CBSMTNTBL.AcctNo;
                                                CBSMTNErr.Amount = CBSMTNTBL.Amount;
                                                CBSMTNErr.DebitCredit = CBSMTNTBL.DebitCredit;
                                                CBSMTNErr.Description = CBSMTNTBL.Description;
                                                CBSMTNErr.TransDate = CBSMTNTBL.TransDate;
                                                CBSMTNErr.OriginBranch = CBSMTNTBL.OriginBranch;
                                                CBSMTNErr.PostedBy = CBSMTNTBL.PostedBy;
                                                CBSMTNErr.Balance = CBSMTNTBL.Balance;
                                                CBSMTNErr.Reference = CBSMTNTBL.Reference;
                                                CBSMTNErr.OrigRefNo = CBSMTNTBL.Reference;
                                                CBSMTNErr.ReversalCode = CBSMTNTBL.ReversalCode;
                                                CBSMTNErr.TrfAcctType = CBSMTNTBL.TrfAcctType;
                                                CBSMTNErr.TrfAcctNo = CBSMTNTBL.TrfAcctNo;
                                                CBSMTNErr.PtId = CBSMTNTBL.PtId;
                                                LastRecordId = CBSMTNTBL.PtId;
                                                CBSMTNErr.PullDate = DateTime.Now;
                                                CBSMTNErr.MatchingStatus = "N";
                                                CBSMTNErr.UserId = 1;
                                                CBSMTNErr.ErrorMsg = "No transaction reference";

                                                repoCBSMTNTransErrorRepository.Add(CBSMTNErr);
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
                                            LogManager.SaveLog("An error occured MTNLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                                            try
                                            {
                                                CBSMTNErr.AcctNo = CBSMTNTBL.AcctNo;
                                                CBSMTNErr.Amount = CBSMTNTBL.Amount;
                                                CBSMTNErr.DebitCredit = CBSMTNTBL.DebitCredit;
                                                CBSMTNErr.Description = CBSMTNTBL.Description;
                                                CBSMTNErr.TransDate = CBSMTNTBL.TransDate;
                                                CBSMTNErr.OriginBranch = CBSMTNTBL.OriginBranch;
                                                CBSMTNErr.PostedBy = CBSMTNTBL.PostedBy;
                                                CBSMTNErr.Balance = CBSMTNTBL.Balance;
                                                CBSMTNErr.Reference = CBSMTNTBL.Reference;
                                                CBSMTNErr.OrigRefNo = CBSMTNTBL.Reference;
                                                CBSMTNErr.TrfAcctType = CBSMTNTBL.TrfAcctType;
                                                CBSMTNErr.TrfAcctNo = CBSMTNTBL.TrfAcctNo;
                                                CBSMTNErr.ReversalCode = CBSMTNTBL.ReversalCode;
                                                CBSMTNErr.PtId = CBSMTNTBL.PtId;
                                                LastRecordId = CBSMTNTBL.PtId;
                                                CBSMTNErr.PullDate = DateTime.Now;
                                                CBSMTNErr.MatchingStatus = "N";
                                                CBSMTNErr.UserId = 1;
                                                CBSMTNErr.ErrorMsg = ex == null ? ex.InnerException.Message : ex.Message;

                                                repoCBSMTNTransErrorRepository.Add(CBSMTNErr);
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
                                                LogManager.SaveLog("An error occured MTNLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
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
                                        controlTable.PullDate = Convert.ToDateTime(dateee);
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
                                        admDataPoollingControlTBL.TableName = "CBSMTNTrans";
                                        admDataPoollingControlTBL.DateCreated = DateTime.Now;
                                        admDataPoollingControlTBL.UserId = 1;
                                        admDataPoollingControlTBL.LastRecordTimeStamp = DateTime.Now;
                                        admDataPoollingControlTBL.LastRecordId = LastRecordId;
                                        DateTime dtf = new DateTime();
                                        string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
                                        controlTable.LastTransDate = Convert.ToDateTime(dateee);
                                        controlTable.PullDate = DateTime.Now;
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
                                LogManager.SaveLog("An error occured MTNLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                                throw;
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
                        LogManager.SaveLog("An error occured in MTNLibrary Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                       
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
                LogManager.SaveLog("An error occured in MTNLibrary Line : " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
            return string.Empty;
        }




    }
}