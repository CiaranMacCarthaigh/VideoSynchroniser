using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace VideoSynchroniserService
{
    [RunInstaller(true)]
    public class WindowsServiceInstaller : Installer
    {
        public const string ServiceName = "VideoSynchroniserService";

        public WindowsServiceInstaller()
        {
            ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
            ServiceInstaller serviceInstaller = new ServiceInstaller();

            //# Service Account Information
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceProcessInstaller.Username = null;
            serviceProcessInstaller.Password = null;


            //# Service Information
            serviceInstaller.DisplayName = "Video synchroniser service";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            //# This must be identical to the WindowsService.ServiceBase name
            //# set in the constructor of WindowsService.cs
            serviceInstaller.ServiceName = ServiceName;

            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
