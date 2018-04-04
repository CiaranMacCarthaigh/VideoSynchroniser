using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using VideoSynchroniserService.WCF;

namespace VideoSynchroniserService
{
    public partial class Service : ServiceBase
    {
        public ServiceHost _serviceHost = null;

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Debugger.Launch();
            if (_serviceHost != null)
            {
                _serviceHost.Close();
            }

            IMediaSupplier mediaSupplier = new MediaSupplier();

            // Create a ServiceHost for the type and 
            // provide the base address.
            _serviceHost = new ServiceHost(mediaSupplier);

            // Open the ServiceHostBase to create listeners and start 
            // listening for messages.
            _serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
                _serviceHost = null;
            }
        }
    }
}
