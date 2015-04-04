using ParsingToolGUI.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ForTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Item item1 = new Item();
            item1.대              = "1";
            item1.회의종류        = "1";
            item1.회의종류하위    = "1";
            item1.날짜            = "1";
            item1.차              = "1";            
            item1.안건들          = "1";
            item1.발언자          = "1";
            item1.직책            = "1";
            item1.발언내용        = "1";
            item1.액션            = "1";
            item1.시간            = "1";
            item1.순서            = "1";


            Item item2 = new Item();
            item2.대              = "2";
            item2.회의종류        = "2";
            item2.회의종류하위    = "2";
            item2.날짜            = "2";
            item2.차              = "2";            
            item2.안건들          = "2";
            item2.발언자          = "2";
            item2.직책            = "2";
            item2.발언내용        = "2";
            item2.액션            = "2";
            item2.시간            = "2";
            item2.순서            = "2";

            List<Item> list = new List<Item>();
            list.Add(item1);
            list.Add(item2);

            string test1 = Item2Json(item1);
            string listTest1 = ItemList2Json(list);

            Console.Out.WriteLine(test1);
            Console.Out.WriteLine();
            Console.Out.WriteLine(listTest1);
        }

        private static string Item2Json(Item asfd)
        {
            return new JsonConverter<Item>().Object2Json(asfd);
        }

        private static string ItemList2Json(List<Item> asfd)
        {
            return new JsonConverter<List<Item>>().Object2Json(asfd);
        }
    }
}
