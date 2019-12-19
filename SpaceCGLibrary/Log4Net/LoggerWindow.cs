using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Media;
using log4net.Core;
using SpaceCG.WindowsAPI.WinUser;

namespace SpaceCG.Log4Net
{
    /// <summary>
    /// 独立的日志窗体对象
    /// </summary>
    public partial class LoggerWindow : Window
    {
        private IntPtr Handle;
        private HwndSource HwndSource;

        protected TextBox TextBox;
        protected ListView ListView;
        protected ListBoxAppender ListBoxAppender;

        /// <summary>
        /// Logger Window
        /// </summary>
        /// <param name="log"></param>
        public LoggerWindow()
        {
            OnInitializeControls();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        protected override void OnClosed(EventArgs e)
        {
            if (Handle != null)
            {
                bool result = WinUser.UnregisterHotKey(Handle, 0);
                Console.WriteLine("Logger Window UnregisterHotKey State:{0}", result);
            }

            if(HwndSource != null)  HwndSource.Dispose();
            ListView.SelectionChanged -= ListView_SelectionChanged;
        }

        /// <summary>
        /// 初使化 UI 控件
        /// </summary>
        protected void OnInitializeControls()
        {
            //Grid
            Grid grid = new Grid();
            //grid.ShowGridLines = true;
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0.85, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0.15, GridUnitType.Star) });

            //ListView
            this.ListView = new ListView();
            this.ListView.SelectionChanged += ListView_SelectionChanged;
            this.ListBoxAppender = new ListBoxAppender(ListView, 1024);

            //GridSplitter
            GridSplitter splitter = new GridSplitter()
            {
                Height = 4.0,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            //TextBox
            this.TextBox = new TextBox()
            {
                IsReadOnly = true,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.WrapWithOverflow,
            };
            TextBox.MouseDoubleClick += (s, e) => { ClearTextBox(); };
            TextBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);

            grid.Children.Add(ListView);
            grid.Children.Add(splitter);
            grid.Children.Add(TextBox);
            Grid.SetRow(ListView, 0);
            Grid.SetRow(splitter, 1);
            Grid.SetRow(TextBox, 2);

            this.Title = "Logger Window";
            this.Width = 960;
            this.Height = 600;
            this.Content = grid;
            this.Loaded += LoggerWindow_Loaded;
        }
        

        /// <summary>
        /// 清除 TextBox 内容
        /// </summary>
        protected void ClearTextBox()
        {
            this.TextBox.Text = "";
            this.TextBox.Foreground = Brushes.Black;
            this.TextBox.FontWeight = FontWeights.Normal;
        }

        /// <summary>
        /// ListView Select Changed Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem item = (ListViewItem)this.ListView.SelectedItem;
            
            if(item != null)
            {
                this.TextBox.Text = item.ToolTip.ToString();

                LoggingEvent logger = (LoggingEvent)item.Content;
                this.TextBox.Foreground = logger.Level >= Level.Error ? Brushes.Red : Brushes.Black;
                this.TextBox.FontWeight = logger.Level >= Level.Warn ? FontWeights.Black : FontWeights.Normal;
            }
            else
            {
                ClearTextBox();
            }
        }

        /// <summary>
        /// Window Loaded Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoggerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Handle = new WindowInteropHelper(this).Handle;
            HwndSource = HwndSource.FromHwnd(Handle);
            HwndSource.AddHook(WindowProcHandler);

            bool result = WinUser.RegisterHotKey(Handle, 0, RhkModifier.CONTROL, VirtualKeyCode.VK_L);
            Console.WriteLine("Logger Window RegisterHotKey State:{0}", result);
        }
        /// <summary>
        /// Window Process Handler
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        protected IntPtr WindowProcHandler(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            MessageType msgType = (MessageType)msg;
            if(msgType == MessageType.WM_HOTKEY)
            {
                RhkModifier rhk = (RhkModifier)(lParam.ToInt32() & 0xFFFF);     //低双字节
                VirtualKeyCode key = (VirtualKeyCode)(lParam.ToInt32() >> 16);  //高双字节 key

                if(rhk == RhkModifier.CONTROL && key == VirtualKeyCode.VK_L)
                {
                    if (this.WindowState == WindowState.Minimized)
                        this.WindowState = WindowState.Normal;

                    this.Show();
                    this.Activate();
                    handled = true;
                }
            }

            return IntPtr.Zero;
        }
    }
}
