using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TechReconWindowService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Service1() 
            };
            ServiceBase.Run(ServicesToRun);

#if DEBUG

            Service1 Scheduler = new Service1();
            Scheduler.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else


                                    ServiceBase[] ServicesToRun;
                                    ServicesToRun = new ServiceBase[] 
                                    { 
                                        new Scheduler() 
                                    };
                                    ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
