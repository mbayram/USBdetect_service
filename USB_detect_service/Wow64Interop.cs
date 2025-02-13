using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace USB_detect_service
{
    public class Wow64Interop
    {
        const string Kernel32dll = "Kernel32.Dll";

        [DllImport(Kernel32dll, EntryPoint = "Wow64DisableWow64FsRedirection")]
        public static extern bool DisableWow64FSRedirection(ref IntPtr ptr);

        [DllImport(Kernel32dll, EntryPoint = "Wow64RevertWow64FsRedirection")]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);
    }
}
