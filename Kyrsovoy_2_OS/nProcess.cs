using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Kyrsovoy_2_OS
{
    public class nProcess
    {
        public static int count { get; private set; } = 0;
        public string name { get; set; }
        public int id { get; set; }
        public int defCpuBurst { get; set; }
        public int cpuBurst { get; set; }
        public int createTime { get; set; }
        public int queue { get; set; }
        public bool work { get; set; }
        public int size { get; set; }
        public SolidColorBrush color { get; set; }
        public ProcessMap processMap { get; set; } = new ProcessMap();
        public nProcess(int cpuBurst, int queue, int createTime, int size)
        {
            this.name = (count++).ToString();
            Random rand = new Random();
            this.id = rand.Next(99999999);
            this.cpuBurst = cpuBurst;
            this.defCpuBurst = cpuBurst;
            this.createTime = createTime;
            this.queue = queue;
            this.work = true;
            this.color = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)));
            this.size = size;

        }
        public DataRow getProcessDataRow(DataTable dataTable)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow["PID"] = this.id;
            dataRow["Исполнения"] = this.cpuBurst;
            dataRow["Появления"] = this.createTime;
            dataRow["Очередь"] = this.queue;
            this.calcMap();
            return dataRow;
        }
        public DataRow getMemoryProcessDataRow(DataTable dataTable)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow["PID"] = this.id;
            dataRow["Color"] = this.color;
            return dataRow;
        }
        public List<DataRow>getMemoryMapProcessDataRow(DataTable dataTable)
        {
            List<DataRow> dataRow = new List<DataRow>();
            int temp_size = this.size;
            int index = -1;

            for(int i = 0; i < temp_size; i+=16)
            {
                if (i % 256 == 0)
                {
                    index++;
                    dataRow.Add(dataTable.NewRow());
                    temp_size -= i;
                    i = 0;
                }

                if(temp_size >= i)
                {
                    dataRow[index][i.ToString()] = "#####";
                }
            }
            return dataRow;
        }
        public void calcMap()
        {
            if (this.size % Vals.LEN_OF_PAGE != 0)
            {
                processMap.Count_Virt_Page = 1 + this.size / Vals.LEN_OF_PAGE;
                processMap.Free_Last_Page = Vals.LEN_OF_PAGE - (this.size % Vals.LEN_OF_PAGE);
            }
            else
            {
                processMap.Count_Virt_Page = this.size / Vals.LEN_OF_PAGE;
                processMap.Free_Last_Page = 0;
            }
            processMap.Count_Page = 1;

            if (processMap.Count_Virt_Page > Vals.q)
            {
                Vals.q = processMap.Count_Virt_Page;
                processMap.Count_Del_Page_in_RAM = 1;
                //q10 = q - processMap.Count_Del_Page_in_RAM;
            }

            if ((Vals.Total_Rem_Pages >= processMap.Count_Page) && (Vals.Total_Rem_Virt_Pages >= processMap.Count_Virt_Page))
            {
                Vals.Total_Rem_Pages -= processMap.Count_Page;
                Vals.Total_Rem_Virt_Pages -= processMap.Count_Virt_Page;
            }
            else
            {
                Vals.Total_Rem_Pages = Vals.Total_Rem_Pages + processMap.Count_Del_Page_in_RAM - processMap.Count_Page;
                Vals.Total_Rem_Virt_Pages = Vals.Total_Rem_Virt_Pages + (Vals.q - processMap.Count_Del_Page_in_RAM) - processMap.Count_Virt_Page;
            }
        }
    }
}
