using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrsovoy_2_OS
{
    public static class Vals
    {
        public static long LEN_OF_PAGE { get; private set; } = 1024;
        public static long LEN_OF_CLUSTER { get; private set; } = 512;
        public static long RAM { get; private set; } = int.MaxValue;
        public static long VAM { get; private set; } = int.MaxValue;
        public static long Total_Pages { get; set; }
        public static long Total_Virt_Pages { get; set; }
        public static long Total_Rem_Pages { get; set; }
        public static long Total_Rem_Virt_Pages { get; set; }
        public static long q { get; set; }
    }
}
