using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime;
using System.Data;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Threading;

namespace Kyrsovoy_2_OS
{
    /// <summary>
    /// Вариант 532
    /// 
    /// 5 - Многоуровневая очередь
    /// 3 - Страничное распределение памяти (страница – 1024 байт)
    /// 2 - Непрерывное размещение файлов (Кластер – 512 байт)
    /// 
    /// Автор умер от недосыпа
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {



        DataTable ProcessDataTable = new DataTable();           //Process_DataGrid центр 1
        DataTable MemoryProcessDataTable = new DataTable();     //Memory_Process_DataGrid слева 2
        DataTable MemoryMapProcessDataTable = new DataTable();  //Memory_Process_DataGrid верх 2
        DataTable MemoryAdreessDataTable = new DataTable();     //Memory_Process_DataGrid низ 2
        DataTable DiskCatalogDataTable = new DataTable();       //Disk_Catalog_DataGrid слева 3
        DataTable DiskMapDataTable = new DataTable();           //Disk_Map_DataGrid верх 3
        DataTable DiskSectorDataTable = new DataTable();        //Disk_Sector_DataGrid слева 3
        //DataTable ProcessDataTable = new DataTable();
        //DataTable ProcessDataTable = new DataTable();

        List<nProcess> processList = new List<nProcess>();

        List<nFile> fileList = new List<nFile>();

        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            ComboBox_Priority.Items.Add("низкий приоритет – FCFS");
            ComboBox_Priority.Items.Add("высокий приоритет – Round robin");

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;

            Vals.Total_Rem_Pages = Vals.RAM / Vals.LEN_OF_PAGE;
            Vals.Total_Rem_Virt_Pages = Vals.VAM / Vals.LEN_OF_PAGE;
            Vals.Total_Pages = Vals.Total_Rem_Pages; Vals.Total_Virt_Pages = Vals.Total_Rem_Virt_Pages;

            Refresh_AllGrid();
        }


        List<List<nProcess>> lhProcesses = new List<List<nProcess?>?>();


        int tact = 0;
        bool done_0 = false;
        bool done_1 = false;
        double Exec = 0;
        double Waiting = 0;

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TextBox_Quantum_Time.IsEnabled = false;
            if (done_0 && done_1)
            {
                timer.Stop();
                TextBox_Quantum_Time.IsEnabled = true;
                return;
            }

            if (tact % 3 == 0)
            {
                Console.WriteLine("123");
            }

            bool worked = false;

            ProcessDataTable.Columns.Add(new DataColumn(tact.ToString(), typeof(string)) { AllowDBNull = true, DefaultValue = "", Unique = false });
            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = tact.ToString();
            textColumn.Binding = new Binding(tact.ToString());
            Process_DataGrid.Columns.Add(textColumn);

            int tik = int.Parse(TextBox_Quantum_Time.Text);
            int kit = tact % tik;
            if (tact % int.Parse(TextBox_Quantum_Time.Text) == 0)
            {
                for (int i = 0; i < lhProcesses[1].Count - 2; i++)
                {
                    if (i == 0)
                    {
                        nProcess temp = lhProcesses[1][i];
                        lhProcesses[1][i] = lhProcesses[1][lhProcesses[1].Count - 1];
                        lhProcesses[1][lhProcesses[1].Count - 1] = temp;
                    }
                    else
                    {
                        (lhProcesses[1][i + 1], lhProcesses[1][i]) = (lhProcesses[1][i], lhProcesses[1][i + 1]);
                    }
                }

            }

