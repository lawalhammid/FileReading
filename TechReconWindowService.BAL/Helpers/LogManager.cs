using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechReconWindowService.BAL.Helpers
{
    public static class LogManager
    {
        private static Object cvLockObject = new Object();
        private static string  cvsLogFile = System.Configuration.ConfigurationManager.AppSettings["LogFile"];
        private static string filePath = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"];
        private static string LogSize = System.Configuration.ConfigurationManager.AppSettings["LogSize"];
        public static void SaveLog(string psDetails)
        {
            if (cvsLogFile != null)
            {
                FileInfo f = new FileInfo(cvsLogFile);

                if (File.Exists(cvsLogFile))
                {
                    long s1 = f.Length;
                    if (s1 > Convert.ToInt32(LogSize))
                    {
                        string filename = Path.GetFileNameWithoutExtension(cvsLogFile) + string.Format("{0:yyyyMMddhhttss}", DateTime.Now) + "TechReconWindowServiceBkUp" + ".txt";
                        if (File.Exists(Path.Combine(filePath, filename)))
                            File.Delete(Path.Combine(filePath, filename));
                        File.Move(cvsLogFile, Path.Combine(filePath, filename));
                        f.Delete();
                    }
                }
            }
            lock (cvLockObject)
            {
                if (cvsLogFile != null)
                 {
                    //string sError = DateTime.Now.ToString() + ": " + psDetails;
                    //StreamWriter sw = new StreamWriter(cvsLogFile, true, Encoding.ASCII);
                    //sw.WriteLine(sError);
                    //sw.Close();
                    File.AppendAllText(Path.Combine(cvsLogFile), DateTime.Now.ToString() + ": " + psDetails + Environment.NewLine);
                }
            }
        }
    }
}
