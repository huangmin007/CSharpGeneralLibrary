using SpaceCG.WindowsAPI.DBT;
using SpaceCG.WindowsAPI.WinUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace SpaceCG.Examples
{
    public class HwndSourceExample:IDisposable
    {
        HwndSource hwndSource;


        public void Dispose()
        {
            if(hwndSource != null)
                hwndSource.Dispose();
        }


        public HwndSourceExample()
        {
            //示例
            Window win = new Window();

            hwndSource = PresentationSource.FromVisual(win) as HwndSource;
            if (hwndSource != null)
                hwndSource.AddHook(new HwndSourceHook(WindowProc));

            //OR
            //IntPtr hwnd = new WindowInteropHelper(win).Handle;
            //HwndSource.FromHwnd(hwnd).AddHook(WindowProcHandler);
        }

        protected IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            MessageType msgType = (MessageType)msg;
            Console.WriteLine(msgType);

            if (msg == (int)MessageType.WM_DEVICECHANGE)
            {
                DeviceBroadcastType dbt = (DeviceBroadcastType)wParam.ToInt32();
                Console.WriteLine(dbt);

                switch (dbt)
                {
                    case DeviceBroadcastType.DBT_DEVICEARRIVAL:
                    case DeviceBroadcastType.DBT_DEVICEREMOVECOMPLETE:
                        Console.WriteLine(dbt == DeviceBroadcastType.DBT_DEVICEARRIVAL ? "Device Arrival" : "Device Move Complete");

                        DEV_BROADCAST_HDR hdr = Marshal.PtrToStructure<DEV_BROADCAST_HDR>(lParam);
                        Console.WriteLine("{0}", hdr);

                        if (hdr.dbch_devicetype == DeviceType.DBT_DEVTYP_PORT)
                        {
                            DEV_BROADCAST_PORT port = Marshal.PtrToStructure<DEV_BROADCAST_PORT>(lParam);
                            Console.WriteLine(port);
                        }
                        if (hdr.dbch_devicetype == DeviceType.DBT_DEVTYP_VOLUME)
                        {
                            DEV_BROADCAST_VOLUME volume = Marshal.PtrToStructure<DEV_BROADCAST_VOLUME>(lParam);
                            Console.WriteLine(volume);
                        }
                        break;

                    default:
                        break;
                }

                handled = true;
            }

            return IntPtr.Zero;
        }
    }
}
