using ParsingToolGUI.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParsingToolGUI
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private string originPDFsDir = @"L:\Data\무상급식회의록\";
        private string targetPDFsDir = @"L:\Data\무상급식회의록타켓";

        private string outputJSONFilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private delegate void DelProgressBarValue(int value);
        private void progressBarValue(int value) { this.progressBar.Value = value; }
        public int ProgressBarValue 
        {
            set
            {
                this.Dispatcher.Invoke(new DelProgressBarValue(progressBarValue), new object[] { value });
            }
        }

        private delegate void DelProgressBarVisibility(System.Windows.Visibility value);
        private void progressBarVisibility(System.Windows.Visibility value) { this.progressBar.Visibility = value; }
        public System.Windows.Visibility ProgressBarVisablity
        {
            set
            {
                this.Dispatcher.Invoke(new DelProgressBarVisibility(progressBarVisibility), new object[] { value });
            }
        }
        
        // 넣기 버튼 클릭
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ;
        }

        // JSON 보기 클릭
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ;
        }

        // 대상 문서 선정 클릭.
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            FindTargetDoc findDoc = new FindTargetDoc(originPDFsDir, targetPDFsDir);
            findDoc.UsingProgressBar(this);
            findDoc.RunWorkerAsync();
        }
    }
}
