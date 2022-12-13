using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyrsovoy_2_OS
{
    public class nFile
    {
        public static int AllSize { get; private set; }
        public string name { get; set; }
        public int size { get; set; }

        public nFile(string name, int size)
        {
            this.name = name;
            this.size = size;
            AllSize += size;
        }

        public DataRow getDiskCatalogDataRow(DataTable dataTable)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow["Имя"] = this.name;
            dataRow["Размер"] = this.size;
            return dataRow;
        }
        public DataRow getDiskSectorDataRow(DataTable dataTable)
        {
            DataRow dataRow = dataTable.NewRow();
            dataRow["Имя"] = this.name;
            return dataRow;
        }
    }
}
