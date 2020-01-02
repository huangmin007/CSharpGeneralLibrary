## HwndSourceExample
```C#
using System;
using System.Windows;
using System.Windows.Interop;
using System.ComponentModel;
using SpaceCG.WindowsAPI.WinUser;
using System.Runtime.InteropServices;

namespace SpaceCG.Examples
{
    public class HwndSourceExample:Window
    {
        private IntPtr handle;
        private HwndSource hwndSource;


        public HwndSourceExample()
        {
            this.Loaded += HwndSourceExample_Loaded;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            hwndSource?.Dispose();
            this.Loaded -= HwndSourceExample_Loaded;
        }

        private void HwndSourceExample_Loaded(object sender, RoutedEventArgs e)
        {
            hwndSource = PresentationSource.FromVisual(this) as HwndSource;
            hwndSource?.AddHook(new HwndSourceHook(WindowProcHandler));

            //OR
            //handle = new WindowInteropHelper(this).Handle;
            //HwndSource.FromHwnd(handle).AddHook(WindowProcHandler);
        }

        protected IntPtr WindowProcHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
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
```