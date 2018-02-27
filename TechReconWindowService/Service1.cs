using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
//using System.Timers;
using System.Web;
using System.IO;
using JournalProcessingLIBGH;
using Models;
using TechReconWindowService.BAL.ReconciliationService;
using Excel;
using System.Reflection;
//using System.Threading;
using System.Timers;
using TechReconWindowService.BAL.Helpers;



namespace TechReconWindowService
{
    public partial class Service1 : ServiceBase
    {
        static System.Threading.Timer timer;
        static System.Threading.Timer timer2;
        //private System.Threading.Timer timer1 = null;
        private Timer timer3 = null;
        Library library = new Library();
        RiaTransLibrary riaTransLibrary = new RiaTransLibrary();
        XMLibrary xMLibrary = new XMLibrary();
        MTNLibrary mTNLibrary = new MTNLibrary();
        JournalLibrary journalLibrary = new JournalLibrary();
        WUMTLibrary wUMT = new WUMTLibrary();
        ConsortiumLibrary consortiumLibrary = new ConsortiumLibrary();
        MGLibrary mGLibrary = new MGLibrary();
        ULLibrary unityLibrary = new ULLibrary();
        AirtelLibrary airtellibrary = new AirtelLibrary();
        NostroVostroLibrary nostroVostroLibrary = new NostroVostroLibrary();
        MT940950ReconModel mT940950ReconModel = new MT940950ReconModel();
        AfricaWorldLibrary africaWorldLibrary = new AfricaWorldLibrary();
        AfricaWorldReconModel africaWorldReconModel = new AfricaWorldReconModel();

        ReconciliationModel reconciliationModel = new ReconciliationModel();
        private string _methodname;
        private string _lineErrorNumber;
        private string _classname = "TechReconWindowService/OnStart";
        private string cvsLogFile = System.Configuration.ConfigurationManager.AppSettings["LogFile"];
        private string filePath = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"];
        private string LogSize = System.Configuration.ConfigurationManager.AppSettings["LogSize"];
        public Service1()
        {
            InitializeComponent();
        }
        public void OnDebug()
        {
            OnStart(null);
        }
        protected override async void  OnStart(string[] args)
        {
            try
            {

              //  await test();


              LogManager.SaveLog("TechRecon Service Start");

                // await journalLibrary.PullJournalData();
                // await mTNLibrary.PullMTNData();
                //await reconciliationModel.MatchMTN();
                //await reconciliationModel.MoveMTNTransToHistory();

                // await DefaultProcess(); 
                //await nostroVostroLibrary.PullNostroVostroData();

                System.Threading.TimerCallback cb = new System.Threading.TimerCallback(RiaTrans);
                clsTime time = new clsTime();
                timer = new System.Threading.Timer(cb, time, 5000, 5000);

                System.Threading.TimerCallback cb1 = new System.Threading.TimerCallback(test1);
                clsTime time1 = new clsTime();
                timer2 = new System.Threading.Timer(cb1, time, 60000, 60000);

            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in OnStart Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }

            LogManager.SaveLog("TechRecon Service End");

        }

        //protected override void OnStart(string[] args)
        //{
        //    //Library library = new Library();
        //    library.SaveLog("I just started the windows TechRecon service");
        //    timer3 = new System.Timers.Timer();
        //    this.timer3.Interval = 5000;
        //    this.timer3.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimedMailSenderEvent_Tick);
        //    timer3.Enabled = true;

        //}
        //private void OnTimedMailSenderEvent_Tick(Object source, System.Timers.ElapsedEventArgs e)
        //{

        //    try
        //    {
        //        //Library library = new Library();
        //        library.SaveLog("I just started the windows service");

        //        timer3.Enabled = false;

        //        var tasks = new[]
        //        {
                  
        //             Task.Factory.StartNew(() =>  ProcessTimerEvent(null)),

            

        //        };
        //        Task.WaitAll(tasks);
        //    }
        //    catch (Exception ex)
        //    {
        //        timer3.Enabled = true;

        //        library.SaveLog("TechRecon: " + ex.InnerException.Message);
        //    }
        //    finally
        //    {
        //        timer3.Enabled = true;

        //    }
        //}
        private async void test(object obj)
        {
            Task[] tasks = new Task[3];
            tasks[0] = journalLibrary.PullJournalData();
            tasks[1] = africaWorldLibrary.PullAfricaWorldData();
            tasks[2] = test2();
            //return "";
        }

        private async Task<string> test2()
        {
            LogManager.SaveLog("test2");
            return "";

        }

