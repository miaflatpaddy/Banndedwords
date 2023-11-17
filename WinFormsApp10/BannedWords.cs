using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WinFormsApp10
{
    public class BannedWords
    {
        public static string[] BannedList;
        public static string listAddres = "BannedList.txt";
        public static int strnum;
        public static void Read()
        {
            if (strnum > 0)
            {
                strnum= 0;
            }
            using (StreamReader sr = new StreamReader(listAddres))
            {

                while (sr.Peek()>(-1))
                {
                    Add(sr.ReadLine());
                }
            }
            
        }
        public static void Add(string newword)
        {
            strnum++;
            string[] NBL = new string[strnum];
            for(int i =0; i < NBL.Length; i++)
            {
                if (i < strnum - 1)
                {
                    NBL[i] = BannedList[i];
                }
                else
                {
                    NBL[i] = newword;
                }
            }
            BannedList = NBL;
        }
        public static void Save()
        {
            using(StreamWriter sw = new StreamWriter(listAddres))
            {
                foreach (var item in BannedList)
                {
                    sw.WriteLine(item);
                }
            }
        }
        public static void Remove(int i)
        {
            strnum--;
            string[] NBL = new string[strnum];
            for (int j = 0; j < NBL.Length; j++)
            {
                if (j < i)
                {
                    NBL[j] = BannedList[j];
                }
                else
                {
                    NBL[j] = BannedList[j + 1];
                }
            }
            BannedList= NBL;
        }
    }
}
