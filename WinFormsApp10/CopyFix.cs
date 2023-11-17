using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WinFormsApp10
{
    public class CopyFix
    {
        static Report Repr;
        static string DestinationAdressO { get; set; }
        static string DestinationAdressA { get; set; }
        static string Rep = "*******";
        static string buffer;
        public static void DestinationSet(string Addres)
        {
            DestinationAdressO = Addres + "\\Original";
            DestinationAdressA = Addres + "\\Ajusted";
            Directory.CreateDirectory(DestinationAdressO);
            Directory.CreateDirectory(DestinationAdressA);
        }
        public static void Start(string addr, string fileName)
        {
            Copy(addr, fileName);
            AjustedCopy(addr, fileName);
        }
        static void Copy(string addr, string fileName)
        {
            string buffer1;
            using (StreamReader sr = new StreamReader(addr))
            {
                buffer1 = sr.ReadToEnd();
            }

            using (StreamWriter sw = new StreamWriter(DestinationAdressO + "\\" + fileName))
            {
                sw.WriteLine(buffer1);
            }
        }
        static void AjustedCopy(string addr, string fileName)
        {
            FileInfo fi = new FileInfo(addr);
            using (StreamReader sr = new StreamReader(addr))
            {
                buffer = sr.ReadToEnd();
            }
            string buffer1 = buffer;
            int replacenum = 0;
            int tmp;
            for(int i = 0; i < BannedWords.strnum; i++)
            {
                if (Repr.popularity[0] == null)
                {
                    Repr.Init();
                }
                tmp = (buffer.Length - buffer1.Replace(BannedWords.BannedList[i], "").Length) / BannedWords.BannedList[i].Length;
                Repr.popularity[i] += tmp;
                replacenum += tmp;
                buffer1 = buffer;
            }
            Repr.AddInfo(fi.FullName, fi.Length, replacenum, fi.Name);
            foreach (var item in BannedWords.BannedList)
            {
                buffer  = buffer.Replace(item, Rep);
            }
            using(StreamWriter sw = new StreamWriter(DestinationAdressA + "\\" + fileName))
            {
                sw.WriteLine(buffer);
            }
            
        }
        public static void SetReport(Report rep)
        {
            Repr = rep;
        }
    }
}
