using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechReconWindowService.BAL.Helpers;
using TechReconWindowService.DAL;
using TechReconWindowService.DAL.Implementation;
using TechReconWindowService.DAL.Interfaces;
using TechReconWindowService.Repository.Repositories;

namespace TechReconWindowService
{
    public class ReadMT940
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IVostroMT940950TransRepository repoVostroMT940TransRepository;
        private readonly IVostroMT940950TransErrorRepository repoVostroMT940950TransErrorRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly IadmFailledFileRepository repoadmFailledFileRepository;
        private readonly IadmFileProcessedRepository repoadmFileProcessedRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWeindowService/ReadMT940.cs";
        public ReadMT940()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoVostroMT940TransRepository = new VostroMT940950TransRepository(idbfactory);
            repoVostroMT940950TransErrorRepository = new VostroMT940950TransErrorRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoadmFailledFileRepository = new admFailledFileRepository(idbfactory);
            repoadmFileProcessedRepository = new admFileProcessedRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            
        }
        public async Task<int> Generate940950Message(string filename, int ReconTypeId, string FileDirectory, string fileNamenAndType, string FileLastTime)
        {

            var library = new Library();
            LogManager.SaveLog("Generate940950Message Start in ReadMT940");
            string countNoInserted = string.Empty;
              string trackAmt = string.Empty;
            int ReturnNoofInsertRec = 0;
            string MaxTransDate = string.Empty;
            string dateTest = string.Empty;
            int countTrans  = 0;
           try
            {
                string msg2 = File.ReadAllText(filename);
                LogManager.SaveLog("Generate940950Message Start in ReadMT940 1");
                string[] msgList = msg2.Split('$');
                int count = 0;
                MT940Transaction trx = null;
                foreach (string msg940 in msgList)
                {
                    LogManager.SaveLog("Generate940950Message Start in ReadMT940 2, No of Msgs " + msgList.Length);
                    int index = 0;
                    string returntext = string.Empty;
                    var sq = new VostroMT940950Trans();
                   
                    string[] msg = msg940.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                
                    MT940ApplicationHeaderBlock hb = MessageParser.GetMT940ApplicationHeaderBlock(msg940);
                    MTBasicHeaderBlock bhb = MessageParser.GetBasicHeaderBlock(msg940);
                
                    trx = MessageParser.ReadMT940MessageBody(msg);
                    LogManager.SaveLog("No Of record MT940950: " + trx.SeqB.Count);
                if (trx.SeqA.TagTransactionReferenceNumber != null && trx.SeqA.TagAccountIdentification != null && trx.SeqA.TagStatementNumber_SequenceNumber != null && trx.SeqA.TagOpeningBalance != null)
                {
                    LogManager.SaveLog("Generate940950Message Start in ReadMT940 3");
                    
                    foreach (MT940Transaction.SequenceBData tranx in trx.SeqB)
                    {
                        
                        LogManager.SaveLog("Generate940950Message Start in ReadMT940 4");
                       
                        index++;

                        LogManager.SaveLog("hb.MessageType " + hb.MessageType);

                        #region MT940
                        if (hb.MessageType == "940")
                        {
                            sq.ParentMatchingType = "940";
                            sq.ApplicationId = bhb.ApplicationID;
                            sq.FileName = fileNamenAndType;
                            sq.ServiceId = bhb.ServiceID;
                            sq.SenderSwiftCode = hb.MT940SenderSwiftCode;
                            sq.SessionNumber = bhb.SessionNumber;
                            sq.SequenceNumber = bhb.SequenceNumber;
                            sq.Direction = hb.InputDirection;
                            sq.ReceiverSwiftCode = bhb.MT940ReceiverSwiftCode;
                            sq.F20_TransactionRefNo = trx.SeqA.TransactionReferenceNumber_M;
                            sq.F21_RelatedReference = trx.SeqA.RelatedReference_O;
                            sq.F25_AccountNumber = trx.SeqA.AccountIdentification_M;
                            sq.F28C_StatementNo = trx.SeqA.StatementNumber_SequenceNumber_M;
                            sq.F60_OpeningBalance = trx.SeqA.OpeningBalance_M;
                            string OpeningBal = trx.SeqA.OpeningBalance_M;
                            sq.F60_DebitCredit = OpeningBal.Substring(0, 1) == "C" ? "CR" : OpeningBal.Substring(0, 1) == "D" ? "DR" : OpeningBal.Substring(0, 1);
                            sq.F60_Date = OpeningBal.Substring(1, 6);
                            sq.F60_Currency = OpeningBal.Substring(7, 3);
                            int length = OpeningBal.Length - 10;
                            sq.F60_Amount = decimal.Parse(OpeningBal.Substring(10, length).Replace(",", "."));
                            string Statementdate = "60M";
                            if (trx.SeqA.TagOpeningBalance == Statementdate)
                            {
                                sq.F60_LastStatementDate = OpeningBal.Substring(1, 6);
                            }
                            if (tranx.StatementLine_O != null)
                            {
                                int num = 0;
                                sq.F61_StatementLine = tranx.StatementLine_O;
                                string[] messageParts = tranx.StatementLine_O.Split(new string[] { "," }, StringSplitOptions.None);
                                sq.F61_ValueDate = messageParts[0].Substring(0, 6);

                                string ff = sq.F61_ValueDate;
                                string yr = "20" + ff.Substring(0, 2) + "-";
                                string month = ff.Substring(2, 2) + "-";
                                string day = ff.Substring(4, 2);
                                string fulldate = yr + month + day;
                                DateTime dt = new DateTime();
                                sq.TransDate = fulldate == null ? (DateTime?)null : DateTime.TryParse(fulldate, out dt) ? dt : (DateTime?)null;
                                dateTest = sq.TransDate.ToString();
                                if (countTrans > 1)
                                {
                                    MaxTransDate = sq.TransDate.ToString();

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

                                if (Int32.TryParse(messageParts[0].Substring(6, 4), out num))
                                {
                                    sq.F61_EntryDate = messageParts[0].Substring(6, 4);
                                   
                                    sq.F61_DebitCredit = messageParts[0].Substring(10, 1) == "C" ? "CR" : messageParts[0].Substring(10, 1) == "D" ? "DR" : messageParts[0].Substring(10, 1);
	

                                    if (Int32.TryParse(messageParts[1].Substring(0, 2), out num))
                                    {
                                        sq.F61_Amount = decimal.Parse(messageParts[0].Substring(12, (messageParts[0].Length - 12)) + "." + messageParts[1].Substring(0, 2));
                                        sq.F61_TransactionTypeIdCode = messageParts[1].Substring(2, 4);
                                        string[] TranRef = messageParts[1].Split(new string[] { "//" }, StringSplitOptions.None);
                                        sq.F61_CustomerReference = TranRef[0].Substring(6, (TranRef[0].Length - 6));
                                        sq.OrigRefNo = sq.F61_CustomerReference;
                                        if (TranRef.Length > 1)
                                        {
                                            string[] BankRef = TranRef[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                            sq.F61_BankReference = BankRef[0];
                                            if (BankRef.Length > 1)
                                            {
                                                sq.F61_InfoToAccountOwner = BankRef[1];
                                            }
                                        }
                                        sq.MessageType = TranRef[0].Substring(3, 3) == "TRF" ? "103" : TranRef[0].Substring(3, 3) == "100" ? "103" : TranRef[0].Substring(3, 3);
                                    }
                                    else
                                    {
                                        sq.F61_Amount = decimal.Parse(messageParts[0].Substring(12, (messageParts[0].Length - 12)).Replace(",", "."));
                                        sq.F61_TransactionTypeIdCode = messageParts[1].Substring(0, 4);
                                        string[] TranRef = messageParts[1].Split(new string[] { "//" }, StringSplitOptions.None);
                                        sq.F61_CustomerReference = TranRef[0].Substring(4, (TranRef[0].Length - 4));
                                        sq.OrigRefNo = sq.F61_CustomerReference;
                                        if (TranRef.Length > 1)
                                        {
                                            string[] BankRef = TranRef[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                            sq.F61_BankReference = BankRef[0];
                                            if (BankRef.Length > 1)
                                            {
                                                sq.F61_InfoToAccountOwner = BankRef[1];
                                            }
                                        }
                                        sq.MessageType = TranRef[0].Substring(1, 3) == "TRF" ? "103" : TranRef[0].Substring(1, 3) == "100" ? "103" : TranRef[0].Substring(1, 3);
                                    }
                                }
                                else
                                {

                                    sq.F61_DebitCredit = messageParts[0].Substring(6, 1) == "C" ? "CR" : messageParts[0].Substring(6, 1) == "D" ? "DR" : messageParts[0].Substring(6, 1);

                                    if (Int32.TryParse(messageParts[1].Substring(0, 2), out num))
                                    {
                                        sq.F61_Amount = decimal.Parse(messageParts[0].Substring(8, (messageParts[0].Length - 8)) + "." + messageParts[1].Substring(0, 2));
                                        sq.F61_TransactionTypeIdCode = messageParts[1].Substring(2, 4);
                                        string[] TranRef = messageParts[1].Split(new string[] { "//" }, StringSplitOptions.None);
                                        sq.F61_CustomerReference = TranRef[0].Substring(6, (TranRef[0].Length - 6));
                                        sq.OrigRefNo = sq.F61_CustomerReference;
                                        if (TranRef.Length > 1)
                                        {
                                            string[] BankRef = TranRef[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                            sq.F61_BankReference = BankRef[0];
                                            if (BankRef.Length > 1)
                                            {
                                                sq.F61_InfoToAccountOwner = BankRef[1];
                                            }
                                        }
                                        sq.MessageType = TranRef[0].Substring(3, 3) == "TRF" ? "103" : TranRef[0].Substring(3, 3) == "100" ? "103" : TranRef[0].Substring(3, 3);

                                    }
                                    else
                                    {
                                        sq.F61_Amount = decimal.Parse(messageParts[0].Substring(8, (messageParts[0].Length - 8)).Replace(",", "."));
                                        sq.F61_TransactionTypeIdCode = messageParts[1].Substring(0, 4);
                                        string[] TranRef = messageParts[1].Split(new string[] { "//" }, StringSplitOptions.None);
                                        sq.F61_CustomerReference = TranRef[0].Substring(4, (TranRef[0].Length - 4));
                                        sq.OrigRefNo = sq.F61_CustomerReference;
                                        if (TranRef.Length > 1)
                                        {
                                            string[] BankRef = TranRef[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                            sq.F61_BankReference = BankRef[0];
                                            if (BankRef.Length > 1)
                                            {
                                                sq.F61_InfoToAccountOwner = BankRef[1];
                                            }
                                        }
                                        sq.MessageType = TranRef[0].Substring(1, 3) == "TRF" ? "103" : TranRef[0].Substring(1, 3) == "100" ? "103" : TranRef[0].Substring(1, 3);
                                    }
                                }
                            }
                            if (tranx.InformationToAccountOwner_O != null)
                            {
                                sq.F86_InfoToAccountOwner = tranx.InformationToAccountOwner_O;
                            }
                            if (trx.SeqC.ClosingBalance_BookedFunds_M != null)
                            {
                                sq.F62_ClosingBalance = trx.SeqC.ClosingBalance_BookedFunds_M;

                                sq.F62_DebitCredit = trx.SeqC.ClosingBalance_BookedFunds_M.Substring(0, 1) == "C" ? "CR" : trx.SeqC.ClosingBalance_BookedFunds_M.Substring(0, 1) == "D" ? "DR" : trx.SeqC.ClosingBalance_BookedFunds_M.Substring(0, 1);
	
                                sq.F62_LastStatementDate = trx.SeqC.ClosingBalance_BookedFunds_M.Substring(1, 6);
                                sq.F62_Currency = trx.SeqC.ClosingBalance_BookedFunds_M.Substring(7, 3);
                                int lengt = trx.SeqC.ClosingBalance_BookedFunds_M.Length - 10;
                                sq.F62_Amount = decimal.Parse(trx.SeqC.ClosingBalance_BookedFunds_M.Substring(10, lengt).Replace(",", "."));
                            }
                            if (trx.SeqC.ClosingAvailableBalance_AvailableFunds_O != null)
                            {
                                sq.F64_ClosingBalance = trx.SeqC.ClosingAvailableBalance_AvailableFunds_O;
                                sq.F64_DebitCredit = trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Substring(0, 1) == "C" ? "CR" : trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Substring(0, 1) == "D" ? "DR" : trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Substring(0, 1);
                                sq.F64_Date = trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Substring(1, 6);
                                sq.F64_Currency = trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Substring(7, 3);
                                int lengt = trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Length - 10;
                                sq.F64_Amount = decimal.Parse(trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Substring(10, lengt).Replace(",", "."));
                            }
                            if (trx.SeqC.ForwardAvailableBalance_O != null)
                            {
                                sq.F65_ForwardAvailBalance = trx.SeqC.ForwardAvailableBalance_O;
                                sq.F65_DebitCredit = trx.SeqC.ForwardAvailableBalance_O.Substring(0, 1);
                                sq.F65_Date = trx.SeqC.ForwardAvailableBalance_O.Substring(1, 6);
                                sq.F65_Currency = trx.SeqC.ForwardAvailableBalance_O.Substring(7, 3);
                                int lengt = trx.SeqC.ForwardAvailableBalance_O.Length - 10;
                                sq.F65_Amount = decimal.Parse(trx.SeqC.ForwardAvailableBalance_O.Substring(10, lengt).Replace(",", "."));
                            }
                            sq.SeqNo = index;
                            sq.UserId = 1;
                            sq.DateCreated = DateTime.Now;
                            sq.ReconDate = DateTime.Now;
                            sq.MatchingStatus = "N";
                        }
                        #endregion
                        #region MT950
                        else if (hb.MessageType == "950")
                        {
                            sq.ParentMatchingType = "950";
                            sq.FileName = fileNamenAndType;
                            sq.ApplicationId = bhb.ApplicationID;
                            sq.ServiceId = bhb.ServiceID;
                            sq.SenderSwiftCode = hb.MT940SenderSwiftCode;
                            sq.SessionNumber = bhb.SessionNumber;
                            sq.SequenceNumber = bhb.SequenceNumber;
                            sq.Direction = hb.InputDirection;
                            sq.ReceiverSwiftCode = bhb.MT940ReceiverSwiftCode;
                            sq.F20_TransactionRefNo = trx.SeqA.TransactionReferenceNumber_M;
                            //sq.F21_RelatedReference = trx.SeqA.RelatedReference_O;
                            sq.F25_AccountNumber = trx.SeqA.AccountIdentification_M;
                            sq.F28C_StatementNo = trx.SeqA.StatementNumber_SequenceNumber_M;
                            sq.F60_OpeningBalance = trx.SeqA.OpeningBalance_M;
                            string OpeningBal = trx.SeqA.OpeningBalance_M;
                            sq.F60_DebitCredit = OpeningBal.Substring(0, 1) == "C" ? "CR" : OpeningBal.Substring(0, 1) == "D" ? "DR" : OpeningBal.Substring(0, 1);
                            sq.F60_Date = OpeningBal.Substring(1, 6);
                            sq.F60_Currency = OpeningBal.Substring(7, 3);
                            int length = OpeningBal.Length - 10;
                            sq.F60_Amount = decimal.Parse(OpeningBal.Substring(10, length).Replace(",", "."));
                            string Statementdate = "60M";
                            if (trx.SeqA.TagOpeningBalance == Statementdate)
                            {
                                sq.F60_LastStatementDate = OpeningBal.Substring(1, 6);
                            }
                            if (tranx.StatementLine_O != null)
                            {
                                int num = 0;
                                sq.F61_StatementLine = tranx.StatementLine_O;
                                string[] messageParts = tranx.StatementLine_O.Split(new string[] { "," }, StringSplitOptions.None);
                                sq.F61_ValueDate = messageParts[0].Substring(0, 6);
                                string ff = sq.F61_ValueDate;
                                string yr = "20" + ff.Substring(0, 2) + "-";
                                string month = ff.Substring(2, 2) + "-";
                                string day = ff.Substring(4, 2);
                                string fulldate = yr + month + day;
                                DateTime dt = new DateTime();
                                sq.TransDate = fulldate == null ? (DateTime?)null : DateTime.TryParse(fulldate, out dt) ? dt : (DateTime?)null;

                                dateTest = sq.TransDate.ToString();
                                if (countTrans > 1)
                                {
                                    MaxTransDate = sq.TransDate.ToString();

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
                                
                                if (Int32.TryParse(messageParts[0].Substring(7, 1), out num) && messageParts[0].Length > 9)
                                {
                                    if (Int32.TryParse(messageParts[0].Substring(6, 4), out num))
                                    {
                                        sq.F61_EntryDate = messageParts[0].Substring(6, 4);

                                        sq.F61_DebitCredit = messageParts[0].Substring(10, 1) == "C" ? "CR" : messageParts[0].Substring(10, 1) == "D" ? "DR" : messageParts[0].Substring(10, 1);

                                        if (Int32.TryParse(messageParts[1].Substring(0, 2), out num))
                                        {
                                            sq.F61_Amount = decimal.Parse(messageParts[0].Substring(12, (messageParts[0].Length - 12)) + "." + messageParts[1].Substring(0, 2));
                                            sq.F61_TransactionTypeIdCode = messageParts[1].Substring(2, 4);
                                            string[] TranRef = messageParts[1].Split(new string[] { "//" }, StringSplitOptions.None);
                                            sq.F61_CustomerReference = TranRef[0].Substring(6, (TranRef[0].Length - 6));
                                            sq.OrigRefNo = sq.F61_CustomerReference;
                                            string[] BankRef = TranRef[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                            sq.F61_BankReference = BankRef[0];
                                            if (BankRef.Length > 1)
                                            {
                                                sq.F61_InfoToAccountOwner = BankRef[1];
                                            }
                                            sq.MessageType = TranRef[0].Substring(3, 3) == "TRF" ? "103" : TranRef[0].Substring(3, 3) == "100" ? "103" : TranRef[0].Substring(3, 3);

                                        }
                                        else
                                        {
                                            sq.F61_Amount = decimal.Parse(messageParts[0].Substring(12, (messageParts[0].Length - 12)).Replace(",", "."));
                                            sq.F61_TransactionTypeIdCode = messageParts[1].Substring(0, 4);
                                            string[] TranRef = messageParts[1].Split(new string[] { "//" }, StringSplitOptions.None);
                                            sq.F61_CustomerReference = TranRef[0].Substring(4, (TranRef[0].Length - 4));
                                            sq.OrigRefNo = sq.F61_CustomerReference;
                                            string[] BankRef = TranRef[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                            sq.F61_BankReference = BankRef[0];
                                            if (BankRef.Length > 1)
                                            {
                                                sq.F61_InfoToAccountOwner = BankRef[1];
                                            }
                                            sq.MessageType = TranRef[0].Substring(1, 3) == "TRF" ? "103" : TranRef[0].Substring(1, 3) == "100" ? "103" : TranRef[0].Substring(1, 3);
                                        }
                                    }
                                    else
                                    {

                                        sq.F61_DebitCredit = messageParts[0].Substring(6, 1) == "C" ? "CR" : messageParts[0].Substring(6, 1) == "D" ? "DR" : messageParts[0].Substring(6, 1);

                                        if (Int32.TryParse(messageParts[1].Substring(0, 2), out num))
                                        {
                                            sq.F61_Amount = decimal.Parse(messageParts[0].Substring(8, (messageParts[0].Length - 8)) + "." + messageParts[1].Substring(0, 2));
                                            sq.F61_TransactionTypeIdCode = messageParts[1].Substring(2, 4);
                                            string[] TranRef = messageParts[1].Contains("//") ? messageParts[1].Split(new string[] { "//" }, StringSplitOptions.None) :
                                                                messageParts[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                            sq.F61_CustomerReference = TranRef[0].Substring(6, (TranRef[0].Length - 6));
                                            sq.OrigRefNo = sq.F61_CustomerReference;
                                            if (TranRef.Length > 1)
                                            {
                                                if (TranRef[1].Contains("\r\n"))
                                                {
                                                    string[] BankRef = TranRef[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                                    sq.F61_BankReference = BankRef[0];
                                                    if (BankRef.Length > 1)
                                                    {
                                                        sq.F61_InfoToAccountOwner = BankRef[1];
                                                    }
                                                }
                                                else
                                                {
                                                    sq.F61_InfoToAccountOwner = TranRef[1];
                                                }
                                            }
                                            sq.MessageType = TranRef[0].Substring(3, 3) == "TRF" ? "103" : TranRef[0].Substring(3, 3) == "100" ? "103" : TranRef[0].Substring(3, 3);

                                        }
                                        else
                                        {
                                            sq.F61_Amount = decimal.Parse(messageParts[0].Substring(8, (messageParts[0].Length - 8)).Replace(",", "."));
                                            sq.F61_TransactionTypeIdCode = messageParts[1].Substring(0, 4);
                                            string[] TranRef = messageParts[1].Contains("//") ? messageParts[1].Split(new string[] { "//" }, StringSplitOptions.None) :
                                                                messageParts[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                            sq.F61_CustomerReference = TranRef[0].Substring(6, (TranRef[0].Length - 6));
                                            sq.OrigRefNo = sq.F61_CustomerReference;
                                            if (TranRef.Length > 1)
                                            {
                                                if (TranRef[1].Contains("\r\n"))
                                                {
                                                    string[] BankRef = TranRef[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                                    sq.F61_BankReference = BankRef[0];
                                                    if (BankRef.Length > 1)
                                                    {
                                                        sq.F61_InfoToAccountOwner = BankRef[1];
                                                    }
                                                }
                                                else
                                                {
                                                    sq.F61_InfoToAccountOwner = TranRef[1];
                                                }
                                            }
                                            sq.MessageType = TranRef[0].Substring(1, 3) == "TRF" ? "103" : TranRef[0].Substring(1, 3) == "100" ? "103" : TranRef[0].Substring(1, 3);
                                        }
                                    }
                                }
                                else
                                {

                                    sq.F61_DebitCredit = messageParts[0].Substring(6, 1) == "C" ? "CR" : messageParts[0].Substring(6, 1) == "D" ? "DR" : messageParts[0].Substring(6, 1);

                                    if (Int32.TryParse(messageParts[1].Substring(0, 2), out num))
                                    {
                                         //*****************Amount Parser**********************//

                                        trackAmt = messageParts[0].Substring(7, (messageParts[0].Length - 7)) + "." + messageParts[1].Substring(0, 2);
                                        decimal amt = 0;
                                        sq.F61_Amount = decimal.TryParse((messageParts[0].Substring(7, (messageParts[0].Length - 7)) + "." + messageParts[1].Substring(0, 2)), out amt) ? amt : decimal.TryParse((messageParts[0].Substring(8, (messageParts[0].Length - 8)) + "." + messageParts[1].Substring(0, 2)), out amt) ? amt : 0;
                                        
                                        sq.F61_TransactionTypeIdCode = messageParts[1].Substring(2, 4);
                                        string[] TranRef = messageParts[1].Contains("//") ? messageParts[1].Split(new string[] { "//" }, StringSplitOptions.None) :
                                                            messageParts[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                        sq.F61_CustomerReference = TranRef[0].Substring(6, (TranRef[0].Length - 6));
                                        sq.OrigRefNo = sq.F61_CustomerReference;
                                        if (TranRef.Length > 1)
                                        {
                                            if (TranRef[1].Contains("\r\n"))
                                            {
                                                string[] BankRef = TranRef[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                                sq.F61_BankReference = BankRef[0];
                                                if (BankRef.Length > 1)
                                                {
                                                    sq.F61_InfoToAccountOwner = BankRef[1];
                                                }
                                            }
                                            else
                                            {
                                                sq.F61_InfoToAccountOwner = TranRef[1];
                                            }
                                        }
                                        sq.MessageType = TranRef[0].Substring(3, 3) == "TRF" ? "103" : TranRef[0].Substring(3, 3) == "100" ? "103" : TranRef[0].Substring(3, 3);

                                    }
                                    else
                                    {
                                        decimal amt = 0;
                                        sq.F61_Amount = decimal.TryParse((messageParts[0].Substring(7, (messageParts[0].Length - 7)).Replace(",", ".")), out amt) ? amt :
                                            decimal.TryParse((messageParts[0].Substring(8, (messageParts[0].Length - 8)).Replace(",", ".")), out amt) ? amt : 0;
                                        sq.F61_TransactionTypeIdCode = messageParts[1].Substring(0, 4);
                                        string[] TranRef = messageParts[1].Contains("//") ? messageParts[1].Split(new string[] { "//" }, StringSplitOptions.None) :
                                                            messageParts[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                        sq.F61_CustomerReference = TranRef[0].Substring(6, (TranRef[0].Length - 6));
                                        sq.OrigRefNo = sq.F61_CustomerReference;
                                        if (TranRef.Length > 1)
                                        {
                                            if (TranRef[1].Contains("\r\n"))
                                            {
                                                string[] BankRef = TranRef[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                                sq.F61_BankReference = BankRef[0];
                                                if (BankRef.Length > 1)
                                                {
                                                    sq.F61_InfoToAccountOwner = BankRef[1];
                                                }
                                            }
                                            else
                                            {
                                                sq.F61_InfoToAccountOwner = TranRef[1];
                                            }
                                        }
                                        sq.MessageType = TranRef[0].Substring(1, 3) == "TRF" ? "103" : TranRef[0].Substring(1, 3) == "100" ? "103" : TranRef[0].Substring(1, 3);
                                    }

                                }

                                if (trx.SeqC.ClosingBalance_BookedFunds_M != null)
                                {
                                    sq.F62_ClosingBalance = trx.SeqC.ClosingBalance_BookedFunds_M;
                                    sq.F62_DebitCredit = trx.SeqC.ClosingBalance_BookedFunds_M.Substring(0, 1) == "C" ? "CR" : trx.SeqC.ClosingBalance_BookedFunds_M.Substring(0, 1) == "D" ? "DR" : trx.SeqC.ClosingBalance_BookedFunds_M.Substring(0, 1);
                                    sq.F62_LastStatementDate = trx.SeqC.ClosingBalance_BookedFunds_M.Substring(1, 6);
                                    sq.F62_Currency = trx.SeqC.ClosingBalance_BookedFunds_M.Substring(7, 3);
                                    int lengt = trx.SeqC.ClosingBalance_BookedFunds_M.Length - 10;
                                    sq.F62_Amount = decimal.Parse(trx.SeqC.ClosingBalance_BookedFunds_M.Substring(10, lengt).Replace(",", "."));
                                }
                                if (trx.SeqC.ClosingAvailableBalance_AvailableFunds_O != null)
                                {
                                    sq.F64_ClosingBalance = trx.SeqC.ClosingAvailableBalance_AvailableFunds_O;

                                    sq.F64_DebitCredit = trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Substring(0, 1) == "C" ? "CR" : trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Substring(0, 1) == "D" ? "DR" : trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Substring(0, 1);

                                    sq.F64_Date = trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Substring(1, 6);
                                    sq.F64_Currency = trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Substring(7, 3);
                                    int lengt = trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Length - 10;
                                    sq.F64_Amount = decimal.Parse(trx.SeqC.ClosingAvailableBalance_AvailableFunds_O.Substring(10, lengt).Replace(",", "."));
                                }

                                sq.SeqNo = index;
                                sq.UserId = 1;
                                sq.DateCreated = DateTime.Now;
                                sq.ReconDate = DateTime.Now;
                                sq.MatchingStatus = "N";
                            }
                        }
                        #endregion 
                        
                        if (!string.IsNullOrWhiteSpace(sq.F20_TransactionRefNo))
                        {
                            //f.F61_CustomerReference, f.F61_Amount, f.ReceiverSwiftCode,f.F20_TransactionRefNo
                            var exist = await  repoVostroMT940TransRepository.GetAsync(c => c.F61_CustomerReference == sq.F61_CustomerReference && c.F61_Amount == sq.F61_Amount && c.ReceiverSwiftCode == sq.ReceiverSwiftCode && c.F20_TransactionRefNo == sq.F20_TransactionRefNo);
                           
                            if (exist != null)
                            {
                                var VostroMT940TransTBLErr = new VostroMT940950TransError();

                                VostroMT940TransTBLErr.ApplicationId = sq.ApplicationId;
                                VostroMT940TransTBLErr.ServiceId = sq.ServiceId;
                                VostroMT940TransTBLErr.SenderSwiftCode = sq.SenderSwiftCode;
                                VostroMT940TransTBLErr.SessionNumber = sq.SessionNumber;
                                VostroMT940TransTBLErr.SequenceNumber = sq.SequenceNumber;
                                VostroMT940TransTBLErr.Direction = sq.Direction;
                                VostroMT940TransTBLErr.MessageType = sq.MessageType;
                                VostroMT940TransTBLErr.ReceiverSwiftCode = sq.ReceiverSwiftCode;
                                VostroMT940TransTBLErr.F20_TransactionRefNo = sq.F20_TransactionRefNo;
                                VostroMT940TransTBLErr.F21_RelatedReference = sq.F21_RelatedReference;
                                VostroMT940TransTBLErr.F25_AccountNumber = sq.F25_AccountNumber;
                                VostroMT940TransTBLErr.F28C_StatementNo = sq.F28C_StatementNo;
                                VostroMT940TransTBLErr.F60_OpeningBalance = sq.F60_OpeningBalance;
                                VostroMT940TransTBLErr.F60_DebitCredit = sq.F60_DebitCredit;
                                VostroMT940TransTBLErr.F60_Date = sq.F60_Date;
                                VostroMT940TransTBLErr.F60_Currency = sq.F60_Currency;
                                VostroMT940TransTBLErr.F60_Amount = sq.F60_Amount;
                                VostroMT940TransTBLErr.F60_LastStatementDate = sq.F60_LastStatementDate;
                                VostroMT940TransTBLErr.F60_CurrentStatementDate = sq.F60_CurrentStatementDate;
                                VostroMT940TransTBLErr.F61_StatementLine = sq.F61_StatementLine;
                                VostroMT940TransTBLErr.F61_ValueDate = sq.F61_ValueDate;
                                string ff = sq.F61_ValueDate;
                                string yr = "20" + ff.Substring(0, 2) + "-";
                                string month = ff.Substring(2, 2) + "-";
                                string day = ff.Substring(4, 2);
                                string fulldate = yr + month + day;
                                DateTime dt = new DateTime();
                                VostroMT940TransTBLErr.TransDate = fulldate == null ? (DateTime?)null : DateTime.TryParse(fulldate, out dt) ? dt : (DateTime?)null;
                                VostroMT940TransTBLErr.F61_EntryDate = sq.F61_EntryDate;
                                VostroMT940TransTBLErr.F61_DebitCredit = sq.F61_DebitCredit;
                                VostroMT940TransTBLErr.F61_Currency = sq.F61_Currency;
                                VostroMT940TransTBLErr.F61_Amount = sq.F61_Amount;
                                VostroMT940TransTBLErr.F61_TransactionTypeIdCode = sq.F61_TransactionTypeIdCode;
                                VostroMT940TransTBLErr.F61_CustomerReference = sq.F61_CustomerReference;
                                VostroMT940TransTBLErr.OrigRefNo = sq.F61_CustomerReference;
                                VostroMT940TransTBLErr.F61_BankReference = sq.F61_BankReference;
                                VostroMT940TransTBLErr.F61_SupplementaryDetails = sq.F61_SupplementaryDetails;
                                VostroMT940TransTBLErr.F61_InfoToAccountOwner = sq.F61_InfoToAccountOwner;
                                VostroMT940TransTBLErr.F62_ClosingBalance = sq.F62_ClosingBalance;
                                VostroMT940TransTBLErr.F62_DebitCredit = sq.F62_DebitCredit;
                                VostroMT940TransTBLErr.F62_LastStatementDate = sq.F62_LastStatementDate;
                                VostroMT940TransTBLErr.F62_Currency = sq.F62_Currency;
                                VostroMT940TransTBLErr.F62_Amount = sq.F62_Amount;
                                VostroMT940TransTBLErr.F64_ClosingBalance = sq.F64_ClosingBalance;
                                VostroMT940TransTBLErr.F64_DebitCredit = sq.F64_DebitCredit;
                                VostroMT940TransTBLErr.F64_Date = sq.F64_Date;
                                VostroMT940TransTBLErr.F64_Currency = sq.F64_Currency;
                                VostroMT940TransTBLErr.F64_Amount = sq.F64_Amount;
                                VostroMT940TransTBLErr.F65_ForwardAvailBalance = sq.F65_ForwardAvailBalance;
                                VostroMT940TransTBLErr.F65_DebitCredit = sq.F65_DebitCredit;
                                VostroMT940TransTBLErr.F65_Date = sq.F65_Date;
                                VostroMT940TransTBLErr.F65_Currency = sq.F65_Currency;
                                VostroMT940TransTBLErr.F65_Amount = sq.F65_Amount;
                                VostroMT940TransTBLErr.F86_InfoToAccountOwner = sq.F86_InfoToAccountOwner;
                                VostroMT940TransTBLErr.SeqNo = sq.SeqNo;
                                VostroMT940TransTBLErr.Status = sq.Status;
                                VostroMT940TransTBLErr.DateCreated = sq.DateCreated;
                                VostroMT940TransTBLErr.UserId = sq.UserId;
                                VostroMT940TransTBLErr.ReconDate = sq.ReconDate;
                                VostroMT940TransTBLErr.MatchingStatus = sq.MatchingStatus;
                                VostroMT940TransTBLErr.MatchingType = "SYSTEM";

                                repoVostroMT940950TransErrorRepository.Add(VostroMT940TransTBLErr);

                                var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                if (ret1)
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                sq.PullDate = DateTime.Now;
                                repoVostroMT940TransRepository.Add(sq);
                                var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                if (ret)
                                {
                                    count += 1;
                                    countNoInserted = count.ToString();
                                    LogManager.SaveLog("No record insterted MT940950: " + countNoInserted);
                                    continue;
                                }
                            }
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
                LogManager.SaveLog("An error occured in Line ReadMT940950: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);

                LogManager.SaveLog("An error occured in Line ReadMT940950: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr + "FAiled at this No when inserting Record Line ReadMT940950: " + countNoInserted + "amt :" + trackAmt);
                throw;
            }

           if (!string.IsNullOrWhiteSpace(countNoInserted))
           {
               ReturnNoofInsertRec = Convert.ToInt32(countNoInserted);
           }

           var controlTable = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == ReconTypeId && c.TableName == "VostroMT940950Trans");

          
           //Update control table
           if (controlTable != null)
           {
               controlTable.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
               DateTime dtf = new DateTime();
               string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
               controlTable.LastTransDate = Convert.ToDateTime(dateee);
               controlTable.PullDate = DateTime.Now;
               controlTable.RecordsCount = Convert.ToInt32(ReturnNoofInsertRec);
               repoadmDataPoollingControlRepository.Update(controlTable);
               var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
               if (ret)
               {
               }
           }
           else
           {
               var admDataPoollingControlTBL = new admDataPullingControl();
               admDataPoollingControlTBL.ReconTypeId = ReconTypeId;
               admDataPoollingControlTBL.FileType = "File";
               admDataPoollingControlTBL.TableName = "VostroMT940950Trans";
               admDataPoollingControlTBL.DateCreated = DateTime.Now;
               admDataPoollingControlTBL.UserId = 1;
               admDataPoollingControlTBL.LastRecordTimeStamp = Convert.ToDateTime(FileLastTime);
               admDataPoollingControlTBL.ReconLevel = 1;
               DateTime dtf = new DateTime();
               string dateee = MaxTransDate == null ? string.Empty : DateTime.TryParse(MaxTransDate, out dtf) ? string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(MaxTransDate)) : DateTime.Now.ToString();
               admDataPoollingControlTBL.LastTransDate = Convert.ToDateTime(dateee);
               admDataPoollingControlTBL.PullDate = DateTime.Now;
               admDataPoollingControlTBL.RecordsCount = Convert.ToInt32(ReturnNoofInsertRec);
               repoadmDataPoollingControlRepository.Add(admDataPoollingControlTBL);
               var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
               if (ret)
               {
               }
           }


            return ReturnNoofInsertRec;
        }
    }
}
