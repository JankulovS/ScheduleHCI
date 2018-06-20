using Schedule.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Schedule
{
    /// <summary>
    /// Interaction logic for Table.xaml
    /// </summary>
    /// 

    public class ScheduleData
    {
        public string classroom;
        public List<Table.DataObject> listMonday { get; set; }
        public List<Table.DataObject> listTuesday { get; set; }
        public List<Table.DataObject> listWednesday { get; set; }
        public List<Table.DataObject> listThursday { get; set; }
        public List<Table.DataObject> listFriday { get; set; }
        public List<Table.DataObject> listSaturday { get; set; }
        public List<Table.DataObject> list { get; set; }
    }

    public partial class Table : UserControl
    {
        int selectedRow;
        Point startPoint;
        int swap_idx;
        static Label _labelClassroom;
        static Table _table;
        public const string REPEAT = " ~ | | ~ ";

        public static Subject _candidate;
        public static ObservableCollection<Subject> _subjects;
        public static ListView _subjectsUI;
        static string _classroom;
        static Dictionary<string, ScheduleData> _classrooms;
        public static bool _isNewDrop;

        public ObservableCollection<DataObject> listMonday = new ObservableCollection<DataObject>();
        ObservableCollection<DataObject> listTuesday = new ObservableCollection<DataObject>();
        ObservableCollection<DataObject> listWednesday = new ObservableCollection<DataObject>();
        ObservableCollection<DataObject> listThursday = new ObservableCollection<DataObject>();
        ObservableCollection<DataObject> listFriday = new ObservableCollection<DataObject>();
        ObservableCollection<DataObject> listSaturday = new ObservableCollection<DataObject>();

        // save shortcut


        public class DataObject
        {
            public string timesList { get; set; }
            public string subjectsList { get; set; }
        }

        public static void SaveSchedule()
        {
            //MainWindow._mainWindow.Save();
            XmlSerializer xs = new XmlSerializer(typeof(List<ScheduleData>));
            TextWriter tw = new StreamWriter(MainWindow._file + ".sch");

            List<ScheduleData> ser_data = new List<ScheduleData>();

            foreach (var data in _classrooms)
            {
                ScheduleData sch = new ScheduleData();
                sch.classroom = data.Key;
                sch.listMonday = data.Value.listMonday.ToList<Table.DataObject>();
                sch.listTuesday = data.Value.listTuesday.ToList<Table.DataObject>();
                sch.listWednesday = data.Value.listWednesday.ToList<Table.DataObject>();
                sch.listThursday = data.Value.listThursday.ToList<Table.DataObject>();
                sch.listFriday = data.Value.listFriday.ToList<Table.DataObject>();
                sch.listSaturday = data.Value.listSaturday.ToList<Table.DataObject>();
                ser_data.Add(sch);
            }

            //ScheduleData sch = new ScheduleData();


            xs.Serialize(tw, ser_data);
            Console.WriteLine("Saved schedule!");
        }

        public static void LoadSchedule()
        {
            //MainWindow._mainWindow.Load();

            XmlSerializer xs = new XmlSerializer(typeof(List<ScheduleData>));
            using (var sr = new StreamReader(MainWindow._file + ".sch"))
            {
                List<ScheduleData> ser_data = (List<ScheduleData>)xs.Deserialize(sr);

                //ScheduleData sch = (ScheduleData)xs.Deserialize(sr);

                foreach(var sch in ser_data)
                {
                    var list = new List<Table.DataObject>();
                    _classrooms[sch.classroom] = new ScheduleData();
                    foreach (var item in sch.listMonday)
                    {
                        list.Add(item);
                    }
                    _classrooms[sch.classroom].listMonday = list;

                    list = new List<Table.DataObject>();
                    foreach (var item in sch.listTuesday)
                    {
                        list.Add(item);
                    }
                    _classrooms[sch.classroom].listTuesday = list;

                    list = new List<Table.DataObject>();
                    foreach (var item in sch.listWednesday)
                    {
                        list.Add(item);
                    }
                    _classrooms[sch.classroom].listWednesday = list;

                    list = new List<Table.DataObject>();
                    foreach (var item in sch.listThursday)
                    {
                        list.Add(item);
                    }
                    _classrooms[sch.classroom].listThursday = list;

                    list = new List<Table.DataObject>();
                    foreach (var item in sch.listFriday)
                    {
                        list.Add(item);
                    }
                    _classrooms[sch.classroom].listFriday = list;

                    list = new List<Table.DataObject>();
                    foreach (var item in sch.listSaturday)
                    {
                        list.Add(item);
                    }
                    _classrooms[sch.classroom].listSaturday = list;
                }

                


                Console.WriteLine("Loaded schedule!");

                // refresh UI.

                try {
                    _labelClassroom.Content = _classrooms.Keys.First();
                    ChangeClassroomSchedule((_labelClassroom.Content.ToString()));
                }
                catch (Exception)
                {
                    ChangeClassroomSchedule("NOT SELECTED");
                }
                ItemList._itemList.lv3.SelectedIndex = 0;

                if (_table.Monday.IsSelected)
                {
                    _table.tableGrid.ItemsSource = _table.listMonday;
                }

                if (_table.Tuesday.IsSelected)
                {
                    _table.tableGrid.ItemsSource = _table.listTuesday;
                }

                if (_table.Wednesday.IsSelected)
                {
                    _table.tableGrid.ItemsSource = _table.listWednesday;
                }

                if (_table.Thursday.IsSelected)
                {
                    _table.tableGrid.ItemsSource = _table.listThursday;
                }

                if (_table.Friday.IsSelected)
                {
                    _table.tableGrid.ItemsSource = _table.listFriday;
                }

                if (_table.Saturday.IsSelected)
                {
                    _table.tableGrid.ItemsSource = _table.listSaturday;
                }
            }
        }

        public static void ChangeClassroomSchedule(string classroom)
        {
            _classroom = classroom;
            if (Table._classrooms.ContainsKey(classroom))
            {

                ScheduleData sch = Table._classrooms[classroom];
                var list = new ObservableCollection<Table.DataObject>();

                foreach (var item in sch.listMonday)
                {
                    list.Add(item);
                }
                _table.listMonday = list;

                list = new ObservableCollection<Table.DataObject>();
                foreach (var item in sch.listTuesday)
                {
                    list.Add(item);
                }
                _table.listTuesday = list;

                list = new ObservableCollection<Table.DataObject>();
                foreach (var item in sch.listWednesday)
                {
                    list.Add(item);
                }
                _table.listWednesday = list;

                list = new ObservableCollection<Table.DataObject>();
                foreach (var item in sch.listThursday)
                {
                    list.Add(item);
                }
                _table.listThursday = list;

                list = new ObservableCollection<Table.DataObject>();
                foreach (var item in sch.listFriday)
                {
                    list.Add(item);
                }
                _table.listFriday = list;

                list = new ObservableCollection<Table.DataObject>();
                foreach (var item in sch.listSaturday)
                {
                    list.Add(item);
                }
                _table.listSaturday = list;


            }
            else
            {
                ScheduleData sch = new ScheduleData();
                sch.listMonday = new List<Table.DataObject>();
                sch.listTuesday = new List<Table.DataObject>();
                sch.listWednesday = new List<Table.DataObject>();
                sch.listThursday = new List<Table.DataObject>();
                sch.listFriday = new List<Table.DataObject>();
                sch.listSaturday = new List<Table.DataObject>();

                    //sch.list.Add(new DataObject() { timesList = i + 7 + ":00", subjectsList = "" });
                    sch.listMonday.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });

                    sch.listMonday.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });
                    sch.listTuesday.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });
                    sch.listWednesday.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });
                    sch.listThursday.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });
                    sch.listFriday.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });
                    sch.listSaturday.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });
                
                _classrooms[classroom] = sch;

                var list = new ObservableCollection<Table.DataObject>();

                foreach (var item in sch.listMonday)
                {
                    list.Add(item);
                }
                _table.listMonday = list;

                list = new ObservableCollection<Table.DataObject>();
                foreach (var item in sch.listTuesday)
                {
                    list.Add(item);
                }
                _table.listTuesday = list;

                list = new ObservableCollection<Table.DataObject>();
                foreach (var item in sch.listWednesday)
                {
                    list.Add(item);
                }
                _table.listWednesday = list;

                list = new ObservableCollection<Table.DataObject>();
                foreach (var item in sch.listThursday)
                {
                    list.Add(item);
                }
                _table.listThursday = list;

                list = new ObservableCollection<Table.DataObject>();
                foreach (var item in sch.listFriday)
                {
                    list.Add(item);
                }
                _table.listFriday = list;

                list = new ObservableCollection<Table.DataObject>();
                foreach (var item in sch.listSaturday)
                {
                    list.Add(item);
                }
                _table.listSaturday = list;
            }

            // refresh UI.
            if (_table.Monday.IsSelected)
            {
                _table.tableGrid.ItemsSource = _table.listMonday;
            }

            if (_table.Tuesday.IsSelected)
            {
                _table.tableGrid.ItemsSource = _table.listTuesday;
            }

            if (_table.Wednesday.IsSelected)
            {
                _table.tableGrid.ItemsSource = _table.listWednesday;
            }

            if (_table.Thursday.IsSelected)
            {
                _table.tableGrid.ItemsSource = _table.listThursday;
            }

            if (_table.Friday.IsSelected)
            {
                _table.tableGrid.ItemsSource = _table.listFriday;
            }

            if (_table.Saturday.IsSelected)
            {
                _table.tableGrid.ItemsSource = _table.listSaturday;
            }
        }

        public Table()
        {
            InitializeComponent();
            InitTimes();
            _labelClassroom = labelClassroom;
            swap_idx = -1;
            _table = this;
            _isNewDrop = true;
            _classrooms = new Dictionary<string, ScheduleData>();
        }

        public void ResetTable()
        {
            _labelClassroom = labelClassroom;
            swap_idx = -1;
            _table = this;
            _isNewDrop = true;
            _classrooms.Clear();
            listMonday = new ObservableCollection<DataObject>();
            listTuesday = new ObservableCollection<DataObject>();
            listWednesday = new ObservableCollection<DataObject>();
            listThursday = new ObservableCollection<DataObject>();
            listFriday = new ObservableCollection<DataObject>();
            listSaturday = new ObservableCollection<DataObject>();
            InitTimes();
            _labelClassroom.Content = "NOT SELECTED";

        }

        public void InitTimes()
        {

            var list = new ObservableCollection<DataObject>();
            
            list.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 7 + ":00", subjectsList = "" });

            list.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 7 + ":45", subjectsList = "" });

            list.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 8 + ":30", subjectsList = "" });

            list.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 9 + ":15", subjectsList = "" });

            list.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 10 + ":00", subjectsList = "" });

            list.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 10 + ":45", subjectsList = "" });

            list.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 11 + ":30", subjectsList = "" });

            list.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 12 + ":15", subjectsList = "" });

            list.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 13 + ":00", subjectsList = "" });

            list.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 13 + ":45", subjectsList = "" });

            list.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 14 + ":30", subjectsList = "" });

            list.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 15 + ":15", subjectsList = "" });

            list.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 16 + ":00", subjectsList = "" });

            list.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 16 + ":45", subjectsList = "" });

            list.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 17 + ":30", subjectsList = "" });

            list.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 18 + ":15", subjectsList = "" });

            list.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 19 + ":00", subjectsList = "" });

            list.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 19 + ":45", subjectsList = "" });

            list.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 20 + ":30", subjectsList = "" });

            list.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });
            listMonday.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });
            listTuesday.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });
            listWednesday.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });
            listThursday.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });
            listFriday.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });
            listSaturday.Add(new DataObject() { timesList = 21 + ":15", subjectsList = "" });

            this.tableGrid.ItemsSource = list;
        }

        

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
            e.Effects = DragDropEffects.All;
            //selectedRow = tableGrid.ItemContainerGenerator.IndexFromContainer((DataGridRow)sender);

        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat") || true)
            {
                if ((string)_labelClassroom.Content == "NOT SELECTED")
                {
                    MessageBox.Show("Please select a classroom first.");
                    return;
                }


                Subject subject = e.Data.GetData("myFormat") as Subject;

                if (subject.NoOfClassesSet >= subject.NoOfClasses && swap_idx < 0)
                {
                    MessageBox.Show("Subject will exceed maximum number of usage.");
                    return;
                }

                var list = new ObservableCollection<DataObject>();
                list = (ObservableCollection < DataObject > )tableGrid.ItemsSource;

                // racunaj poziciju
                Point mousePosition = e.GetPosition(tableGrid);
                double mouseY = mousePosition.Y;
                double screenHeight = tableGrid.Height;// -40;
                selectedRow = (int)(((mouseY) / (screenHeight)) * 20);
                if (selectedRow > 20)
                {
                    selectedRow = 20;
                }

                Console.WriteLine("mouse y: " + mouseY + " table height: "+ tableGrid.Height +" selectedRow = " + (mouseY / screenHeight) * 16);
                

                DataObject obj = list.ElementAt(selectedRow);

                if (obj.subjectsList == subject.Name)
                {
                    swap_idx = -1;
                    _isNewDrop = true;
                    return;
                }

                // check if there are enough free slots
                for (int i = 0; i < subject.ClassLength; i++)
                {
                    try
                    {
                        if (list.ElementAt(selectedRow + i).subjectsList != "")
                        {
                            string IsAre = "is";
                            if (i > 1)
                            {
                                IsAre = "are";
                            }
                            MessageBox.Show("There are not enough free slots. This subject requires " + subject.ClassLength + " successive free slots but there " + IsAre + " " + i + " available.");
                            return;
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("Subjects are out of bounds of the schedule table. " + subject.ClassLength + " successive slots required.");
                        return;
                    }
                }
                

                for (int i = 0; i < subject.ClassLength; i++ )
                {
                    obj = list.ElementAt(selectedRow + i);
                    list.RemoveAt(selectedRow + i);
                    if (i == 0)
                        list.Insert(selectedRow + i, new DataObject { timesList = obj.timesList, subjectsList = subject.Name });
                    else
                        list.Insert(selectedRow + i, new DataObject { timesList = obj.timesList, subjectsList = REPEAT });
                }

                //if (swap_idx >= 0 && swap_idx != selectedRow)
                //{
                //    string swap_name = list.ElementAt(swap_idx).subjectsList;
                //    string swap_time = list.ElementAt(swap_idx).timesList;
                //    list.RemoveAt(selectedRow);
                //    list.Insert(selectedRow, new DataObject { timesList = obj.timesList, subjectsList = subject.Name });
                //    list.RemoveAt(swap_idx);
                //    list.Insert(swap_idx, new DataObject { timesList = swap_time, subjectsList = obj.subjectsList });
                //}

                bool real = true;
                
                if (obj.subjectsList == subject.Name)
                {
                    _isNewDrop = false;
                }

                if (swap_idx >= 0 && _isNewDrop == false)
                {
                    real = false;
                    var candidate = list[tableGrid.SelectedIndex];
                    int swap_id = tableGrid.SelectedIndex;
                    int subject_length = 1;

                    while (list.ElementAt(swap_id).subjectsList == REPEAT)
                    {
                        swap_id--;
                    }
                    candidate = list[swap_id];
                    subject.Name = candidate.subjectsList;

                    Delete(false);


                    // find class length
                    foreach (var item in _subjects)
                    {
                        if (item.Name == candidate.subjectsList)
                        {
                            subject_length = item.ClassLength;
                        }
                    }
                    // check if there are enough free slots
                    for (int i = 0; i < subject_length; i++)
                    {
                        try
                        {
                            if (list.ElementAt(selectedRow + i).subjectsList != "")
                            {
                                string IsAre = "is";
                                if (i > 1)
                                {
                                    IsAre = "are";
                                }
                                MessageBox.Show("There are not enough free slots. This subject requires " + subject_length + " successive free slots but there " + IsAre + " " + i + " available.");
                                selectedRow = swap_id;
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            MessageBox.Show("Subjects are out of bounds of the schedule table. " + subject_length + " successive slots required.");
                            selectedRow = swap_id;
                        }
                    }

                    for (int i = 0; i < subject_length; i++)
                    {
                        obj = list.ElementAt(selectedRow + i);
                        list.RemoveAt(selectedRow + i);
                        if (i == 0)
                            list.Insert(selectedRow + i, new DataObject { timesList = obj.timesList, subjectsList = subject.Name });
                        else
                            list.Insert(selectedRow + i, new DataObject { timesList = obj.timesList, subjectsList = REPEAT });
                    }

                    _isNewDrop = true;
                }

                //if (swap_idx >= 0)
                //{
                //    list.RemoveAt(swap_idx);
                //    switch(swap_idx)
                //    {
                //        case 0:
                //            list.Insert(swap_idx, new DataObject { timesList = "7:00", subjectsList = obj.subjectsList });
                //            break;
                //        case 1:
                //            list.Insert(swap_idx, new DataObject { timesList = "7:45", subjectsList = obj.subjectsList });
                //            break;
                //        case 2:
                //            list.Insert(swap_idx, new DataObject { timesList = "8:30", subjectsList = obj.subjectsList });
                //            break;
                //        case 3:
                //            list.Insert(swap_idx, new DataObject { timesList = "9:15", subjectsList = obj.subjectsList });
                //            break;
                //        case 4:
                //            list.Insert(swap_idx, new DataObject { timesList = "10:00", subjectsList = obj.subjectsList });
                //            break;
                //        case 5:
                //            list.Insert(swap_idx, new DataObject { timesList = "10:45", subjectsList = obj.subjectsList });
                //            break;
                //        case 6:
                //            list.Insert(swap_idx, new DataObject { timesList = "11:30", subjectsList = obj.subjectsList });
                //            break;
                //        case 7:
                //            list.Insert(swap_idx, new DataObject { timesList = "12:15", subjectsList = obj.subjectsList });
                //            break;
                //        case 8:
                //            list.Insert(swap_idx, new DataObject { timesList = "13:00", subjectsList = obj.subjectsList });
                //            break;
                //        case 9:
                //            list.Insert(swap_idx, new DataObject { timesList = "13:45", subjectsList = obj.subjectsList });
                //            break;
                //        case 10:
                //            list.Insert(swap_idx, new DataObject { timesList = "14:30", subjectsList = obj.subjectsList });
                //            break;
                //        case 11:
                //            list.Insert(swap_idx, new DataObject { timesList = "15:15", subjectsList = obj.subjectsList });
                //            break;
                //        case 12:
                //            list.Insert(swap_idx, new DataObject { timesList = "16:00", subjectsList = obj.subjectsList });
                //            break;
                //        case 13:
                //            list.Insert(swap_idx, new DataObject { timesList = "16:45", subjectsList = obj.subjectsList });
                //            break;
                //        case 14:
                //            list.Insert(swap_idx, new DataObject { timesList = "17:30", subjectsList = obj.subjectsList });
                //            break;
                //        case 15:
                //            list.Insert(swap_idx, new DataObject { timesList = "18:15", subjectsList = obj.subjectsList });
                //            break;
                //        case 16:
                //            list.Insert(swap_idx, new DataObject { timesList = "19:00", subjectsList = obj.subjectsList });
                //            break;
                //        case 17:
                //            list.Insert(swap_idx, new DataObject { timesList = "19:45", subjectsList = obj.subjectsList });
                //            break;
                //        case 18:
                //            list.Insert(swap_idx, new DataObject { timesList = "20:30", subjectsList = obj.subjectsList });
                //            break;
                //        case 19:
                //            list.Insert(swap_idx, new DataObject { timesList = "21:15", subjectsList = obj.subjectsList });
                //            break;

                //    }
                //list.Insert(swap_idx, new DataObject { timesList = (swap_idx+7)+":00", subjectsList = obj.subjectsList});
                //}

                this.tableGrid.ItemsSource = list;

                ScheduleData sch = new ScheduleData();
                sch.listMonday = listMonday.ToList<Table.DataObject>();
                sch.listTuesday = listTuesday.ToList<Table.DataObject>();
                sch.listWednesday = listWednesday.ToList<Table.DataObject>();
                sch.listThursday = listThursday.ToList<Table.DataObject>();
                sch.listFriday = listFriday.ToList<Table.DataObject>();
                sch.listSaturday = listSaturday.ToList<Table.DataObject>();


                _classrooms[_classroom] = sch;



                if (_isNewDrop)
                {
                    var newSubjects = new ObservableCollection<Subject>();

                    foreach (var item in _subjects)
                    {
                        try
                        {
                            if (item.ID == _candidate.ID)
                            {
                                if (real)
                                    item.NoOfClassesSet = item.NoOfClassesSet + 1;

                            }
                        }
                        catch (NullReferenceException)
                        {

                        }
                        newSubjects.Add(item);
                    }
                    _subjectsUI.ItemsSource = newSubjects;
                }

                //if (obj.subjectsList != "" && swap_idx < 0)
                //{
                //    var newSubjects = new ObservableCollection<Subject>();

                //    foreach (var item in _subjects)
                //    {
                //        if (item.Name == obj.subjectsList)
                //        {
                //            item.NoOfClassesSet = item.NoOfClassesSet - 1;
                //        }
                //        newSubjects.Add(item);
                //    }
                //    _subjectsUI.ItemsSource = newSubjects;
                //} 
                

                // reset
                swap_idx = -1;
                _isNewDrop = true;
            }
            Console.WriteLine("DROP FROM GRID DETECTED");
        }



        private void Row_MouseEnter(object sender, MouseEventArgs e)
        {
            //selectedRow = tableGrid.ItemContainerGenerator.IndexFromContainer((DataGridRow)sender);
            Console.WriteLine(selectedRow);
        }

        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void GridMove_PreviewMoouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void GridMove_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                //Console.WriteLine(sender);

                var list = new ObservableCollection<DataObject>();
                list = (ObservableCollection<DataObject>)tableGrid.ItemsSource;
                swap_idx = tableGrid.SelectedIndex;
                DataObject obj;
                try { obj= list.ElementAt(swap_idx); }
                catch (ArgumentOutOfRangeException)
                {
                    return;
                }
                _isNewDrop = false;

                Subject subject = new Subject();
                subject.Name = obj.subjectsList;

                System.Windows.DataObject dragData = new System.Windows.DataObject("myFormat", subject);
                DragDrop.DoDragDrop(tableGrid, dragData, DragDropEffects.Move);
            }
        }

        private void GridTable_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Delete(true);
                e.Handled = true;
            }
        }

        private void Delete(bool real)
        {
            var list = new ObservableCollection<DataObject>();
            list = (ObservableCollection<DataObject>)tableGrid.ItemsSource;

            int deleteIdx = tableGrid.SelectedIndex;


            var newSubjects = new ObservableCollection<Subject>();
            var candidate = list[deleteIdx];
            int subject_length = 0;


            // find root
            while (list.ElementAt(deleteIdx).subjectsList == REPEAT)
            {
                deleteIdx--;
            }
            candidate = list[deleteIdx];


            foreach (var item in _subjects)
            {
                if (item.Name == candidate.subjectsList)
                {
                    if(real)
                        item.NoOfClassesSet = item.NoOfClassesSet - 1;
                }
                newSubjects.Add(item);
            }
            _subjectsUI.ItemsSource = newSubjects;




            // find class length
            foreach (var item in _subjects)
            {
                if (item.Name == candidate.subjectsList)
                {
                    subject_length = item.ClassLength;
                }
            }

            // store times
            List<string> times = new List<string>();
            for (int i = 0; i < subject_length; i++)
            {
                times.Add(list.ElementAt(deleteIdx + i).timesList);
            }

            // delete from root
            if (list.ElementAt(deleteIdx).subjectsList != REPEAT)
            {
                for (int i = 0; i < subject_length; i++)
                {
                    list.RemoveAt(deleteIdx + i);
                    list.Insert(deleteIdx + i, new DataObject { timesList = times.ElementAt(i), subjectsList = "" });
                }
            }

            //list.RemoveAt(deleteIdx);
            //list.Insert(deleteIdx, new DataObject { timesList = time , subjectsList = "" });
            this.tableGrid.ItemsSource = list;

            Console.WriteLine("deleted item at position " + deleteIdx);

        }

        public static void ChangeClassroomLabel(string label)
        {
            _labelClassroom.Content = label;

            Table.ChangeClassroomSchedule(label);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if(Monday.IsSelected)
                {
                    tableGrid.ItemsSource = listMonday;
                }

                if (Tuesday.IsSelected)
                {
                    tableGrid.ItemsSource = listTuesday;
                }

                if (Wednesday.IsSelected)
                {
                    tableGrid.ItemsSource = listWednesday;
                }

                if (Thursday.IsSelected)
                {
                    tableGrid.ItemsSource = listThursday;
                }

                if (Friday.IsSelected)
                {
                    tableGrid.ItemsSource = listFriday;
                }

                if (Saturday.IsSelected)
                {
                    tableGrid.ItemsSource = listSaturday;
                }
            }

        }
    }
}