            if (!done_1 && lhProcesses[1] != null)
                for (int j = 0; j < lhProcesses[1].Count; j++)  // high
                {

                    if (lhProcesses[1][j].createTime > tact)
                        continue;
                    if (lhProcesses[1][j].cpuBurst <= 0)
                    {
                        lhProcesses[1][j].work = false;
                        continue;
                    }
                    if (worked)
                    {
                        for (int k = 0; k < ProcessDataTable.Rows.Count; k++)
                            if (ProcessDataTable.Rows[k][0].ToString() == lhProcesses[1][j].id.ToString())
                            {
                                ProcessDataTable.Rows[k][tact.ToString()] = "Г";
                                Waiting++;
                            }
                        continue;
                    }

                    for (int k = 0; k < ProcessDataTable.Rows.Count; k++)
                        if (ProcessDataTable.Rows[k][0].ToString() == lhProcesses[1][j].id.ToString())
                        {

                            int burst = lhProcesses[1][j].cpuBurst;
                            //for (int i = 0; i < int.Parse(TextBox_Quantum_Time.Text) && i < burst; i++)
                            //{
                            ProcessDataTable.Rows[k][tact.ToString()] = "И";
                            lhProcesses[1][j].cpuBurst--;
                            Exec++;
                            worked = true;
                            //}
                            break;
                        }
                }

            if (lhProcesses[0] != null)
                for (int j = 0; j < lhProcesses[0].Count; j++)  // low
                {
                    if (lhProcesses[0][j].createTime > tact)
                        continue;
                    if (lhProcesses[0][j].cpuBurst <= 0)
                    {
                        lhProcesses[0][j].work = false;
                        continue;
                    }
                    if (worked)
                    {
                        for (int k = 0; k < ProcessDataTable.Rows.Count; k++)
                            if (ProcessDataTable.Rows[k][0].ToString() == lhProcesses[0][j].id.ToString())
                            {
                                ProcessDataTable.Rows[k][tact.ToString()] = "Г";
                                Waiting++;
                            }
                        continue;
                    }

                    for (int k = 0; k < ProcessDataTable.Rows.Count; k++)
                        if (ProcessDataTable.Rows[k][0].ToString() == lhProcesses[0][j].id.ToString())
                        {
                            int burst = lhProcesses[0][j].cpuBurst;
                            //for (int i = 0; i < burst; i++)
                            //{
                            ProcessDataTable.Rows[k][tact.ToString()] = "И";
                            lhProcesses[0][j].cpuBurst--;
                            Exec++;
                            worked = true;
                            //}
                            break;
                        }
                }

            if (lhProcesses[0].All(x => x.work == false || x.cpuBurst <= 0))
                done_0 = true;

            if (lhProcesses[1] != null)
            {
                if (lhProcesses[1].All(x => x.work == false || x.cpuBurst <= 0))
                    done_1 = true;
            }
            else
                done_1 = true;

            tact++;

            ProcessDataTable.AcceptChanges();
            Process_DataGrid.Items.Refresh();