        private async void test1(object obj)
        {
            LogManager.SaveLog("test1 after 1 min");

        }
        protected override void OnStop()
        {
            LogManager.SaveLog("TechRecon Service Stopped");
        }
        private async void ProcessTimerEvent(object obj)
        {
            try
            {
                 LogManager.SaveLog("Start Task ProcessTimerEvent");
               
                  //Consortium       
                 await consortiumLibrary.PullConsortiumData();
                 await reconciliationModel.MatchCBSTranAndConsortiumTrans();
                 await reconciliationModel.CBSTranAndConsortiumTransHistory();
                ///Ria
                 await riaTransLibrary.PullRiaData();
                 await reconciliationModel.MatchCBSRiaAndRia();
                 await reconciliationModel.CBSRiaAndRiaHistory();
                ///XpressMoney 
                 await xMLibrary.PullXpressMoneyData();
                 await reconciliationModel.MatchCBSXpressMoneyAndXpressMoney();
                 await reconciliationModel.CBSxpressAndXperessHistory();
                
               // Westerrn Union 
                 await wUMT.PullWUMTData();
                 await reconciliationModel.MatchCBSWUMTAndWUMTTrans();
                 await reconciliationModel.CBSWUMTTransHistory();
                //Money Gram 
                 await mGLibrary.PullMoneyGramData();
                 await reconciliationModel.MatchCBSMGAndMGTrans();
                 await reconciliationModel.CBSMGTransAndMGTransHistory();

                //Unity Link
                 await unityLibrary.PullUnityLinkData();
                 await reconciliationModel.MatchUnityLink();
                 await reconciliationModel.MoveUnityTransToHistory();

                //MTN
                 await mTNLibrary.PullMTNData();
                 await reconciliationModel.MatchMTN();
                 await reconciliationModel.MoveMTNTransToHistory();

                // Airtel
                 await airtellibrary.PullAirtelData();
                 await reconciliationModel.MatchAirtel();
                 await reconciliationModel.MoveAirtelTransToHistory();

                //NostroVostro
                  await nostroVostroLibrary.PullNostroVostroData();
                  await mT940950ReconModel.MatchNostroVostro();
                  await mT940950ReconModel.MoveNotroVostrHistory();

                  //Africa World 
                  await africaWorldLibrary.PullAfricaWorldData();
                  await africaWorldReconModel.MatchAfricaWorld();
                  await africaWorldReconModel.MoveAfricaWorldToHis();

                  Task<string> hhh =  journalLibrary.PullJournalData();
                  string juy = await hhh;

                Task[] tasks = new Task[2];
                tasks[0] = journalLibrary.PullJournalData();
                tasks[1] = africaWorldLibrary.PullAfricaWorldData();
                //From above, At this point, all two tasks are running at the same time.
                // await Task.WhenAll();
                LogManager.SaveLog("End Task ProcessTimerEvent");
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in ProcessTimerEvent Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
        }
        private async void RiaTrans(object obj)
        {
            try
            {
                LogManager.SaveLog("Start Task RiaTrans");
                ///Ria
                await riaTransLibrary.PullRiaData();
                await reconciliationModel.MatchCBSRiaAndRia();
                await reconciliationModel.CBSRiaAndRiaHistory();

                LogManager.SaveLog("End Task RiaTrans");
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in ProcessTimerEvent Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
        }
        private async void XM(object obj)
        {
            try
            {
                LogManager.SaveLog("Start Task XMTrans");
                ///XpressMoney 
                await xMLibrary.PullXpressMoneyData();
                await reconciliationModel.MatchCBSXpressMoneyAndXpressMoney();
                await reconciliationModel.CBSxpressAndXperessHistory();

                LogManager.SaveLog("End Task XMTrans");
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in ProcessTimerEvent Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
        }
        private async void WUMT(object obj)
        {
            try
            {
                LogManager.SaveLog("Start Task WUMTTrans");
                // Westerrn Union 
                await wUMT.PullWUMTData();
                await reconciliationModel.MatchCBSWUMTAndWUMTTrans();
                await reconciliationModel.CBSWUMTTransHistory();

                LogManager.SaveLog("End Task WUMTTrans");
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in ProcessTimerEvent Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
        }
        private async void MG(object obj)
        {
            try
            {
                LogManager.SaveLog("Start Task MGs");

                await mGLibrary.PullMoneyGramData();
                await reconciliationModel.MatchCBSMGAndMGTrans();
                await reconciliationModel.CBSMGTransAndMGTransHistory();
                LogManager.SaveLog("End Task WUMTTrans");
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in ProcessTimerEvent Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
        }
        private async void UL(object obj)
        {
            try
            {
                LogManager.SaveLog("Start Task UL");
                await unityLibrary.PullUnityLinkData();
                await reconciliationModel.MatchUnityLink();
                await reconciliationModel.MoveUnityTransToHistory();
                LogManager.SaveLog("End Task UL");
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in ProcessTimerEvent Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
        }
        private async void MTN(object obj)
        {
            try
            {
                LogManager.SaveLog("Start Task MTN");
                await mTNLibrary.PullMTNData();
                await reconciliationModel.MatchMTN();
                await reconciliationModel.MoveMTNTransToHistory();
                LogManager.SaveLog("End Task MTN");
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in ProcessTimerEvent Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
        }
        private async void Airtel(object obj)
        {
            try
            {
                LogManager.SaveLog("Start Task Airtel");
                await airtellibrary.PullAirtelData();
                await reconciliationModel.MatchAirtel();
                await reconciliationModel.MoveAirtelTransToHistory();
                LogManager.SaveLog("End Task Airtel");
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in ProcessTimerEvent Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
        }
        private async void NostroVostro(object obj)
        {
            try
            {
                LogManager.SaveLog("Start Task NostroVostro");
                await nostroVostroLibrary.PullNostroVostroData();
               // await mT940950ReconModel.MatchNostroVostro();
                //await mT940950ReconModel.MoveNotroVostrHistory();
                LogManager.SaveLog("End Task NostroVostro");
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in ProcessTimerEvent Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
        }
        private async void AfriWorld(object obj)
        {
            try
            {
                LogManager.SaveLog("Start Task NostroVostro");
                await africaWorldLibrary.PullAfricaWorldData();
                await africaWorldReconModel.MatchAfricaWorld();
                await africaWorldReconModel.MoveAfricaWorldToHis();

                LogManager.SaveLog("End Task NostroVostro");
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in ProcessTimerEvent Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr);
                throw;
            }
        }

        private async void DefaultProcess(object obj)
        {
            try
            {
                LogManager.SaveLog("Task Start in DefaultProcess");

                Task[] tasks = new Task[18];               

                //Consortium       
                tasks[0] = consortiumLibrary.PullConsortiumData();
                tasks[1] = reconciliationModel.MatchCBSTranAndConsortiumTrans();
                tasks[2] = reconciliationModel.CBSTranAndConsortiumTransHistory();
                
                //ria
                tasks[3] = riaTransLibrary.PullRiaData();
                tasks[4] = reconciliationModel.MatchCBSRiaAndRia();
                tasks[5] = reconciliationModel.CBSRiaAndRiaHistory();
                
               //XM
                tasks[6] = xMLibrary.PullXpressMoneyData();
                tasks[7] = reconciliationModel.MatchCBSXpressMoneyAndXpressMoney();
                tasks[8] = reconciliationModel.CBSxpressAndXperessHistory();
                //WUMT
                tasks[9] = wUMT.PullWUMTData();
                tasks[10] = reconciliationModel.MatchCBSWUMTAndWUMTTrans();
                tasks[11] = reconciliationModel.CBSWUMTTransHistory();
               //MG
                tasks[12] = mGLibrary.PullMoneyGramData();
                tasks[13] = reconciliationModel.MatchCBSMGAndMGTrans();
                tasks[14] = reconciliationModel.CBSMGTransAndMGTransHistory();
               //UL
                tasks[15] = unityLibrary.PullUnityLinkData();
                tasks[16] = reconciliationModel.MatchUnityLink();
                tasks[17] = reconciliationModel.MoveUnityTransToHistory();

                ////MTN 
                //await mTNLibrary.PullMTNData();
                //await reconciliationModel.MatchMTN();
                //await reconciliationModel.MoveMTNTransToHistory();

                //// Airtel
                //await airtellibrary.PullAirtelData();
                //await reconciliationModel.MatchAirtel();
                //await reconciliationModel.MoveAirtelTransToHistory();

                //////NostroVostro
                //await nostroVostroLibrary.PullNostroVostroData();
                //await mT940950ReconModel.MatchNostroVostro();
                //await mT940950ReconModel.MoveNotroVostrHistory();

                ////////Africa World 
                //await africaWorldLibrary.PullAfricaWorldData();
                //await africaWorldReconModel.MatchAfricaWorld();
                //await africaWorldReconModel.MoveAfricaWorldToHis();

                //////Journal 
               //await journalLibrary.PullJournalData();
               // await Task.WhenAll();
                LogManager.SaveLog("Task End in DefaultProcess");
            }
            catch (Exception ex)
            {
                var exErr = ex == null ? ex.InnerException.Message : ex.Message;
                var stackTrace = new StackTrace(ex);
                var thisasm = Assembly.GetExecutingAssembly();
                _methodname = stackTrace.GetFrames().Select(f => f.GetMethod()).First(m => m.Module.Assembly == thisasm).Name;
                _lineErrorNumber = ex.StackTrace.Substring(ex.StackTrace.Length - 7, 7);
                LogManager.SaveLog("An error occured in ProcessTimerEvent Line: " + _lineErrorNumber + " CLASSNAME: " + _classname + " METHOD NAME: [" + _methodname + "] ERROR: " + exErr); 
            }

           // return string.Empty;
        }
        public static void SaveLogstatic(string psDetails)
        {
          try
          {
              string cvsLogFile = System.Configuration.ConfigurationManager.AppSettings["LogFile"];
              string filePath = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"];
              string LogSize = System.Configuration.ConfigurationManager.AppSettings["LogSize"];

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

              if (cvsLogFile != null)
              {
                  string sError = DateTime.Now.ToString() + ": " + psDetails;
                  StreamWriter sw = new StreamWriter(cvsLogFile, true, Encoding.ASCII);
                  sw.WriteLine(sError);
                  sw.Close();
              }
          }
          catch (Exception ex1)
          {
              
          }
      }

    }
    class clsTime
    {
        public string GetTimeString ()
        {
            string str = DateTime.Now.ToString ();
            int index = str.IndexOf(" ");
            return (str.Substring (index + 1));
        }
    }
}



