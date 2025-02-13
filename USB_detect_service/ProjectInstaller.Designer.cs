namespace USB_detect_service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.usb_detect = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // usb_detect
            // 
            this.usb_detect.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.usb_detect.Password = null;
            this.usb_detect.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.DelayedAutoStart = true;
            this.serviceInstaller1.Description = "usb_detect";
            this.serviceInstaller1.DisplayName = "usb_detect";
            this.serviceInstaller1.ServiceName = "usb_detect";
            this.serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.usb_detect,
            this.serviceInstaller1});

        }

        #endregion
        public System.ServiceProcess.ServiceInstaller serviceInstaller1;
        public System.ServiceProcess.ServiceProcessInstaller usb_detect;
    }
}