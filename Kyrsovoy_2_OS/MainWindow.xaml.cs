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

namespace Kyrsovoy_2_OS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool Selected_Process_control_algorithm = false;
        bool Selected_Memory_control_algorithm = false;
        bool Selected_VSU_control_algorithm = false;
        bool Started = false;

        DataTable ProcessDataTable = new DataTable();

        List<nProcess> processList = new List<nProcess>();

        public MainWindow()
        {
            InitializeComponent();
            ComboBox_Priority.Items.Add("высокий приоритет – Round robin");
            ComboBox_Priority.Items.Add("низкий приоритет – FCFS");

            //int p = new int();
            //Byte[] value = { 12, 13, 2 };

            //DLL.WriteMemory(ref p, value, 3);

            //MessageBox.Show(DLL.ReadMemory(ref p, 3)[2].ToString());


            //Process_StatusBar.Items.Add(new Separator());
            //Process_StatusBar.Items.Add("hui");
            //MessageBox.Show(DLL.GetPhysMemoryBlockCount().ToString());


            Refresh_ProcessGrid();
        }

        private void Refresh_ProcessGrid()
        {
            Process_DataGrid.Columns.Clear();

            //ProcessDataTable.Columns.Add(new DataColumn("PID", null, "id"));
            //ProcessDataTable.Columns.Add(new DataColumn("Время.\nиспольнения", null, "cpuBurst"));
            ProcessDataTable.Columns.Add(new DataColumn("PID", typeof(int)));
            ProcessDataTable.Columns.Add(new DataColumn("Время.\nисполнения", typeof(int)));
            ProcessDataTable.Columns.Add(new DataColumn("Время.\nпоявления", typeof(int)));
            ProcessDataTable.Columns.Add(new DataColumn("Очередь", typeof(int)));
            ProcessDataTable.Columns.Add(new DataColumn("Приоритет", typeof(int)));

            //for (int i = 0; i < 46; i++)
            //{
            //    ProcessDataTable.Columns.Add((i + 1).ToString());
            //    //DataGridTextColumn num = new DataGridTextColumn();
            //    //num.Header = i + 1;
            //    //num.Binding = new Binding((i + 1).ToString());
            //    //Process_DataGrid.Columns.Add(num);
            //}

            Process_DataGrid.ItemsSource = ProcessDataTable.DefaultView;

            //Process process = new Process();
            //process.StartInfo.FileName = "zxc.exe";
            //process.StartInfo.Arguments = "-n";
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            //process.Start();
            //process.WaitForExit();// Waits here for the process to exit.



            //var _ds = new DataSet("Test");

            //employeeDataTable = _ds.Tables.Add("DT");
            //for (int i = 0; i < 10; i++)//create columns
            //{
            //    employeeDataTable.Columns.Add(i.ToString());
            //}

            //for (int i = 0; i < 50; i++)//fill data to rows
            //{
            //    var theRow = employeeDataTable.NewRow();
            //    for (int j = 0; j < 10; j++)
            //    {
            //        if (j % 2 == 0)
            //            theRow[j] = "a";
            //        else
            //            theRow[j] = "b";
            //    }
            //    employeeDataTable.Rows.Add(theRow);
            //}
            //Process_DataGrid.ItemsSource = employeeDataTable.AsDataView();

            /*
            DataGridTextColumn id = new DataGridTextColumn();
            id.Header = "PID";
            id.Binding = new Binding("id");
            id.MinWidth = 65;
            Process_DataGrid.Columns.Add(id);

            DataGridTextColumn burstTime = new DataGridTextColumn();
            burstTime.Header = "Время.\nиспольнения";
            burstTime.Binding = new Binding("cpuBurst");
            burstTime.MinWidth = 50;
            Process_DataGrid.Columns.Add(burstTime);

            DataGridTextColumn createTime = new DataGridTextColumn();
            createTime.Header = "Время.\nпоявления";
            createTime.Binding = new Binding("cpuBurst");
            createTime.MinWidth = 50;
            Process_DataGrid.Columns.Add(createTime);

            DataGridTextColumn queue = new DataGridTextColumn();
            queue.Header = "Очередь";
            queue.Binding = new Binding("queue");
            queue.MinWidth = 50;
            Process_DataGrid.Columns.Add(queue);

            DataGridTextColumn priority = new DataGridTextColumn();
            priority.Header = "Приоритет";
            priority.Binding = new Binding("priority");
            priority.MinWidth = 50;
            Process_DataGrid.Columns.Add(priority);

            for (int i = 0; i < 50; i++)
            {
                DataGridTextColumn num = new DataGridTextColumn();
                num.Header = i + 1;
                num.Binding = new Binding((i + 1).ToString());
                Process_DataGrid.Columns.Add(num);
            }
            */


            //List<nProcess> processes1 = new List<nProcess>();
            //for (int i = 0; i < 10; i++)
            //{
            //    processes1.Add(new nProcess() { ProcessData = "12345", AddressSpace = "78564" });
            //}
            //Process_DataGrid.Items.Add(processes1[0]);

            //ProcessStartInfo procInfo = new ProcessStartInfo();
            //// исполняемый файл программы - браузер хром
            //procInfo.FileName = @"C:\Program Files\Google\Chrome\Application\chrome";
            //// аргументы запуска - адрес интернет-ресурса
            //procInfo.Arguments = "https://metanit.com";
            //Process.Start(procInfo);

            //var processes = Process.GetProcessesByName("Idle");
            //foreach (Process process in processes)
            //    Process_DataGrid.Items.Add(new nProcess() { ProcessData = process.ProcessName, AddressSpace = process.Id });


            //Process_DataGrid.ItemsSource = processes;
            //Memory_Process_DataGrid.Items.Clear();
            //Disk_Sector_DataGrid.Items.Clear();
            //Disk_Map_DataGrid.Items.Clear();
            //Disk_Catalog_DataGrid.Items.Clear();
            //Memory_Address_DataGrid.Items.Clear();
            //Memory_Map_DataGrid.Items.Clear();
            //Memory_Process_DataGrid.Items.Clear();
            //Process_DataGrid.Items.Clear();
        }
        //void Process_DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        //}
        //private void Reverse_IsEnabled()
        //{
        //    Start_Button.IsEnabled = !Start_Button.IsEnabled;
        //    Process_control_algorithm_comboBox.IsEnabled = !Process_control_algorithm_comboBox.IsEnabled;
        //    Memory_control_algorithm_comboBox.IsEnabled = !Memory_control_algorithm_comboBox.IsEnabled;
        //    VSU_control_algorithm_comboBox.IsEnabled = !VSU_control_algorithm_comboBox.IsEnabled;
        //    Stop_Button.IsEnabled = !Stop_Button.IsEnabled;
        //    Create_Process_Button.IsEnabled = !Create_Process_Button.IsEnabled;
        //    Delete_Process_Button.IsEnabled = !Delete_Process_Button.IsEnabled;
        //    Empty_Comand_Button.IsEnabled = !Empty_Comand_Button.IsEnabled;
        //    Write_Linear_Button.IsEnabled = !Write_Linear_Button.IsEnabled;
        //    Read_Linear_Button.IsEnabled = !Read_Linear_Button.IsEnabled;
        //    Write_Segment_Button.IsEnabled = !Write_Segment_Button.IsEnabled;
        //    Read_Segment_Button.IsEnabled = !Read_Segment_Button.IsEnabled;
        //    Write_Memory_Button.IsEnabled = !Write_Memory_Button.IsEnabled;
        //    Read_Memory_Button.IsEnabled = !Read_Memory_Button.IsEnabled;
        //    Linear_TextBox.IsEnabled = !Linear_TextBox.IsEnabled;
        //    Segment_1_TextBox.IsEnabled = !Segment_1_TextBox.IsEnabled;
        //    Segment_2_TextBox.IsEnabled = !Segment_2_TextBox.IsEnabled;
        //    Value_TextBox.IsEnabled = !Value_TextBox.IsEnabled;
        //    Physical_Address_TextBox.IsEnabled = !Physical_Address_TextBox.IsEnabled;
        //    File_Name_TextBox.IsEnabled = !File_Name_TextBox.IsEnabled;
        //    File_Offset_TextBox.IsEnabled = !File_Offset_TextBox.IsEnabled;
        //    Number_Bytes_TextBox.IsEnabled = !Number_Bytes_TextBox.IsEnabled;
        //    Address_Content_Button.IsEnabled = !Address_Content_Button.IsEnabled;
        //    Address_Decrement_Button.IsEnabled = !Address_Decrement_Button.IsEnabled;
        //    Address_Increment_Button.IsEnabled = !Address_Increment_Button.IsEnabled;
        //    Address_TextBox.IsEnabled = !Address_TextBox.IsEnabled;
        //    Sector_Content_Button.IsEnabled = !Sector_Content_Button.IsEnabled;
        //    Sector_Decrement_Button.IsEnabled = !Sector_Decrement_Button.IsEnabled;
        //    Sector_Increment_Button.IsEnabled = !Sector_Increment_Button.IsEnabled;
        //    Sector_TextBox.IsEnabled = !Sector_TextBox.IsEnabled;
        //    Edit_File_Button.IsEnabled = !Edit_File_Button.IsEnabled;
        //    Read_File_Button.IsEnabled = !Read_File_Button.IsEnabled;
        //    Delete_File_Button.IsEnabled = !Delete_File_Button.IsEnabled;
        //    Create_File_Button.IsEnabled = !Create_File_Button.IsEnabled;
        //    Started = !Started;
        //}

        private void Sector_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Sector_TextBox.Text = string.Join("", Sector_TextBox.Text.Where(c => char.IsDigit(c)));
        }

        private void Sector_Increment_Button_Click(object sender, RoutedEventArgs e)
        {
            string str = string.Join("", Sector_TextBox.Text.Where(c => char.IsDigit(c)));
            if (int.TryParse(str, out _))
            {
                Sector_TextBox.Text = (int.Parse(str) + 1).ToString();
            }
            else
            {
                MessageBox.Show("В поле сектор должно быть число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Sector_Decrement_Button_Click(object sender, RoutedEventArgs e)
        {
            string str = string.Join("", Sector_TextBox.Text.Where(c => char.IsDigit(c)));
            if (int.TryParse(str, out _))
            {
                Sector_TextBox.Text = (int.Parse(str) - 1).ToString();
            }
            else
            {
                MessageBox.Show("В поле сектор должно быть число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Sector_Content_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Create_File_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_File_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Read_File_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_File_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Address_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Address_TextBox.Text = string.Join("", Address_TextBox.Text.Where(c => char.IsDigit(c)));
        }

        private void Address_Increment_Button_Click(object sender, RoutedEventArgs e)
        {
            string str = string.Join("", Address_TextBox.Text.Where(c => char.IsDigit(c)));
            if (int.TryParse(str, out _))
            {
                Address_TextBox.Text = (int.Parse(str) + 1).ToString();
            }
            else
            {
                MessageBox.Show("В поле адресс должно быть число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Address_Decrement_Button_Click(object sender, RoutedEventArgs e)
        {
            string str = string.Join("", Address_TextBox.Text.Where(c => char.IsDigit(c)));
            if (int.TryParse(str, out _))
            {
                Address_TextBox.Text = (int.Parse(str) - 1).ToString();
            }
            else
            {
                MessageBox.Show("В поле адресс должно быть число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Address_Content_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void ADD_LOG(string message)
        //{
        //    if (Log_ListBox != null)
        //    {
        //        //DLL.AddLog(message.ToCharArray(), message.Length);
        //        Log_ListBox.Items.Add(message);
        //        Log_ListBox.ScrollIntoView(Log_ListBox.Items[Log_ListBox.Items.Count - 1]);
        //    }
        //}

        private void Process_control_algorithm_comboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (ComboBox_Priority.SelectedIndex == -1) Selected_Process_control_algorithm = false;
            else Selected_Process_control_algorithm = true;
            if (Selected_Process_control_algorithm && Selected_Memory_control_algorithm && Selected_VSU_control_algorithm)
                Button_Start_Processes.IsEnabled = true;
            else Button_Start_Processes.IsEnabled = false;
        }

        private void Button_Start_Processes_Click(object sender, RoutedEventArgs e)
        {
            var selectedCell = Process_DataGrid.Items[1];
            DataRowView? drv = (DataRowView)Process_DataGrid.Items[1];
            for (int i = 4; i < 20; i++)
                drv[i] = "G";
            //var cellContent = selectedCell.Column.GetCellContent(selectedCell.Item);

            MessageBox.Show("" + ((nProcess)Process_DataGrid.Items[1]).id);
        }

        private void Button_Stop_Processes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Clear_Processes_Click(object sender, RoutedEventArgs e)
        {
            ProcessDataTable.Rows.Clear();
            Process_DataGrid.ItemsSource = ProcessDataTable.DefaultView;
        }

        private void TextBox_Priority_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox_Priority.Text = string.Join("", TextBox_Priority.Text.Where(c => char.IsDigit(c)));
            if (int.TryParse(TextBox_Priority.Text, out _))
                if (int.Parse(TextBox_Priority.Text) > 36)
                {
                    MessageBox.Show("Максимальное значение приоритета 36", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    TextBox_Priority.Text = "36";
                }
        }

        private void TextBox_Exec_Time_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox_Exec_Time.Text = string.Join("", TextBox_Exec_Time.Text.Where(c => char.IsDigit(c)));
        }

        private void TextBox_PID_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox_PID.Text = string.Join("", TextBox_PID.Text.Where(c => char.IsDigit(c)));
        }

        private void Button_Create_Processes_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox_Priority.SelectedIndex == -1)
            {
                MessageBox.Show("Выбрирете очередь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(TextBox_Priority.Text))
            {
                MessageBox.Show("Введите приоритет процеса", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(TextBox_Exec_Time.Text))
            {
                MessageBox.Show("Введите время выполнения процеса", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            nProcess process = new nProcess(int.Parse(TextBox_Exec_Time.Text), ComboBox_Priority.SelectedIndex, int.Parse(TextBox_Priority.Text), 5);
            processList.Add(process);
            ProcessDataTable.Rows.Add(process.getDataRow(ProcessDataTable));
            Process_DataGrid.ItemsSource = ProcessDataTable.DefaultView;
        }

        private void Button_Delete_Processes_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}