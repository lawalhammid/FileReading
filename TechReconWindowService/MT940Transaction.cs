using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReconWindowService
{
   /// <summary>
/// Summary description for MT940Transaction
/// </summary>
 public class MT940Transaction
    {
        public SequenceAData SeqA { get; set; }
        public List<SequenceBData> SeqB { get; set; }
        public SequenceCData SeqC { get; set; }

        public class SequenceAData
        {
            public string TransactionReferenceNumber_M { get; set; }
            public string RelatedReference_O { get; set; }
            public string AccountIdentification_M { get; set; }
            public string StatementNumber_SequenceNumber_M { get; set; }
            public string OpeningBalance_M { get; set; }

            public string TagTransactionReferenceNumber;
            public string TagRelatedReference;
            public string TagAccountIdentification;
            public string TagStatementNumber_SequenceNumber;
            public string TagOpeningBalance;
        }
        public class SequenceBData
        {
            public string StatementLine_O { get; set; }
            public string InformationToAccountOwner_O { get; set; }

            public string TagStatementLine;
            public string TagInformationToAccountOwner;
        }
        public class SequenceCData
        {
            public string ClosingBalance_BookedFunds_M { get; set; }
            public string ClosingAvailableBalance_AvailableFunds_O { get; set; }
            public string ForwardAvailableBalance_O { get; set; }

            public string TagClosingBalance_BookedFunds;
            public string TagClosingAvailableBalance_AvailableFunds;
            public string TagForwardAvailableBalance;
        }

        public static class Tags
        {
            public const string TransactionReferenceNumber = "20";
            public const string RelatedReference = "21";
            public const string AccountIdentification = "25";
            public const string StatementNumber_SequenceNumber = "28C";
            public const string OpeningBalance_First = "60F";
            public const string OpeningBalance_Intermediate = "60M";
            public const string StatementLine = "61";
            public const string InformationToAccountOwner = "86";
            public const string ClosingBalance_BookedFunds_First = "62F";
            public const string ClosingBalance_BookedFunds_Intermediate = "62M";
            public const string ClosingAvailableBalance_AvailableFunds = "64";
            public const string ForwardAvailableBalance = "65";

        }

        public static class Prefix
        {

            public const string OpeningBalance_CreditBalance = "C";
            public const string OpeningBalance_DebitBalance = "D";


        }

            public MT940Transaction() {
            SeqA = new SequenceAData();
            SeqB = new List<SequenceBData>();
            SeqC = new SequenceCData();
        }

        public class SequenceA
        {
            public const string TransactionReferenceNumber = "20";
            public const string RelatedReference = "21";
            public const string AccountIdentification = "25";
            public const string StatementNumber_SequenceNumber = "28C";
            public const string OpeningBalance_First = "60F";
            public const string OpeningBalance_Intermediate = "60M";

        }


        //TRANSACTION RECURRING
        public class SequenceB
        {
            public const string StatementLine = "61";
            public const string InformationToAccountOwner = "86";
        }

        public class SequenceC
        {
            public const string ClosingBalance_BookedFunds_First = "62F";
            public const string ClosingBalance_BookedFunds_Intermediate = "62M";
            public const string ClosingAvailableBalance_AvailableFunds = "64";
            public const string ForwardAvailableBalance = "65";
        }

    }

    
}
