using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrsovoy_2_OS
{
    public class ProcessMap
    {
        public long Count_Del_Page_in_RAM { get; set; }
        public long Page_in_RAM { get; set; }
        public long Page_in_Virt { get; set; }
        public long Count_Page { get; set; }
        public long Count_Virt_Page { get; set; }
        public long Free_Last_Page { get; set; }
    }
}
