using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechReconWindowService.BAL.Helpers;
using TechReconWindowService.DAL;
using TechReconWindowService.DAL.Implementation;
using TechReconWindowService.DAL.Interfaces;
using TechReconWindowService.Repository.Repositories;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Configuration;

namespace TechReconWindowService.BAL.ReconciliationService
{
    public class MT940950ReconModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IBatchCounterRepository repoBatchCounterRepository;
        private string _classname = "TechReconWindowService.BAL/ReconciliationService/MT940950ReconModel.cs";
        private string _lineErrorNumber;
        private string _methodname;
       
        private ReturnValues retVal;

        private readonly ICBSNostroVostroTransRepository repoCBSNostroVostroTransRepository;
        private readonly ICBSNostroVostroTransHistoryRepository repoCBSNostroVostroTransHistoryRepository;
        private readonly IVostroMT940950TransRepository repoVostroMT940950TransRepository;
        private readonly IVostroMT940950TransHistoryRepository repoVostroMT940950TransHistoryRepository;
        private readonly IadmMactingCriteriaRepository repoadmMactingCriteriaRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly IadmMatchingTypeRepository repoadmMatchingTypeRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        public MT940950ReconModel()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoBatchCounterRepository = new BatchCounterRepository(idbfactory);
            repoCBSNostroVostroTransRepository = new CBSNostroVostroTransRepository(idbfactory);
            repoCBSNostroVostroTransHistoryRepository = new CBSNostroVostroTransHistoryRepository(idbfactory);
            repoVostroMT940950TransRepository = new VostroMT940950TransRepository(idbfactory);
            repoVostroMT940950TransHistoryRepository = new VostroMT940950TransHistoryRepository(idbfactory);
            repoadmMactingCriteriaRepository = new admMactingCriteriaRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoadmMatchingTypeRepository = new admMatchingTypeRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
        }

        #region
       
        
        
        // VOSTROMT940950Trans           CBSNostroVostro
        //F61_CustomerReference = [Reference]
        //[F25_AccountNumber] = CBS VostroAcctNo
        //[F60_DebitCredit] = DebitCredit,       --DR MUST MATCH WITH CR in source1
        //[F61_Currency] = [Currency]          --Currency
        //[F61_Amount] = Amount  NULL,     --Amount
        

        public async Task<ReturnValues> MatchNostroVostro()
        {
            LogManager.SaveLog("MatchNostroVostro Start");
            string ReconTypeName = string.Empty;
            int ConsortiumId = 0;
            string listcoluminTable1Source1 = string.Empty;
            string listcoluminTable2Source2 = string.Empty;
            try
            {
                var batchCounter = new BatchCounter();
                var procesBatchNo = await repoBatchCounterRepository.GetManyAsync(m => m.ItbId > 0);

                double procesBatchNokk = 0;
                if (procesBatchNo.Count() > 0)
                {
                    procesBatchNokk = Convert.ToDouble(procesBatchNo.LastOrDefault().ItbId) == null ? 0 : Convert.ToDouble(procesBatchNo.LastOrDefault().ItbId);
                    procesBatchNokk += 1;
                }
                else
                {
                    procesBatchNokk += 1;
                }
                batchCounter.BatchProcessNo = procesBatchNokk.ToString();
                batchCounter.DateTime = DateTime.Now;
                repoBatchCounterRepository.Add(batchCounter);
                var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                if (ret)
                {
                }

                var matchingCriteria = await repoadmMactingCriteriaRepository.GetManyAsync(c => c.ItbId > 0);

                foreach (var rTypeName in matchingCriteria.ToList())
                {
                    var recName = repoReconTypeRepository.Get(c => c.ReconTypeId == rTypeName.ReconTypeId);
                    if (recName.WSReconName == "NostroVostro")
                    {
                        ConsortiumId = recName.ReconTypeId;
                        break;
                    }
                }

                var ConsortiumMachingList = await repoadmMactingCriteriaRepository.GetManyAsync(c => c.ReconTypeId == ConsortiumId);

                foreach (var m in ConsortiumMachingList)
                {
                    //Unique Column below
                    string columnName = m.Source1ColumnName;
                    //List of Column below
                    foreach (var listofOtherColumn in ConsortiumMachingList)
                    {
                        listcoluminTable1Source1 += listofOtherColumn.Source1ColumnName + ";";
                        listcoluminTable2Source2 += listofOtherColumn.Source2ColumnName + ";";

                    }
                    //Update Macthing Criteria
                    var updateMachingCriteria = await repoadmMactingCriteriaRepository.GetManyAsync(c => c.MatchingTypeId == m.MatchingTypeId);

                    if (updateMachingCriteria != null)
                    {
                        foreach (var up in updateMachingCriteria)
                        {
                            up.ReconciliationBatchNo = procesBatchNokk.ToString();
                            up.ReconciliationStartTime = DateTime.Now;
                            up.ReconciliationStatus = "Start";
                            up.ReconcilledBy = "1";
                            repoadmMactingCriteriaRepository.Update(up);
                            ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                            if (ret)
                            {
                            }
                        }
                    }
                    var getMatchType = await repoadmMatchingTypeRepository.GetAsync(c => c.MatchingTypeId == m.MatchingTypeId);
                    string Itemtype = getMatchType.Description;
                    //For  One Item To One Item
                    #region One To One

                    if (Itemtype == "One Item To One Item")
                    {
                        var CBS = await repoCBSNostroVostroTransRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());

                        foreach (var p in CBS)
                        {
                            var CBS1 =  await repoCBSNostroVostroTransRepository.GetAsync(q => q.Reference == p.Reference && q.MatchingStatus == "N");
                            var Vostro = await repoVostroMT940950TransRepository.GetAsync(h => h.F61_CustomerReference == p.Reference && h.MatchingStatus == "N");

                            if (CBS1 != null && Vostro != null)
                            {
                                if ((CBS1.Reference == Vostro.F61_CustomerReference) && (CBS1.DebitCredit != Vostro.F61_DebitCredit) && (CBS1.Amount == Vostro.F61_Amount) && (CBS1.VostroAcctNo == Vostro.F25_AccountNumber))
                                {
                                    LogManager.SaveLog("NostroVostro Both are matched CBS itbId: " + CBS1.ItbId + "Vostro ItbId :" + Vostro.ItbId);

                                    CBS1.ReconDate = DateTime.Now;
                                    CBS1.MatchingStatus = "M";
                                    CBS1.MatchingType = "SYSTEM";
                                    repoCBSNostroVostroTransRepository.Update(CBS1);
                                    ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                    if (ret)
                                    {
                                    }
                                    Vostro.ReconDate = DateTime.Now;
                                    Vostro.MatchingStatus = "M";
                                    Vostro.MatchingType = "SYSTEM";
                                    repoVostroMT940950TransRepository.Update(Vostro);
                                    ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                    if (ret)
                                    {
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    var rectype = repoReconTypeRepository.Get(c => c.WSReconName == "NostroVostro");
                    var controlTablecbs = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSNostroTrans");
                    if (controlTablecbs != null)
                    {
                        controlTablecbs.LastTimeMatched = DateTime.Now;
                        repoadmDataPoollingControlRepository.Update(controlTablecbs);
                        var retww = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (retww)
                        {
                        }
                    }
                    var controlTablecon = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "VostroMT940950Trans");
                    if (controlTablecon != null)
                    {
                        controlTablecon.LastTimeMatched = DateTime.Now;
                        repoadmDataPoollingControlRepository.Update(controlTablecon);
                        var retww = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (retww)
                        {

                        }
                    }
                    if (updateMachingCriteria != null)
                    {
                        foreach (var up in updateMachingCriteria)
                        {
                            up.ReconciliationBatchNo = procesBatchNokk.ToString();
                            up.ReconciliationStartTime = DateTime.Now;
                            up.ReconciliationStatus = "FINISHED";
                            up.ReconcilledBy = "1";
                            repoadmMactingCriteriaRepository.Update(up);
                            ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
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
                LogManager.SaveLog("An error occured MatchNostroVostro Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
            }

            LogManager.SaveLog("MatchNostroVostro End");
            return retVal;
        }
        public async Task<ReturnValues> MoveNotroVostrHistory()
        {
            LogManager.SaveLog("MoveNotroVostrHistory Start");
            var CBSNostroTransHistoryTBL = new CBSNostroTransHistory();
            var VostroMT940950TransHistoryTBL = new VostroMT940950TransHistory();
            try
            {
                var CBSNosVostro = await repoCBSNostroVostroTransRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var p in CBSNosVostro)
                {
                    CBSNostroTransHistoryTBL.ItbId = p.ItbId ;
                    CBSNostroTransHistoryTBL.AcctNo = p.AcctNo ;
                    CBSNostroTransHistoryTBL.AcctType = p.AcctType ;
                    CBSNostroTransHistoryTBL.TransDate = p.TransDate ;
                    CBSNostroTransHistoryTBL.Amount = p.Amount ;
                    CBSNostroTransHistoryTBL.Description = p.Description ;
                    CBSNostroTransHistoryTBL.Reference = p.Reference ;
                    CBSNostroTransHistoryTBL.DebitCredit = p.DebitCredit ;
                    CBSNostroTransHistoryTBL.OriginatingBranch = p.OriginatingBranch ;
                    CBSNostroTransHistoryTBL.PostedBy = p.PostedBy ;
                    CBSNostroTransHistoryTBL.Currency = p.Currency ;
                    CBSNostroTransHistoryTBL.PtId = p.PtId ;
                    CBSNostroTransHistoryTBL.VostroAcctNo = p.VostroAcctNo ;
                    CBSNostroTransHistoryTBL.MatchingStatus = p.MatchingStatus ;
                    CBSNostroTransHistoryTBL.ReconDate = p.ReconDate ;
                    CBSNostroTransHistoryTBL.PullDate = p.PullDate;
                    CBSNostroTransHistoryTBL.UserId = p.UserId ;
                    CBSNostroTransHistoryTBL.MatchingType = p.MatchingType ;
                    CBSNostroTransHistoryTBL.DateMoved = DateTime.Now ;
                    CBSNostroTransHistoryTBL.OrigRefNo = p.OrigRefNo;
                    CBSNostroTransHistoryTBL.ReversalCode = p.ReversalCode;

                    repoCBSNostroVostroTransHistoryRepository.Add(CBSNostroTransHistoryTBL);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }
                    //Delete 
                    var del = await repoCBSNostroVostroTransRepository.GetAsync(d => d.ItbId == p.ItbId);
                    if (del != null)
                    {
                        repoCBSNostroVostroTransRepository.Delete(del);
                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                        }
                    }
                }

                var vostro940950 = await repoVostroMT940950TransRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var l in vostro940950)
                {
                    VostroMT940950TransHistoryTBL.ItbId = l.ItbId ;
                    VostroMT940950TransHistoryTBL.ApplicationId = l.ApplicationId ;
                    VostroMT940950TransHistoryTBL.ServiceId = l.ServiceId ;
                    VostroMT940950TransHistoryTBL.SenderSwiftCode = l.SenderSwiftCode ;
                    VostroMT940950TransHistoryTBL.SessionNumber = l.SessionNumber ;
                    VostroMT940950TransHistoryTBL.SequenceNumber = l.SequenceNumber ;
                    VostroMT940950TransHistoryTBL.Direction = l.Direction ;
                    VostroMT940950TransHistoryTBL.MessageType = l.MessageType ;
                    VostroMT940950TransHistoryTBL.ReceiverSwiftCode = l.ReceiverSwiftCode ;
                    VostroMT940950TransHistoryTBL.F20_TransactionRefNo = l.F20_TransactionRefNo ;
                    VostroMT940950TransHistoryTBL.F21_RelatedReference = l.F21_RelatedReference ;
                    VostroMT940950TransHistoryTBL.F25_AccountNumber = l.F25_AccountNumber ;
                    VostroMT940950TransHistoryTBL.F28C_StatementNo = l.F28C_StatementNo ;
                    VostroMT940950TransHistoryTBL.F60_OpeningBalance = l.F60_OpeningBalance ;
                    VostroMT940950TransHistoryTBL.F60_DebitCredit = l.F60_DebitCredit ;
                    VostroMT940950TransHistoryTBL.F60_Date = l.F60_Date ;
                    VostroMT940950TransHistoryTBL.F60_Currency = l.F60_Currency ;
                    VostroMT940950TransHistoryTBL.F60_Amount = l.F60_Amount ;
                    VostroMT940950TransHistoryTBL.F60_LastStatementDate = l.F60_LastStatementDate ;
                    VostroMT940950TransHistoryTBL.F60_CurrentStatementDate = l.F60_CurrentStatementDate ;
                    VostroMT940950TransHistoryTBL.F61_StatementLine = l.F61_StatementLine ;
                    VostroMT940950TransHistoryTBL.F61_ValueDate = l.F61_ValueDate ;
                    VostroMT940950TransHistoryTBL.F61_EntryDate = l.F61_EntryDate ;
                    VostroMT940950TransHistoryTBL.F61_DebitCredit = l.F61_DebitCredit ;
                    VostroMT940950TransHistoryTBL.F61_Currency = l.F61_Currency ;
                    VostroMT940950TransHistoryTBL.F61_Amount = l.F61_Amount ;
                    VostroMT940950TransHistoryTBL.F61_TransactionTypeIdCode = l.F61_TransactionTypeIdCode ;
                    VostroMT940950TransHistoryTBL.F61_CustomerReference = l.F61_CustomerReference ;
                    VostroMT940950TransHistoryTBL.F61_BankReference = l.F61_BankReference ;
                    VostroMT940950TransHistoryTBL.F61_SupplementaryDetails = l.F61_SupplementaryDetails ;
                    VostroMT940950TransHistoryTBL.F61_InfoToAccountOwner = l.F61_InfoToAccountOwner ;
                    VostroMT940950TransHistoryTBL.F62_ClosingBalance = l.F62_ClosingBalance ;
                    VostroMT940950TransHistoryTBL.F62_DebitCredit = l.F62_DebitCredit ;
                    VostroMT940950TransHistoryTBL.F62_LastStatementDate = l.F62_LastStatementDate ;
                    VostroMT940950TransHistoryTBL.F62_Currency = l.F62_Currency ;
                    VostroMT940950TransHistoryTBL.F62_Amount = l.F62_Amount ;
                    VostroMT940950TransHistoryTBL.F64_ClosingBalance = l.F64_ClosingBalance ;
                    VostroMT940950TransHistoryTBL.F64_DebitCredit = l.F64_DebitCredit ;
                    VostroMT940950TransHistoryTBL.F64_Date = l.F64_Date ;
                    VostroMT940950TransHistoryTBL.F64_Currency = l.F64_Currency ;
                    VostroMT940950TransHistoryTBL.F64_Amount = l.F64_Amount ;
                    VostroMT940950TransHistoryTBL.F65_ForwardAvailBalance = l.F65_ForwardAvailBalance ;
                    VostroMT940950TransHistoryTBL.F65_DebitCredit = l.F65_DebitCredit ;
                    VostroMT940950TransHistoryTBL.F65_Date = l.F65_Date ;
                    VostroMT940950TransHistoryTBL.F65_Currency = l.F65_Currency ;
                    VostroMT940950TransHistoryTBL.F65_Amount = l.F65_Amount ;
                    VostroMT940950TransHistoryTBL.F86_InfoToAccountOwner = l.F86_InfoToAccountOwner ;
                    VostroMT940950TransHistoryTBL.SeqNo = l.SeqNo ;
                    VostroMT940950TransHistoryTBL.Status = l.Status ;
                    VostroMT940950TransHistoryTBL.DateCreated = l.DateCreated ;
                    VostroMT940950TransHistoryTBL.UserId = l.UserId ;
                    VostroMT940950TransHistoryTBL.ReconDate = l.ReconDate ;
                    VostroMT940950TransHistoryTBL.MatchingStatus = l.MatchingStatus ;
                    VostroMT940950TransHistoryTBL.MatchingType = l.MatchingType ;
                    VostroMT940950TransHistoryTBL.TransDate = l.TransDate ;
                    VostroMT940950TransHistoryTBL.ParentMatchingType = l.ParentMatchingType ;
                    VostroMT940950TransHistoryTBL.FileName = l.FileName ;
                    VostroMT940950TransHistoryTBL.OrigRefNo = l.OrigRefNo;
                    VostroMT940950TransHistoryTBL.DateMoved = DateTime.Now;
                    VostroMT940950TransHistoryTBL.PullDate = l.PullDate;

                    repoVostroMT940950TransHistoryRepository.Add(VostroMT940950TransHistoryTBL);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }
                    //delete
                    var del = await repoVostroMT940950TransRepository.GetAsync(d => d.ItbId == l.ItbId);
                    if (del != null)
                    {
                        repoVostroMT940950TransRepository.Delete(del);
                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
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
                LogManager.SaveLog("An error occured CBSTranAndConsortiumTransHistory Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + " = p. ; ERROR: " + exErr);
                return retVal;
            }
            LogManager.SaveLog("MoveNotroVostrHistory End");
            return retVal;
        }
        
        
        #endregion

      
    }
}