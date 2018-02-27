using Microsoft.ApplicationBlocks.Data;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace JournalProcessingLIBGH
{
    public class ParseJournalFile
    {
        private string constr = ConfigurationManager.ConnectionStrings["AppDBConnect"].ConnectionString;

        public List<p_transaction> ptranxs = new List<p_transaction>();

        public int ProcessJournal(string filePath, string date, string ip, string tid, int brand, string brandname)
        {
            int num = 0;
            int num2 = num;
            switch (num2)
            {
                case 0:
                    {
                    IL_17:
                        num = 16104;
                        int arg_33_0 = num;
                        num = 16104;
                        int count;
                        switch ((arg_33_0 == num) ? 1 : 0)
                        {
                            case 0:
                            case 2:
                                {
                                IL_1AB:
                                    List<p_transaction> list = this.ptranxs;
                                    DataTable dataTable = this.ConvertToDataTable<p_transaction>(list);
                                    count = list.Count;
                                    num = 2;
                                    num2 = num;
                                    goto IL_60;
                                }
                            case 1:
                            IL_47:
                                num = 0;
                                if (num != 0)
                                {
                                }
                                switch (0)
                                {
                                    case 0:
                                        goto IL_7B;
                                }
                                goto IL_60;
                        }
                        goto IL_47;
                        List<OneJournalsRawTranx> list2;
                        int num3;
                        while (true)
                        {
                        IL_60:
                            bool flag;
                            switch (num2)
                            {
                                case 0:
                                    {
                                        num = 0;
                                        if (!flag)
                                        {
                                            num = 3;
                                            num2 = num;
                                            continue;
                                        }
                                        p_transaction p_transaction = new JournalParser().ProcessTranBlock(list2[num3].trandata, ip, tid, "", brandname, brand);
                                        p_transaction.detail = list2[num3].trandata;
                                        p_transaction.jrn_filename = filePath;
                                        this.ptranxs.Add(p_transaction);
                                        num3++;
                                        num = 4;
                                        num2 = num;
                                        continue;
                                    }
                                case 1:
                                    num = 1;
                                    if (num != 0)
                                    {
                                    }
                                    goto IL_159;
                                case 2:
                                    return count;
                                case 3:
                                    goto IL_1AB;
                                case 4:
                                    goto IL_159;
                            }
                            goto IL_7B;
                        IL_159:
                            flag = (num3 < list2.Count);
                            num = 0;
                            num2 = num;
                        }
                        return count;
                    IL_7B:
                        JournalProcessorGH journalProcessorGH = new JournalProcessorGH();
                        list2 = new List<OneJournalsRawTranx>();
                        list2 = journalProcessorGH.GetJournalTransactions(filePath, date, ip, tid, brand);
                        Library.writeErrorLog(string.Concat(new object[]
				{
					"GOTTEN FILE ",
					filePath,
					" FOR PROCESSING: ",
					brand,
					" - ",
					brandname,
					" . ",
					ip,
					" TID :: ",
					tid
				}));
                        num3 = 0;
                        num = 1;
                        num2 = num;
                        goto IL_60;
                    }
            }
            goto IL_17;
        }

        public int LogJournalFileData(string filePath, string date, string ip, string tid, int brand, string brandname, string data, int count)
        {
            int num = 4278;
            int arg_1C_0 = num;
            num = 4278;
            switch ((arg_1C_0 == num) ? 1 : 0)
            {
                case 0:
                case 2:
                    {
                        int result;
                        return result;
                    }
                case 1:
                    {
                    IL_30:
                        num = 0;
                        num = 1;
                        if (num != 0)
                        {
                        }
                        num = 0;
                        if (num != 0)
                        {
                        }
                        JournalsStore journalsStore = new JournalsStore();
                        journalsStore.ip = ip;
                        journalsStore.terminalid = tid;
                        journalsStore.filename = filePath;
                        journalsStore.filedate = date;
                        string text = "INSERT INTO [dbo].[JournalsStore]\r\n           ([ip]\r\n           ,[terminalid]\r\n           ,[filename]\r\n           ,[filedate]\r\n           ,[data]\r\n           ,[databin]\r\n           ,[transactioncount]\r\n           ,[footagedownloadstatus]\r\n           ,[footagescount])\r\n     VALUES\r\n           (@ip, \r\n           @terminalid,\r\n           @filename,\r\n           @filedate,\r\n           @data,\r\n           @databin,\r\n           @transactioncount,\r\n @footagestatus,\r\n 0)";
                        byte[] bytes = Encoding.ASCII.GetBytes(File.ReadAllText(filePath));
                        DateTime dateTime = DateTime.Parse(date);
                        string value = string.Format("{0:yyyy-MM-dd}", dateTime);
                        int num2 = SqlHelper.ExecuteNonQuery(this.constr, CommandType.Text, text, new SqlParameter[]
				{
					new SqlParameter("ip", ip),
					new SqlParameter("terminalid", tid),
					new SqlParameter("filename", filePath),
					new SqlParameter("filedate", value),
					new SqlParameter("data", ""),
					new SqlParameter("databin", bytes),
					new SqlParameter("transactioncount", count),
					new SqlParameter("footagestatus", "not downloaded")
				});
                        int result = num2;
                        return result;
                    }
            }
            goto IL_30;
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
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
                                goto IL_34;
                        }
                        DataTable dataTable;
                        PropertyDescriptorCollection properties;
                        DataTable result;
                        IEnumerator enumerator3;
                        while (true)
                        {
                        IL_21:
                            switch (num2)
                            {
                                case 0:
                                IL_BB:
                                    try
                                    {
                                        num = 0;
                                        num2 = num;
                                        while (true)
                                        {
                                            switch (num2)
                                            {
                                                case 0:
                                                    switch (0)
                                                    {
                                                        case 0:
                                                            goto IL_2A1;
                                                    }
                                                    continue;
                                                case 1:
                                                    {
                                                        DataRow dataRow2;
                                                        try
                                                        {
                                                            num = 1;
                                                            num2 = num;
                                                            while (true)
                                                            {
                                                                object value;
                                                                DataRow dataRow;
                                                                string name;
                                                                switch (num2)
                                                                {
                                                                    case 0:
                                                                        value = DBNull.Value;
                                                                        num = 5;
                                                                        num2 = num;
                                                                        continue;
                                                                    case 1:
                                                                        switch (0)
                                                                        {
                                                                            case 0:
                                                                                goto IL_2E9;
                                                                        }
                                                                        continue;
                                                                    case 2:
                                                                        {
                                                                            bool flag;
                                                                            if (flag)
                                                                            {
                                                                                num = 0;
                                                                                num2 = num;
                                                                                continue;
                                                                            }
                                                                            goto IL_39C;
                                                                        }
                                                                    case 3:
                                                                        goto IL_3D8;
                                                                    case 4:
                                                                        {
                                                                            IEnumerator enumerator;
                                                                            if (!enumerator.MoveNext())
                                                                            {
                                                                                num = 6;
                                                                                num2 = num;
                                                                                continue;
                                                                            }
                                                                            PropertyDescriptor propertyDescriptor = (PropertyDescriptor)enumerator.Current;
                                                                            dataRow = dataRow2;
                                                                            name = propertyDescriptor.Name;
                                                                            T current;
                                                                            value = propertyDescriptor.GetValue(current);
                                                                            bool flag = value == null;
                                                                            num = 2;
                                                                            num2 = num;
                                                                            continue;
                                                                        }
                                                                    case 5:
                                                                        goto IL_39C;
                                                                    case 6:
                                                                        num = 3;
                                                                        num2 = num;
                                                                        continue;
                                                                }
                                                            IL_2EB:
                                                                num = 4;
                                                                num2 = num;
                                                                continue;
                                                            IL_2E9:
                                                                goto IL_2EB;
                                                            IL_39C:
                                                                dataRow[name] = value;
                                                                num = 7;
                                                                num2 = num;
                                                            }
                                                        IL_3D8: ;
                                                        }
                                                        finally
                                                        {
                                                            switch (0)
                                                            {
                                                                case 0:
                                                                    goto IL_3F7;
                                                            }
                                                            IDisposable disposable;
                                                            while (true)
                                                            {
                                                            IL_3E4:
                                                                switch (num2)
                                                                {
                                                                    case 0:
                                                                        disposable.Dispose();
                                                                        num = 2;
                                                                        num2 = num;
                                                                        continue;
                                                                    case 1:
                                                                        if (disposable != null)
                                                                        {
                                                                            num = 0;
                                                                            num2 = num;
                                                                            continue;
                                                                        }
                                                                        goto IL_442;
                                                                    case 2:
                                                                        goto IL_440;
                                                                }
                                                                goto IL_3F7;
                                                            }
                                                        IL_440:
                                                        IL_442:
                                                            goto EndFinally_16;
                                                        IL_3F7:
                                                            IEnumerator enumerator;
                                                            disposable = (enumerator as IDisposable);
                                                            num = 1;
                                                            num2 = num;
                                                            goto IL_3E4;
                                                        EndFinally_16: ;
                                                        }
                                                        dataTable.Rows.Add(dataRow2);
                                                        num = 4;
                                                        num2 = num;
                                                        continue;
                                                    }
                                                case 2:
                                                    num = 5;
                                                    num2 = num;
                                                    continue;
                                                case 3:
                                                    {
                                                        IEnumerator<T> enumerator2;
                                                        if (!enumerator2.MoveNext())
                                                        {
                                                            num = 2;
                                                            num2 = num;
                                                            continue;
                                                        }
                                                        T current = enumerator2.Current;
                                                        DataRow dataRow2 = dataTable.NewRow();
                                                        IEnumerator enumerator = properties.GetEnumerator();
                                                        num = 1;
                                                        num2 = num;
                                                        continue;
                                                    }
                                                case 5:
                                                    goto IL_4EB;
                                            }
                                        IL_49E:
                                            num = 3;
                                            num2 = num;
                                            continue;
                                        IL_2A1:
                                            goto IL_49E;
                                        }
                                    IL_4EB:
                                        goto IL_20E;
                                    }
                                    finally
                                    {
                                        num = 1;
                                        num2 = num;
                                        while (true)
                                        {
                                            IEnumerator<T> enumerator2;
                                            switch (num2)
                                            {
                                                case 0:
                                                    enumerator2.Dispose();
                                                    num = 2;
                                                    num2 = num;
                                                    continue;
                                                case 1:
                                                    switch (0)
                                                    {
                                                        case 0:
                                                            goto IL_51F;
                                                    }
                                                    continue;
                                                case 2:
                                                    goto IL_54D;
                                            }
                                        IL_51F:
                                            if (enumerator2 == null)
                                            {
                                                break;
                                            }
                                            num = 0;
                                            num2 = num;
                                        }
                                    IL_54D: ;
                                    }
                                    return result;
                                case 1:
                                    {
                                        try
                                        {
                                            num = 0;
                                            num2 = num;
                                            while (true)
                                            {
                                                switch (num2)
                                                {
                                                    case 0:
                                                        switch (0)
                                                        {
                                                            case 0:
                                                                goto IL_F7;
                                                        }
                                                        continue;
                                                    case 1:
                                                        {
                                                            if (!enumerator3.MoveNext())
                                                            {
                                                                num = 3;
                                                                num2 = num;
                                                                continue;
                                                            }
                                                            PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)enumerator3.Current;
                                                            num = 4;
                                                            num2 = num;
                                                            continue;
                                                        }
                                                    case 3:
                                                        num = 5;
                                                        num2 = num;
                                                        continue;
                                                    case 4:
                                                        {
                                                            PropertyDescriptor propertyDescriptor2;
                                                            dataTable.Columns.Add(propertyDescriptor2.Name, Nullable.GetUnderlyingType(propertyDescriptor2.PropertyType) ?? propertyDescriptor2.PropertyType);
                                                            num = 2;
                                                            num2 = num;
                                                            continue;
                                                        }
                                                    case 5:
                                                        goto IL_1A1;
                                                }
                                            IL_13D:
                                                num = 1;
                                                num2 = num;
                                                continue;
                                            IL_F7:
                                                goto IL_13D;
                                            }
                                        IL_1A1:
                                            goto IL_67;
                                        }
                                        finally
                                        {
                                            switch (0)
                                            {
                                                case 0:
                                                    goto IL_1C3;
                                            }
                                            IDisposable disposable;
                                            while (true)
                                            {
                                            IL_1B0:
                                                switch (num2)
                                                {
                                                    case 0:
                                                        disposable.Dispose();
                                                        num = 2;
                                                        num2 = num;
                                                        continue;
                                                    case 1:
                                                        if (disposable != null)
                                                        {
                                                            num = 0;
                                                            num2 = num;
                                                            continue;
                                                        }
                                                        goto IL_20D;
                                                    case 2:
                                                        goto IL_20B;
                                                }
                                                goto IL_1C3;
                                            }
                                        IL_20B:
                                        IL_20D:
                                            goto EndFinally_15;
                                        IL_1C3:
                                            disposable = (enumerator3 as IDisposable);
                                            num = 1;
                                            num2 = num;
                                            goto IL_1B0;
                                        EndFinally_15: ;
                                        }
                                        goto IL_20E;
                                    IL_67:
                                        num = 0;
                                        num = 1;
                                        if (num != 0)
                                        {
                                        }
                                        IEnumerator<T> enumerator2 = data.GetEnumerator();
                                        num = 0;
                                        num2 = num;
                                        continue;
                                    }
                                case 2:
                                    return result;
                            }
                            goto IL_34;
                        IL_20E:
                            num = -16866;
                            int arg_22A_0 = num;
                            num = -16866;
                            switch ((arg_22A_0 == num) ? 1 : 0)
                            {
                                case 0:
                                case 2:
                                    goto IL_BB;
                                case 1:
                                IL_23E:
                                    num = 0;
                                    if (num != 0)
                                    {
                                    }
                                    result = dataTable;
                                    num = 2;
                                    num2 = num;
                                    continue;
                            }
                            goto IL_23E;
                        }
                        return result;
                    IL_34:
                        properties = TypeDescriptor.GetProperties(typeof(T));
                        dataTable = new DataTable();
                        enumerator3 = properties.GetEnumerator();
                        num = 1;
                        num2 = num;
                        goto IL_21;
                    }
            }
            goto IL_17;
        }

        public int AddParsedTransactionBulk(DataTable dt, string filepath)
        {
            int num = 0;
            int num2 = num;
            switch (num2)
            {
                case 0:
                    {
                        int num3;
                        int result;
                        while (true)
                        {
                        IL_17:
                            num3 = 0;
                            dt.TableName = "p_transaction";
                            string extension = Path.GetExtension(filepath);
                            char[] trimChars = new char[]
					{
						'.'
					};
                            string.Format(".{0}", extension.Trim(trimChars));
                            string connectionString = ConfigurationManager.ConnectionStrings["AppDBConnect"].ConnectionString;
                            string connectionString2 = connectionString;
                            SqlConnection sqlConnection = null;
                            SqlTransaction sqlTransaction = null;
                            try
                            {
                                SqlConnection sqlConnection2 = new SqlConnection(connectionString2);
                                sqlConnection = sqlConnection2;
                                SqlConnection sqlConnection3 = sqlConnection2;
                                try
                                {
                                    try
                                    {
                                        sqlConnection.Open();
                                        sqlTransaction = sqlConnection.BeginTransaction();
                                        SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection, SqlBulkCopyOptions.KeepNulls | SqlBulkCopyOptions.FireTriggers, sqlTransaction);
                                        try
                                        {
                                            sqlBulkCopy.BulkCopyTimeout = 0;
                                            sqlBulkCopy.DestinationTableName = dt.TableName;
                                            sqlBulkCopy.WriteToServer(dt);
                                            num3 = dt.Rows.Count;
                                        }
                                        finally
                                        {
                                            switch (0)
                                            {
                                                case 0:
                                                    goto IL_17F;
                                            }
                                            bool flag;
                                            while (true)
                                            {
                                            IL_16C:
                                                switch (num2)
                                                {
                                                    case 0:
                                                        ((IDisposable)sqlBulkCopy).Dispose();
                                                        num = 2;
                                                        num2 = num;
                                                        continue;
                                                    case 1:
                                                        if (flag)
                                                        {
                                                            num = 0;
                                                            num2 = num;
                                                            continue;
                                                        }
                                                        goto IL_1CB;
                                                    case 2:
                                                        goto IL_1C9;
                                                }
                                                goto IL_17F;
                                            }
                                        IL_1C9:
                                        IL_1CB:
                                            goto EndFinally_14;
                                        IL_17F:
                                            flag = (sqlBulkCopy != null);
                                            num = 1;
                                            num2 = num;
                                            goto IL_16C;
                                        EndFinally_14: ;
                                        }
                                        sqlTransaction.Commit();
                                        num3 = dt.Rows.Count;
                                    }
                                    catch (Exception var_14_1E4)
                                    {
                                    }
                                    finally
                                    {
                                        switch (0)
                                        {
                                            case 0:
                                                goto IL_209;
                                        }
                                        bool flag2;
                                        while (true)
                                        {
                                        IL_1F6:
                                            switch (num2)
                                            {
                                                case 0:
                                                    sqlConnection.Close();
                                                    num = 2;
                                                    num2 = num;
                                                    continue;
                                                case 1:
                                                    if (flag2)
                                                    {
                                                        num = 0;
                                                        num2 = num;
                                                        continue;
                                                    }
                                                    goto IL_255;
                                                case 2:
                                                    goto IL_253;
                                            }
                                            goto IL_209;
                                        }
                                    IL_253:
                                    IL_255:
                                        goto EndFinally_15;
                                    IL_209:
                                        flag2 = (sqlConnection != null);
                                        num = 1;
                                        num2 = num;
                                        goto IL_1F6;
                                    EndFinally_15: ;
                                    }
                                }
                                finally
                                {
                                    switch (0)
                                    {
                                        case 0:
                                            goto IL_277;
                                    }
                                    bool flag3;
                                    while (true)
                                    {
                                    IL_264:
                                        switch (num2)
                                        {
                                            case 0:
                                                ((IDisposable)sqlConnection3).Dispose();
                                                num = 2;
                                                num2 = num;
                                                continue;
                                            case 1:
                                                if (flag3)
                                                {
                                                    num = 0;
                                                    num2 = num;
                                                    continue;
                                                }
                                                goto IL_2C3;
                                            case 2:
                                                goto IL_2C1;
                                        }
                                        goto IL_277;
                                    }
                                IL_2C1:
                                IL_2C3:
                                    goto EndFinally_16;
                                IL_277:
                                    flag3 = (sqlConnection3 != null);
                                    num = 1;
                                    num2 = num;
                                    goto IL_264;
                                EndFinally_16: ;
                                }
                                goto IL_79;
                            }
                            catch (Exception var_17_2CB)
                            {
                                num3 = 0;
                                goto IL_79;
                            }
                            return result;
                        IL_79:
                            num = -25974;
                            int arg_95_0 = num;
                            num = -25974;
                            switch ((arg_95_0 == num) ? 1 : 0)
                            {
                                case 0:
                                case 2:
                                    continue;
                            }
                            break;
                        }
                        num = 0;
                        num = 1;
                        if (num != 0)
                        {
                        }
                        num = 0;
                        if (num != 0)
                        {
                        }
                        result = num3;
                        return result;
                    }
            }
            goto IL_17;
        }
    }
}
