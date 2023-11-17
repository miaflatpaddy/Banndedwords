using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WinFormsApp10
{
    public class Report
    {
        ListBox InfoBox;
        ListBox TopBox;
        int infonum = 0;
        string[] Address;
        long[] Size;
        int[] RepNum;
        string[] Top = new string[10];
        string[] FileName;
        public int?[] popularity = new int?[BannedWords.strnum];
        string RepFile = "Report.txt";
        public void Init()
        {
            for(int i = 0; i < popularity.Length; i++)
            {
                popularity[i] = 0;
            }
        }
        public void Save()
        {
            using(StreamWriter sw = new StreamWriter(RepFile, true))
            {
                sw.WriteLine("===================" + DateTime.Now.ToString() + "===================");
                for(int i = 0; i < infonum; i++)
                {
                    sw.WriteLine($"{FileName[i]}, {Address[i]}, {Size[i].ToString()}, {RepNum[i].ToString()}") ;
                }
                sw.WriteLine("Most popular words:");
                for (int i = 0; i < 10; i++)
                { 
                    if(i < 9)
                    {

                        sw.WriteLine($"{(i+1).ToString()}. {Top[i]},");
                    }
                    else
                    {
                        sw.WriteLine($"{(i+1).ToString()}. {Top[i]}.");
                    }
                }
                sw.WriteLine("=========================================================");
            }
        }
        public void AddInfo(string addr, long size, int repnum, string FN)
        {
            infonum++;
            string[] addre = new string[infonum];
            long []si = new long[infonum];
            int[]rep = new int[infonum];
            string[] fn = new string[infonum];
            for(int i = 0; i < infonum; i++)
            {
                if (i < (infonum - 1))
                {
                    addre[i] = Address[i];
                    si[i] = Size[i];
                    rep[i] = RepNum[i];
                    fn[i] = FileName[i];
                }
                else
                {
                    addre[i] = addr;
                    si[i] = size;
                    rep[i] = repnum;
                    fn[i] = FN;
                }
            }
            Address = addre;
            Size = si;
            RepNum = rep;
            FileName = fn;
            TopCreate();

        }
        public void ShowInfo()
        {
            if (infonum > 0)
            {
                TopBox.Invoke(new Action(UpdateTop));
                InfoBox.Invoke(new Action(UpdateInfo));

            }
        }
        void UpdateTop()
        {
            TopBox.Items.Clear();
            for(int i = 0; i < Top.Length; i++)
            {
                TopBox.Items.Add(Top[i]);
            }
        }
        void UpdateInfo()
        {
            InfoBox.Items.Clear();
            for(int i = 0; i < Size.Length; i++)
            {
                InfoBox.Items.Add($"{FileName[i]}, {Address[i]}, {Size[i]/1024} KB, {RepNum[i]}");
            }
        }
        void TopCreate()
        {
            string[] buffer = BannedWords.BannedList;
            int? temp = 0;
            string temps;
            for(int i = 0; i < buffer.Length - 1; i++)
            {
                for(int j = i+1; j < buffer.Length; j++)
                {
                    if (popularity[i] < popularity[j])
                    {
                        temp = popularity[i];
                        temps = buffer[i];
                        popularity[i] = popularity[j];
                        buffer[i] = buffer[j];
                        popularity[j] = temp;
                        buffer[j] = temps;
                    }
                }
            }
            for(int i = 0; i <10; i++)
            {
                Top[i] = buffer[i];
            }
        }
        public void Start(ListBox InfoBox_, ListBox TopBox_)
        {
            InfoBox = InfoBox_;
            TopBox = TopBox_;
        }
    }
}
