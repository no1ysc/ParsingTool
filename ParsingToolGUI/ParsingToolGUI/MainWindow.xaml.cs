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
        private string targetPDFsDir = @"L:\Data\무상급식회의록타켓\";

        private string outputJSONFilePath = @"L:\Data\무상급식회의록타켓\JSON\";

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
        
        // 붙여넣기 클릭.
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string clipBoard = Clipboard.GetText();

            Item inputedItem = ExtractItem.ParsingText(clipBoard);

            item2TextBox(inputedItem);

            if (this.순서.Text.Trim().Equals("") || this.순서.Text.Trim().Equals("0"))
            {
                this.순서.Text = "1";
            }
            else
            {
                int countValue = Int32.Parse(this.순서.Text);
                this.순서.Text = countValue.ToString();
            }
        }

        // 리셋
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

        }

        private void item2TextBox(Item inputedItem)
        {
            this.직책.Text = inputedItem.직책;
            this.발언자.Text = inputedItem.발언자;
            this.발언내용.Text = inputedItem.발언내용;
            this.액션.Text = inputedItem.액션;
            this.시간.Text = inputedItem.시간;
        }

        private Item textBox2Item()
        {
            Item ret = new Item();
            ret.대 = this.대.Text;
            ret.회의종류 = this.회의종류.Text;
            ret.날짜 = this.날짜.Text;
            ret.회의종류하위 = this.종류하위.Text;
            ret.차 = this.차.Text;
            ret.안건들 = this.안건들.Text;

            ret.직책 = this.직책.Text; ;
            ret.발언자 = this.발언자.Text;;
            ret.발언내용 = this.발언내용.Text;
            ret.액션 = this.액션.Text;
            ret.시간 = this.시간.Text;
            ret.순서 = this.순서.Text;

            return ret;           
        }

    }
}
