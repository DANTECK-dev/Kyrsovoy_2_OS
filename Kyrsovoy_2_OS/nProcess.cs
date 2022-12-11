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
        public int cpuBurst { get; set; }
        public int createTime { get; set; }
        public int queue { get; set; }
        public int priority { get; set; }
        public bool work { get; set; }
        public nProcess(int cpuBurst, int queue, int priority, int createTime)
        {
            Random rand = new Random();
            this.id = rand.Next(99999999);
            this.cpuBurst = cpuBurst;
            this.createTime = createTime;
            this.queue = queue;
            this.priority = priority;
            this.work = true;
        }
        public DataRow getDataRow(DataTable dataTable)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow[0] = this.id;
            dataRow[1] = this.cpuBurst;
            dataRow[2] = this.createTime;
            dataRow[3] = this.queue;
            dataRow[4] = this.priority;
            return dataRow;
        }
    }
}
