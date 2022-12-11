using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrsovoy_2_OS
{
    internal class nProcess
    {
        public string state { get; set; }
        public int id { get; set; }
        public int defCpuBurst { get; set; }
        public int cpuBurst { get; set; }
        public int createTime { get; set; }
        public int queue { get; set; }
        public bool work { get; set; }
        public nProcess(int cpuBurst, int queue, int createTime)
        {
            Random rand = new Random();
            this.id = rand.Next(99999999);
            this.cpuBurst = cpuBurst;
            this.defCpuBurst = cpuBurst;
            this.createTime = createTime;
            this.queue = queue;
            this.work = true;
        }
        public DataRow getDataRow(DataTable dataTable)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow["PID"] = this.id;
            dataRow["Исполнения"] = this.cpuBurst;
            dataRow["Появления"] = this.createTime;
            dataRow["Очередь"] = this.queue;
            return dataRow;
        }
    }
}
