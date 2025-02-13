using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Usb.Events;
using System.IO;
using System.Runtime.InteropServices;
using Toolkit;
using System.Management;
using System.ServiceProcess;



namespace USB_detect_service
{
    public partial class Service1 : ServiceBase
    {
        static readonly IUsbEventWatcher usbEventWatcher = new UsbEventWatcher();
        static string logfile = "C:\\Windows\\Logs\\usb.log";
        static string errorlogfile = "C:\\Windows\\Logs\\errorusb.log";
        static string diskpartfile = "C:\\Windows\\Logs\\diskpart.txt";


        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            listenusb();
        }

        protected override void OnStop()
        {
        }
        protected override void OnContinue()
        {
            listenusb();
        }

        static void listenusb()
        {
            foreach (string path in usbEventWatcher.UsbDrivePathList)
            {
                //Console.WriteLine(path);
                File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + " Added " + path + "\n");
            }

            usbEventWatcher.UsbDriveInserted += (_, path) => eventUsbDriveInserted(path);

            usbEventWatcher.UsbDriveRemoved += (_, path) => eventUsbDriveRemoved(path);

            usbEventWatcher.UsbDeviceInserted += (_, device) => eventUsbDeviceInserted(device);

            usbEventWatcher.UsbDeviceRemoved += (_, device) => eventUsbDeviceRemoved(device);

