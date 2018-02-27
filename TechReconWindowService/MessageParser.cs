using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TechReconWindowService
{
    public class MessageParser
    {
        /// <summary>
        /// Get the basic header part of a swift message
        /// </summary>
        /// <param name="mtMessage"></param>
        /// <returns></returns>
        public static MTBasicHeaderBlock GetBasicHeaderBlock(string mtMessage)
        {

            MTBasicHeaderBlock objBasicHeaderBlock = new MTBasicHeaderBlock();

            string[] messageParts = mtMessage.Split(new string[] { "}{" }, StringSplitOptions.RemoveEmptyEntries);
            string[] rawHeader = messageParts[0].Split(new char[] { ':' });
            string chunkableHeader = rawHeader[1];

            objBasicHeaderBlock.ApplicationID = chunkableHeader.Substring(0, 1);
            objBasicHeaderBlock.ServiceID = chunkableHeader.Substring(1, 2);
            objBasicHeaderBlock.Bic = chunkableHeader.Substring(3, 12);
            objBasicHeaderBlock.MT940ReceiverSwiftCode = chunkableHeader.Substring(3, 8);
            try
            {
                objBasicHeaderBlock.SessionNumber = chunkableHeader.Substring(15, 4);
                objBasicHeaderBlock.SequenceNumber = chunkableHeader.Substring(19, 6);
            }
            catch
            {

            }

            return objBasicHeaderBlock;
        }

        public static string BuildBasicHeaderBlock(string bic, string sessionid, string sequenceno, string mtApplicationID, string mtServiceId)
        {
            StringBuilder bld = new StringBuilder();
            bld.Append("{1:");
            bld.Append(mtApplicationID);
            bld.Append(mtServiceId);
            bld.Append(bic);
            bld.Append(sessionid.PadRight(4, '0'));
            bld.Append(sequenceno.PadRight(6, '0'));
            bld.Append("}");

            return bld.ToString();
        }

        /// <summary>
        /// Get the application header part of a swift message
        /// </summary>
        /// <param name="mtMessage"></param>
        /// <returns></returns>

        public static MT940ApplicationHeaderBlock GetMT940ApplicationHeaderBlock(string mtMessage)
        {

            MT940ApplicationHeaderBlock objApplicationHeaderBlock = new MT940ApplicationHeaderBlock();

            string[] messageParts = mtMessage.Split(new string[] { "}{" }, StringSplitOptions.RemoveEmptyEntries);
            string[] rawHeader = messageParts[4].Split(new char[] { ':' });
            string chunkableHeader = rawHeader[1];

            objApplicationHeaderBlock.InputDirection = chunkableHeader.Substring(0, 1);

            if (objApplicationHeaderBlock.InputDirection == "O")
            {
                //objApplicationHeaderBlock.MessageType = chunkableHeader.Substring(1, 3);
                //objApplicationHeaderBlock.ReceiversAddress = chunkableHeader.Substring(4, 11);
                //objApplicationHeaderBlock.MessagePriority  = chunkableHeader.Substring(15, 1);
                //objApplicationHeaderBlock.DeliveryMonitoring  = chunkableHeader.Substring(16, 1);               

                objApplicationHeaderBlock.MessageType = chunkableHeader.Substring(1, 3);
                objApplicationHeaderBlock.ReceiversAddress = chunkableHeader.Substring(4, 12);
                objApplicationHeaderBlock.MessagePriority = chunkableHeader.Substring(16, 1);

                objApplicationHeaderBlock.MT940SenderSwiftCode = chunkableHeader.Substring(14, 8);

                if (chunkableHeader.Length > 17)
                    objApplicationHeaderBlock.DeliveryMonitoring = chunkableHeader.Substring(17, 1);


                if (chunkableHeader.Length >= 21)
                    objApplicationHeaderBlock.ObsolescencePeriod = chunkableHeader.Substring(18, 3);
            }
            else
            {
                //throw new Exception("Output Message Direction not implemented for MT101");
                //objApplicationHeaderBlock.MessageType = chunkableHeader.Substring(1, 3);
                //objApplicationHeaderBlock.InputTime = chunkableHeader.Substring(4, 4);
                //objApplicationHeaderBlock.MIRWithSendersAddress = chunkableHeader.Substring(8, 19);
                //objApplicationHeaderBlock.OutputDate  = chunkableHeader.Substring(27, 6);
                //objApplicationHeaderBlock.MessagePriority  = chunkableHeader.Substring(33, 1);                
            }

            return objApplicationHeaderBlock;
        }

        public static string BuildApplicationHeaderBlock(string objSWMessageType, string mtMessageDirection, string mtMessagePriority, string recieversBic)
        {
            StringBuilder bld = new StringBuilder();
            bld.Append("{2:");
            bld.Append(mtMessageDirection);
            bld.Append(objSWMessageType);
            recieversBic = recieversBic.ToUpper();
            if (recieversBic.Length < 8)
            {
                throw new Exception("Invalid BIC supplied. Characters less than 8");
            }
            else if (recieversBic.Length == 8)
            {
                bld.Append(recieversBic.Substring(0, 8).PadRight(12, 'X'));
            }
            else if (recieversBic.Length == 9)
            {
                bld.Append(recieversBic.Substring(0, 8).PadRight(8, 'X') + "X" + recieversBic.Substring(8, 1).PadRight(3, 'X'));
            }
            else if (recieversBic.Length == 10)
            {
                bld.Append(recieversBic.Substring(0, 8).PadRight(8, 'X') + "X" + recieversBic.Substring(8, 2).PadRight(3, 'X'));
            }
            else if (recieversBic.Length == 11)
            {
                bld.Append(recieversBic.Substring(0, 8).PadRight(8, 'X') + "X" + recieversBic.Substring(8, 3).PadRight(3, 'X'));
            }
            else if (recieversBic.Length == 12)
            {
                bld.Append(recieversBic.ToUpper());
            }
            bld.Append(mtMessagePriority);
            bld.Append("}");
            return bld.ToString();
        }

        

        //public static MT103Transaction ReadMT103MessageBodyRTGS(string[] mtMessage)
        //{
        //    MT103Transaction tranx = ReadMT103MessageBody(mtMessage);
        //    if(tranx.)
        //}



        public static MT940Transaction ReadMT940MessageBody(string[] mtMessage)
        {
            MT940Transaction objMT940Transaction = new MT940Transaction();

            //objMT940Transaction.SeqA
            int counter = 0;

            string lastTag = string.Empty;
            //Extract Sequence A
            #region Extract SequenceA
            foreach (string eachLine in mtMessage)
            {
                //objMT940Transaction.SeqA=new 
                counter++;
                if (counter > 0)
                {
                    if (eachLine.StartsWith(":"))
                    {
                        if (eachLine.Substring(1).Contains(":"))
                        {
                            string[] tagWithContent = eachLine.Substring(1).Split(new char[] { ':' });
                            string tag = tagWithContent[0];
                            string tagValue = tagWithContent[1];
                            lastTag = tag;
                            switch (tag)
                            {
                                case MT940Transaction.SequenceA.TransactionReferenceNumber:
                                    objMT940Transaction.SeqA.TransactionReferenceNumber_M = tagValue;
                                    objMT940Transaction.SeqA.TagTransactionReferenceNumber = tag;
                                    break;
                                case MT940Transaction.SequenceA.RelatedReference:
                                    objMT940Transaction.SeqA.RelatedReference_O = tagValue;
                                    objMT940Transaction.SeqA.TagRelatedReference = tag;
                                    break;
                                case MT940Transaction.SequenceA.AccountIdentification:
                                    objMT940Transaction.SeqA.AccountIdentification_M = tagValue;
                                    objMT940Transaction.SeqA.TagAccountIdentification = tag;
                                    break;
                                case MT940Transaction.SequenceA.StatementNumber_SequenceNumber:
                                    objMT940Transaction.SeqA.StatementNumber_SequenceNumber_M = tagValue;
                                    objMT940Transaction.SeqA.TagStatementNumber_SequenceNumber = tag;
                                    break;
                                case MT940Transaction.SequenceA.OpeningBalance_First:
                                    objMT940Transaction.SeqA.OpeningBalance_M = tagValue;
                                    objMT940Transaction.SeqA.TagOpeningBalance = tag;
                                    break;
                                case MT940Transaction.SequenceA.OpeningBalance_Intermediate:
                                    objMT940Transaction.SeqA.OpeningBalance_M = tagValue;
                                    objMT940Transaction.SeqA.TagOpeningBalance = tag;
                                    break;

                            }
                        }
                    }
                    else if (eachLine.StartsWith("-"))
                    {
                        //Do Nothing
                    }
                    else
                    {

                    }
                }
                //counter++;
            }
            #endregion

            //Extract Sequence B
            #region Extract SequenceB
            List<string> tranrefs = GetNumberOfSequenceB940(mtMessage);
            int count = GetNumberOfSequenceB940(mtMessage).Count;
            List<List<string>> msgList = GetRaw102SequenceB940(mtMessage);
            //int index = 0;
            foreach (List<string> lst1 in msgList)
            {
                MT940Transaction.SequenceBData sbd = new MT940Transaction.SequenceBData();
                foreach (string eachLineb in lst1)
                {
                    counter++;

                    if (counter > 0)
                    {

                        if (eachLineb.StartsWith(":"))
                        {
                            if (eachLineb.Substring(1).Contains(":"))
                            {
                                string[] tagWithContent = eachLineb.Substring(1).Split(new char[] { ':' });
                                string tag = tagWithContent[0];
                                string tagValue = tagWithContent[1];
                                lastTag = tag;

                                switch (tag)
                                {
                                    case MT940Transaction.SequenceB.StatementLine:
                                        sbd.StatementLine_O = tagValue;
                                        sbd.TagStatementLine = tag;
                                        break;
                                    case MT940Transaction.SequenceB.InformationToAccountOwner:
                                        sbd.InformationToAccountOwner_O = tagValue;
                                        sbd.TagInformationToAccountOwner = tag;
                                        break;
                                }
                            }
                        }
                        else if (eachLineb.StartsWith("-"))
                        {
                            //Do Nothing
                        }
                        else
                        {
                            switch (lastTag)
                            {
                                //case MT940Transaction.SequenceB.OrderingCustomerF:
                                case MT940Transaction.SequenceB.StatementLine:
                                    sbd.StatementLine_O += System.Environment.NewLine + eachLineb;
                                    break;
                                case MT940Transaction.SequenceB.InformationToAccountOwner:
                                    sbd.InformationToAccountOwner_O += System.Environment.NewLine + eachLineb;
                                    break;
                            }
                        }
                    }
                    //if (lastTag.Contains(":21:"))
                    //{

                    //}
                }
                objMT940Transaction.SeqB.Add(sbd);
            }
            #endregion

            //Extract Sequence C
            #region Extract SequenceC
            foreach (string eachLinec in mtMessage)
            {
                counter++;
                if (counter > 0)
                {

                    if (eachLinec.StartsWith(":"))
                    {
                        if (eachLinec.Substring(1).Contains(":"))
                        {
                            string[] tagWithContent = eachLinec.Substring(1).Split(new char[] { ':' });
                            string tag = tagWithContent[0];
                            string tagValue = tagWithContent[1];
                            lastTag = tag;

                            switch (tag)
                            {

                                case MT940Transaction.SequenceC.ClosingBalance_BookedFunds_First:
                                    objMT940Transaction.SeqC.ClosingBalance_BookedFunds_M = tagValue;
                                    objMT940Transaction.SeqC.TagClosingBalance_BookedFunds = tag;
                                    break;
                                case MT940Transaction.SequenceC.ClosingBalance_BookedFunds_Intermediate:
                                     objMT940Transaction.SeqC.ClosingBalance_BookedFunds_M = tagValue;
                                    objMT940Transaction.SeqC.TagClosingBalance_BookedFunds = tag;
                                    break;
                                case MT940Transaction.SequenceC.ClosingAvailableBalance_AvailableFunds:
                                    objMT940Transaction.SeqC.ClosingAvailableBalance_AvailableFunds_O = tagValue;
                                    objMT940Transaction.SeqC.TagClosingAvailableBalance_AvailableFunds = tag;
                                    break;
                                case MT940Transaction.SequenceC.ForwardAvailableBalance:
                                    objMT940Transaction.SeqC.ForwardAvailableBalance_O = tagValue;
                                    objMT940Transaction.SeqC.TagForwardAvailableBalance = tag;
                                    break;
                                //case MT102Transaction.SequenceA.TransactionTypeCode:
                                //    objMT940Transaction.SeqA.TransactionTypeCodeO = tagValue;
                                //    objMT940Transaction.SeqA.TagTransactionTypeCode = tag;
                                //    break;
                                //case MT102Transaction.SequenceA.DetailsOfCharges:
                                //    objMT940Transaction.SeqA.DetailsOfChargesM = tagValue;
                                //    objMT940Transaction.SeqA.TagDetailsOfCharges = tag;
                                //    break;

                            }
                        }
                    }
                    else if (eachLinec.StartsWith("-"))
                    {
                        //Do Nothing
                    }
                    else
                    {
                        switch (lastTag)
                        {
                            case MT940Transaction.SequenceC.ClosingBalance_BookedFunds_First:
                                //case MT103Transaction.Tag.OrderingCustomerK:
                                objMT940Transaction.SeqC.ClosingBalance_BookedFunds_M += System.Environment.NewLine + eachLinec;
                                break;
                        }
                    }
                }
            }
            #endregion

            return objMT940Transaction;
        }


        private static List<string> GetNumberOfSequenceB(string[] mtMessage)
        {

            List<string> tagCollection = new List<string>();
            int x = 0;
            foreach (string eachLine in mtMessage)
            {

                if (eachLine.StartsWith(":21"))
                {
                    x++;
                    if (eachLine.Substring(1).Contains(":"))
                    {
                        string[] tagWithContent = eachLine.Substring(1).Split(new char[] { ':' });
                        string tag = tagWithContent[0];
                        tagCollection.Add(tagWithContent[1]);
                    }
                }

            }

           // int noOfOccurance = (from c in tagCollection where c == MT101Transaction.Tag.TransactionReference select c).Count();

            return tagCollection;
        }

        private static List<string> GetNumberOfSequenceB940(string[] mtMessage)
        {

            List<string> tagCollection = new List<string>();
            int x = 0;
            foreach (string eachLine in mtMessage)
            {

                if (eachLine.StartsWith(":61"))
                {
                    x++;
                    if (eachLine.Substring(1).Contains(":"))
                    {
                        string[] tagWithContent = eachLine.Substring(1).Split(new char[] { ':' });
                        string tag = tagWithContent[0];
                        tagCollection.Add(tagWithContent[1]);
                    }
                }

            }

            // int noOfOccurance = (from c in tagCollection where c == MT101Transaction.Tag.TransactionReference select c).Count();

            return tagCollection;
        }
        

        private static List<List<string>> GetRaw102SequenceB940(string[] mtMessage)
        {

            List<List<string>> tagCollection = new List<List<string>>();
            int x = 0;
            for (int c = 0; c < mtMessage.Length; c++)
            {
                string eachLine = mtMessage[c].ToString();
                int ind21 = 0;
                if (eachLine.StartsWith(":61"))
                {
                    ind21 = c;
                    string val = "";
                    List<string> inner = new List<string>();
                    int innercounter = 0;
                    val = eachLine;
                    inner.Add(val);
                    innercounter++;
                    val = "";
                    for (int i = c + 1; i < mtMessage.Length && inner.Count <= 7; i++)
                    {
                        eachLine = mtMessage[i];
                        if (eachLine.Substring(1).Contains(":"))
                        {
                            string[] tagWithContent = eachLine.Substring(1).Split(new char[] { ':' });
                            string tag = tagWithContent[0];
                            val = eachLine;
                            inner.Add(val);
                            innercounter++;
                        }
                        else if (eachLine.StartsWith("-"))
                        {

                        }
                        else
                        {
                            //string[] tagWithContent = eachLine.Substring(1).Split(new char[] { ':' });
                            //string tag = tagWithContent[0];
                            //val += Environment.NewLine + tagWithContent[1];
                            inner[innercounter - 1] += Environment.NewLine + eachLine;
                        }
                        x++;
                        if (eachLine.StartsWith(":61"))
                        {
                            break;
                        }
                    }
                    inner.RemoveAt(inner.Count - 1);
                    tagCollection.Add(inner);
                }
                x++;
            }

            // int noOfOccurance = (from c in tagCollection where c == MT101Transaction.Tag.TransactionReference select c).Count();

            return tagCollection;
        }

        

        public static string[] GetMessageContentFromFile(string path)
        {
            return System.IO.File.ReadAllLines(path, UTF8Encoding.UTF8);
        }

        public static void SaveMTMessage(string filepath, string mtContent)
        {
            System.IO.File.WriteAllText(filepath, mtContent, Encoding.UTF8);
        }


        private static void CopyFile(string filepath, string filename)
        {

            string newFilePath = filepath;
            string oldfilename = filename;

            string source = filepath;
            string destination = newFilePath.Replace(oldfilename, (@"Processed\" + filename));
            try
            {
                File.Move(source, destination);
            }
            catch { }

        }
        public static IEnumerable<string> GetFilesInFtpDirectory(string fullpath, string username, string password)
        {
            // Get the object used to communicate with the server.
            var request = (FtpWebRequest)WebRequest.Create(fullpath);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(username, password);

            using (var response = (FtpWebResponse)request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    var reader = new StreamReader(responseStream);
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line) == false)
                        {
                            yield return line.Split(new[] { ' ', '\t' }).Last();
                        }
                    }
                }
            }
        }

    }

}
