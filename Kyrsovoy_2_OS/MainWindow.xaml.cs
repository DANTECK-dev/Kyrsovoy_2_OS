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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataTable ProcessDataTable = new DataTable();

        List<nProcess> processList = new List<nProcess>();

        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            ComboBox_Priority.Items.Add("низкий приоритет – FCFS");
            ComboBox_Priority.Items.Add("высокий приоритет – Round robin");

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;

            Refresh_ProcessGrid();
        }


        List<List<nProcess>> lhProcesses = new List<List<nProcess?>?>();


        int tact = 0;
        bool done_0 = false;
        bool done_1 = false;
        double Exec = 0;
        double Waiting = 0;
        int Quant = 0;

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (done_0 && done_1)
            {
                timer.Stop();
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

            if (tact % int.Parse(TextBox_Quantum_Time.Text) == 0 && lhProcesses[1] != null);
            {
                for (int i = 0; i < lhProcesses[1].Count - 1; i++)
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
                        i++;
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

        private void Refresh_ProcessGrid()
        {
            ProcessDataTable.Reset();
            //Process_DataGrid.Columns.Clear();
            Process_DataGrid.ItemsSource = ProcessDataTable.DefaultView;

            processList = new List<nProcess>();

            ProcessDataTable.Columns.Add(new DataColumn("PID", typeof(int)));
            ProcessDataTable.Columns.Add(new DataColumn("Исполнения", typeof(int)));
            ProcessDataTable.Columns.Add(new DataColumn("Появления", typeof(int)));
            ProcessDataTable.Columns.Add(new DataColumn("Очередь", typeof(int)));
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
                MessageBox.Show("Сначала добавьте хотябы один процесс", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
        }

        private void Button_Clear_Processes_Click(object sender, RoutedEventArgs e)
        {
            Del_Column_Process_DataGrid();

            foreach (DataRow row in ProcessDataTable.Rows)
            {
                _ = processList.Remove(processList.Single(x => x.id.ToString() == row["PID"].ToString()));
                ProcessDataTable.Rows.Remove(row);
                break;
            }
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
            nProcess process = new nProcess(int.Parse(TextBox_Exec_Time.Text), ComboBox_Priority.SelectedIndex, int.Parse(TextBox_Waiting_Time.Text));
            processList.Add(process);
            ProcessDataTable.Rows.Add(process.getDataRow(ProcessDataTable));
            Process_DataGrid.ItemsSource = ProcessDataTable.DefaultView;
        }

        private void Button_Delete_Processes_Click(object sender, RoutedEventArgs e)
        {
            Del_Column_Process_DataGrid();

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
            foreach (DataRow row in ProcessDataTable.Rows)
            {
                if (row["PID"].ToString() == TextBox_PID.Text.ToString())
                {
                    ProcessDataTable.Rows.Remove(row);
                    processList.Remove(processList.Single(x => x.id.ToString() == TextBox_PID.Text));
                    break;
                }
            }
            ProcessDataTable.AcceptChanges();
        }

        private void Process_DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            Del_Column_Process_DataGrid();

            if (Process_DataGrid.SelectedIndex != -1)
            {
                DataRowView dataRowView = (DataRowView)Process_DataGrid.SelectedItem;
                TextBox_PID.Text = dataRowView["PID"].ToString();
            }
        }
    }
}