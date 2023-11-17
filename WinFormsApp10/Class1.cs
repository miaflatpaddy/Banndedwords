using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp10
{
    internal class InstanceCheck
    {
        static Mutex ICM;
        static public bool IC()
        {
            bool isNew;
            ICM = new Mutex(true, "filechecker", out isNew);
            return isNew;
        }
    }
}
