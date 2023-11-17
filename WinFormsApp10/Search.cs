using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WinFormsApp10
{
    internal class Search
    {
        public static CancellationTokenSource cTS = new CancellationTokenSource();
        public CancellationToken token = cTS.Token;
        ProgressBar pB;
        Stopper stp;
        public int progress;
        string[] types = { ".txt", ".text", ".ini" };
        int lenght = 0;
        string[] Adresses;
        string[] FileNames;
        public void SearchF(ProgressBar progressBar, Stopper stop, string drive = "")
        {
            stp = stop;
            DriveInfo[] Di;
            string[] drivenames;
            pB = progressBar;
            if (drive == "")
            {
                Di = DriveInfo.GetDrives();
                int l = 0;
                for(int i = 0; i < Di.Length; i++)
                {
                    if (Di[i].IsReady)
                    {
                        l++;
                    }
                }
                drivenames = new string[l];
                l = 0;
                foreach (var item in Di)
                {
                    if (item.IsReady)
                    {
                        drivenames[l] = item.Name;
                        l++;
                    }
                }
                foreach(var item in drivenames)
                {
                    OpenDir(new DirectoryInfo(item));
                    if (stp.Pause)
                    {
                        stp.ew.WaitOne();
                    }
                }
            }

            else
            {
                OpenDir(new DirectoryInfo(drive));
            }
            if (stp.Pause)
            {
                stp.ew.WaitOne();
            }

            if (token.IsCancellationRequested)
            {
                MessageBox.Show("Выполнение прервано.");
                return;
            }

            CheckF();
        }
        void AddFile(FileInfo fi)
        {
            if (stp.Pause)
            {
                stp.ew.WaitOne();
            }
            lenght++;
            string[] newaddr = new string[lenght];
            string[] newFN = new string[lenght];
            for(int i = 0; i < lenght; i++)
            {
                if(i < lenght - 1)
                {
                    newaddr[i] = Adresses[i];
                    newFN[i] = FileNames[i];
                }
                else
                {
                    newaddr[i] = fi.FullName;
                    newFN[i] = fi.Name;
                }
            }
            Adresses = newaddr;
            FileNames = newFN;
        }
        void OpenDir(DirectoryInfo di)
        {
            
            try
            {


                FileInfo[] files = di.GetFiles();
                foreach (var item in files)
                {
                    if (stp.Pause)
                    {
                        stp.ew.WaitOne();
                    }
                    if (token.IsCancellationRequested)
                    {
                        
                        return;
                    }
                    for (int i = 0; i < types.Length; i++)
                    {
                        if (item.Name.Contains(types[i]))
                        {
                            AddFile(item);
                        }

                    }

                }
                DirectoryInfo[] dir = di.GetDirectories();
                foreach (var item in dir)
                {
                    if (token.IsCancellationRequested)
                    {
                        
                        return;
                    }
                    if (stp.Pause)
                    {
                        stp.ew.WaitOne();
                    }
                    OpenDir(item);
                }
            }
            catch (Exception ex)
            {
               
            }
            if (token.IsCancellationRequested)
            {
                return;
            }
        }
        void CheckF()
        {
            if (stp.Pause)
            {
                stp.ew.WaitOne();
            }
            if (token.IsCancellationRequested)
            {
                MessageBox.Show("Выполнение прервано.");
                return;
            }
            pB.Invoke(new Action(() => pB.Maximum = FileNames.Length));
            string buff;
            int i = 0;
            foreach (var item in Adresses)
            {
                foreach (var item1 in BannedWords.BannedList)
                {
                    try
                    {

                        using (StreamReader sr = new StreamReader(item))
                        {
                            buff = sr.ReadToEnd();
                        }
                        if (buff.Contains(item1))
                        {
                            CopyFix.Start(item, FileNames[i]);
                        }
                    }
                    catch(Exception ex)
                    {

                    }


                }
                UpdateProgressBar(1);
                i++;
                if (stp.Pause)
                {
                    stp.ew.WaitOne();
                }
                if (token.IsCancellationRequested)
                {
                    MessageBox.Show("Выполнение прервано.");
                    return;
                }

            }
        }
        private void UpdateProgressBar(int p)
        {
            //if (pB.InvokeRequired)
            //{
            //    pB.Invoke(new Action<int>(UpdateProgressBar),p);
            //}
            //else
            //{
            //    pB.Value += p;
            //}
            progress += p;
        }
    }
}
