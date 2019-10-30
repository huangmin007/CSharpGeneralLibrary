using System;
using log4net.Core;
using log4net.Layout;
using log4net.Appender;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SpaceCG.Log4Net
{
    /// <summary>
    /// Log4Net WPF TextBoxBase Appender
    /// </summary>
    public class TextBoxBaseAppender : AppenderSkeleton
    {
        /// <summary>
        /// 获取或设置最大可见行数
        /// </summary>
        private uint MaxLines = 512;

        private TextBoxBase TextBox;
        private Action<String> AppendTextDelegate;

        /// <summary>
        /// Log4Net Appender for WPF TextBoxBase 
        /// </summary>
        /// <param name="textBox"></param>
        public TextBoxBaseAppender(TextBoxBase textBox)
        {
            this.TextBox = textBox;
            this.AppendTextDelegate = TextBoxAppendText;
            this.Layout = new PatternLayout("[%date{yyyy-MM-dd HH:mm:ss}] [%thread] %level %logger [%method(%line)] - %message (%r) %newline");

            //Set Controls Default Config
            if(this.TextBox is TextBox)
            {
                TextBox tb = (TextBox)this.TextBox;
                tb.IsReadOnly = true;
                tb.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
            }
            else if(this.TextBox is RichTextBox)
            {
                RichTextBox rtb = (RichTextBox)this.TextBox;
                rtb.IsReadOnly = true;
                rtb.AcceptsReturn = true;
                rtb.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                rtb.Document.LineHeight = 1;
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
            if (!this.TextBox.IsInitialized) return;
            if (!this.TextBox.IsLoaded) return;

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

            this.TextBox.Dispatcher.BeginInvoke(this.AppendTextDelegate, text);
        }

        /// <summary>
        /// TextBox AppendText
        /// </summary>
        /// <param name="text"></param>
        protected void TextBoxAppendText(String text)
        {
            this.TextBox.AppendText(text);
            this.TextBox.ScrollToEnd();
            
            if (this.TextBox is TextBox)
            {
                TextBox tb = (TextBox)this.TextBox;
                if (tb.LineCount > MaxLines)
                {
                    int count = tb.GetCharacterIndexFromLineIndex(1);
                    tb.Text = tb.Text.Remove(0, count);
                }
                //if (tb.LineCount > MaxLine) tb.Clear();
            }
            else if (this.TextBox is RichTextBox)
            {
                RichTextBox rtb = (RichTextBox)this.TextBox;
                
                if (rtb.Document.Blocks.Count > MaxLines)
                    rtb.Document.Blocks.Remove(rtb.Document.Blocks.FirstBlock);

                //if (rtb.Document.Blocks.Count > MaxLines) rtb.Document.Blocks.Clear();
            }
        }
    }
}
