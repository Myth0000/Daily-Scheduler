using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using System.Text.Json;

namespace Daily_Scheduler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadAllSchedules();
        }
        
// Bindings
        public static string[] Hours
        {
            get { return new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" }; }
        }
        public static string[] Minutes
        {
            get { return new string[] {
            "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" };
            }
        }
        public static string[] Meridiem
        {
            get { return new string[] { "AM", "PM" }; }
        }
        // End of Bindings

        private string SchedulesJsonFilePath = @"C:\Users\infin\source\repos\Daily Scheduler\Datastore\Schedules.json";

        private void ScheduleButton_Click(object sender, RoutedEventArgs e)
        {
            string ActivityName = ActivityName_textbox.Text.Trim();
            string ActivityDescription = ActivityDescription_textbox.Text.Trim();
            string FormatedActivityTime = $"{Hours_combobox.Text}:{Minutes_combobox.Text} {Meridiem_combobox.Text}";

            if (IsNotNullOrWhiteSpace(ActivityName) && 
                IsNotNullOrWhiteSpace(ActivityDescription) && 
                IsNotNullOrWhiteSpace(FormatedActivityTime.Replace(":", " "))
                )
            {
                Schedule NewSchedule = new()
                {
                    ActivityName = ActivityName,
                    ActivityDescription = ActivityDescription,
                    ActivityTime = FormatedActivityTime
                };

                List<Schedule> DeserializedSchedulesData = JsonSerializer.Deserialize<List<Schedule>>(File.ReadAllText(SchedulesJsonFilePath));
                DeserializedSchedulesData.Add(NewSchedule);
                UpdateSchedulesDataAndListbox(DeserializedSchedulesData);
            }
        }

        bool IsNotNullOrWhiteSpace(string _string) { return !string.IsNullOrWhiteSpace(_string); }

        void UpdateSchedulesDataAndListbox(List<Schedule> DeserializedSchedulesData)
        {
            string SerializedScheduleData = JsonSerializer.Serialize(DeserializedSchedulesData, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText(SchedulesJsonFilePath, SerializedScheduleData);
            Schedules_listbox.ItemsSource = DeserializedSchedulesData;
        }

        private void LoadAllSchedules()
        {
            List<Schedule> DeserializedSchedulesData = JsonSerializer.Deserialize<List<Schedule>>(File.ReadAllText(SchedulesJsonFilePath));
            Schedules_listbox.ItemsSource = DeserializedSchedulesData;
        }

        private void DeleteSchedule_contextmenu(object sender, RoutedEventArgs e)
        {
            int ScheduleIndex = Schedules_listbox.SelectedIndex;

            List<Schedule> DeserializedSchedulesData = JsonSerializer.Deserialize<List<Schedule>>(File.ReadAllText(SchedulesJsonFilePath));
            DeserializedSchedulesData.RemoveAt(ScheduleIndex);
            UpdateSchedulesDataAndListbox(DeserializedSchedulesData);
        }
    }
}
