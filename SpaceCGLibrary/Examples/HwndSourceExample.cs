using SpaceCG.WindowsAPI.DBT;
using SpaceCG.WindowsAPI.WinUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace SpaceCG.Examples
{
    public class HwndSourceExample:IDisposable
    {
        HwndSource hwndSource;

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

        public void Dispose()
        {
            hwndSource.Dispose();
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
                        Console.WriteLine("Device Arrival");
                        break;

                    case DeviceBroadcastType.DBT_DEVICEREMOVECOMPLETE:
                        Console.WriteLine("Device Move Complete");
                        break;

                    default:
                        break;
                }
                handled = true; //这里有一个引用传递的参数handled ，处理消息后设置为true 告诉系统这个消息已经处理过了。

            }

            return IntPtr.Zero;
        }
    }
}
