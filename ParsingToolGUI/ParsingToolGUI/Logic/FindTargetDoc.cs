using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ParsingToolGUI.Logic
{
    class FindTargetDoc : BackgroundWorker
    {
        private class ResultSet
        {
            private string filePath, fileNamePDF;

            public ResultSet(string filePath, string fileNamePDF)
            {
                this.fileNamePDF = fileNamePDF;
                this.filePath = filePath;
            }

            public string FilePath { get { return filePath; } }
            public string FileName { get { return fileNamePDF; } }

            public override string ToString()
            {
                return filePath + "\t" + fileNamePDF;
            }
        }

        private Queue<ResultSet> fileList = new Queue<ResultSet>();

        // check words
        private string[] checkWord = {
        "무상급식",
        "무상 급식",
        "무\n상급식",
        "무상\n급식",
        "무상급\n식",
        "무\n상 급식",
        "무상\n 급식",
        "무상 \n급식",
        "무상 급\n식",
        "무\r\n상급식",
        "무상\r\n급식",
        "무상급\r\n식",
        "무\r\n상 급식",
        "무상\r\n 급식",
        "무상 \r\n급식",
        "무상 급\r\n식",
        };




        private string rootPath;
        private string resultPath;
        private StreamWriter log;
        private StreamWriter wordCountDate;

        private MainWindow progressBarInWindow = null;

        public FindTargetDoc(string rootPath, string resultPath)
        {
            this.rootPath = rootPath;
            this.resultPath = resultPath;

            this.log = File.CreateText(Environment.CurrentDirectory + "\\Log.txt");
            this.wordCountDate = File.CreateText(Environment.CurrentDirectory + "\\WordCountDate.csv");

            this.DoWork += new DoWorkEventHandler(work);
            this.RunWorkerCompleted += new RunWorkerCompletedEventHandler(completeWork);
        }

        public void Run()
        {
            // 폴더들 탐색하여 대상들 가져옴.
            searchChildDirs(rootPath);

            int totalTargetDocCount = fileList.Count;
            int workingCount = 0;

            log.WriteLine(fileList.Count + "개 파일 시작.");

            // 대상들 처리
            while(fileList.Count > 0)
            {
                workingCount++;

                ResultSet item = fileList.Dequeue();

                //procContainWord(workingCount, item);

                procContainWordCount(workingCount, item);
                
                int percent = (int)((double)workingCount / (double)totalTargetDocCount) * 100;
                this.ReportProgress(percent);
            }
        }

        private void procContainWord(int workingCount, ResultSet item)
        {
            // PDF로부터 원문 가져옴.
            // Getting rawText from PDF file.
            string rawText = getRawText(item.FilePath + item.FileName);

            // Checking a target word
            if (containWordsCheck(rawText))
            {
                // Copy to des.
                copyFileToResultDir(item);
                log.WriteLine(workingCount + "번째 파일 복사.");
                log.Flush();
            }
            else
            {
                log.WriteLine(workingCount + "번째 파일 패스.");
                log.Flush();
            }
        }

        private void procContainWordCount(int workingCount, ResultSet item)
        {
            // PDF로부터 원문 가져옴.
            // Getting rawText from PDF file.
            string rawText = getRawText(item.FilePath + item.FileName);
            
            // Checking a target word
            int containsWordCount = getContainsWordsCount(rawText);
            if (containsWordCount > 0)
            {
                string date = getDateFromDoc(item.FilePath + item.FileName);
                wordCountDate.WriteLine(date + "\t" + containsWordCount.ToString() + "\t" + item.FilePath + item.FileName);
                wordCountDate.Flush();

                // Copy to des.
                copyFileToResultDir(item);
                log.WriteLine(workingCount + "번째 파일 복사.");
                log.Flush();
            }
            else
            {
                log.WriteLine(workingCount + "번째 파일 패스.");
                log.Flush();
            }
        }

        private string getDateFromDoc(string pdfFilePath)
        {
            PDDocument pdfFile = PDDocument.load(pdfFilePath);
            PDFTextStripper stripper = new PDFTextStripper();

            stripper.setStartPage(1);
            stripper.setEndPage(1);

            string firstPageText = stripper.getText(pdfFile);

            Regex r = new Regex(@"(19|20)\d{2}(년|年)([1-9]|1[012])(월|月)([1-9]|[12][0-9]|3[0-1])(일|日)");
            //Regex r = new Regex(@"(19|20)\d{2}-(0[1-9]|1[012])-(0[1-9]|[12][0-9]|3[0-1])");
            Match m = r.Match(firstPageText);

            if (m.Success)
            {
                return m.Value;
            }

            return "";
        }

        private int getContainsWordsCount(string rawText)
        {
            int count = 0;

            foreach (string word in checkWord)
            {
                Regex r = new Regex(word);
                MatchCollection mc = r.Matches(rawText);
                count += mc.Count;
            }

            return count;
        }

        /// <summary>
        /// 프로그래스바 사용할껀지.
        /// </summary>
        public void UsingProgressBar(MainWindow progressBarInWindow)
        {
            this.progressBarInWindow = progressBarInWindow;
            this.ProgressChanged += new ProgressChangedEventHandler(progressBarHandler);
            this.WorkerReportsProgress = true;
        }

        private void work(object sender, DoWorkEventArgs e)
        {
            if (this.WorkerReportsProgress)
            {
                progressBarInWindow.ProgressBarVisablity = System.Windows.Visibility.Visible;
                progressBarInWindow.ProgressBarValue = 0;
            }

            Run();
        }

        private void completeWork(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.WorkerReportsProgress)
            {
                progressBarInWindow.ProgressBarVisablity = System.Windows.Visibility.Hidden;
            }
        }

        private void progressBarHandler(object sender, ProgressChangedEventArgs e)
        {
            progressBarInWindow.ProgressBarValue = e.ProgressPercentage;
        }


        private string getRawText(string pdfFilePath)
        {
            PDDocument pdfFile = PDDocument.load(pdfFilePath);
            PDFTextStripper stripper = new PDFTextStripper();

            //int pageCount = pdfFile.getNumberOfPages();
            //StringBuilder rawText = new StringBuilder();

            //for (int index = 0; index < pageCount; index++)
            //{
            //    stripper.setStartPage(index);
            //    stripper.setEndPage(index);

            //    rawText.Append(stripper.getText(pdfFile));
            //}

            return stripper.getText(pdfFile);
        }

        private void copyFileToResultDir(ResultSet item)
        {
            File.Copy(item.FilePath + item.FileName, resultPath + item.FileName);
        }

        private bool containWordsCheck(string text)
        {
            foreach (string word in checkWord)
            {
                if (text.Contains(word))
                {
                    return true;
                }
            }
            
            return false;
        }


        private void searchChildDirs(string startDir)
        {
            // 루트부터 전부 뒤짐.
            // PDF 파일이름은 각 폴더에 한개씩만 물면됨.
            // Text 는 이름만 만들어도 상관없음(경로의 합)

            DirectoryInfo dirInfo = new DirectoryInfo(startDir);
            bool isSub = dirInfo.EnumerateDirectories().Count() != 0 ? true : false;
            if (!isSub)
            {
                // leaf 임.
                string pdf = "", dir;
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    if (file.Extension.Contains("pdf"))
                    {
                        pdf = file.Name;
                        dir = dirInfo.FullName + "\\";
                        fileList.Enqueue(new ResultSet(dir, pdf));
                    }
                }

                return;
            }

            // 하위 폴더 리커시브 탐색
            foreach (DirectoryInfo subDir in dirInfo.EnumerateDirectories())
            {
                searchChildDirs(subDir.FullName);
            }
        }


    }
}
