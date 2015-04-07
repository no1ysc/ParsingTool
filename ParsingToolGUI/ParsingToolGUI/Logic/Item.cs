using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingToolGUI.Logic
{
    /// <summary>
    /// 아래는 하나의 발언 (사람 - 발언) 을 표현하기 위한 자료구조임.
    /// </summary>
    class Item
    {
        public string 대;        // 몇대
        public string 회;
        public string 회의종류;     // 회의종류 (본회의, 특임위)
        public string 회의종류하위; // (본회의 등을 제외한 특임위 상임위 등은 하위항목이 있음., 평소에는 없는걸로!)
        public string 날짜;
        public string 차;            // 몇차
        public string 안건들;           // 지금은 아마 안쓸듯?

        //사람에 대한것.
        public string 발언자;
        public string 직책;
        //발언에대한것.
        public string 발언내용;
        public string 액션;
        public string 시간;

        public string 순서;
    }
}
