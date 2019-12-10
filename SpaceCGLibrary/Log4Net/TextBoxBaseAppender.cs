using System;
using log4net.Core;
using log4net.Layout;
using log4net.Appender;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace SpaceCG.Log4Net
{
    /// <summary>
    /// Log4Net WPF TextBoxBase Appender
    /// </summary>
    public class TextBoxBaseAppender : AppenderSkeleton
    {
        /// <summary> Info Color </summary>
        protected static readonly SolidColorBrush InfoColor = new SolidColorBrush(Color.FromArgb(0x7F, 0xFF, 0xFF, 0xFF));
        /// <summary> Warn Color </summary>
        protected static readonly SolidColorBrush WarnColor = new SolidColorBrush(Color.FromArgb(0x7F, 0xFF, 0xFF, 0x00));
        /// <summary> Error Color </summary>
        protected static readonly SolidColorBrush ErrorColor = new SolidColorBrush(Color.FromArgb(0x7F, 0xFF, 0x00, 0x00));
        /// <summary> Fatal Color </summary>
        protected static readonly SolidColorBrush FatalColor = new SolidColorBrush(Color.FromArgb(0xBF, 0xFF, 0x00, 0x00));

        /// <summary>
        /// 获取或设置最大可见行数
        /// </summary>
        protected uint MaxLines = 512;
        /// <summary> TextBoxBase </summary>
        protected TextBoxBase TextBox;
        /// <summary> TextBox.AppendText Delegate Function </summary>
        protected Action<String, Level> AppendTextDelegate;

        private TextBox tb;
        private RichTextBox rtb;

        /// <summary>
        /// Log4Net Appender for WPF TextBoxBase 
        /// </summary>
        /// <param name="textBox"></param>
        public TextBoxBaseAppender(TextBoxBase textBox)
        {
            this.TextBox = textBox;
            this.AppendTextDelegate = TextBoxAppendText;
            this.Layout = new PatternLayout("[%date{yyyy-MM-dd HH:mm:ss}] [%thread] [%level] [%method(%line)] %logger - %message (%r) %newline");

            //Set Controls Default Config
            if (this.TextBox is TextBox)
            {
                tb = (TextBox)this.TextBox;
                tb.IsReadOnly = true;
                tb.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
            }
            else if(this.TextBox is RichTextBox)
            {
                rtb = (RichTextBox)this.TextBox;
                rtb.IsReadOnly = true;
                rtb.AcceptsReturn = true;
                rtb.Document.LineHeight = 2;
                rtb.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            else
            {
                //...                
            }

            log4net.Config.BasicConfigurator.Configure(this);
        }

        /// <summary>
        /// Log4Net Appender for WPF TextBoxBase 
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="maxLines">最大行数为 1024 行，默认为 512 行</param>
        public TextBoxBaseAppender(TextBoxBase textBox, uint maxLines):this(textBox)
        {
            this.MaxLines = maxLines > 1024 ? 1024 : maxLines;
        }

        /// <summary>
        /// @override
        /// </summary>
        /// <param name="loggingEvent"></param>
        protected override void Append(LoggingEvent loggingEvent)
        {
            if (this.TextBox == null) return;
            //if (!this.TextBox.IsLoaded) return; //在其它线程中会产生错误

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

            this.TextBox.Dispatcher.BeginInvoke(this.AppendTextDelegate, text, loggingEvent.Level);
        }

        /// <summary>
        /// TextBox AppendText
        /// </summary>
        /// <param name="text"></param>
        /// <param name="level"></param>
        protected void TextBoxAppendText(String text, Level level)
        {
            if (tb != null && tb.IsLoaded)
            {
                tb.AppendText(text);
                tb.ScrollToEnd();

                if (tb.LineCount > MaxLines)
                    tb.Text = tb.Text.Remove(0, tb.GetCharacterIndexFromLineIndex(1));

                return;
            }

            if (rtb != null && rtb.IsLoaded)
            {
                Paragraph paragraph = new Paragraph(new Run(text.Trim()));
                paragraph.Background = level == Level.Fatal ? FatalColor : level == Level.Error ? ErrorColor : level == Level.Warn ? WarnColor : InfoColor;

                rtb.Document.Blocks.Add(paragraph);
                rtb.ScrollToEnd();

                if (rtb.Document.Blocks.Count > MaxLines)
                    rtb.Document.Blocks.Remove(rtb.Document.Blocks.FirstBlock);

                return;
            }

        }
        
    }
}
