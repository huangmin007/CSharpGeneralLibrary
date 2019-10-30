using System;
using System.Collections.ObjectModel;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;
using System.Windows;

namespace SpeechSynthesizer_Example
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool IsStart = false;

        SpeechRecognitionEngine recognizer;
        //SpeechRecognizer recognizer;

        public MainWindow()
        {
            InitializeComponent();

            //recognizer = new SpeechRecognizer();

            ReadOnlyCollection<RecognizerInfo> infos = SpeechRecognitionEngine.InstalledRecognizers();
            foreach(RecognizerInfo info in infos)
            {
                recognizer = new SpeechRecognitionEngine(info);
                textBox_Log.AppendText(String.Format("ID:{0} Name:{1} Culture:{2} Description:{3}\n", info.Id, info.Name, info.Culture, info.Description));
            }

            //Choices c0 = new Choices(new string[] { "测试", "赖成", "开始", "王汉民" });
            //Choices c1 = new Choices(new string[] { "像", "是", "不是", "不像" });
            //Choices c2 = new Choices(new string[] { "飞机", "二货", "电脑", "汽车" });
            
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(new Choices("测试", "赖成", "开始", "王汉民"));
            gb.Append(new Choices("像", "是", "不是", "不像"));
            gb.Append(new Choices("飞机", "二货", "电脑", "汽车"));
            Grammar gr = new Grammar(gb);
            gr.Name = "test";
            //gr.Weight = 0.8f;
            //recognizer.LoadGrammar(gr);
            //Console.WriteLine(gr.Weight);

            SrgsDocument doc = new SrgsDocument("gaarmmar.xml");
            recognizer.LoadGrammar(new Grammar(doc));

            //recognizer.LoadGrammar(CreateColorGrammar());
            recognizer.LoadGrammar(new DictationGrammar());

            recognizer.SpeechRecognized += Recognizer_SpeechRecognized;
            recognizer.SpeechHypothesized += Recognizer_SpeechHypothesized;

            // 配置以接收来自默认音频设备的输入.  
            recognizer.SetInputToDefaultAudioDevice();

            //执行一个或多个异步语音识别操作  
            //recognizer.RecognizeAsync(RecognizeMode.Multiple);

        }
        private Grammar CreateColorGrammar()
        {

            // Create a Choices object that contains a set of alternative colors.  
            Choices colorChoice = new Choices(new string[] { "红色", "绿色", "蓝色", "白色" });
            colorChoice.Add(new string[] { "黑色", "黄色" });

            // Construct the phrase.  
            GrammarBuilder builder = new GrammarBuilder("背景");
            builder.Append(colorChoice);

            // Create a grammar for the phrase.  
            Grammar colorGrammar = new Grammar(builder);
            colorGrammar.Name = "SetBackground";

            return colorGrammar;
        }

        private void Recognizer_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            textBox_Log.AppendText(String.Format("SpeechHypothesized Text:{0}  Confidence:{1}\n", e.Result.Text, e.Result.Confidence));
            textBox_Log.ScrollToEnd();

            Console.WriteLine("SH::{0}", e.Result.Grammar.Name);
        }

        private void Recognizer_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            //Console.WriteLine(e.AudioPosition);
        }

        private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine(e.Result.Grammar.Name);
            textBox_Log.AppendText(String.Format("SpeechRecognized>>> Text:{0}  Confidence:{1}\n", e.Result.Text, e.Result.Confidence));
            textBox_Log.ScrollToEnd();

            if(e.Result.Grammar.Name == "SetBackground")
            {
                Console.WriteLine(e.Result.Text);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(button.Content.ToString());
            if(button.Content.ToString() == "Start")
            {
                IsStart = true;
                button.Content = "Stop";
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
            else if(button.Content.ToString() == "Stop")
            {
                IsStart = false;
                button.Content = "Start";
                recognizer.RecognizeAsyncStop();
            }
            
        }
    }
}
