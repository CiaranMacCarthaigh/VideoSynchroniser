using System.ServiceProcess;

namespace VideoSynchroniserService
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
                new Service()
                {
                    ServiceName = WindowsServiceInstaller.ServiceName
                }
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}