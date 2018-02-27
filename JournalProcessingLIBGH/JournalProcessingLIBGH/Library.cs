using System;
using System.IO;

namespace JournalProcessingLIBGH
{
    public static class Library
    {
        public static void writeErrorLog(string Message)
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
                                goto IL_40;
                        }
                        bool flag;
                        string path;
                        while (true)
                        {
                        IL_21:
                            bool flag2;
                            switch (num2)
                            {
                                case 0:
                                    if (flag)
                                    {
                                        num = -18369;
                                        int arg_8E_0 = num;
                                        num = -18369;
                                        switch ((arg_8E_0 == num) ? 1 : 0)
                                        {
                                            case 0:
                                            case 2:
                                                continue;
                                            case 1:
                                            IL_A2:
                                                num = 0;
                                                num = 0;
                                                if (num != 0)
                                                {
                                                }
                                                num = 4;
                                                num2 = num;
                                                continue;
                                        }
                                        goto IL_A2;
                                    }
                                    goto IL_103;
                                case 1:
                                    goto IL_103;
                                case 2:
                                    goto IL_FE;
                                case 3:
                                    num = 2;
                                    num2 = num;
                                    continue;
                                case 4:
                                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "LOGS");
                                    num = 1;
                                    num2 = num;
                                    continue;
                                case 5:
                                    num = 1;
                                    if (num != 0)
                                    {
                                    }
                                    if (flag2)
                                    {
                                        num = 3;
                                        num2 = num;
                                        continue;
                                    }
                                    goto IL_1C2;
                            }
                            goto IL_40;
                        IL_103:
                            string str = string.Format("{0:ddMMyyyy}", DateTime.Now);
                            path = AppDomain.CurrentDomain.BaseDirectory + "LOGS/" + str + ".txt";
                            flag2 = File.Exists(path);
                            num = 5;
                            num2 = num;
                        }
                    IL_FE:
                    IL_1C2:
                        StreamWriter streamWriter = new StreamWriter(path, true);
                        streamWriter.WriteLine(DateTime.Now.ToString() + ":" + Message);
                        streamWriter.Flush();
                        streamWriter.Close();
                        return;
                    IL_40:
                        flag = !Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "LOGS");
                        num = 0;
                        num2 = num;
                        goto IL_21;
                    }
            }
            goto IL_17;
        }

        public static void writeErrorLog(Exception ex)
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
                        while (true)
                        {
                        IL_21:
                            switch (num2)
                            {
                                case 0:
                                    goto IL_129;
                                case 1:
                                    {
                                        bool flag;
                                        if (flag)
                                        {
                                            num = 2;
                                            num2 = num;
                                            continue;
                                        }
                                        goto IL_12B;
                                    }
                                case 2:
                                    goto IL_FA;
                            }
                            goto IL_34;
                        }
                    IL_FA:
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "LOGS");
                        goto IL_118;
                    IL_129:
                    IL_12B:
                        string str = string.Format("{0:ddMMyyyy}", DateTime.Now);
                        string path = AppDomain.CurrentDomain.BaseDirectory + "LOGS/" + str + ".txt";
                        StreamWriter streamWriter = new StreamWriter(path, true);
                        streamWriter.WriteLine(string.Concat(new string[]
				{
					DateTime.Now.ToString(),
					":",
					ex.Source.ToString().Trim(),
					":",
					ex.Message.ToString().Trim()
				}));
                        streamWriter.Flush();
                        streamWriter.Close();
                        return;
                    IL_34:
                        num = 2403;
                        int arg_50_0 = num;
                        num = 2403;
                        switch ((arg_50_0 == num) ? 1 : 0)
                        {
                            case 0:
                            case 2:
                            IL_118:
                                num = 0;
                                num2 = num;
                                goto IL_21;
                            case 1:
                                {
                                IL_64:
                                    num = 0;
                                    num = 0;
                                    if (num != 0)
                                    {
                                    }
                                    num = 1;
                                    if (num != 0)
                                    {
                                    }
                                    bool flag = !Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "LOGS");
                                    num = 1;
                                    num2 = num;
                                    goto IL_21;
                                }
                        }
                        goto IL_64;
                    }
            }
            goto IL_17;
        }

        public static void AddToDownloadLog(string ipaddress, string sourcepath, string fileName, string mode, string status, string date)
        {
            int num = -2157;
            int arg_1C_0 = num;
            num = -2157;
            switch ((arg_1C_0 == num) ? 1 : 0)
            {
                case 0:
                case 2:
                IL_2F:
                    break;
                case 1:
                    goto IL_30;
            }
            goto IL_2F;
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
            date = DateTime.Now.ToString();
            string text = string.Concat(new string[]
			{
				ipaddress,
				"=",
				sourcepath,
				"=",
				fileName,
				"=",
				mode
			});
            string text2 = string.Concat(new string[]
			{
				"INSERT INTO filelog\r\n           ([IP]\r\n           ,[source_path]\r\n           ,[filename]\r\n           ,[filetype]\r\n           ,[status]\r\n           ,[date])\r\n     VALUES\r\n           ('",
				ipaddress,
				"','",
				sourcepath,
				"','",
				fileName,
				"','",
				mode,
				"','",
				status,
				"','",
				date,
				"')"
			});
        }

        public static int IsFileLogged(string ipaddress, string sourcepath, string fileName, string mode, string status, string date)
        {
            int num = 0;
            num = 32723;
            int arg_38_0 = num;
            num = 32723;
            switch ((arg_38_0 == num) ? 1 : 0)
            {
                case 0:
                case 2:
                    {
                        int result;
                        return result;
                    }
                case 1:
                    {
                    IL_4C:
                        num = 0;
                        if (num != 0)
                        {
                        }
                        num = 1;
                        if (num != 0)
                        {
                        }
                        string text = string.Concat(new string[]
				{
					ipaddress,
					"=",
					sourcepath,
					"=",
					fileName,
					"=",
					mode
				});
                        int result = 0;
                        return result;
                    }
            }
            goto IL_4C;
        }

        public static void AddToZipDownloadLog(string ipaddress, string sourcepath, string zipfilename, string fileName, string mode, string status, string date)
        {
            int num = 12506;
            int arg_1C_0 = num;
            num = 12506;
            int num2;
            switch ((arg_1C_0 == num) ? 1 : 0)
            {
                case 0:
                case 2:
                    goto IL_AD;
                case 1:
                IL_30:
                    num = 0;
                    if (num != 0)
                    {
                    }
                    num = 0;
                    num = 0;
                    num2 = num;
                    switch (num2)
                    {
                        case 0:
                        IL_72:
                            num = 1;
                            if (num != 0)
                            {
                            }
                            switch (0)
                            {
                                case 0:
                                    goto IL_AD;
                            }
                            goto IL_9A;
                    }
                    goto IL_72;
            }
            goto IL_30;
            bool flag;
            while (true)
            {
            IL_9A:
                switch (num2)
                {
                    case 0:
                        if (flag)
                        {
                            num = 1;
                            num2 = num;
                            continue;
                        }
                        return;
                    case 1:
                        num = 2;
                        num2 = num;
                        continue;
                    case 2:
                        return;
                }
                goto IL_AD;
            }
            return;
        IL_AD:
            date = DateTime.Now.ToString();
            string text = string.Concat(new string[]
			{
				"INSERT INTO downloads\r\n           ([IP]\r\n           ,[source_path]\r\n           ,[destination_path]\r\n           ,[filename]\r\n           ,[filetype]\r\n           ,[status]\r\n           ,[date])\r\n           Values\r\n           ('",
				ipaddress,
				"','",
				sourcepath,
				"','",
				zipfilename,
				"','",
				fileName,
				"','",
				mode,
				"','",
				status,
				"','",
				date,
				"')"
			});
            flag = (fileName.Length > 5);
            num = 0;
            num2 = num;
            goto IL_9A;
        }

        public static int IsZipFileLogged(string ipaddress, string sourcepath, string zipfilename, string fileName, string mode, string status, string date)
        {
            int num = 19614;
            int arg_1C_0 = num;
            num = 19614;
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
                        string text = string.Concat(new string[]
				{
					ipaddress,
					"=",
					sourcepath,
					"==",
					fileName,
					"=",
					mode
				});
                        int result = 0;
                        return result;
                    }
            }
            goto IL_30;
        }
    }
}