            TextBlock_Avg_Exec_Time.Text = (Math.Round(Exec / (double)processList.Count, 3)).ToString();
            TextBlock_Avg_Waiting_Time.Text = (Math.Round(Waiting / (double)processList.Count, 3)).ToString();
        }


        private void Refresh_AllGrid()
        {
            processList = new List<nProcess>();
            Refresh_ProcessGrid();
            Refresh_MemoryProcessGrid();
            Refresh_MemoryMapGrid();
            Refresh_DiskCatalogGrid();
            Refresh_DiskSectorGrid();
        }
        private void Refresh_ProcessGrid()
        {
            ProcessDataTable.Reset();
            Process_DataGrid.ItemsSource = ProcessDataTable.DefaultView;
            ProcessDataTable.Columns.Add(new DataColumn("PID", typeof(int)));
            ProcessDataTable.Columns.Add(new DataColumn("Исполнения", typeof(int)));
            ProcessDataTable.Columns.Add(new DataColumn("Появления", typeof(int)));
            ProcessDataTable.Columns.Add(new DataColumn("Очередь", typeof(int)));
        }

        private void Refresh_MemoryProcessGrid()
        {
            MemoryProcessDataTable.Rows.Clear();
            MemoryProcessDataTable.Columns.Clear();
            MemoryProcessDataTable.Reset();
            Memory_Process_DataGrid.ItemsSource = MemoryProcessDataTable.DefaultView;
            MemoryProcessDataTable.Columns.Add(new DataColumn("PID", typeof(int)));
            MemoryProcessDataTable.Columns.Add(new DataColumn("Color", typeof(SolidColorBrush)));
        }
        private void Refresh_MemoryMapGrid()
        {
            MemoryMapProcessDataTable.Reset();
            Memory_Map_DataGrid.ItemsSource = MemoryMapProcessDataTable.DefaultView;
            for (int i = 0; i < 256; i += 16)
            {
                MemoryMapProcessDataTable.Columns.Add(new DataColumn(i.ToString(), typeof(string)));
            }
        }
        private void Refresh_DiskCatalogGrid()
        {
            DiskCatalogDataTable.Reset();
            Disk_Catalog_DataGrid.ItemsSource = DiskCatalogDataTable.DefaultView;
            DiskCatalogDataTable.Columns.Add(new DataColumn("Имя", typeof(string)));
            DiskCatalogDataTable.Columns.Add(new DataColumn("Размер", typeof(int)));
        }
        private void Refresh_DiskSectorGrid()
        {
            DiskSectorDataTable.Reset();
            Disk_Sector_DataGrid.ItemsSource = DiskSectorDataTable.DefaultView;
            DiskSectorDataTable.Columns.Add(new DataColumn("Имя", typeof(string)));
        }

        private void Del_Column_Process_DataGrid()
        {
            if (ProcessDataTable.Columns.Count > 3)
            {
                int dt_count = Process_DataGrid.Columns.Count - 1;
                for (int i = dt_count; i >= 4; i--)
                {
                    Process_DataGrid.Columns.Remove(Process_DataGrid.Columns[i]);
                }
                int dg_count = ProcessDataTable.Columns.Count - 1;
                for (int i = dg_count; i >= 4; i--)
                {
                    ProcessDataTable.Columns.Remove(ProcessDataTable.Columns[i]);
                }
                processList.ForEach(x =>
                {
                    x.cpuBurst = x.defCpuBurst;
                    x.work = true;
                });
            }
        }

        private void Button_Start_Processes_Click(object sender, RoutedEventArgs e)
        {
            Del_Column_Process_DataGrid();

            if (ProcessDataTable.Rows.Count <= 0)
            {
                MessageBox.Show("Сначала добавьте хотябы один процес", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(TextBox_Quantum_Time.Text))
            {
                MessageBox.Show("Введите квант времени выполнения процеса", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            processList = processList.OrderBy(x => x.queue).ToList();

            lhProcesses = processList // [0] - lower [1] - high
                .GroupBy(x => x.queue)
                .Select(x => x.ToList())
                .ToList();

            lhProcesses = lhProcesses.Select(x => x.OrderBy(x => x.createTime).ToList()).ToList();

            if (lhProcesses.Count < 2)
            {
                MessageBox.Show("Добавьте хотябы один процесс другой очереди", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            tact = 0;
            done_0 = false;
            done_1 = false;
            Exec = 0;
            Waiting = 0;

            timer.Start();
        }

        private void Button_Stop_Processes_Click(object sender, RoutedEventArgs e)
        {
            tact = 0;
            done_0 = false;
            done_1 = false;
            Exec = 0;
            Waiting = 0;
            timer.Stop();
            TextBox_Quantum_Time.IsEnabled = true;
        }

        private void Button_Clear_Processes_Click(object sender, RoutedEventArgs e)
        {
            Del_Column_Process_DataGrid();

            if (ProcessDataTable.Rows.Count <= 0)
            {
                MessageBox.Show("Сначала добавьте хотябы один процесс", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (DataRow row in MemoryProcessDataTable.Rows)
            {
                MemoryProcessDataTable.Rows.Remove(row);
                break;
            }

            foreach (DataRow row in ProcessDataTable.Rows)
            {
                var del_proc = processList.Single(x => x.id.ToString() == row["PID"].ToString());
                Vals.Total_Rem_Pages += del_proc.processMap.Count_Page;
                Vals.Total_Rem_Virt_Pages += del_proc.processMap.Count_Virt_Page;
                _ = processList.Remove(del_proc);
                ProcessDataTable.Rows.Remove(row);
                break;
            }
            Label_Page_Size.Content = Vals.LEN_OF_PAGE;
            Label_Total_Count_Page.Content = Vals.Total_Pages;
            Label_Total_Count_Virt_Page.Content = Vals.Total_Virt_Pages;
            Label_Rem_Total_Count_Page.Content = Vals.Total_Rem_Pages;
            Label_Rem_Total_Count_Virt_Page.Content = Vals.Total_Rem_Virt_Pages;
            ProcessDataTable.AcceptChanges();
        }

        //private void TextBox_Priority_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    TextBox_Priority.Text = string.Join("", TextBox_Priority.Text.Where(c => char.IsDigit(c)));
        //    if (int.TryParse(TextBox_Priority.Text, out _))
        //        if (int.Parse(TextBox_Priority.Text) > 36)
        //        {
        //            MessageBox.Show("Максимальное значение приоритета 36", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //            TextBox_Priority.Text = "36";
        //        }
        //}

        private void TextBox_INT_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBox)e.Source).Text = string.Join("", ((TextBox)e.Source).Text.Where(c => char.IsDigit(c)));
            if (((TextBox)e.Source).Text.Length > 10) ((TextBox)e.Source).Text = ((TextBox)e.Source).Text.Substring(0, 10);
        }

        private void Button_Create_Processes_Click(object sender, RoutedEventArgs e)
        {
            Del_Column_Process_DataGrid();

            if (ComboBox_Priority.SelectedIndex == -1)
            {
                MessageBox.Show("Выбрирете очередь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(TextBox_Exec_Time.Text))
            {
                MessageBox.Show("Введите время выполнения процеса", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(TextBox_Waiting_Time.Text))
            {
                MessageBox.Show("Введите время появления процеса", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(TextBox_Size.Text))
            {
                MessageBox.Show("Введите размер процеса, занимаемого в памяти", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            nProcess process = new nProcess(int.Parse(TextBox_Exec_Time.Text), ComboBox_Priority.SelectedIndex, int.Parse(TextBox_Waiting_Time.Text), int.Parse(TextBox_Size.Text));
            processList.Add(process);
            ProcessDataTable.Rows.Add(process.getProcessDataRow(ProcessDataTable));
            MemoryProcessDataTable.Rows.Add(process.getMemoryProcessDataRow(MemoryProcessDataTable));
            //Process_DataGrid.ItemsSource = ProcessDataTable.DefaultView;

            Label_Page_Size.Content = Vals.LEN_OF_PAGE;
            Label_Total_Count_Page.Content = Vals.Total_Pages;
            Label_Total_Count_Virt_Page.Content = Vals.Total_Virt_Pages;
            Label_Rem_Total_Count_Page.Content = Vals.Total_Rem_Pages;
            Label_Rem_Total_Count_Virt_Page.Content = Vals.Total_Rem_Virt_Pages;
        }

        private void Button_Delete_Processes_Click(object sender, RoutedEventArgs e)
        {
            Del_Column_Process_DataGrid();

            if (!processList.Any(x => x.id.ToString() == TextBox_PID.Text))
            {
                MessageBox.Show("Процесс не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(TextBox_PID.Text))
            {
                MessageBox.Show("Введите PID (ID процесса)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ProcessDataTable.Rows.Count <= 0)
            {
                MessageBox.Show("Сначала добавьте хотябы один процесс", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (DataRow row in MemoryProcessDataTable.Rows)
            {
                if (row["PID"].ToString() == TextBox_PID.Text.ToString())
                {
                    MemoryProcessDataTable.Rows.Remove(row);
                    break;
                }
            }
            foreach (DataRow row in ProcessDataTable.Rows)
            {
                if (row["PID"].ToString() == TextBox_PID.Text.ToString())
                {
                    ProcessDataTable.Rows.Remove(row);
                    break;
                }
            }
            var del_proc = processList.Single(x => x.id.ToString() == TextBox_PID.Text);
            Vals.Total_Rem_Pages += del_proc.processMap.Count_Page;
            Vals.Total_Rem_Virt_Pages += del_proc.processMap.Count_Virt_Page;
            _ = processList.Remove(del_proc);
            //processList.Remove(del_proc);
            ProcessDataTable.AcceptChanges();
        }
        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex()).ToString();
        }
        void Disk_Sector_DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = "Кластер " + e.Row.GetIndex();
        }

        private void Process_DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //Del_Column_Process_DataGrid();

            if (Process_DataGrid.SelectedIndex != -1)
            {
                DataRowView dataRowView = (DataRowView)Process_DataGrid.SelectedItem;
                TextBox_PID.Text = dataRowView["PID"].ToString();
            }
        }

        private void Memory_Process_DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (Memory_Process_DataGrid.SelectedIndex == -1) return;

            DataRowView dataRowView = (DataRowView)Memory_Process_DataGrid.SelectedItem;
            nProcess process = processList.Single(x => x.id.ToString() == dataRowView["PID"].ToString());

            Text_Del_Page.Content = $"Количество удаленных страниц в оперативной памяти {process.name}:";

            Label_PID.Content = process.id;
            Label_Size.Content = process.size;
            Label_Count_Del_Page_in_RAM.Content = process.processMap.Count_Del_Page_in_RAM;
            Label_Page_in_RAM.Content = process.processMap.Count_Page;
            Label_Page_in_Virt.Content = process.processMap.Count_Virt_Page;
            Label_Count_Page.Content = process.processMap.Count_Virt_Page;
            Label_Free_Last_Page.Content = process.processMap.Free_Last_Page;

            MemoryMapProcessDataTable.Rows.Clear();

            var dataRows = process.getMemoryMapProcessDataRow(MemoryMapProcessDataTable);

            foreach (DataRow row in dataRows)
                MemoryMapProcessDataTable.Rows.Add(row);

        }

        private void Memory_Map_DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() % 4 * 256).ToString();
        }



        private void Create_File_Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_File_Size_RAM.Text))
            {
                MessageBox.Show("Введите объём оперативной памяти", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (int.Parse(TextBox_File_Size_RAM.Text) <= (Vals.LEN_OF_CLUSTER / 1024))
            {
                MessageBox.Show("Введите объём больше объёма одного кластера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //if (int.Parse(TextBox_File_Size_RAM.Text) > 1000)
            //{
            //    MessageBox.Show("Введите объём меньше 1000, т.к. такая нагрузка может приветсти к внезапному прекращению работы программы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            if (string.IsNullOrEmpty(TextBox_File_Name.Text))
            {
                MessageBox.Show("Введите название файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(TextBox_File_Size.Text))
            {
                MessageBox.Show("Введите размер файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if ((int.Parse(TextBox_File_Size.Text) / 1024) > int.Parse(TextBox_File_Size_RAM.Text))
            {
                MessageBox.Show("Размер файла не должен привышать объём ОЗУ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (int.Parse(TextBox_File_Size.Text) <= 0)
            {
                MessageBox.Show("Слишком маленький размер файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (fileList.Any(x => x.name == TextBox_File_Name.Text))
            {
                MessageBox.Show("Такой файл уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            nFile new_file = new nFile(TextBox_File_Name.Text, int.Parse(TextBox_File_Size.Text));

            long new_file_clusters = 0; 
            if (new_file.size % Vals.LEN_OF_CLUSTER == 0)
                new_file_clusters = new_file.size / Vals.LEN_OF_CLUSTER;
            else new_file_clusters = new_file.size / Vals.LEN_OF_CLUSTER + 1;

            long busy_Clusters = 0;
            long free_Clusters = int.Parse(TextBox_File_Size_RAM.Text) * 2; ;

            foreach (var file in fileList)
            {
                long clusters = 0;

                if (file.size % Vals.LEN_OF_CLUSTER == 0)
                    clusters = file.size / Vals.LEN_OF_CLUSTER;
                else clusters = file.size / Vals.LEN_OF_CLUSTER + 1;

                free_Clusters -= clusters;
                busy_Clusters += clusters;
            }

            if (free_Clusters - new_file_clusters < 0)
            {
                MessageBox.Show("Слишком большой размер файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (TextBox_File_Size_RAM.IsEnabled)
            {
                TextBox_File_Size_RAM.IsEnabled = false;
                Label_Count_Cluster.Content = int.Parse(TextBox_File_Size_RAM.Text) * 2;
            }

            fileList.Add(new_file);
            DiskCatalogDataTable.Rows.Add(new_file.getDiskCatalogDataRow(DiskCatalogDataTable));

            Update_Disk_Sector();
        }

        private void Delete_File_Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_File_Name.Text))
            {
                MessageBox.Show("Введите название файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!fileList.Any(x => x.name.ToString() == TextBox_File_Name.Text))
            {
                MessageBox.Show("Файл не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            foreach (DataRow row in DiskCatalogDataTable.Rows)
            {
                if (row["Имя"].ToString() == TextBox_File_Name.Text.ToString())
                {
                    DiskCatalogDataTable.Rows.Remove(row);
                    break;
                }
            }

            nFile file = fileList.Single(x => x.name.ToString() == TextBox_File_Name.Text);
            fileList.Remove(file);

            if(fileList.Count == 0) TextBox_File_Size_RAM.IsEnabled = true;

            Update_Disk_Sector();
        }

        private void Update_Disk_Sector()
        {
            DiskSectorDataTable.Rows.Clear();

            if (fileList.Count == 0)
            {
                Label_Count_Cluster.Content = "-";
                Label_Count_Busy_Cluster.Content ="-";
                Label_Count_Free_Cluster.Content ="-";
                return;
            }

            long busy_Clusters = 0, free_Clusters = int.Parse(Label_Count_Cluster.Content.ToString());

            foreach (var file in fileList)
            {
                long clusters = 0;

                if (file.size % Vals.LEN_OF_CLUSTER == 0)
                    clusters = file.size / Vals.LEN_OF_CLUSTER;
                else clusters = file.size / Vals.LEN_OF_CLUSTER + 1;

                free_Clusters-=clusters;
                busy_Clusters+=clusters;

                for (int i = 0; i < clusters; i++)
                {
                    DiskSectorDataTable.Rows.Add(file.getDiskSectorDataRow(DiskSectorDataTable));
                }
            }
            for(int i = 0; i < free_Clusters; i++)
            {
                DataRow dataRow = DiskSectorDataTable.NewRow();
                dataRow["Имя"] = "Свободный";
                DiskSectorDataTable.Rows.Add(dataRow);
            }
            Label_Count_Busy_Cluster.Content = busy_Clusters;
            Label_Count_Free_Cluster.Content = free_Clusters;
        }

        private void Disk_Catalog_DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (Disk_Catalog_DataGrid.SelectedIndex == -1) return;

            DataRowView dataRowView = (DataRowView)Disk_Catalog_DataGrid.SelectedItem;
            nFile file = fileList.Single(x => x.name.ToString() == dataRowView["Имя"].ToString());
            TextBox_File_Name.Text = file.name;
            TextBox_File_Size.Text = file.size.ToString();
        }
    }
}