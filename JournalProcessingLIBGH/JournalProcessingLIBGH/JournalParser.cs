using Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace JournalProcessingLIBGH
{
    public class JournalParser
    {
        private JournalProcessorGH proc = new JournalProcessorGH();

        public p_transaction ProcessTranBlock(string tranblock, string ip, string tid, string termname, string brand, int brandid)
        {
            int num = 0;
            int num2 = num;
            switch (num2)
            {
                case 0:
                    {
                    IL_17:
                        switch (0)
                        {
                            case 0:
                                goto IL_E0;
                        }
                        p_transaction p_transaction;
                        List<comment> allCOmments;
                        string pANTSNRRN;
                        string text2;
                        string startandEndTime;
                        p_transaction result;
                        string datetimeTerminalId;
                        bool flag11;
                        string opCode;
                        while (true)
                        {
                        IL_21:
                            bool flag;
                            string availableBalance;
                            bool flag2;
                            string text;
                            string surcharge;
                            string errcode;
                            bool flag4;
                            bool arg_4FB_0;
                            bool flag6;
                            bool flag7;
                            bool flag9;
                            bool arg_3BE_0;
                            string currentLedgerBalance;
                            bool flag10;
                            string inquiry;
                            switch (num2)
                            {
                                case 0:
                                    goto IL_55C;
                                case 1:
                                    goto IL_433;
                                case 2:
                                IL_7CC:
                                    if (flag)
                                    {
                                        num = 11;
                                        num2 = num;
                                        continue;
                                    }
                                    goto IL_813;
                                case 3:
                                    p_transaction.availBal = new double?((availableBalance.Length > 0) ? double.Parse(availableBalance) : 0.0);
                                    num = 10;
                                    num2 = num;
                                    continue;
                                case 4:
                                    num = 44;
                                    num2 = num;
                                    continue;
                                case 5:
                                    p_transaction.tran_type = "PREPAID";
                                    num = 43;
                                    num2 = num;
                                    continue;
                                case 6:
                                    goto IL_76D;
                                case 7:
                                    p_transaction.tran_type = "INQUIRY";
                                    num = 1;
                                    num2 = num;
                                    continue;
                                case 8:
                                    if (flag2)
                                    {
                                        num = 1;
                                        if (num != 0)
                                        {
                                        }
                                        num = 7;
                                        num2 = num;
                                        continue;
                                    }
                                    goto IL_433;
                                case 9:
                                    text = this.proc.GetTransactionStatusNew(allCOmments, "", "", tranblock);
                                    num = 0;
                                    num2 = num;
                                    continue;
                                case 10:
                                    {
                                        p_transaction.surcharge = new double?((surcharge.Length > 0) ? double.Parse(surcharge) : 0.0);
                                        p_transaction.currencyCode = "GHS";
                                        p_transaction.errcode = errcode;
                                        p_transaction.trxn_status = text;
                                        bool flag3 = !string.IsNullOrWhiteSpace(pANTSNRRN);
                                        num = 30;
                                        num2 = num;
                                        continue;
                                    }
                                case 11:
                                    p_transaction.pan = this.proc.GetPAN(tranblock);
                                    num = 40;
                                    num2 = num;
                                    continue;
                                case 12:
                                    if (flag4)
                                    {
                                        num = 18;
                                        num2 = num;
                                        continue;
                                    }
                                    goto IL_33B;
                                case 13:
                                    {
                                        bool flag5;
                                        if (flag5)
                                        {
                                            num = 5;
                                            num2 = num;
                                            continue;
                                        }
                                        goto IL_323;
                                    }
                                case 14:
                                    if (!(text == ""))
                                    {
                                        num = 19;
                                        num2 = num;
                                        continue;
                                    }
                                    num = 22;
                                    num2 = num;
                                    continue;
                                case 15:
                                    goto IL_33B;
                                case 16:
                                    arg_4FB_0 = (text == null);
                                    goto IL_4FB;
                                case 17:
                                    if (flag6)
                                    {
                                        num = 23;
                                        num2 = num;
                                        continue;
                                    }
                                    goto IL_94B;
                                case 18:
                                    {
                                        text2 = this.proc.GetPrepaid(tranblock);
                                        bool flag5 = text2 != "";
                                        num = 13;
                                        num2 = num;
                                        continue;
                                    }
                                case 19:
                                    num = 16;
                                    num2 = num;
                                    continue;
                                case 20:
                                    goto IL_258;
                                case 21:
                                    if (flag7)
                                    {
                                        num = 9;
                                        num2 = num;
                                        continue;
                                    }
                                    goto IL_55C;
                                case 22:
                                    arg_4FB_0 = true;
                                    goto IL_4FB;
                                case 23:
                                    p_transaction.TransTimeRange = startandEndTime.Replace('&', '-');
                                    num = 34;
                                    num2 = num;
                                    continue;
                                case 24:
                                    goto IL_8E6;
                                case 25:
                                    return result;
                                case 26:
                                    {
                                        text2 = this.proc.GetTransfer(tranblock);
                                        bool flag8 = text2 != "";
                                        num = 38;
                                        num2 = num;
                                        continue;
                                    }
                                case 27:
                                    if (flag9)
                                    {
                                        num = 32;
                                        num2 = num;
                                        continue;
                                    }
                                    goto IL_8E6;
                                case 28:
                                    arg_3BE_0 = false;
                                    goto IL_3BE;
                                case 29:
                                    p_transaction.ledger = new double?((currentLedgerBalance.Length > 0) ? double.Parse(currentLedgerBalance) : 0.0);
                                    num = 42;
                                    num2 = num;
                                    continue;
                                case 30:
                                    {
                                        bool flag3;
                                        if (flag3)
                                        {
                                            num = 33;
                                            num2 = num;
                                            continue;
                                        }
                                        goto IL_76D;
                                    }
                                case 31:
                                    p_transaction.tran_type = "TRANSFER";
                                    num = 20;
                                    num2 = num;
                                    continue;
                                case 32:
                                    p_transaction.trxn_date = new DateTime?(this.proc.GetDateTime(this.proc.ReFormatDate(datetimeTerminalId.Split(new char[]
						{
							'&'
						})[1]), datetimeTerminalId.Split(new char[]
						{
							'&'
						})[2]));
                                    p_transaction.terminalId = datetimeTerminalId.Split(new char[]
						{
							'&'
						})[0];
                                    p_transaction.trxn_time = datetimeTerminalId.Split(new char[]
						{
							'&'
						})[2];
                                    num = 24;
                                    num2 = num;
                                    continue;
                                case 33:
                                    {
                                        num = 0;
                                        p_transaction.pan = pANTSNRRN.Split(new char[]
						{
							'&'
						})[0];
                                        p_transaction.tsn = pANTSNRRN.Split(new char[]
						{
							'&'
						})[1];
                                        p_transaction.rrn = pANTSNRRN.Split(new char[]
						{
							'&'
						})[2];
                                        string[] errorCodeAndCountsNCR = this.proc.GetErrorCodeAndCountsNCR(tranblock, p_transaction.tsn);
                                        num = 6;
                                        num2 = num;
                                        continue;
                                    }
                                case 34:
                                    goto IL_94B;
                                case 35:
                                    if (flag10)
                                    {
                                        num = 26;
                                        num2 = num;
                                        continue;
                                    }
                                    goto IL_270;
                                case 36:
                                    if (text2 == "")
                                    {
                                        num = 4;
                                        num2 = num;
                                        continue;
                                    }
                                    num = 28;
                                    num2 = num;
                                    continue;
                                case 37:
                                    goto IL_1A5;
                                case 38:
                                    {
                                        bool flag8;
                                        if (flag8)
                                        {
                                            num = 31;
                                            num2 = num;
                                            continue;
                                        }
                                        goto IL_258;
                                    }
                                case 39:
                                    p_transaction.tran_type = "WITHDRAWAL";
                                    num = 37;
                                    num2 = num;
                                    continue;
                                case 40:
                                    goto IL_813;
                                case 41:
                                    if (flag11)
                                    {
                                        num = 39;
                                        num2 = num;
                                        continue;
                                    }
                                    goto IL_1A5;
                                case 42:
                                    p_transaction.amount = new double?((text2.Length > 0) ? double.Parse(text2) : 0.0);
                                    num = 3;
                                    num2 = num;
                                    continue;
                                case 43:
                                    goto IL_323;
                                case 44:
                                    arg_3BE_0 = (inquiry != "");
                                    goto IL_3BE;
                                case 45:
                                    goto IL_270;
                            }
                            goto IL_E0;
                        IL_1A5:
                            flag10 = (text2 == "");
                            num = 35;
                            num2 = num;
                            continue;
                        IL_258:
                            num = 45;
                            num2 = num;
                            continue;
                        IL_270:
                            flag4 = (text2 == "");
                            num = 12;
                            num2 = num;
                            continue;
                        IL_323:
                            num = 15;
                            num2 = num;
                            continue;
                        IL_33B:
                            inquiry = this.proc.GetInquiry(tranblock);
                            num = 36;
                            num2 = num;
                            continue;
                        IL_3BE:
                            flag2 = arg_3BE_0;
                            num = 8;
                            num2 = num;
                            continue;
                        IL_433:
                            availableBalance = this.proc.GetAvailableBalance(tranblock, brandid);
                            currentLedgerBalance = this.proc.GetCurrentLedgerBalance(tranblock, brandid);
                            surcharge = this.proc.GetSurcharge(tranblock, brandid);
                            errcode = this.proc.NCRDispenseErrorCodeParser(tranblock);
                            string text3 = this.proc.NCROtherErrorCodeParser(tranblock);
                            text = this.proc.ProcessNCRDispenseError(tranblock);
                            num = 14;
                            num2 = num;
                            continue;
                        IL_4FB:
                            flag7 = arg_4FB_0;
                            num = 21;
                            num2 = num;
                            continue;
                        IL_55C:
                            p_transaction.brand = brand;
                            p_transaction.atmip = ip;
                            p_transaction.opCode = opCode;
                            num = 29;
                            num2 = num;
                            continue;
                        IL_76D:
                            flag = (p_transaction.pan == null);
                            num = 7200;
                            int arg_794_0 = num;
                            num = 7200;
                            switch ((arg_794_0 == num) ? 1 : 0)
                            {
                                case 0:
                                case 2:
                                    goto IL_7CC;
                                case 1:
                                IL_7A8:
                                    num = 0;
                                    if (num != 0)
                                    {
                                    }
                                    num = 2;
                                    num2 = num;
                                    continue;
                            }
                            goto IL_7A8;
                        IL_813:
                            flag9 = !string.IsNullOrWhiteSpace(datetimeTerminalId);
                            num = 27;
                            num2 = num;
                            continue;
                        IL_8E6:
                            flag6 = !string.IsNullOrWhiteSpace(startandEndTime);
                            num = 17;
                            num2 = num;
                            continue;
                        IL_94B:
                            string transactionComment = this.proc.GetTransactionComment(tranblock);
                            p_transaction.comments = transactionComment;
                            p_transaction.accountfrm = this.proc.GetFromAccount(tranblock, brandid);
                            p_transaction.amount_entered = this.proc.GetAmountEntered(tranblock);
                            result = p_transaction;
                            num = 25;
                            num2 = num;
                        }
                        return result;
                    IL_E0:
                        p_transaction = new p_transaction();
                        pANTSNRRN = this.proc.GetPANTSNRRN(tranblock);
                        datetimeTerminalId = this.proc.GetDatetimeTerminalId(tranblock);
                        startandEndTime = this.proc.GetStartandEndTime(tranblock);
                        opCode = this.proc.GetOpCode(tranblock, brandid);
                        string[] requestAmount = this.proc.GetRequestAmount(tranblock);
                        allCOmments = this.GetAllCOmments();
                        text2 = this.proc.GetWithdrawal(tranblock);
                        flag11 = (text2 != "");
                        num = 41;
                        num2 = num;
                        goto IL_21;
                    }
            }
            goto IL_17;
        }

        public List<comment> GetAllCOmments()
        {
            int num = -24938;
            int arg_1C_0 = num;
            num = -24938;
            List<comment> result;
            switch ((arg_1C_0 == num) ? 1 : 0)
            {
                case 0:
                case 2:
                    goto IL_79;
                case 1:
                    {
                    IL_30:
                        num = 1;
                        if (num != 0)
                        {
                        }
                        num = 0;
                        if (num != 0)
                        {
                        }
                        string connectionString = ConfigurationManager.ConnectionStrings["AppDBConnect"].ConnectionString;
                        List<comment> list = null;
                        result = list;
                        goto IL_79;
                    }
            }
            goto IL_30;
        IL_79:
            num = 0;
            return result;
        }
    }
}