            //Console.ReadLine();
        }

        static void eventUsbDriveInserted(string path)
        {
            File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + " Inserted " + path + "\n");
            //Console.WriteLine($"Inserted: {path}");
            System.Threading.Thread.Sleep(10000);
            bitlocker(path);





        }
        static void eventUsbDriveRemoved(string path)
        {
            File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + " Removed " + path + "\n");
            //Console.WriteLine($"Removed: {path}");

        }
        static void eventUsbDeviceInserted(UsbDevice device)
        {
            File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + " Inserted " + device.ToString() + "\n");
            //Console.WriteLine($"Inserted: {device}");
            //bitlocker(device.ToString());
        }
        static void eventUsbDeviceRemoved(UsbDevice device)
        {
            File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + " Removed " + device.ToString() + "\n");
            //Console.WriteLine($"Removed: {device}");

        }

        static void bitlocker(string path)
        {
            try
            {

                string surucu_1 = path.Substring(0, 2);


                string query = "SELECT * FROM Win32_EncryptableVolume WHERE DriveLetter = " + "'" + surucu_1 + "'";
                //            //MessageBox.Show(query);
                ManagementObjectSearcher searcher =
               new ManagementObjectSearcher("root\\cimv2\\Security\\MicrosoftVolumeEncryption", query);

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string driveletter = @"Drive DriveLetter: " + queryObj["DriveLetter"].ToString();
                    File.AppendAllText(logfile, DateTime.Now.ToString() +  "    " + driveletter + "\r\n");
                    string drivestatus = queryObj["ProtectionStatus"].ToString();
                    File.AppendAllText(logfile, DateTime.Now.ToString() + " drivestatus   " + drivestatus + "\r\n");
                    string Deviceid = queryObj["DeviceID"].ToString();
                    File.AppendAllText(logfile, DateTime.Now.ToString() + " Deviceid   " + Deviceid + "\r\n");
                    string ConversionStatus = queryObj["ConversionStatus"].ToString();
                    File.AppendAllText(logfile, DateTime.Now.ToString() + " ConversionStatus   " + ConversionStatus + "\r\n");
                    string EncryptionMethod = queryObj["EncryptionMethod"].ToString();
                    File.AppendAllText(logfile, DateTime.Now.ToString() + " EncryptionMethod   " + EncryptionMethod + "\r\n");

                    if (ConversionStatus == "0")
                    {
                        File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + " SIFRELENMEMIS " + path + "\n");
                        String applicationName = "Disk_encrypt.exe";
                        File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + " PIN ile SIRELENMIS " + path + "\n");

                        string[] lines = { "select volume " + path.Substring(0, 1), "disk readonly yapildi. \n" };
                        // WriteAllLines creates a file, writes a collection of strings to the file,
                        // and then closes the file.  You do NOT need to call Flush() or Close().
                        System.IO.File.WriteAllLines(diskpartfile, lines);

                        File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + "added diskpart.txt file : select volume" + path.Substring(0, 1) + "\n");
                        File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + "added diskpart.txt file : attributes disk set readonly" + path.Substring(0, 1));


                        Process dskcmd1 = new Process();
                        dskcmd1.StartInfo.FileName = @"C:\windows\System32\diskpart.exe";
                        dskcmd1.StartInfo.Arguments = " /s  " + diskpartfile;
                        dskcmd1.StartInfo.UseShellExecute = false;
                        dskcmd1.StartInfo.CreateNoWindow = true;
                        dskcmd1.StartInfo.RedirectStandardOutput = true;
                        dskcmd1.StartInfo.RedirectStandardError = true;
                        dskcmd1.Start();
                        System.IO.File.Delete(diskpartfile);

                        // launch the application
                        ApplicationLoader.PROCESS_INFORMATION procInfo;
                        ApplicationLoader.StartProcessAndBypassUAC(applicationName, out procInfo);

                        File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + applicationName+ " Uygulaması Baslatiliyor " + path + "\n" + "\r\n");

                    }
                    else
                    {
                        if (drivestatus=="2")
                        {
                            File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + " PIN ile SIRELENMIS " + path + "\n");

                                                        string[] lines = { "select volume " + path.Substring(0, 1), " disk set readonly" };
                            // WriteAllLines creates a file, writes a collection of strings to the file,
                            // and then closes the file.  You do NOT need to call Flush() or Close().
                            System.IO.File.WriteAllLines(diskpartfile, lines);

                                File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + "added diskpart.txt file : select volume" + path.Substring(0, 1) + "\n");
                                File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + "added diskpart.txt file : attributes disk set readonly" + path.Substring(0, 1));


                                Process dskcmd1 = new Process();
                                dskcmd1.StartInfo.FileName = @"C:\windows\System32\diskpart.exe";
                                dskcmd1.StartInfo.Arguments = " /s  " + diskpartfile;
                                dskcmd1.StartInfo.UseShellExecute = false;
                                dskcmd1.StartInfo.CreateNoWindow = true;
                                dskcmd1.StartInfo.RedirectStandardOutput = true;
                                dskcmd1.StartInfo.RedirectStandardError = true;
                                dskcmd1.Start();
                            System.IO.File.Delete(diskpartfile);


                            // launch the application
                            String applicationName = "Disk_encrypt.exe";
                            ApplicationLoader.PROCESS_INFORMATION procInfo;
                            ApplicationLoader.StartProcessAndBypassUAC(applicationName, out procInfo);


                            File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + applicationName + " Uygulaması Baslatiliyor " + path + "\n" + "\r\n");
                        }
                        else
                        {

                            File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + " BITLOCKER AKTIF " + path + "\n" + "\r\n");

                            File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + "added diskpart.txt file : select volume" + path.Substring(0, 1) + "\n");
                            File.AppendAllText(logfile, DateTime.Now.ToString() + "\t" + "added diskpart.txt file : attributes disk clear readonly" + path.Substring(0, 1));


                            Process dskcmd1 = new Process();
                            dskcmd1.StartInfo.FileName = @"C:\windows\System32\diskpart.exe";
                            dskcmd1.StartInfo.Arguments = " /s  " + diskpartfile;
                            dskcmd1.StartInfo.UseShellExecute = false;
                            dskcmd1.StartInfo.CreateNoWindow = true;
                            dskcmd1.StartInfo.RedirectStandardOutput = true;
                            dskcmd1.StartInfo.RedirectStandardError = true;
                            dskcmd1.Start();
                            System.IO.File.Delete(diskpartfile);
                            bitlockerbackupad(surucu_1);
                        }
                    }


                }
            }

            catch (Exception hata)
            {
                File.AppendAllText(errorlogfile, DateTime.Now.ToString() + "\t" + "  " + hata.ToString() + "\n");
            }

        }

        static void bitlockerbackupad(string drive1)
        {
            IntPtr wow64Value = IntPtr.Zero;
            try
            {
                Wow64Interop.DisableWow64FSRedirection(ref wow64Value);
                Process bit_cmd1 = new Process();
                bit_cmd1.StartInfo.FileName = @"C:\windows\System32\manage-bde.exe";
                bit_cmd1.StartInfo.Arguments = " -protectors -get  " + drive1;
                bit_cmd1.StartInfo.UseShellExecute = false;
                bit_cmd1.StartInfo.CreateNoWindow = true;
                bit_cmd1.StartInfo.RedirectStandardOutput = true;
                bit_cmd1.StartInfo.RedirectStandardError = true;
                bit_cmd1.Start();

                string sonuc = bit_cmd1.StandardOutput.ReadToEnd();
                //textBox1.Text = sonuc.ToString();
                bit_cmd1.Close();
                File.AppendAllText(logfile, DateTime.Now.ToString() + "   C:\\windows\\System32\\manage-bde.exe -protectors -get  " + drive1 + "     Komutu Tamamlandi.\n" + Environment.NewLine);

                int pform = sonuc.IndexOf("{") + "{".Length;
                //int pto = sonuc.LastIndexOf("}");
                string USB_id = sonuc.Substring(pform, 36);





                Process bit_cmd3 = new Process();
                bit_cmd3.StartInfo.FileName = @"C:\windows\System32\manage-bde.exe";
                bit_cmd3.StartInfo.Arguments = " -protectors -adbackup " + drive1 + " -id " + "{" + USB_id + "}";
                bit_cmd3.StartInfo.UseShellExecute = false;
                bit_cmd3.StartInfo.CreateNoWindow = true;
                bit_cmd3.StartInfo.RedirectStandardOutput = true;
                bit_cmd3.StartInfo.RedirectStandardError = true;
                bit_cmd3.Start();
                //textBox1.AppendText(bit_cmd3.StandardOutput.ReadToEnd());
                bit_cmd3.Close();
                File.AppendAllText(logfile, DateTime.Now.ToString() + "   C:\\windows\\System32\\manage-bde.exe " + "-protectors - adbackup " + drive1 + " - id " + "{ " + USB_id + "}" + "     Command completed.\n" + Environment.NewLine);
                File.AppendAllText(logfile, DateTime.Now.ToString() + "{ " + USB_id + "}" + "     Recovery password uploaded to AD.\n" + Environment.NewLine);
                File.AppendAllText(logfile, DateTime.Now.ToString() + "--------------------------------------------------------------------\n" + Environment.NewLine);
            }
            catch (Exception err)
            {
                File.AppendAllText(logfile, DateTime.Now.ToString() + "Unabled to disable/enable WOW64 File System Redirection");
                File.AppendAllText(logfile, DateTime.Now.ToString() + err.Message);
            }
            finally
            {
                Wow64Interop.Wow64RevertWow64FsRedirection(wow64Value);
            }
        }




    }
}
