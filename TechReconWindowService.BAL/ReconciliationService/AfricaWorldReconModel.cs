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
    public class AfricaWorldReconModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmMactingCriteriaRepository repoadmMactingCriteriaRepository;
        private readonly IadmMatchingTableRepository repoadmMatchingTableRepository;
        private readonly IadmMatchingTypeRepository repoadmMatchingTypeRepository;
        private readonly IPostillionTransationTestRepository repoPostillionTransationTestRepository;
        private readonly IBatchCounterRepository repoBatchCounterRepository;
        private string _classname = "TechReconWindowService.BAL/ReconciliationService/AfricaWorldReconModel.cs";
        private string _lineErrorNumber;
        private string _methodname;
        private ReturnValues retVal;
        private readonly ICBSConsortiumTransactionRepository repoCBSConsortiumTransactionRepository;
        private readonly IConsortiumTransactionRepository repoConsortiumTransactionRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly ICBSAfricaWorldTransactionRepository repoCBSAfricaWorldTransactionRepository;
        private readonly IAfricaWorldTransactionRepository repoAfricaWorldTransactionRepository;
        private readonly ICBSAfricaWorldTransHistoryRepository repoCBSAfricaWorldTransHistoryRepository;
        private readonly IAfricaWorldTransHistoryRepository repoAfricaWorldTransHistoryRepository;

        public AfricaWorldReconModel()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmMactingCriteriaRepository = new admMactingCriteriaRepository(idbfactory);
            repoadmMatchingTypeRepository = new admMatchingTypeRepository(idbfactory);
            repoPostillionTransationTestRepository = new PostillionTransationTestRepository(idbfactory);
            repoBatchCounterRepository = new BatchCounterRepository(idbfactory);
            retVal = new ReturnValues();
            repoCBSConsortiumTransactionRepository = new CBSConsortiumTransactionRepository(idbfactory);
            repoConsortiumTransactionRepository = new ConsortiumTransactionRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoCBSAfricaWorldTransactionRepository = new CBSAfricaWorldTransactionRepository(idbfactory);
            repoAfricaWorldTransactionRepository = new AfricaWorldTransactionRepository(idbfactory);
            repoCBSAfricaWorldTransHistoryRepository = new CBSAfricaWorldTransHistoryRepository(idbfactory);
            repoAfricaWorldTransHistoryRepository = new AfricaWorldTransHistoryRepository(idbfactory);
        }

        #region Africa World
        public async Task<ReturnValues> MatchAfricaWorld()
        {
            LogManager.SaveLog("MatchAfricaWorld Start in AfricaWorldReconModel");
            string ReconTypeName = string.Empty;
            int ReconTypeId = 0;
            string listcoluminTable1Source1 = string.Empty;
            string listcoluminTable2Source2 = string.Empty;
            try
            {
                var batchCounter = new BatchCounter();
                var CBSConsortiumTransactionTBL = new CBSConsortiumTransaction();
                var ConsortiumTransactionTBL = new ConsortiumTransaction();
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
                    if (recName.WSReconName == "AfricaWorld")
                    {
                        ReconTypeId = recName.ReconTypeId;
                        break;
                    }
                }

                var ConsortiumMachingList = await  repoadmMactingCriteriaRepository.GetManyAsync(c => c.ReconTypeId == ReconTypeId);

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
                        var CBSAfriWorld = await repoCBSAfricaWorldTransactionRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());

                        foreach (var p in CBSAfriWorld)
                        {
                            var CBSAfri = await repoCBSAfricaWorldTransactionRepository.GetAsync(q => q.Reference == p.Reference && q.MatchingStatus == "N");
                            var Afr = await repoAfricaWorldTransactionRepository.GetAsync(h => h.ConfirmationNo == p.Reference && h.MatchingStatus == "N");

                            if (CBSAfri != null && Afr != null)
                            {
                                if (CBSAfri.Reference == Afr.ConfirmationNo)
                                {
                                    LogManager.SaveLog("MatchAfricaWorld Both are matched CBS itbId: " + CBSAfri.ItbId + "Afri Trans ItbId :" + Afr.ItbId);

                                    CBSAfri.ReconDate = DateTime.Now;
                                    CBSAfri.MatchingStatus = "M";
                                    CBSAfri.MatchingType = "SYSTEM";
                                    repoCBSAfricaWorldTransactionRepository.Update(CBSAfri);
                                    ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                    if (ret)
                                    {
                                    }

                                    Afr.ReconDate = DateTime.Now;
                                    Afr.MatchingStatus = "M";
                                    Afr.MatchingType = "SYSTEM";
                                    repoAfricaWorldTransactionRepository.Update(Afr);
                                    ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                    if (ret)
                                    {
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    var controlTablecbs = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == ReconTypeId && c.TableName == "CBSAfricaWorldTrans");
                    if (controlTablecbs != null)
                    {

                        controlTablecbs.LastTimeMatched = DateTime.Now;
                        repoadmDataPoollingControlRepository.Update(controlTablecbs);
                        var retww = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (retww)
                        {

                        }
                    }

                    var controlTablecon = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == ReconTypeId && c.TableName == "AfricaWorldTrans");
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
                LogManager.SaveLog("An error occured AfricaWorldReconModel function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
            }

            LogManager.SaveLog("Match Africa World End");
            return retVal;
        }
        public async Task<string> MoveAfricaWorldToHis()
        {
            LogManager.SaveLog("Move Afrca World Task Start in AfricaWorldReconModel");
            var cbsAfriWorldHis = new CBSAfricaWorldTransHistory();
            var AfriWorldHis = new AfricaWorldTransHistory();
            try
            {
                var CBSAfri = await repoCBSAfricaWorldTransactionRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var p in CBSAfri)
                {
                  cbsAfriWorldHis.ptId = p.ptId ;
                  cbsAfriWorldHis.AcctNo = p.AcctNo ;
                  cbsAfriWorldHis.AcctType = p.AcctType ;
                  cbsAfriWorldHis.TransDate = p.TransDate ;
                  cbsAfriWorldHis.EffectiveDate = p.EffectiveDate ;
                  cbsAfriWorldHis.Amount = p.Amount ;
                  cbsAfriWorldHis.DebitCredit = p.DebitCredit;
                  cbsAfriWorldHis.Description = p.Description ;
                  cbsAfriWorldHis.Reference = p.Reference ;
                  cbsAfriWorldHis.OriginTracerNo = p.OriginTracerNo;
                  cbsAfriWorldHis.CheckNo = p.CheckNo ;
                  cbsAfriWorldHis.PNR = p.PNR ;
                  cbsAfriWorldHis.AgentName = p.AgentName ;
                  cbsAfriWorldHis.ContactNo = p.ContactNo ;
                  cbsAfriWorldHis.Location = p.Location ;
                  cbsAfriWorldHis.TravelDate = p.TravelDate ;
                  cbsAfriWorldHis.DepositorName = p.DepositorName ;
                  cbsAfriWorldHis.ReconDate = p.ReconDate ;
                  cbsAfriWorldHis.PullDate = p.PullDate;
                  cbsAfriWorldHis.MatchingStatus = p.MatchingStatus ;
                  cbsAfriWorldHis.UserId = p.UserId ;
                  cbsAfriWorldHis.DateMoved = DateTime.Now;
                  cbsAfriWorldHis.MatchingType = p.MatchingType ;
                  cbsAfriWorldHis.OrigRefNo = p.OrigRefNo ;
                  cbsAfriWorldHis.ReversalCode = p.ReversalCode ;

                  repoCBSAfricaWorldTransHistoryRepository.Add(cbsAfriWorldHis);
                  var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                  if (ret)
                  {
                  }
                    //Delete 
                    var del = repoCBSAfricaWorldTransactionRepository.Get(d => d.ItbId == p.ItbId);
                    if (del != null)
                    {
                        repoCBSAfricaWorldTransactionRepository.Delete(del);
                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                        }
                    }
                }

                var AfriW = await repoAfricaWorldTransactionRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var c in AfriW)
                {
                      AfriWorldHis.ItbId = c.ItbId ;
                      AfriWorldHis.ConfirmationNo = c.ConfirmationNo ;
                      AfriWorldHis.BookingAgent = c.BookingAgent ;
                      AfriWorldHis.IATANo = c.IATANo ;
                      AfriWorldHis.AfricaWorldUserId = c.AfricaWorldUserId ;
                      AfriWorldHis.TransDate = c.TransDate ;
                      AfriWorldHis.Currency = c.Currency ;
                      AfriWorldHis.FirstName = c.FirstName ;
                      AfriWorldHis.LastName = c.LastName ;
                      AfriWorldHis.Deposit = c.Deposit ;
                      AfriWorldHis.BookedAmount = c.BookedAmount ;
                      AfriWorldHis.CancelledAmount = c.CancelledAmount ;
                      AfriWorldHis.BookingDate = c.BookingDate ;
                      AfriWorldHis.ReconDate = c.ReconDate ;
                      AfriWorldHis.PullDate = c.PullDate;
                      AfriWorldHis.MatchingStatus = c.MatchingStatus ;
                      AfriWorldHis.UserId = c.UserId ;
                      AfriWorldHis.DateMoved = DateTime.Now ;
                      AfriWorldHis.OrigRefNo = c.OrigRefNo ;
                      AfriWorldHis.MatchingType = c.MatchingType;

                      repoAfricaWorldTransHistoryRepository.Add(AfriWorldHis);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }
                    var del = repoAfricaWorldTransactionRepository.Get(d => d.ItbId == c.ItbId);
                    if (del != null)
                    {
                        repoAfricaWorldTransactionRepository.Delete(del);
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
                LogManager.SaveLog("An error occured in AfricaWorldReconModel Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return string.Empty;
            }
            LogManager.SaveLog("Move Africa World End");
            return string.Empty;
        }
        #endregion
    }
}