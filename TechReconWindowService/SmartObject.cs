using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReconWindowService
{
     public static class SmartObject
        {
            private static object cvLockObject = new object();
            private static string cvsLogFile = System.Configuration.ConfigurationManager.AppSettings["LogFile"];
            private static string filePath = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"];
            private static string LogSize = System.Configuration.ConfigurationManager.AppSettings["LogSize"];
            public async static Task FileWriteAsync(string filePath, string messaage, bool append = true)
            {
                using (FileStream stream = new FileStream(filePath, append ? FileMode.Append : FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    await sw.WriteLineAsync(messaage);
                }
            }
            public static void SaveLog(string psDetails)
            {
                //    FileInfo f = new FileInfo(cvsLogFile);

                //    if (File.Exists(cvsLogFile))
                //    {
                //        long s1 = f.Length;
                //        if (s1 > Convert.ToInt32(LogSize))
                //        {
                //            string filename = Path.GetFileNameWithoutExtension(cvsLogFile) + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".txt";
                //            if (File.Exists(Path.Combine(filePath, filename)))
                //                File.Delete(Path.Combine(filePath, filename));

                //            File.Move(cvsLogFile, Path.Combine(filePath, filename));

                //            f.Delete();
                //        }
                //    }
                lock (cvLockObject)
                {
                    File.AppendAllText(Path.Combine(cvsLogFile), DateTime.Now.ToString() + ": " + psDetails + Environment.NewLine);
                    //using (var sw = new StreamWriter(cvsLogFile, true, Encoding.ASCII))
                    //{
                    //    string sError = DateTime.Now.ToString() + ": " + psDetails;
                    //    sw.WriteLine(sError);
                    //    sw.Close();
                    //    // Use stream
                    //}

                    // StreamWriter sw = new StreamWriter(cvsLogFile, true, Encoding.ASCII);
                    // sw.WriteLine(sError);
                    //sw.Close();
                }
            }
         
        }
}
