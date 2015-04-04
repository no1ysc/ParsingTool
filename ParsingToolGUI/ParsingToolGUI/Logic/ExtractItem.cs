using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParsingToolGUI.Logic
{
    class ExtractItem
    {
        public static Item ParsingText(string text)
        {
            Item result = new Item();
                        
            string 발언자 = "";
            string 직책 = "";
            string 발언내용 = "";
            string 액션 = "";
            string 시간 = "";
            
            string procStr = text.Replace("◯", ""); // 특문 있으면 제거.
            string[] lines = procStr.Split(new string[]{"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);    // 라인 분할


            
            // 첫번째 라인에는 발언자직책, 발언자 가 들어옴. 공백으로 두개 구분.
            string[] 발언자직책사람 = lines[0].Split(' ');
            직책 = 발언자직책사람[0];
            발언자 = 발언자직책사람[1];

            // 두번째 라인 부터 내용이 들어있을 것.
            StringBuilder content = new StringBuilder();
            StringBuilder action = new StringBuilder();
            for (int i = 1; i < lines.Length; i++)
            {
                // 시간, 액션 은 한라인에 있음. 즉 시간액션 걸러낸다음 나머지는 그냥 발언내용으로 넣으면 됨.
                if (lines[i].StartsWith("("))
                {
                    string procWithRemoved = lines[i].Trim().Replace("(", "").Replace(")", "");

                    시간 = getTime(procWithRemoved);
                    procWithRemoved = procWithRemoved.Replace(시간, "").Trim();

                    // 액션이 여러개 나올 수도.
                    if (!액션.Equals(""))
                    {
                        action.Append(", " + procWithRemoved);
                    }
                    else
                    {
                        action.Append(procWithRemoved + "\r\n");
                    }
                    액션 = procWithRemoved; // 마킹용으로 사용했음.                    
                }
                else
                {
                    content.Append(lines[i]);
                }
            }

            액션 = action.ToString();
            발언내용 = content.ToString();

            result.발언자 = 발언자;
            result.직책 = 직책;
            result.발언내용 = 발언내용;
            result.액션 = 액션;
            result.시간 = 시간;

            return result;
        }

        private static string getTime(string procWithRemoved)
        {
            return Regex.Match(procWithRemoved, "[0-9][0-9]시[0-9][0-9]분").Value;
        }
        
    }
}
