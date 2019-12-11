using log4net.Appender;
using System;
using log4net.Core;
using System.Windows.Controls;
using log4net.Layout;
using System.Windows;
using System.Data;
using System.Windows.Data;
using System.Globalization;

namespace SpaceCG.Log4Net
{
    /// <summary>
    /// 
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class MultiStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || parameter == null) return "";

            return string.Format(parameter.ToString(), values);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Log4Net WPF ListBox/ListView Appender
    /// </summary>
    public class ListBoxAppender : AppenderSkeleton
    {
        /// <summary>
        /// 获取或设置最大可见行数
        /// </summary>
        protected uint MaxLines = 512;
        /// <summary> ListBox </summary>
        protected ListBox ListBox;
        /// <summary> ListView </summary>
        protected ListView ListView;
        /// <summary> TextBox.AppendText Delegate Function </summary>
        protected Action<LoggingEvent> AppendLoggingEventDelegate;

        private bool changeBgColor = true;  //切换背景标志变量

        /// <summary>
        /// Log4Net Appender for WPF ListBox and ListView
        /// </summary>
        /// <param name="listBox"></param>
        public ListBoxAppender(ListBox listBox)
        {
            if (listBox == null) throw new ArgumentNullException("参数不能为空");

            this.AppendLoggingEventDelegate = AppendLoggingEvent;
            this.Layout = new PatternLayout("[%date{HH:mm:ss.fff}] [%thread] [%5level] [%method(%line)] %logger - %message (%r) %newline");

            DefaultStyle(listBox);
            log4net.Config.BasicConfigurator.Configure(this);
        }

        /// <summary>
        /// 设置控件默认样式
        /// </summary>
        /// <param name="listBox"></param>
        protected void DefaultStyle(ListBox listBox)
        {
            // 必须使用 typeof 运算符
            if (listBox.GetType() == typeof(ListBox))
            {
                this.ListBox = listBox;
                this.ListBox.VerticalContentAlignment = VerticalAlignment.Center;
            }
            else if (listBox.GetType() == typeof(ListView))
            {
                this.ListView = listBox as ListView;
                this.ListView.SelectionMode = SelectionMode.Single;

                GridViewColumn ID = new GridViewColumn()
                {
                    Header = "ID",
                };
                GridViewColumn Time = new GridViewColumn()
                {
                    Width = 90,
                    Header = "Time",
                    DisplayMemberBinding = new Binding("TimeStamp") { StringFormat = "HH:mm:ss.fff" },
                };
                GridViewColumn Location = new GridViewColumn()
                {
                    Width = 120,
                    Header = "Location",
                    DisplayMemberBinding = new MultiBinding()
                    {
                        Bindings = {
                            //new Binding("LocationInformation.ClassName"),
                            new Binding("LocationInformation.MethodName"),
                            new Binding("LocationInformation.LineNumber"),
                        },
                        Converter = new MultiStringConverter(),
                        ConverterParameter = "{0}({1})",
                    }
                };

                GridView view = new GridView();
                //view.Columns.Add(ID);
                view.Columns.Add(Time);
                //view.Columns.Add(CreateColumn("UserName", "UserName"));
                //view.Columns.Add(CreateColumn("Identity", "Identity"));
                view.Columns.Add(CreateColumn("Thread", "ThreadName", 50));
                view.Columns.Add(CreateColumn("Level", "Level", 60));
                view.Columns.Add(Location);
                view.Columns.Add(CreateColumn("Logger", "LoggerName", 120));
                view.Columns.Add(CreateColumn("Message", "RenderedMessage", 240));
                view.Columns.Add(CreateColumn("Exception", "ExceptionObject", 240));

                ListView.View = view;

                //Setter set = new Setter(Control.FontWeightProperty, FontWeights.Bold);
                //column.HeaderContainerStyle.Setters.Add(set);
            }
            else
            {
                // ...
            }
        }

        /// <summary>
        /// Log4Net Appender for WPF ListBox and ListView
        /// </summary>
        /// <param name="listBox"></param>
        /// <param name="maxLines">最大行数为 1024 行，默认为 512 行</param>
        public ListBoxAppender(ListBox listBox, uint maxLines):this(listBox)
        {
            this.MaxLines = maxLines > 1024 ? 1024 : maxLines;
        }

        /// <summary>
        /// @override
        /// </summary>
        /// <param name="loggingEvent"></param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            this.ListBox?.Dispatcher.BeginInvoke(AppendLoggingEventDelegate, loggingEvent);
            this.ListView?.Dispatcher.BeginInvoke(AppendLoggingEventDelegate, loggingEvent);
        }

        /// <summary>
        /// Addend Logging Event
        /// </summary>
        /// <param name="loggingEvent"></param>
        protected void AppendLoggingEvent(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null) return;

            //LoggingEvent
            String text = string.Empty;
            PatternLayout patternLayout = this.Layout as PatternLayout;
            if (patternLayout != null)
            {
                text = patternLayout.Format(loggingEvent);
                if (loggingEvent.ExceptionObject != null)
                    text += loggingEvent.ExceptionObject.ToString() + Environment.NewLine;
            }
            else
            {
                text = loggingEvent.LoggerName + "-" + loggingEvent.RenderedMessage + Environment.NewLine;
            }

            //ListBox
            if(this.ListBox != null)
            {
                ListBoxItem item = new ListBoxItem();
                item.Height = 24;
                item.Content = text.TrimEnd();
                item.ToolTip = item.Content;
                item.Background = TextBoxBaseAppender.GetColorBrush(loggingEvent.Level, changeBgColor = !changeBgColor);

                this.ListBox.Items.Add(item);                
                this.ListBox.ScrollIntoView(item);
                if (ListBox.Items.Count > MaxLines)   this.ListBox.Items.RemoveAt(0);
            }
            //ListView
            if(this.ListView != null)
            {
                ListViewItem item = new ListViewItem();
                item.Height = 24;
                item.Content = loggingEvent;
                item.ToolTip = text.TrimEnd();
                item.Background = TextBoxBaseAppender.GetColorBrush(loggingEvent.Level, changeBgColor = !changeBgColor);

                this.ListView.Items.Add(item);                
                this.ListView.ScrollIntoView(item);
                if (this.ListView.Items.Count > MaxLines) this.ListView.Items.RemoveAt(0);
            }
        }

        /// <summary>
        /// 创建 GridViewColumn
        /// </summary>
        /// <param name="header"></param>
        /// <param name="path"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static GridViewColumn CreateColumn(string header, string path, int width = -1)
        {
            GridViewColumn column = new GridViewColumn()
            {
                Header = header,
                DisplayMemberBinding = new Binding(path),
            };
            if (width > 0) column.Width = width;

            return column;
        }
        /// <summary>
        /// 创建 GridViewColumn
        /// </summary>
        /// <param name="header"></param>
        /// <param name="paths"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static GridViewColumn CreateColumn(string header, string[] paths, string parameter)
        {
            MultiBinding Binds = new MultiBinding();
            Binds.ConverterParameter = parameter;
            Binds.Converter = new MultiStringConverter();
            for (int i = 0; i < paths.Length; i++)
                Binds.Bindings.Add(new Binding(paths[i]));

            GridViewColumn column = new GridViewColumn()
            {
                Header = header,
                DisplayMemberBinding = Binds,
            };

            return column;
        }
    }
}


