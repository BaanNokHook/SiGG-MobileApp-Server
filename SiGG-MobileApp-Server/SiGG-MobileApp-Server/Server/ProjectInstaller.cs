using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.Server
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            this.serviceInstaller1.DelayedAutoStart = true;

            //Install service with Windows Authentication
            //this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.User;
            //this.serviceProcessInstaller1.Username = "TEN\\U0171868";
            //this.serviceProcessInstaller1.Password = "";

            //Install service with Local System
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Username = null;
            this.serviceProcessInstaller1.Password = null;
        }

        private void SetServiceName()
        {
            if (Context.Parameters.ContainsKey("ServiceName"))
            {
                this.serviceInstaller1.ServiceName = Context.Parameters["ServiceName"];
            }
        }

        private void SetDisplayName()
        {
            if (Context.Parameters.ContainsKey("DisplayName"))
            {
                this.serviceInstaller1.DisplayName = Context.Parameters["DisplayName"];
            }
        }

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            SetServiceName();
            SetDisplayName();
            base.OnBeforeInstall(savedState);
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            SetServiceName();
            base.OnBeforeUninstall(savedState);
        }
    }
}
