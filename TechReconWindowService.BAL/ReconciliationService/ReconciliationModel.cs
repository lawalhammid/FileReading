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
    public class ReconciliationModel
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDbFactory idbfactory;
        private readonly IadmMactingCriteriaRepository repoadmMactingCriteriaRepository;
        private readonly IadmMatchingTableRepository repoadmMatchingTableRepository;
        private readonly IadmMatchingTypeRepository repoadmMatchingTypeRepository;
        private readonly ICBSTransationTestRepository repoCBSTransationTestRepository;
        private readonly IPostillionTransationTestRepository repoPostillionTransationTestRepository;
        private readonly IBatchCounterRepository repoBatchCounterRepository;
        private string _classname = "TechReconWindowService.BAL/ReconciliationService/ReconciliationModel.cs";
        private string _lineErrorNumber;
        private string _methodname;
        private ReturnValues retVal;
        private readonly ICBSConsortiumTransactionRepository repoCBSConsortiumTransactionRepository;
        private readonly IConsortiumTransactionRepository repoConsortiumTransactionRepository;
        private readonly IReconTypeRepository repoReconTypeRepository;
        private readonly ICBSConsortiumTransactionHistoryRepository repoCBSConsortiumTransactionHistoryRepository;
        private readonly IConsortiumTransactionHistoryRepository repoConsortiumTransactionHistoryRepository;
        private readonly IadmDataPoollingControlRepository repoadmDataPoollingControlRepository;
        private readonly ICBSXPressMoneyRepository repoCBSXPressMoneyRepository;
        private readonly ICBSXPressMoneyHistoryRepository repoCBSXPressMoneyHistoryRepository;
        private readonly IXPressMoneyTransHistoryRepository repoXPressMoneyTransHistoryRepository;
        private readonly IXPressMoneyRepository repoXPressMoneyRepository;
        private readonly ICBSRiaRepository repoCBSRiaRepository;
        private readonly IRiaRepository repoRiaRepository;
        private readonly ICBSRiaHistoryRepository repoCBSRiaHistoryRepository;
        private readonly IRiaHistoryRepository repoRiaHistoryRepository;
        private readonly IMGTransRepository repoMGTransRepository;
        private readonly IMGTRansHistoryRepository repoMGTRansHistoryRepository;
        private readonly ICBSMGHistoryRepository repoCBSMGHistoryRepository;
        private readonly ICBSMGTransRepository repoCBSMGTransRepository;
        private readonly ICBSWUMTTransRepository repoCBSWUMTTransRepository;
        private readonly ICBSWUMTTransHistoryRepository repoCBSWUMTTransHistoryRepository;
        private readonly IWUMTTransRepository repoWUMTTransRepository;
        private readonly IWUMTTransHistoryRepository repoWUMTTransHistoryRepository;

        private readonly IUnityLinkTransRepository repoUnityLinkTransRepository;
        private readonly IUnityLinkTransHistoryRepository repoUnityLinkTransHistoryRepository;
        private readonly ICBSUnityLinkTransHistoryRepository repoCBSUnityLinkTransHistoryRepository;
        private readonly ICBSUnityLinkTransRepository repoCBSUnityLinkTransRepository;


        private readonly IMTNTRansRepository repoMTNTRansRepository;
        private readonly IMTNTRansErrorRepository repoMTNTRansErrorRepository;
        private readonly IMTNTRansHistoryRepository repoMTNTRansHistoryRepository;
        private readonly ICBSMTNTransErrorRepository repoCBSMTNTransErrorRepository;
        private readonly ICBSMTNTransHistoryRepository repoCBSMTNTransHistoryRepository;
        private readonly ICBSMTNTransRepository repoCBSMTNTransRepository;


        private readonly IAIRTELTransRepository repoAIRTELTransRepository;
        private readonly IAIRTELTransErrorRepository repoAIRTELTransErrorRepository;
        private readonly IAIRTELTransHistoryRepository repoAIRTELTransHistoryRepository;

        private readonly ICBSAirtelTransRepository repoCBSAirtelTransRepository;
        private readonly ICBSAirtelTransErrorRepository repoICBSAirtelTransErrorRepository;
        private readonly ICBSAirtelTransHistoryRepository repoCBSAirtelTransHistoryRepository;

        private readonly ICBSNostroVostroTransRepository repoCBSNostroVostroTransRepository;
        private readonly ICBSNostroVostroTransHistoryRepository repoCBSNostroVostroTransHistoryRepository;

        private readonly IVostroMT940950TransRepository repoVostroMT940950TransRepository;
        private readonly IVostroMT940950TransHistoryRepository repoVostroMT940950TransHistoryRepository;

        //VostroMT940950TransRepository  VostroMT940950TransHistoryRepository
        public ReconciliationModel()
        {
            idbfactory = new DbFactory();
            unitOfWork = new UnitOfWork(idbfactory);
            repoadmMactingCriteriaRepository = new admMactingCriteriaRepository(idbfactory);
            repoadmMatchingTypeRepository = new admMatchingTypeRepository(idbfactory);
            repoCBSTransationTestRepository = new CBSTransationTestRepository(idbfactory);
            repoPostillionTransationTestRepository = new PostillionTransationTestRepository(idbfactory);
            repoBatchCounterRepository = new BatchCounterRepository(idbfactory);
            retVal = new ReturnValues();
            repoCBSConsortiumTransactionRepository = new CBSConsortiumTransactionRepository(idbfactory);
            repoConsortiumTransactionRepository = new ConsortiumTransactionRepository(idbfactory);
            repoReconTypeRepository = new ReconTypeRepository(idbfactory);
            repoCBSConsortiumTransactionHistoryRepository = new CBSConsortiumTransactionHistoryRepository(idbfactory);
            repoConsortiumTransactionHistoryRepository = new ConsortiumTransactionHistoryRepository(idbfactory);
            repoadmDataPoollingControlRepository = new admDataPoollingControlRepository(idbfactory);
            repoCBSXPressMoneyRepository = new CBSXPressMoneyRepository(idbfactory);
            repoRiaRepository = new RiaRepository(idbfactory);
            repoCBSRiaRepository = new CBSRiaRepository(idbfactory);
            repoCBSRiaHistoryRepository = new CBSRiaHistoryRepository(idbfactory);
            repoRiaHistoryRepository = new RiaHistoryRepository(idbfactory);
            repoXPressMoneyRepository = new XPressMoneyRepository(idbfactory);
            repoXPressMoneyTransHistoryRepository = new XPressMoneyTransHistoryRepository(idbfactory);
            repoCBSXPressMoneyHistoryRepository = new CBSXPressMoneyHistoryRepository(idbfactory);
            repoMGTRansHistoryRepository = new MGTRansHistoryRepository(idbfactory);
            repoMGTransRepository = new MGTransRepository(idbfactory);
            repoCBSMGHistoryRepository = new CBSMGHistoryRepository(idbfactory);
            repoCBSMGTransRepository = new CBSMGTransRepository(idbfactory);
            repoCBSWUMTTransRepository = new CBSWUMTTransRepository(idbfactory);
            repoCBSWUMTTransHistoryRepository = new CBSWUMTTransHistoryRepository(idbfactory);
            repoWUMTTransRepository = new WUMTTransRepository(idbfactory);
            repoWUMTTransHistoryRepository = new WUMTTransHistoryRepository(idbfactory);
            repoUnityLinkTransRepository = new UnityLinkTransRepository(idbfactory);
            repoUnityLinkTransHistoryRepository = new UnityLinkTransHistoryRepository(idbfactory);
            repoCBSUnityLinkTransHistoryRepository = new CBSUnityLinkTransHistoryRepository(idbfactory);
            repoCBSUnityLinkTransRepository = new CBSUnityLinkTransRepository(idbfactory);

            repoMTNTRansRepository = new MTNTRansRepository(idbfactory);
            repoMTNTRansErrorRepository = new MTNTRansErrorRepository(idbfactory);
            repoMTNTRansHistoryRepository = new MTNTRansHistoryRepository (idbfactory);
            repoCBSMTNTransErrorRepository = new CBSMTNTransErrorRepository(idbfactory);
            repoCBSMTNTransHistoryRepository = new CBSMTNTransHistoryRepository(idbfactory);
            repoCBSMTNTransRepository = new CBSMTNTransRepository(idbfactory);

            repoAIRTELTransRepository = new AIRTELTransRepository(idbfactory);
            repoAIRTELTransErrorRepository = new AIRTELTransErrorRepository(idbfactory);
            repoAIRTELTransHistoryRepository = new AIRTELTransHistoryRepository(idbfactory);

            repoCBSAirtelTransRepository = new CBSAirtelTransRepository(idbfactory);
            repoICBSAirtelTransErrorRepository = new CBSAirtelTransErrorRepository(idbfactory);
            repoCBSAirtelTransHistoryRepository = new CBSAirtelTransHistoryRepository(idbfactory);

            repoCBSNostroVostroTransRepository = new CBSNostroVostroTransRepository(idbfactory);
            repoCBSNostroVostroTransHistoryRepository = new CBSNostroVostroTransHistoryRepository(idbfactory);
            repoVostroMT940950TransRepository = new VostroMT940950TransRepository(idbfactory);
            repoVostroMT940950TransHistoryRepository = new VostroMT940950TransHistoryRepository(idbfactory);
        }

        public async Task<ReturnValues> Reconciliation()
        {
            TechReconContext context = new TechReconContext();
            try
            {
                var batchCounter = new BatchCounter();
                var cBSTransation = new CbsAtmTransaction();
                var postillionTransationTest = new PostilionTransaction();
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

                foreach (var m in matchingCriteria)
                {

                    string[] columnslist1 = { m.Source1ColumnName };
                    var columns1 = columnslist1[0].Split(';');
                    string columnsgongon2 = string.Empty;
                    string col1 = string.Empty;
                    string col2 = string.Empty;

                    for (int i = 0; i < columns1.Count(); i++)
                    {
                        var columnsgongon1 = columns1[i].Split(';');

                        col1 = columnsgongon1[i].ToString();



                        if (!string.IsNullOrEmpty(columnsgongon1.ToString()))
                        {
                            string[] columnslist2 = { m.Source2ColumnName };
                            var columns2 = columnslist2[0].Split(';');
                            for (int i2 = 0; i2 < columns2.Count(); i2++)
                            {
                                if (i == i2)
                                {
                                    col2 = columns2[i2].ToString();
                                    break;
                                }
                            }

                            //Update Macthing Criteria
                            var updateMachingCriteria = await repoadmMactingCriteriaRepository.GetAsync(c => c.MatchingTypeId == m.MatchingTypeId);
                            updateMachingCriteria.ReconciliationBatchNo = procesBatchNokk.ToString();
                            updateMachingCriteria.ReconciliationStartTime = DateTime.Now;
                            updateMachingCriteria.ReconciliationStatus = "Start";
                            updateMachingCriteria.ReconcilledBy = "SYSTEM";
                            repoadmMactingCriteriaRepository.Update(updateMachingCriteria);
                            ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                            if (ret)
                            {
                            }

                            var getMatchType = await repoadmMatchingTypeRepository.GetAsync(c => c.MatchingTypeId == m.MatchingTypeId);
                            string Itemtype = getMatchType.Description;

                            //For  One Item To One Item
                            #region One To One
                            if (Itemtype == "One Item To One Item")
                            {
                                var posttillion = await repoPostillionTransationTestRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());
                                var CBS = await repoCBSTransationTestRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());

                                foreach (var p in posttillion)
                                {
                                    foreach (var c in CBS)
                                    {
                                        if (p.ReferenceNo == c.ReferenceNo)
                                        {
                                            var PostilionUniqu = repoPostillionTransationTestRepository.Get(q => q.ReferenceNo == p.ReferenceNo);

                                            var CBSUniqu = repoCBSTransationTestRepository.Get(q => q.ReferenceNo == p.ReferenceNo);

                                            string dtSource1 = string.Empty;
                                            decimal dtSource1SumValue = 0;
                                            string dtSource2 = string.Empty;
                                            decimal dtSource2SumValue = 0;

                                            if (m.Source1Table == "PostilionTransaction")
                                            {
                                                i += 1;

                                                col1 = columns1[i].ToString();
                                                col2 = columns2[i].ToString();
                                                var Table1Source = repoPostillionTransationTestRepository.GetMany(q => col1 == col2).Take(1)
                                                                .Select(q => q.GetType().GetProperty(col1).GetValue(q));

                                                foreach (var dtsource1 in Table1Source)
                                                {
                                                    //Would later think if to remove the below for One to One matching

                                                    if (m.MachingBasisTable1 == true && m.SumValueTable1 == true)
                                                    {
                                                        dtSource1SumValue += Convert.ToDecimal(dtsource1);
                                                    }
                                                    else
                                                    {
                                                        dtSource1SumValue = Convert.ToDecimal(dtsource1);
                                                        break;
                                                    }

                                                }
                                            }
                                            if (m.Source2Table == "CbsAtmTransaction")
                                            {
                                                var Table2Source = repoCBSTransationTestRepository.GetMany(q => col2 == col1).Take(1)
                                                        .Select(q => q.GetType().GetProperty(col2).GetValue(q));
                                                foreach (var dtsource2 in Table2Source)
                                                {
                                                    if (m.MachingBasisTable2 == true && m.SumValueTable2 == true)
                                                    {
                                                        dtSource2SumValue += Convert.ToDecimal(dtsource2);
                                                    }
                                                    else
                                                    {
                                                        dtSource2SumValue = Convert.ToDecimal(dtsource2);
                                                        break;
                                                    }
                                                }
                                            }

                                            if (m.Source1Table == "CbsAtmTransaction")
                                            {
                                                var Table2Source = repoCBSTransationTestRepository.GetMany(q => col2 == col1).Take(1)
                                                       .Select(q => q.GetType().GetProperty(col2).GetValue(q));
                                                foreach (var dtsource2 in Table2Source)
                                                {
                                                    if (m.MachingBasisTable2 == true && m.SumValueTable2 == true)
                                                    {
                                                        dtSource2SumValue += Convert.ToDecimal(dtsource2);
                                                    }
                                                    else
                                                    {
                                                        dtSource2SumValue = Convert.ToDecimal(dtsource2);
                                                        break;
                                                    }
                                                }
                                            }

                                            if (m.Source2Table == "PostilionTransaction")
                                            {
                                                i += 1;

                                                col1 = columns1[i].ToString();
                                                col2 = columns2[i].ToString();
                                                var Table1Source = repoPostillionTransationTestRepository.GetMany(q => col1 == col2).Take(1)
                                                                .Select(q => q.GetType().GetProperty(col1).GetValue(q));

                                                foreach (var dtsource1 in Table1Source)
                                                {
                                                    //Would later think if to remove the below for One to One matching

                                                    if (m.MachingBasisTable1 == true && m.SumValueTable1 == true)
                                                    {
                                                        dtSource1SumValue += Convert.ToDecimal(dtsource1);
                                                    }
                                                    else
                                                    {
                                                        dtSource1SumValue = Convert.ToDecimal(dtsource1);
                                                        break;
                                                    }

                                                }
                                            }
                                            if (dtSource1SumValue == dtSource2SumValue)
                                            {
                                                CBSUniqu.MatchingStatus = "M";
                                                repoCBSTransationTestRepository.Update(CBSUniqu);
                                                ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret)
                                                {
                                                }

                                                PostilionUniqu.MatchingStatus = "M";
                                                repoPostillionTransationTestRepository.Update(PostilionUniqu);
                                                ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret)
                                                {
                                                }
                                            }
                                            else
                                            {
                                                CBSUniqu.MatchingStatus = "N";
                                                repoCBSTransationTestRepository.Update(CBSUniqu);
                                                ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret)
                                                {

                                                }
                                                PostilionUniqu.MatchingStatus = "N";
                                                repoPostillionTransationTestRepository.Update(PostilionUniqu);
                                                ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                if (ret)
                                                {
                                                }
                                            }
                                        }
                                        else
                                        {
                                        }
                                    }
                                }



                            }
                            #endregion
                            #region One To Many
                            else
                                if (Itemtype == "One Item To Many Items")
                                {
                                    var posttillion = await repoPostillionTransationTestRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());
                                    var CBS = await repoCBSTransationTestRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());

                                    foreach (var p in posttillion)
                                    {
                                        foreach (var c in CBS)
                                        {
                                            if (p.ReferenceNo == c.ReferenceNo)
                                            {
                                                var PostilionUniqu = repoPostillionTransationTestRepository.Get(q => q.ReferenceNo == p.ReferenceNo);

                                                var CBSUniqu = repoCBSTransationTestRepository.Get(q => q.ReferenceNo == p.ReferenceNo);

                                                string dtSource1 = string.Empty;
                                                decimal dtSource1SumValue = 0;
                                                string dtSource2 = string.Empty;
                                                decimal dtSource2SumValue = 0;

                                                if (m.Source1Table == "PostilionTransaction")
                                                {
                                                    i += 1;

                                                    col1 = columns1[i].ToString();
                                                    col2 = columns2[i].ToString();
                                                    var Table1Source = repoPostillionTransationTestRepository.GetMany(q => col1 == col2)
                                                                    .Select(q => q.GetType().GetProperty(col1).GetValue(q));

                                                    foreach (var dtsource1 in Table1Source)
                                                    {
                                                        //Would later think if to remove the below for One to One matching

                                                        if (m.MachingBasisTable1 == true && m.SumValueTable1 == true)
                                                        {
                                                            dtSource1SumValue += Convert.ToDecimal(dtsource1);
                                                        }
                                                        else
                                                        {
                                                            dtSource1SumValue = Convert.ToDecimal(dtsource1);
                                                            break;
                                                        }

                                                    }
                                                }
                                                if (m.Source2Table == "CbsAtmTransaction")
                                                {
                                                    var Table2Source = repoCBSTransationTestRepository.GetMany(q => col2 == col1)
                                                            .Select(q => q.GetType().GetProperty(col2).GetValue(q));
                                                    foreach (var dtsource2 in Table2Source)
                                                    {
                                                        if (m.MachingBasisTable2 == true && m.SumValueTable2 == true)
                                                        {
                                                            dtSource2SumValue += Convert.ToDecimal(dtsource2);
                                                        }
                                                        else
                                                        {
                                                            dtSource2SumValue = Convert.ToDecimal(dtsource2);
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (m.Source1Table == "CbsAtmTransaction")
                                                {
                                                    var Table2Source = repoCBSTransationTestRepository.GetMany(q => col2 == col1)
                                                             .Select(q => q.GetType().GetProperty(col2).GetValue(q));
                                                    foreach (var dtsource2 in Table2Source)
                                                    {
                                                        if (m.MachingBasisTable2 == true && m.SumValueTable2 == true)
                                                        {
                                                            dtSource2SumValue += Convert.ToDecimal(dtsource2);
                                                        }
                                                        else
                                                        {
                                                            dtSource2SumValue = Convert.ToDecimal(dtsource2);
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (m.Source2Table == "PostilionTransaction")
                                                {
                                                    i += 1;

                                                    col1 = columns1[i].ToString();
                                                    col2 = columns2[i].ToString();
                                                    var Table1Source = repoPostillionTransationTestRepository.GetMany(q => col1 == col2)
                                                                    .Select(q => q.GetType().GetProperty(col1).GetValue(q));

                                                    foreach (var dtsource1 in Table1Source)
                                                    {
                                                        //Would later think if to remove the below for One to One matching

                                                        if (m.MachingBasisTable1 == true && m.SumValueTable1 == true)
                                                        {
                                                            dtSource1SumValue += Convert.ToDecimal(dtsource1);
                                                        }
                                                        else
                                                        {
                                                            dtSource1SumValue = Convert.ToDecimal(dtsource1);
                                                            break;
                                                        }

                                                    }
                                                }
                                                if (dtSource1SumValue == dtSource2SumValue)
                                                {
                                                    CBSUniqu.MatchingStatus = "M";
                                                    repoCBSTransationTestRepository.Update(CBSUniqu);
                                                    ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                    }

                                                    PostilionUniqu.MatchingStatus = "M";
                                                    repoPostillionTransationTestRepository.Update(PostilionUniqu);
                                                    ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                    }
                                                }
                                                else
                                                {
                                                    CBSUniqu.MatchingStatus = "N";
                                                    repoCBSTransationTestRepository.Update(CBSUniqu);
                                                    ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {

                                                    }
                                                    PostilionUniqu.MatchingStatus = "N";
                                                    repoPostillionTransationTestRepository.Update(PostilionUniqu);
                                                    ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                    if (ret)
                                                    {
                                                    }
                                                }
                                            }
                                            else
                                            {
                                            }
                                        }
                                    }
                                }
                            #endregion
                                #region Many Items To Many Items
                                else
                                    if (Itemtype == "Many Items To Many Items")
                                    {
                                        var posttillion = await repoPostillionTransationTestRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());
                                        var CBS = await repoCBSTransationTestRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());

                                        foreach (var p in posttillion)
                                        {
                                            foreach (var c in CBS)
                                            {
                                                if (p.ReferenceNo == c.ReferenceNo)
                                                {
                                                    var PostilionUniqu = repoPostillionTransationTestRepository.Get(q => q.ReferenceNo == p.ReferenceNo);

                                                    var CBSUniqu = repoCBSTransationTestRepository.Get(q => q.ReferenceNo == p.ReferenceNo);

                                                    string dtSource1 = string.Empty;
                                                    decimal dtSource1SumValue = 0;
                                                    string dtSource2 = string.Empty;
                                                    decimal dtSource2SumValue = 0;

                                                    if (m.Source1Table == "PostilionTransaction")
                                                    {
                                                        i += 1;

                                                        col1 = columns1[i].ToString();
                                                        col2 = columns2[i].ToString();
                                                        var Table1Source = repoPostillionTransationTestRepository.GetMany(q => col1 == col2)
                                                                        .Select(q => q.GetType().GetProperty(col1).GetValue(q));

                                                        foreach (var dtsource1 in Table1Source)
                                                        {
                                                            //Would later think if to remove the below for One to One matching

                                                            if (m.MachingBasisTable1 == true && m.SumValueTable1 == true)
                                                            {
                                                                dtSource1SumValue += Convert.ToDecimal(dtsource1);
                                                            }
                                                            else
                                                            {
                                                                dtSource1SumValue = Convert.ToDecimal(dtsource1);
                                                                break;
                                                            }

                                                        }
                                                    }
                                                    if (m.Source2Table == "CbsAtmTransaction")
                                                    {
                                                        var Table2Source = repoCBSTransationTestRepository.GetMany(q => col2 == col1)
                                                                .Select(q => q.GetType().GetProperty(col2).GetValue(q));
                                                        foreach (var dtsource2 in Table2Source)
                                                        {
                                                            if (m.MachingBasisTable2 == true && m.SumValueTable2 == true)
                                                            {
                                                                dtSource2SumValue += Convert.ToDecimal(dtsource2);
                                                            }
                                                            else
                                                            {
                                                                dtSource2SumValue = Convert.ToDecimal(dtsource2);
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (m.Source1Table == "CbsAtmTransaction")
                                                    {
                                                        var Table2Source = repoCBSTransationTestRepository.GetMany(q => m.Source2ColumnName == m.Source1ColumnName)
                                                            .Select(q => q.GetType().GetProperty(m.Source2ColumnName).GetValue(q));

                                                        foreach (var dtsource2 in Table2Source)
                                                        {
                                                            if (m.MachingBasisTable2 == true && m.SumValueTable2 == true)
                                                            {
                                                                dtSource2SumValue += Convert.ToDecimal(dtsource2);
                                                            }
                                                            else
                                                            {
                                                                dtSource2SumValue = Convert.ToDecimal(dtsource2);
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (m.Source2Table == "PostilionTransaction")
                                                    {
                                                        i += 1;

                                                        col1 = columns1[i].ToString();
                                                        col2 = columns2[i].ToString();
                                                        var Table1Source = repoPostillionTransationTestRepository.GetMany(q => col1 == col2)
                                                                        .Select(q => q.GetType().GetProperty(col1).GetValue(q));

                                                        foreach (var dtsource1 in Table1Source)
                                                        {
                                                            //Would later think if to remove the below for One to One matching

                                                            if (m.MachingBasisTable1 == true && m.SumValueTable1 == true)
                                                            {
                                                                dtSource1SumValue += Convert.ToDecimal(dtsource1);
                                                            }
                                                            else
                                                            {
                                                                dtSource1SumValue = Convert.ToDecimal(dtsource1);
                                                                break;
                                                            }

                                                        }
                                                    }
                                                    if (dtSource1SumValue == dtSource2SumValue)
                                                    {
                                                        CBSUniqu.MatchingStatus = "M";
                                                        repoCBSTransationTestRepository.Update(CBSUniqu);
                                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                        if (ret)
                                                        {
                                                        }

                                                        PostilionUniqu.MatchingStatus = "M";
                                                        repoPostillionTransationTestRepository.Update(PostilionUniqu);
                                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                        if (ret)
                                                        {
                                                        }
                                                    }
                                                    else
                                                    {
                                                        CBSUniqu.MatchingStatus = "N";
                                                        repoCBSTransationTestRepository.Update(CBSUniqu);
                                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                        if (ret)
                                                        {

                                                        }
                                                        PostilionUniqu.MatchingStatus = "N";
                                                        repoPostillionTransationTestRepository.Update(PostilionUniqu);
                                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                                        if (ret)
                                                        {
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                }
                                            }
                                        }
                                    }
                                #endregion
                            //Update Macthing Criteria
                            var updateMachingCriteriafinal = repoadmMactingCriteriaRepository.Get(c => c.MatchingTypeId == m.MatchingTypeId);
                            updateMachingCriteriafinal.ReconciliationBatchNo = procesBatchNokk.ToString();
                            updateMachingCriteriafinal.ReconciliationEndTime = DateTime.Now;
                            updateMachingCriteriafinal.ReconciliationStatus = "Done";
                            updateMachingCriteriafinal.ReconcilledBy = "SYSTEM";
                            repoadmMactingCriteriaRepository.Update(updateMachingCriteriafinal);
                            ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                            if (ret)
                            {
                            }
                        }

                    }


                }
                return retVal;
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
            }
        }

        #region Consortium 
        public async Task<ReturnValues> MatchCBSTranAndConsortiumTrans()
        {
          //  LogManager.SaveLog("MatchCBSTranAndConsortiumTrans Start");
            string ReconTypeName = string.Empty;
            int ConsortiumId = 0;
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
                    if (recName.WSReconName == "Consortium")
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
                        var CBSConsortiumList = await repoCBSConsortiumTransactionRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());

                        foreach (var p in CBSConsortiumList)
                        {
                            var CBSConsortium = await repoCBSConsortiumTransactionRepository.GetAsync(q => q.SerialNo == p.SerialNo && q.MatchingStatus == "N");
                            var Consortium = await repoConsortiumTransactionRepository.GetAsync(h => h.SerialNo == p.SerialNo && h.MatchingStatus == "N");

                            if (CBSConsortium != null && Consortium != null)
                            {
                                if (CBSConsortium.SerialNo == Consortium.SerialNo)
                                {
                                    if (CBSConsortium.Amount == Consortium.Amount)
                                    {
                                       // LogManager.SaveLog("MatchCBSTranAndConsortiumTrans Both are matched CBS itbId: " + CBSConsortium.ItbId + "Consortium ItbId :" + Consortium.ItbId);

                                        CBSConsortium.ReconDate = DateTime.Now;
                                        CBSConsortium.MatchingStatus = "M";
                                        CBSConsortium.MatchingType = "SYSTEM";
                                        repoCBSConsortiumTransactionRepository.Update(CBSConsortium);
                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }

                                        Consortium.ReconDate = DateTime.Now;
                                        Consortium.MatchingStatus = "M";
                                        Consortium.MatchingType = "SYSTEM";
                                        repoConsortiumTransactionRepository.Update(Consortium);
                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }
                                    }
                                }
                            }


                        }
                    }
                    #endregion
                    var rectype = repoReconTypeRepository.Get(c => c.WSReconName == "Consortium");
                    var controlTablecbs = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSConsortiumTransaction");
                    if (controlTablecbs != null)
                    {

                        controlTablecbs.LastTimeMatched = DateTime.Now;
                        repoadmDataPoollingControlRepository.Update(controlTablecbs);
                        var retww = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (retww)
                        {

                        }
                    }

                    var controlTablecon = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "ConsortiumTransaction");
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
                LogManager.SaveLog("An error occured MatchCBSTranAndConsortiumTrans1 function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
            }

            LogManager.SaveLog("MatchCBSTranAndConsortiumTrans End");
            return retVal;
        }
        public async Task<ReturnValues> CBSTranAndConsortiumTransHistory()
        {
            LogManager.SaveLog("CBSTranAndConsortiumTransHistory Start");
            var CBSConsortiumTransactionHistoryTBL = new CBSConsortiumTransactionHistory();
            var ConsortiumTransactionHistoryTBL = new ConsortiumTransactionHistory();
            try
            {
                var CBSConsortiumList = await repoCBSConsortiumTransactionRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var p in CBSConsortiumList)
                {
                    CBSConsortiumTransactionHistoryTBL.ItbId = p.ItbId;
                    CBSConsortiumTransactionHistoryTBL.AcctNo = p.AcctNo;
                    CBSConsortiumTransactionHistoryTBL.TransDate =  p.TransDate;
                    CBSConsortiumTransactionHistoryTBL.Description = p.Description;
                    CBSConsortiumTransactionHistoryTBL.OriginatingBranch = p.OriginatingBranch;
                    CBSConsortiumTransactionHistoryTBL.Amount = p.Amount;
                    CBSConsortiumTransactionHistoryTBL.DebitCredit = p.DebitCredit;
                    CBSConsortiumTransactionHistoryTBL.PayerName = p.PayerName;
                    CBSConsortiumTransactionHistoryTBL.SerialNo = p.SerialNo;
                    CBSConsortiumTransactionHistoryTBL.PostedBy = p.PostedBy;
                    CBSConsortiumTransactionHistoryTBL.UserId = p.UserId;
                    CBSConsortiumTransactionHistoryTBL.ReconDate = p.ReconDate;
                    CBSConsortiumTransactionHistoryTBL.PullDate = p.PullDate;
                    CBSConsortiumTransactionHistoryTBL.DateMoved = DateTime.Now;
                    CBSConsortiumTransactionHistoryTBL.MatchingStatus = p.MatchingStatus;
                    CBSConsortiumTransactionHistoryTBL.MatchingType = p.MatchingType;
                    CBSConsortiumTransactionHistoryTBL.OrigRefNo = p.OrigRefNo;
                    CBSConsortiumTransactionHistoryTBL.ReversalCode = p.ReversalCode;

                    repoCBSConsortiumTransactionHistoryRepository.Add(CBSConsortiumTransactionHistoryTBL);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }


                    //Delete 
                    var del = repoCBSConsortiumTransactionRepository.Get(d => d.ItbId == p.ItbId);
                    if (del != null)
                    {
                        repoCBSConsortiumTransactionRepository.Delete(del);
                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                        }
                    }


                }

                var ConsortiumList = await repoConsortiumTransactionRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var c in ConsortiumList)
                {

                    ConsortiumTransactionHistoryTBL.ItbId = c.ItbId;
                    ConsortiumTransactionHistoryTBL.Institution = c.Institution;
                    ConsortiumTransactionHistoryTBL.FormName = c.FormName;
                    ConsortiumTransactionHistoryTBL.Amount = c.Amount;
                    ConsortiumTransactionHistoryTBL.TransDate = c.TransDate;
                    ConsortiumTransactionHistoryTBL.BuyVouchersID = c.BuyVouchersID;
                    ConsortiumTransactionHistoryTBL.MobileNo = c.MobileNo;
                    ConsortiumTransactionHistoryTBL.OriginatingBranch = c.OriginatingBranch;
                    ConsortiumTransactionHistoryTBL.TellerName = c.TellerName;
                    ConsortiumTransactionHistoryTBL.SerialNo = c.SerialNo;
                    ConsortiumTransactionHistoryTBL.OrigRefNo = c.OrigRefNo;
                    ConsortiumTransactionHistoryTBL.PostedBy = c.PostedBy;
                    ConsortiumTransactionHistoryTBL.AcctNo = c.AcctNo;
                    ConsortiumTransactionHistoryTBL.UserId = c.UserId;
                    ConsortiumTransactionHistoryTBL.ReconDate = c.ReconDate;
                    ConsortiumTransactionHistoryTBL.PullDate = c.PullDate;
                    ConsortiumTransactionHistoryTBL.DateMoved = DateTime.Now;
                    ConsortiumTransactionHistoryTBL.MatchingStatus = c.MatchingStatus;
                    ConsortiumTransactionHistoryTBL.MatchingType = c.MatchingType;
                    repoConsortiumTransactionHistoryRepository.Add(ConsortiumTransactionHistoryTBL);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }


                    var del = repoConsortiumTransactionRepository.Get(d => d.ItbId == c.ItbId);
                    if (del != null)
                    {
                        repoConsortiumTransactionRepository.Delete(del);
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
                LogManager.SaveLog("An error occured CBSTranAndConsortiumTransHistory1 function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
            }

            LogManager.SaveLog("CBSTranAndConsortiumTransHistory End");
            return retVal;
        }

        #endregion

        #region Ria
        public async Task<ReturnValues> MatchCBSRiaAndRia()
        {
            LogManager.SaveLog("MatchCBSRiaAndRia Start");
            string ReconTypeName = string.Empty;
            int ConsortiumId = 0;
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
                    var recName = await repoReconTypeRepository.GetAsync(c => c.ReconTypeId == rTypeName.ReconTypeId);
                    if (recName.WSReconName == "Ria")
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
                        var CBSRia = await repoCBSRiaRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());

                        foreach (var p in CBSRia)
                        {
                            var CBSRia1 =  await repoCBSRiaRepository.GetAsync(q => q.Refference == p.Refference && q.MatchingStatus == "N");
                            var Ria1 = await repoRiaRepository.GetAsync(h => h.TransactionNo == p.Refference && h.MatchingStatus == "N");

                            if (CBSRia1 != null && Ria1 != null)
                            {
                                if (CBSRia1.Refference == Ria1.TransactionNo)
                                {
                                    if (CBSRia1.Amount == Ria1.PaidAmount)
                                    {
                                        CBSRia1.ReconDate = DateTime.Now;
                                        CBSRia1.MatchingStatus = "M";
                                        CBSRia1.MatchingType = "SYSTEM";
                                        repoCBSRiaRepository.Update(CBSRia1);
                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }

                                        Ria1.ReconDate = DateTime.Now;
                                        Ria1.MatchingStatus = "M";
                                        Ria1.MatchingType = "SYSTEM";
                                        repoRiaRepository.Update(Ria1);
                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }
                                    }
                                }
                            }


                        }
                    }
                    #endregion
                    var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "Ria");
                    var controlTablecbs = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSRiaTrans");
                    if (controlTablecbs != null)
                    {

                        controlTablecbs.LastTimeMatched = DateTime.Now;
                        repoadmDataPoollingControlRepository.Update(controlTablecbs);
                        var retww = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (retww)
                        {

                        }
                    }

                    var controlTablecon = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "RiaTrans");
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
                LogManager.SaveLog("An error occured MatchCBSTranAndConsortiumTrans1 function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
            }

            LogManager.SaveLog("MatchCBSTranAndConsortiumTrans End");
            return retVal;
        }
        public async Task<ReturnValues> CBSRiaAndRiaHistory()
        {
            LogManager.SaveLog("CBSTranAndConsortiumTransHistory Start");
            var CBSRiaTBLHistory = new CBSRiaTransHistory();
            var RiaHisTBL = new RiaTransHistory();
            try
            {
                var CBSRia = await repoCBSRiaRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var p in CBSRia)
                {

                    CBSRiaTBLHistory.ItbId = p.ItbId;
                    CBSRiaTBLHistory.AcctNo = p.AcctNo;
                    CBSRiaTBLHistory.Amount = p.Amount;
                    CBSRiaTBLHistory.DebitCredit = p.DebitCredit;
                    CBSRiaTBLHistory.Description = p.Description;
                    CBSRiaTBLHistory.TransDate = p.TransDate;
                    CBSRiaTBLHistory.OriginBranch = p.OriginBranch;
                    CBSRiaTBLHistory.PostedBy = p.PostedBy;
                    CBSRiaTBLHistory.Balance = p.Balance;
                    CBSRiaTBLHistory.ReconDate = p.ReconDate;
                    CBSRiaTBLHistory.PullDate = p.PullDate;
                    CBSRiaTBLHistory.MatchingStatus = p.MatchingStatus;
                    CBSRiaTBLHistory.UserId = p.UserId;
                    CBSRiaTBLHistory.Refference = p.Refference;
                    CBSRiaTBLHistory.OrigRefNo = p.OrigRefNo;
                    CBSRiaTBLHistory.PtId = p.PtId;
                    CBSRiaTBLHistory.DateMoved = DateTime.Now;
                    CBSRiaTBLHistory.MatchingType = p.MatchingType;
                    CBSRiaTBLHistory.ReversalCode = p.ReversalCode;
  
                    repoCBSRiaHistoryRepository.Add(CBSRiaTBLHistory);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }

                    //Delete 
                    var del = await repoCBSRiaRepository.GetAsync(d => d.ItbId == p.ItbId);
                    if (del != null)
                    {
                        repoCBSRiaRepository.Delete(del);
                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                        }
                    }
                }

                var RiaList = await repoRiaRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var r in RiaList)
                {
                    RiaHisTBL.ItbId = r.ItbId;
                    RiaHisTBL.TransDate = r.TransDate;
                    RiaHisTBL.TransactionNo = r.TransactionNo;
                    RiaHisTBL.OrigRefNo = r.OrigRefNo;
                    RiaHisTBL.TransPin = r.TransPin;
                    RiaHisTBL.PayAbleAmount = r.PayAbleAmount;
                    RiaHisTBL.Currency = r.Currency;
                    RiaHisTBL.CommissionUSD = r.CommissionUSD;
                    RiaHisTBL.BranchName = r.BranchName;
                    RiaHisTBL.BranchNo = r.BranchNo;
                    RiaHisTBL.PaidBy = r.PaidBy;
                    RiaHisTBL.EnteredTime = r.EnteredTime;
                    RiaHisTBL.PaidTime = r.PaidTime;
                    RiaHisTBL.PaidCurrency = r.PaidCurrency;
                    RiaHisTBL.PaidAmount = r.PaidAmount;
                    RiaHisTBL.Commission_2 = r.Commission_2;
                    RiaHisTBL.FxShare = r.FxShare;
                    RiaHisTBL.Commission_2AndFxShare = r.Commission_2AndFxShare;
                    RiaHisTBL.BeneficiaryFirstName = r.BeneficiaryFirstName;
                    RiaHisTBL.BeneficiaryLastName1 = r.BeneficiaryLastName1;
                    RiaHisTBL.BeneficiaryLastName2 = r.BeneficiaryLastName2;
                    RiaHisTBL.BeneficiaryIdType = r.BeneficiaryIdType;
                    RiaHisTBL.BeneficiaryIdNo = r.BeneficiaryIdNo;
                    RiaHisTBL.PaymentAmountUSD = r.PaymentAmountUSD;
                    RiaHisTBL.ReconDate = r.ReconDate;
                    RiaHisTBL.PullDate = r.PullDate;
                    RiaHisTBL.MatchingStatus = r.MatchingStatus;
                    RiaHisTBL.DateMoved = DateTime.Now;
                    RiaHisTBL.UserId = r.UserId;
                    RiaHisTBL.MatchingType = r.MatchingType;
                    RiaHisTBL.FileName = r.FileName;
                    
                    repoRiaHistoryRepository.Add(RiaHisTBL);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }

                    var del = await repoRiaRepository.GetAsync(d => d.ItbId == r.ItbId);
                    if (del != null)
                    {
                        repoRiaRepository.Delete(del);
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
                LogManager.SaveLog("An error occured CBSTranAndConsortiumTransHistory1 function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
            }

            LogManager.SaveLog("CBSTranAndConsortiumTransHistory End");
            return retVal;
        }
        #endregion

        #region XpressMoney
        public async Task<ReturnValues> MatchCBSXpressMoneyAndXpressMoney()
        {
            LogManager.SaveLog("MatchCBSXpressMoneyAndXpressMoney Start");
            string ReconTypeName = string.Empty;
            int ConsortiumId = 0;
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
                    var recName = await repoReconTypeRepository.GetAsync(c => c.ReconTypeId == rTypeName.ReconTypeId);
                    if (recName.WSReconName == "XpressMoney")
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
                        var CBSXpress = await repoCBSXPressMoneyRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());

                        foreach (var p in CBSXpress)
                        {
                            var CBSXpress1 = await repoCBSXPressMoneyRepository.GetAsync(q => q.Reference == p.Reference && q.MatchingStatus == "N");
                            var Xpress1 = await repoXPressMoneyRepository.GetAsync(h => h.Xpin == p.Reference && h.MatchingStatus == "N");

                            if (CBSXpress1 != null && Xpress1 != null)
                            {
                                if (CBSXpress1.Reference == Xpress1.Xpin)
                                {
                                    decimal compare = Math.Round(Convert.ToDecimal(Xpress1.AmountPaid), 2);
                                    if (CBSXpress1.Amount == compare)
                                    {
                                        CBSXpress1.ReconDate = DateTime.Now;
                                        CBSXpress1.MatchingStatus = "M";
                                        CBSXpress1.MatchingType = "SYSTEM";
                                        repoCBSXPressMoneyRepository.Update(CBSXpress1);
                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }
                                        Xpress1.ReconDate = DateTime.Now;
                                        Xpress1.MatchingStatus = "M";
                                        Xpress1.MatchingType = "SYSTEM";
                                        repoXPressMoneyRepository.Update(Xpress1);
                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    var rectype = await repoReconTypeRepository.GetAsync(c => c.WSReconName == "XpressMoney");
                    var controlTablecbs = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSXpressMoneyTrans");
                    if (controlTablecbs != null)
                    {

                        controlTablecbs.LastTimeMatched = DateTime.Now;
                        repoadmDataPoollingControlRepository.Update(controlTablecbs);
                        var retww = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (retww)
                        {

                        }
                    }

                    var controlTablecon = await repoadmDataPoollingControlRepository.GetAsync(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "XpressMoneyTrans");
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
                LogManager.SaveLog("An error occured MatchCBSTranAndConsortiumTrans1 function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
            }

            LogManager.SaveLog("MatchCBSXpressMoneyAndXpressMoney End");
            return retVal;
        }
        public async Task<ReturnValues> CBSxpressAndXperessHistory()
        {
            LogManager.SaveLog("CBSTranAndConsortiumTransHistory Start");
            var cBSXpressMoneyTransHistory = new CBSXpressMoneyTransHistory();
            var XpressHisTBL = new XpressMoneyTransHistory();
            try
            {
                var CBSRia = await repoCBSXPressMoneyRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var p in CBSRia)
                {

                    cBSXpressMoneyTransHistory.AcctNo = p.AcctNo;
                    cBSXpressMoneyTransHistory.Amount = p.Amount;
                    cBSXpressMoneyTransHistory.DebitCredit = p.DebitCredit;
                    cBSXpressMoneyTransHistory.Description = p.Description;
                    cBSXpressMoneyTransHistory.TransDate = p.TransDate;
                    cBSXpressMoneyTransHistory.OriginBranch = p.OriginBranch;
                    cBSXpressMoneyTransHistory.PostedBy = p.PostedBy;
                    cBSXpressMoneyTransHistory.Balance = p.Balance;
                    cBSXpressMoneyTransHistory.ReconDate = p.ReconDate;
                    cBSXpressMoneyTransHistory.PullDate = p.PullDate;
                    cBSXpressMoneyTransHistory.MachingStatus = p.MatchingStatus;
                    cBSXpressMoneyTransHistory.Refference = p.Reference;
                    cBSXpressMoneyTransHistory.OrigRefNo = p.OrigRefNo;
                    cBSXpressMoneyTransHistory.PtId = p.PtId;
                    cBSXpressMoneyTransHistory.ItbId = p.ItbId;
                    cBSXpressMoneyTransHistory.UserId = p.UserId;
                    cBSXpressMoneyTransHistory.PtId = p.PtId;
                    cBSXpressMoneyTransHistory.DateMoved = DateTime.Now;
                    cBSXpressMoneyTransHistory.MatchingType = p.MatchingType;
                    cBSXpressMoneyTransHistory.ReversalCode = p.ReversalCode;

                    repoCBSXPressMoneyHistoryRepository.Add(cBSXpressMoneyTransHistory);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }

                    //Delete 
                    var del = await repoCBSXPressMoneyRepository.GetAsync(d => d.ItbId == p.ItbId);
                    if (del != null)
                    {
                        repoCBSXPressMoneyRepository.Delete(del);
                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                        }
                    }
                }

                var XpressTranList = await repoXPressMoneyRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var l in XpressTranList)
                {

                    XpressHisTBL.ItbId = l.ItbId;
                    XpressHisTBL.PayoutDate = l.PayoutDate;
                    XpressHisTBL.Xpin = l.Xpin;
                    XpressHisTBL.OrigRefNo = l.OrigRefNo;
                    XpressHisTBL.RcvAgentTxnRefNo = l.RcvAgentTxnRefNo;
                    XpressHisTBL.Commission = l.Commission;
                    XpressHisTBL.PayoutAmount = l.PayoutAmount;
                    XpressHisTBL.AmountPaid = l.AmountPaid;
                    XpressHisTBL.Currency = l.Currency;
                    XpressHisTBL.AmountInUSD = l.AmountInUSD;
                    XpressHisTBL.NoofTxn = l.NoofTxn;
                    XpressHisTBL.ReconDate = l.ReconDate;
                    XpressHisTBL.PullDate = l.PullDate;
                    XpressHisTBL.MatchingStatus = l.MatchingStatus;
                    XpressHisTBL.UserId = l.UserId;
                    XpressHisTBL.DateMoved = DateTime.Now;
                    XpressHisTBL.MatchingType = l.MatchingType;
                    XpressHisTBL.FileName = l.FileName;


                    repoXPressMoneyTransHistoryRepository.Add(XpressHisTBL);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }

                    //delete
                    var del = await repoXPressMoneyRepository.GetAsync(d => d.ItbId == l.ItbId);
                    if (del != null)
                    {
                        repoXPressMoneyRepository.Delete(del);
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
                LogManager.SaveLog("An error occured CBSTranAndConsortiumTransHistory1 function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
            }

            LogManager.SaveLog("CBSTranAndConsortiumTransHistory End");
            return retVal;
        }
        #endregion

        #region Wetern Union Money Transfer
        public async Task<ReturnValues> MatchCBSWUMTAndWUMTTrans()
        {
            LogManager.SaveLog("MatchCBSWUMTAndWUMTTrans Start");
            string ReconTypeName = string.Empty;
            int ConsortiumId = 0;
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
                    if (recName.WSReconName == "WUMT")
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
                        var CBSWUMTList = await repoCBSWUMTTransRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());

                        foreach (var p in CBSWUMTList)
                        {
                            var CBSWUMTT1 = await repoCBSWUMTTransRepository.GetAsync(q => q.Reference == p.Reference && q.MatchingStatus == "N");
                            var WUMTT1 = await repoWUMTTransRepository.GetAsync(h => h.TransReference == p.Reference && h.MatchingStatus == "N");

                            if (CBSWUMTT1 != null && WUMTT1 != null)
                            {
                                if (CBSWUMTT1.Reference == WUMTT1.TransReference)
                                {
                                   //LogManager.SaveLog("CBSWUMTT1 Both are matched CBS itbId: " + CBSWUMTT1.ItbId + "WUMTT1 ItbId :" + WUMTT1.ItbId);
                                    decimal chkDec;
                                    var calMG = WUMTT1.LcySettledPrincipal == null ? 0 : decimal.TryParse(WUMTT1.LcySettledPrincipal.ToString(), out chkDec) ? -1 * Convert.ToDecimal(WUMTT1.LcySettledPrincipal) : 0;
                                     if (CBSWUMTT1.Amount == calMG)
                                     {
                                         CBSWUMTT1.ReconDate = DateTime.Now;
                                         CBSWUMTT1.MatchingStatus = "M";
                                         CBSWUMTT1.MatchingType = "SYSTEM";
                                         repoCBSWUMTTransRepository.Update(CBSWUMTT1);
                                         ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                         if (ret)
                                         {
                                         }

                                         WUMTT1.ReconDate = DateTime.Now;
                                         WUMTT1.MatchingStatus = "M";
                                         WUMTT1.MatchingType = "SYSTEM";
                                         repoWUMTTransRepository.Update(WUMTT1);
                                         ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                         if (ret)
                                         {
                                         }
                                     }
                                }
                            }
                        }
                    }
                    #endregion
                    var rectype = repoReconTypeRepository.Get(c => c.WSReconName == "WUMT");
                    var controlTablecbs = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSWUMTTrans");
                    if (controlTablecbs != null)
                    {
                        controlTablecbs.LastTimeMatched = DateTime.Now;
                        repoadmDataPoollingControlRepository.Update(controlTablecbs);
                        var retww = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (retww)
                        {

                        }
                    }

                    var controlTablecon = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "WUMTTrans");
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
                LogManager.SaveLog("An error occured MatchCBSWUMTAndWUMTTrans function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
            }

            LogManager.SaveLog("MatchCBSWUMTAndWUMTTrans End");
            return retVal;
        }
        public async Task<ReturnValues> CBSWUMTTransHistory()
        {
            LogManager.SaveLog("CBSWUMTTransHistory Start");
            var CBSWUMTTRansHis = new CBSWUMTTransHistory();
            var WUMTTransHistoryHis = new WUMTTransHistory();
            try
            {
                var CBSMG = await repoCBSWUMTTransRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var p in CBSMG)
                {
                    CBSWUMTTRansHis.ItbId = p.ItbId;
                    CBSWUMTTRansHis.AcctNo = p.AcctNo;
                    CBSWUMTTRansHis.Amount = p.Amount;
                    CBSWUMTTRansHis.DrCr = p.DrCr;
                    CBSWUMTTRansHis.Description = p.Description;
                    CBSWUMTTRansHis.TransDate = p.TransDate;
                    CBSWUMTTRansHis.OriginBranch = p.OriginBranch;
                    CBSWUMTTRansHis.Postedby = p.Postedby;
                    CBSWUMTTRansHis.Balance = p.Balance;
                    CBSWUMTTRansHis.Reference = p.Reference;
                    CBSWUMTTRansHis.OrigRefNo = p.OrigRefNo;
                    CBSWUMTTRansHis.PtId = p.PtId;
                    CBSWUMTTRansHis.UserId = p.UserId;
                    CBSWUMTTRansHis.MatchingStatus = p.MatchingStatus;
                    CBSWUMTTRansHis.ReconDate = p.ReconDate;
                    CBSWUMTTRansHis.PullDate = p.PullDate;
                    CBSWUMTTRansHis.DateMoved = DateTime.Now;
                    CBSWUMTTRansHis.MatchingType  = p.MatchingType;
                    CBSWUMTTRansHis.ReversalCode = p.ReversalCode;
                    repoCBSWUMTTransHistoryRepository.Add(CBSWUMTTRansHis);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }

                    //Delete 
                    var del = await repoCBSWUMTTransRepository.GetAsync(d => d.ItbId == p.ItbId);
                    if (del != null)
                    {
                        repoCBSWUMTTransRepository.Delete(del);
                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                        }
                    }
                }

                var MGTRanList = await repoWUMTTransRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());


                foreach (var r in MGTRanList)
                {
                    WUMTTransHistoryHis.ItbId = r.ItbId;
                    WUMTTransHistoryHis.AgentCode = r.AgentCode;
                    WUMTTransHistoryHis.SentDate = r.SentDate;
                    WUMTTransHistoryHis.TransReference = r.TransReference;
                    WUMTTransHistoryHis.OrigRefNo = r.OrigRefNo;
                    WUMTTransHistoryHis.Spread = r.Spread;
                    WUMTTransHistoryHis.SettleRate = r.SettleRate;
                    WUMTTransHistoryHis.PayoutRate = r.PayoutRate;
                    WUMTTransHistoryHis.SentCurrencyCode = r.SentCurrencyCode;
                    WUMTTransHistoryHis.SentPrincipal = r.SentPrincipal;
                    WUMTTransHistoryHis.SentCharges = r.SentCharges;
                    WUMTTransHistoryHis.SettledPrincipal = r.SettledPrincipal;
                    WUMTTransHistoryHis.SettledCharges = r.SettledCharges;
                    WUMTTransHistoryHis.SettledFx = r.SettledFx;
                    WUMTTransHistoryHis.Total = r.Total;
                    WUMTTransHistoryHis.PaidDate = r.PaidDate;
                    WUMTTransHistoryHis.SentCountry = r.SentCountry;
                    WUMTTransHistoryHis.LcySettleRate = r.LcySettleRate;
                    WUMTTransHistoryHis.LcyPayoutRate = r.LcyPayoutRate;
                    WUMTTransHistoryHis.LcyCurrencyCode = r.LcyCurrencyCode;
                    WUMTTransHistoryHis.LcySettledPrin = r.LcySettledPrin;
                    WUMTTransHistoryHis.LcyCharges = r.LcyCharges;
                    WUMTTransHistoryHis.LcySettledPrincipal = r.LcySettledPrincipal;
                    WUMTTransHistoryHis.LcySettledCharges = r.LcySettledCharges;
                    WUMTTransHistoryHis.lcySettledFx = r.lcySettledFx;
                    WUMTTransHistoryHis.lcyTotal = r.lcyTotal;
                    WUMTTransHistoryHis.MatchingStatus = r.MatchingStatus;
                    WUMTTransHistoryHis.ReconDate = r.ReconDate;
                    WUMTTransHistoryHis.PullDate = r.PullDate;
                    WUMTTransHistoryHis.UserId = r.UserId;
                    WUMTTransHistoryHis.DateMoved = DateTime.Now;
                    WUMTTransHistoryHis.FileName = r.FileName;
                    WUMTTransHistoryHis.MatchingType = r.MatchingType;

                    repoWUMTTransHistoryRepository.Add(WUMTTransHistoryHis);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }

                    var del = await repoWUMTTransRepository.GetAsync(d => d.ItbId == r.ItbId);
                    if (del != null)
                    {
                        repoWUMTTransRepository.Delete(del);
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
                LogManager.SaveLog("An error occured CBSMGTransAndMGTransHistory function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
            }

            LogManager.SaveLog("CBSMGTransAndMGTransHistory End");
            return retVal;
        }

        #endregion

        #region Money Gram
        public async Task<ReturnValues> MatchCBSMGAndMGTrans()
        {
            LogManager.SaveLog("MatchCBSMGAndMGTrans Start");
            string ReconTypeName = string.Empty;
            int ConsortiumId = 0;
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
                    if (recName.WSReconName == "MoneyGram")
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
                        var CBSMGList = await repoCBSMGTransRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());

                        foreach (var p in CBSMGList)
                        {
                            var CBSMG1 = await repoCBSMGTransRepository.GetAsync(q => q.Reference == p.Reference && q.MatchingStatus == "N");
                            var MGTranT1 =  await repoMGTransRepository.GetAsync(h => h.RefNo == p.Reference && h.MatchingStatus == "N");

                            if (CBSMG1 != null && MGTranT1 != null)
                            {
                                if (CBSMG1.Reference == MGTranT1.RefNo)
                                {
                                   // LogManager.SaveLog("CBSWUMTT1 Both are matched CBS itbId: " + CBSMG1.ItbId + "MGTranT1 ItbId :" + MGTranT1.ItbId);

                                    var calMG = -1 * MGTranT1.BaseAmount;
                                    if (CBSMG1.Amount == calMG)
                                    {
                                        CBSMG1.ReconDate = DateTime.Now;
                                        CBSMG1.MatchingStatus = "M";
                                        CBSMG1.MatchingType = "SYSTEM";
                                        repoCBSMGTransRepository.Update(CBSMG1);
                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }

                                        MGTranT1.ReconDate = DateTime.Now;
                                        MGTranT1.MatchingStatus = "M";
                                        MGTranT1.MatchingType = "SYSTEM";
                                        repoMGTransRepository.Update(MGTranT1);
                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }

                                    }
                                }
                            }
                        }
                   }
                    #endregion
                    var rectype = repoReconTypeRepository.Get(c => c.WSReconName == "MoneyGram");
                    var controlTablecbs = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSMGTrans");
                    if (controlTablecbs != null)
                    {
                        controlTablecbs.LastTimeMatched = DateTime.Now;
                        repoadmDataPoollingControlRepository.Update(controlTablecbs);
                        var retww = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (retww)
                        {
                        }
                    }

                    var controlTablecon = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "MGTrans");
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
                LogManager.SaveLog("An error occured MatchCBSWUMTAndWUMTTrans function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
                throw;
            }

            LogManager.SaveLog("MatchCBSMGAndMGTrans End");
            return retVal;
        }

        public async Task<ReturnValues> CBSMGTransAndMGTransHistory()
        {
            LogManager.SaveLog("CBSMGTransAndMGTransHistory Start");
            var CBSMGTRansHis = new CBSMGTransHistory();
            var MGTransHist = new MGTransHistory();
            try
            {
                var CBSMG = await repoCBSMGTransRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var p in CBSMG)
                {
                    CBSMGTRansHis.ItbId = p.ItbId;
                    CBSMGTRansHis.AcctNo = p.AcctNo;
                    CBSMGTRansHis.Amount = p.Amount;
                    CBSMGTRansHis.DebitCredit = p.DebitCredit;
                    CBSMGTRansHis.Description = p.DebitCredit;
                    CBSMGTRansHis.TransDate = p.TransDate;
                    CBSMGTRansHis.OriginBranch = p.OriginBranch;
                    CBSMGTRansHis.PostedBy = p.OriginBranch;
                    CBSMGTRansHis.Balance = p.Balance;
                    CBSMGTRansHis.Reference = p.Reference;
                    CBSMGTRansHis.OrigRefNo = p.OrigRefNo;
                    CBSMGTRansHis.PtId = p.PtId;
                    CBSMGTRansHis.ReconDate = p.ReconDate;
                    CBSMGTRansHis.PullDate = p.PullDate;
                    CBSMGTRansHis.MatchingStatus = p.MatchingStatus;
                    CBSMGTRansHis.UserId = p.UserId;
                    CBSMGTRansHis.DateMoved = DateTime.Now;
                    CBSMGTRansHis.MatchingType = p.MatchingType;
                    CBSMGTRansHis.ReversalCode = p.ReversalCode;

                    repoCBSMGHistoryRepository.Add(CBSMGTRansHis);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }

                    //Delete 
                    var del = await repoCBSMGTransRepository.GetAsync(d => d.ItbId == p.ItbId);
                    if (del != null)
                    {
                        repoCBSMGTransRepository.Delete(del);
                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                        }
                    }
                }

                var MGTRanList = await repoMGTransRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());


                foreach (var r in MGTRanList)
                {
                    MGTransHist.ItbId = r.ItbId;
                    MGTransHist.SettlementCurrency = r.SettlementCurrency;
                    MGTransHist.TransactionCurrency = r.TransactionCurrency;
                    MGTransHist.AccountNo = r.AccountNo;
                    MGTransHist.LegacyID = r.LegacyID;
                    MGTransHist.OrigRefNo = r.OrigRefNo;
                    MGTransHist.TransactionDate = r.TransactionDate;
                    MGTransHist.TransactionId = r.TransactionId;
                    MGTransHist.RefNo = r.RefNo;
                    MGTransHist.Prod = r.Prod;
                    MGTransHist.TransactionType = r.TransactionType;
                    MGTransHist.OrigCountry = r.OrigCountry;
                    MGTransHist.RevCountry = r.RevCountry;
                    MGTransHist.FXRate = r.FXRate;
                    MGTransHist.FXExchngRate = r.FXExchngRate;
                    MGTransHist.FXDate = r.FXDate;
                    MGTransHist.FxExchangeDate = r.FxExchangeDate;
                    MGTransHist.FXMargin = r.FXMargin;
                    MGTransHist.Direction = r.Direction;
                    MGTransHist.XchangeDirection = r.XchangeDirection;
                    MGTransHist.BaseAmount = r.BaseAmount;
                    MGTransHist.ExchangeAmount = r.ExchangeAmount;
                    MGTransHist.FeeAmount = r.FeeAmount;
                    MGTransHist.ExchangeFeeAmount = r.ExchangeFeeAmount;
                    MGTransHist.ShareAmount = r.ShareAmount;
                    MGTransHist.ExchangeShareAmount = r.ExchangeShareAmount;
                    MGTransHist.CommissionAmount = r.CommissionAmount;
                    MGTransHist.ExchangeComAmount = r.ExchangeComAmount;
                    MGTransHist.MatchingStatus = r.MatchingStatus;
                    MGTransHist.ReconDate = r.ReconDate;
                    MGTransHist.UserId = r.UserId;
                    MGTransHist.DateMoved = DateTime.Now;
                    MGTransHist.FileName = r.FileName;
                    MGTransHist.MatchingType = r.MatchingType;


                    repoMGTRansHistoryRepository.Add(MGTransHist);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }

                    var del = await repoRiaRepository.GetAsync(d => d.ItbId == r.ItbId);
                    if (del != null)
                    {
                        repoRiaRepository.Delete(del);
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
                LogManager.SaveLog("An error occured CBSMGTransAndMGTransHistory function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
                throw;
            }

            LogManager.SaveLog("CBSMGTransAndMGTransHistory End");
            return retVal;
        }

        #endregion

        #region Unity Link
        public async Task<ReturnValues> MatchUnityLink()
        {
            LogManager.SaveLog("MatchUnityLink Start");
            string ReconTypeName = string.Empty;
            int ConsortiumId = 0;
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
                    if (recName.WSReconName == "UnityLink")
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
                        var CBS = await repoCBSUnityLinkTransRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());

                        foreach (var p in CBS)
                        {
                            var CBS1 = await repoCBSUnityLinkTransRepository.GetAsync(q => q.Reference == p.Reference && q.MatchingStatus == "N");
                            var ULTran = await repoUnityLinkTransRepository.GetAsync(h => h.TransRef == p.Reference && h.MatchingStatus == "N");

                            if (CBS1 != null && ULTran != null)
                            {
                                if (CBS1.Reference == ULTran.TransRef)
                                {
                                    //LogManager.SaveLog("CBSUnityLink Both are matched CBS itbId: " + CBS1.ItbId + "ULTran ItbId :" + ULTran.ItbId);

                                    CBS1.ReconDate = DateTime.Now;
                                    CBS1.MatchingStatus = "M";
                                    CBS1.MatchingType = "SYSTEM";
                                    repoCBSUnityLinkTransRepository.Update(CBS1);
                                    ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                    if (ret)
                                    {
                                    }
                                    ULTran.ReconDate = DateTime.Now;
                                    ULTran.MatchingStatus = "M";
                                    ULTran.MatchingType = "SYSTEM";
                                    repoUnityLinkTransRepository.Update(ULTran);
                                    ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                    if (ret)
                                    {
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    var rectype = repoReconTypeRepository.Get(c => c.WSReconName == "UnityLink");
                    var controlTablecbs = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSUnityLinkTrans");
                    if (controlTablecbs != null)
                    {
                        controlTablecbs.LastTimeMatched = DateTime.Now;
                        repoadmDataPoollingControlRepository.Update(controlTablecbs);
                        var retww = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (retww)
                        {

                        }
                    }

                    var controlTablecon = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "UnityLinkTrans");
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
                LogManager.SaveLog("An error occured MatchCBSWUMTAndWUMTTrans function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
                throw;
            }

            LogManager.SaveLog("MatchCBSMGAndMGTrans End");
            return retVal;
        }
        public async Task<ReturnValues> MoveUnityTransToHistory()
        {
            LogManager.SaveLog("MoveUnityTransToHistory Start");

            var cbsUnityHis = new CBSUnityLinkTransHistory();

            var unityLinkHisTBL = new UnityLinkTransHistory();
            try
            {
                var CBS = await repoCBSUnityLinkTransRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var p in CBS)
                {
                  cbsUnityHis.ItbId = p.ItbId;
                  cbsUnityHis.AcctNo = p.AcctNo ;
                  cbsUnityHis.Amount = p.Amount ;
                  cbsUnityHis.DebitCredit = p.DebitCredit ;
                  cbsUnityHis.Description = p.Description ;
                  cbsUnityHis.TransDate = p.TransDate ;
                  cbsUnityHis.OriginBranch = p.OriginBranch ;
                  cbsUnityHis.PostedBy = p.PostedBy ;
                  cbsUnityHis.Balance = p.Balance ;
                  cbsUnityHis.PtId = p.PtId ;
                  cbsUnityHis.Reference = p.Reference ;
                  cbsUnityHis.OrigRefNo = p.OrigRefNo;
                  cbsUnityHis.MatchingStatus = p.MatchingStatus;
                  cbsUnityHis.UserId = p.UserId ;
                  cbsUnityHis.ReconDate = p.ReconDate ;
                  cbsUnityHis.PullDate = p.PullDate;
                  cbsUnityHis.MatchingType = p.MatchingType ;
                  cbsUnityHis.DateMoved = DateTime.Now;
                  cbsUnityHis.ReversalCode = p.ReversalCode;

                  repoCBSUnityLinkTransHistoryRepository.Add(cbsUnityHis);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }
                    //Delete 
                    var del = await repoCBSUnityLinkTransRepository.GetAsync(d => d.ItbId == p.ItbId);
                    if (del != null)
                    {
                        repoCBSUnityLinkTransRepository.Delete(del);
                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                        }
                    }
                }

                var MGTRanList = await repoUnityLinkTransRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());

                foreach (var r in MGTRanList)
                {
                      unityLinkHisTBL.ItbId = r.ItbId;
                      unityLinkHisTBL.TransDate = r.TransDate ;
                      unityLinkHisTBL.TransRef = r.TransRef ;
                      unityLinkHisTBL.OrigRefNo = r.OrigRefNo;
                      unityLinkHisTBL.TransType = r.TransType ;
                      unityLinkHisTBL.Remitter = r.Remitter ;
                      unityLinkHisTBL.Beneficiary = r.Beneficiary ;
                      unityLinkHisTBL.SourceAmount = r.SourceAmount ;
                      unityLinkHisTBL.ExchangeRate = r.ExchangeRate ;
                      unityLinkHisTBL.DestAmount = r.DestAmount ;
                      unityLinkHisTBL.ProcessedbyAndDate = r.ProcessedbyAndDate ;
                      unityLinkHisTBL.MatchingStatus = r.MatchingStatus ;
                      unityLinkHisTBL.ReconDate = r.ReconDate ;
                      unityLinkHisTBL.PullDate = r.PullDate;
                      unityLinkHisTBL.UserId = r.UserId ;
                      unityLinkHisTBL.MatchingType = r.MatchingType;
                      unityLinkHisTBL.DateMoved= DateTime.Now;
                      unityLinkHisTBL.FileName = r.FileName;
                      repoUnityLinkTransHistoryRepository.Add(unityLinkHisTBL);
                      
                      var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                      if (ret)
                      {
                      }

                     var del = await repoUnityLinkTransRepository.GetAsync(d => d.ItbId == r.ItbId);
                     if (del != null)
                     {
                        repoUnityLinkTransRepository.Delete(del);
                        var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret1)
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
                LogManager.SaveLog("An error occured MoveUnityTransToHistory  in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
                throw;
            }

            LogManager.SaveLog("MoveUnityTransToHistory End");
            return retVal;
        }

        #endregion

        #region MTN 
        public async Task<ReturnValues> MatchMTN()
        {
            LogManager.SaveLog("MatchMTN Start");
            string ReconTypeName = string.Empty;
            int ConsortiumId = 0;
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
                    if (recName.WSReconName == "MTN")
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
                        var CBS = await repoCBSMTNTransRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());
                        LogManager.SaveLog("MatchMTN Start");
                        foreach (var p in CBS)
                        {
                            var CBS1 = await repoCBSMTNTransRepository.GetAsync(q => q.Reference == p.Reference && q.MatchingStatus == "N");
                            var MTNTran = await repoMTNTRansRepository.GetAsync(h => h.TransId == p.Reference && h.MatchingStatus == "N");

                            if (CBS1 != null && MTNTran != null)
                            {
                                if (CBS1.Reference == MTNTran.TransId)
                                {
                                    decimal chkDec;
                                    var calMG = MTNTran.Amount == null ? 0 : decimal.TryParse(MTNTran.Amount.ToString(), out chkDec) ? -1 * Convert.ToDecimal(MTNTran.Amount) : 0;

                                    if (calMG == CBS1.Amount)
                                    {
                                        CBS1.ReconDate = DateTime.Now;
                                        CBS1.MatchingStatus = "M";
                                        CBS1.MatchingType = "SYSTEM";
                                        repoCBSMTNTransRepository.Update(CBS1);
                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }
                                        MTNTran.ReconDate = DateTime.Now;
                                        MTNTran.MatchingStatus = "M";
                                        MTNTran.MatchingType = "SYSTEM";
                                        repoMTNTRansRepository.Update(MTNTran);
                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }
                                    }

                                    else 
                                        if (CBS1.Amount == MTNTran.Amount)
                                    {
                                        CBS1.ReconDate = DateTime.Now;
                                        CBS1.MatchingStatus = "M";
                                        CBS1.MatchingType = "SYSTEM";
                                        repoCBSMTNTransRepository.Update(CBS1);
                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }
                                        MTNTran.ReconDate = DateTime.Now;
                                        MTNTran.MatchingStatus = "M";
                                        MTNTran.MatchingType = "SYSTEM";
                                        repoMTNTRansRepository.Update(MTNTran);
                                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                        if (ret)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    var rectype = repoReconTypeRepository.Get(c => c.WSReconName == "MTN");
                    var controlTablecbs = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSMTNTrans");
                    if (controlTablecbs != null)
                    {
                        controlTablecbs.LastTimeMatched = DateTime.Now;
                        repoadmDataPoollingControlRepository.Update(controlTablecbs);
                        var retww = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (retww)
                        {

                        }
                    }

                    var controlTablecon = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "MTNTrans");
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
                LogManager.SaveLog("An error occured MatchCBSWUMTAndWUMTTrans function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
                throw;
            }

            LogManager.SaveLog("Match MTN Ends");
            return retVal;
        }
        public async Task<ReturnValues> MoveMTNTransToHistory()
        {
            LogManager.SaveLog("MoveMTNTransToHistory Start");

            var cbsMTN = new CBSMTNTransHistory();

            var MTNHisTBL = new MTNTRansHistory();
            try
            {
                var CBS = await repoCBSMTNTransRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var p in CBS)
                {
                      cbsMTN.ItbId = p.ItbId;
                      cbsMTN.AcctNo = p.AcctNo ;
                      cbsMTN.Amount = p.Amount ;
                      cbsMTN.DebitCredit = p.DebitCredit ;
                      cbsMTN.Description = p.Description ;
                      cbsMTN.TransDate = p.TransDate ;
                      cbsMTN.OriginBranch = p.OriginBranch ;
                      cbsMTN.PostedBy = p.PostedBy ;
                      cbsMTN.Balance = p.Balance ;
                      cbsMTN.Reference = p.Reference ;
                      cbsMTN.OrigRefNo = p.OrigRefNo;
                      cbsMTN.PtId = p.PtId ;
                      cbsMTN.ReconDate = p.ReconDate ;
                      cbsMTN.PullDate = p.PullDate;
                      cbsMTN.MatchingStatus = p.MatchingStatus ;
                      cbsMTN.UserId = p.UserId ;
                      cbsMTN.MatchingType = p.MatchingType ;
                      cbsMTN.DateMoved = DateTime.Now;
                      cbsMTN.ReversalCode = p.ReversalCode;


                       repoCBSMTNTransHistoryRepository.Add(cbsMTN);
                       var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                       if (ret)
                       {
                       }
                    //Delete 
                       var del = await repoCBSMTNTransRepository.GetAsync(d => d.ItbId == p.ItbId);
                      if (del != null)
                      {
                        repoCBSMTNTransRepository.Delete(del);
                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                        }
                      }
                }

                var MTNTRanList = await repoMTNTRansRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());

                foreach (var r in MTNTRanList)
                {
                   
                      MTNHisTBL.ItbId = r.ItbId ;
                      MTNHisTBL.TransId = r.TransId ;
                      MTNHisTBL.OrigRefNo = r.OrigRefNo;
                      MTNHisTBL.ExternalTransId = r.ExternalTransId ;
                      MTNHisTBL.Date = r.Date ;
                      MTNHisTBL.Status = r.Status;
                      MTNHisTBL.Type = r.Type ;
                      MTNHisTBL.ProviderCategory = r.ProviderCategory ;
                      MTNHisTBL.ToMsg = r.ToMsg ;
                      MTNHisTBL.From = r.From ;
                      MTNHisTBL.FromName = r.FromName ;
                      MTNHisTBL.To = r.To ;
                      MTNHisTBL.ToName = r.ToName ;
                      MTNHisTBL.InitiatedBy = r.InitiatedBy ;
                      MTNHisTBL.OnBehalfOf = r.OnBehalfOf ;
                      MTNHisTBL.Amount = r.Amount ;
                      MTNHisTBL.Currency1 = r.Currency1 ;
                      MTNHisTBL.Fee = r.Fee ;
                      MTNHisTBL.Currency2 = r.Currency2 ;
                      MTNHisTBL.Discount = r.Discount ;
                      MTNHisTBL.Currency3 = r.Currency3 ;
                      MTNHisTBL.Promotion = r.Promotion ;
                      MTNHisTBL.Currency4 = r.Currency4 ;
                      MTNHisTBL.Coupon = r.Coupon ;
                      MTNHisTBL.Currency5 = r.Currency5 ;
                      MTNHisTBL.Balance = r.Balance ;
                      MTNHisTBL.Currency6 = r.Currency6 ;
                      MTNHisTBL.Information = r.Information ;
                      MTNHisTBL.ReconDate = r.ReconDate ;
                      MTNHisTBL.PullDate = r.PullDate;
                      MTNHisTBL.MatchingStatus = r.MatchingStatus ;
                      MTNHisTBL.UserId = r.UserId ;
                      MTNHisTBL.MatchingType = r.MatchingType ;
                      MTNHisTBL.DateMoved = DateTime.Now;
                      MTNHisTBL.FileName = r.FileName;

                      repoMTNTRansHistoryRepository.Add(MTNHisTBL);

                      var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                      if (ret)
                      {
                      }

                    var del = await repoMTNTRansRepository.GetAsync(d => d.ItbId == r.ItbId);
                    if (del != null)
                    {
                        repoMTNTRansRepository.Delete(del);
                        var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret1)
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
                LogManager.SaveLog("An error occured MoveUnityTransToHistory  in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
                throw;
            }

            LogManager.SaveLog("MoveUnityTransToHistory End");
            return retVal;
        }
        #endregion

        #region AIRTEL  
        public async Task<ReturnValues> MatchAirtel()
        {
            LogManager.SaveLog("MatchAirtel Start");
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
                    if (recName.WSReconName == "AIRTEL")
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
                        var CBS = await repoCBSAirtelTransRepository.GetManyAsync(c => c.MatchingStatus == "N".Trim());

                        foreach (var p in CBS)
                        {
                            var CBS1 = await repoCBSAirtelTransRepository.GetAsync(q => q.Reference == p.Reference && q.MatchingStatus == "N");
                            var Airtel = await repoAIRTELTransRepository.GetAsync(h => h.TransactionId == p.Reference && h.MatchingStatus == "N");

                            if (CBS1 != null && Airtel != null)
                            {
                                if (CBS1.Reference == Airtel.TransactionId)
                                {
                                   // LogManager.SaveLog("Airtel Both are matched CBS itbId: " + CBS1.ItbId + "Airtel ItbId :" + Airtel.ItbId);

                                    CBS1.ReconDate = DateTime.Now;
                                    CBS1.MatchingStatus = "M";
                                    CBS1.MatchingType = "SYSTEM";
                                    repoCBSAirtelTransRepository.Update(CBS1);
                                    ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                    if (ret)
                                    {
                                    }
                                    Airtel.ReconDate = DateTime.Now;
                                    Airtel.MatchingStatus = "M";
                                    Airtel.MatchingType = "SYSTEM";
                                    repoAIRTELTransRepository.Update(Airtel);
                                    ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                                    if (ret)
                                    {
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                    var rectype = repoReconTypeRepository.Get(c => c.WSReconName == "AIRTEL");
                    var controlTablecbs = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "CBSAirtelTrans");
                    if (controlTablecbs != null)
                    {
                        controlTablecbs.LastTimeMatched = DateTime.Now;
                        repoadmDataPoollingControlRepository.Update(controlTablecbs);
                        var retww = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (retww)
                        {
                        }
                    }

                    var controlTablecon = repoadmDataPoollingControlRepository.Get(c => c.ReconTypeId == rectype.ReconTypeId && c.TableName == "AirtelTrans");
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
                LogManager.SaveLog("An error occured MatchAirtel function in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
                throw;
            }

            LogManager.SaveLog("MatchAirtel End");
            return retVal;
        }
        public async Task<ReturnValues> MoveAirtelTransToHistory()
        {
            LogManager.SaveLog("MoveAIRTELTransToHistory Start");

            var cbsAirtelHis = new CBSAirtelTransHistory(); 

            var AIRTELHisTBL = new AirtelTransHistory();
            try
            {
                var CBS = await repoCBSAirtelTransRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());
                foreach (var p in CBS)
                {
                    cbsAirtelHis.ItbId = p.ItbId;
                    cbsAirtelHis.AcctNo = p.AcctNo ;       
                    cbsAirtelHis.Amount = p.Amount ;
                    cbsAirtelHis.DebitCredit = p.DebitCredit ;
                    cbsAirtelHis.Description = p.Description ;
                    cbsAirtelHis.TransDate = p.TransDate ;
                    cbsAirtelHis.OriginBranch = p.OriginBranch ;
                    cbsAirtelHis.PostedBy = p.PostedBy ;
                    cbsAirtelHis.Balance = p.Balance ;
                    cbsAirtelHis.Reference = p.Reference ;
                    cbsAirtelHis.OrigRefNo = p.OrigRefNo;
                    cbsAirtelHis.TrfAcctType = p.TrfAcctType ;
                    cbsAirtelHis.TrfAcctNo = p.TrfAcctNo ;
                    cbsAirtelHis.PtId = p.PtId ;
                    cbsAirtelHis.MatchingStatus = p.MatchingStatus ;
                    cbsAirtelHis.ReconDate = p.ReconDate ;
                    cbsAirtelHis.PullDate = p.PullDate;
                    cbsAirtelHis.UserId = p.UserId ;
                    cbsAirtelHis.MatchingType = p.MatchingType ;
                    cbsAirtelHis.DateMoved = DateTime.Now;
                    cbsAirtelHis.ReversalCode = p.ReversalCode;

                    repoCBSAirtelTransHistoryRepository.Add(cbsAirtelHis);
                    var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                    if (ret)
                    {
                    }
                    //Delete 
                    var del = await repoCBSAirtelTransRepository.GetAsync(d => d.ItbId == p.ItbId);
                    if (del != null)
                    {
                        repoCBSAirtelTransRepository.Delete(del);
                        ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret)
                        {
                        }
                    }
                }

                var AIRTELTRanList = await repoAIRTELTransRepository.GetManyAsync(c => c.MatchingStatus == "M".Trim());

                foreach (var r in AIRTELTRanList)
                {
                      AIRTELHisTBL.ItbId = r.ItbId ;
                      AIRTELHisTBL.RecordNo = r.RecordNo ;
                      AIRTELHisTBL.TransactionId = r.TransactionId ;
                      AIRTELHisTBL.OrigRefNo = r.OrigRefNo;
                      AIRTELHisTBL.TransactionDate = r.TransactionDate ;
                      AIRTELHisTBL.PayerMFSProvider = r.PayerMFSProvider ;
                      AIRTELHisTBL.PayerPaymentInstrument = r.PayerPaymentInstrument ;
                      AIRTELHisTBL.PayerWallet = r.PayerWallet ;
                      AIRTELHisTBL.FeeServiceChargeTax1 = r.FeeServiceChargeTax1 ;
                      AIRTELHisTBL.FeeServiceChargeTax2 = r.FeeServiceChargeTax2 ;
                      AIRTELHisTBL.PayerBankAcctNo = r.PayerBankAcctNo ;
                      AIRTELHisTBL.UserCategory = r.UserCategory ;
                      AIRTELHisTBL.UserGrade = r.UserGrade ;
                      AIRTELHisTBL.PayeeMFSProvider = r.PayeeMFSProvider ;
                      AIRTELHisTBL.PayeePaymentInstrument = r.PayeePaymentInstrument ;
                      AIRTELHisTBL.PayeWalletType = r.PayeWalletType ;
                      AIRTELHisTBL.PayeeBankAcctNo = r.PayeeBankAcctNo ;
                      AIRTELHisTBL.ReceiverCategory = r.ReceiverCategory ;
                      AIRTELHisTBL.ReceiverGrade = r.ReceiverGrade ;
                      AIRTELHisTBL.ServiceType = r.ServiceType ;
                      AIRTELHisTBL.TransactionType = r.TransactionType ;
                      AIRTELHisTBL.TransactionAmount = r.TransactionAmount ;
                      AIRTELHisTBL.PayerPreviousBalance = r.PayerPreviousBalance ;
                      AIRTELHisTBL.PayerPostBalance = r.PayerPostBalance ;
                      AIRTELHisTBL.PayeePreBalance = r.PayeePreBalance ;
                      AIRTELHisTBL.PayeePostBalance = r.PayeePostBalance ;
                      AIRTELHisTBL.ReferenceNo = r.ReferenceNo ;
                      AIRTELHisTBL.ExternalRefNo = r.ExternalRefNo ;
                      AIRTELHisTBL.MatchingStatus = r.MatchingStatus ;
                      AIRTELHisTBL.ReconDate = r.ReconDate ;
                      AIRTELHisTBL.PullDate = r.PullDate;
                      AIRTELHisTBL.UserId = r.UserId ;
                      AIRTELHisTBL.MatchingType = r.MatchingType;
                      AIRTELHisTBL.DateMoved = DateTime.Now;
                      AIRTELHisTBL.FileName = r.FileName;

                      repoAIRTELTransHistoryRepository.Add(AIRTELHisTBL);

                      var ret = await unitOfWork.Commit(0, null) > 0 ? true : false;
                      if (ret)
                      {
                      }

                    var del = await repoAIRTELTransRepository.GetAsync(d => d.ItbId == r.ItbId);
                    if (del != null)
                    {
                        repoAIRTELTransRepository.Delete(del);
                        var ret1 = await unitOfWork.Commit(0, null) > 0 ? true : false;
                        if (ret1)
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
                LogManager.SaveLog("An error occured MoveAIRTELTransToHistory  in Line:" + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                return retVal;
                throw;
            }

            LogManager.SaveLog("MoveAIRTELTransToHistory End");
            return retVal;
        }
        #endregion NostroVostro
    }
}