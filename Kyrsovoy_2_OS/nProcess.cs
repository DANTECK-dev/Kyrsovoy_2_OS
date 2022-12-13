using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kyrsovoy_2_OS
{
    internal class nProcess
    {
        public static int count { get; private set; } = 0;
        public string name { get; set; }
        public int id { get; set; }
        public int defCpuBurst { get; set; }
        public int cpuBurst { get; set; }
        public int createTime { get; set; }
        public int queue { get; set; }
        public bool work { get; set; }
        public SolidColorBrush color {get;set;}
        public nProcess(int cpuBurst, int queue, int createTime)
        {
            this.name = "Proc_" + count++;
            Random rand = new Random();
            this.id = rand.Next(99999999);
            this.cpuBurst = cpuBurst;
            this.defCpuBurst = cpuBurst;
            this.createTime = createTime;
            this.queue = queue;
            this.work = true;
            this.color = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)));
        }
        public DataRow getProcessDataRow(DataTable dataTable)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow["PID"] = this.id;
            dataRow["Исполнения"] = this.cpuBurst;
            dataRow["Появления"] = this.createTime;
            dataRow["Очередь"] = this.queue;
            return dataRow;
        }
        public DataRow getMemoryProcessDataRow(DataTable dataTable)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow["PID"] = this.id;
            dataRow["Color"] = this.color;
            return dataRow;
        }
    }
}
