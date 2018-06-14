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

                _labelClassroom.Content = _classrooms.Keys.First();
                ChangeClassroomSchedule((_labelClassroom.Content.ToString()));
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
                for (int i = 0; i <= 15; i++)
                {
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
                }
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

                var list = new ObservableCollection<DataObject>();
                list = (ObservableCollection < DataObject > )tableGrid.ItemsSource;

                // racunaj poziciju
                Point mousePosition = e.GetPosition(tableGrid);
                double mouseY = mousePosition.Y;
                double screenHeight = tableGrid.Height -40;
                selectedRow = (int)(((mouseY) / (screenHeight)) * 20);
                if (selectedRow > 20)
                {
                    selectedRow = 20;
                }

                Console.WriteLine("mouse y: " + mouseY + " table height: "+ tableGrid.Height +" selectedRow = " + (mouseY / screenHeight) * 16);
                

                DataObject obj = list.ElementAt(selectedRow);
                list.RemoveAt(selectedRow);
                list.Insert(selectedRow, new DataObject { timesList = obj.timesList, subjectsList = subject.Name });
                if (obj.subjectsList == subject.Name)
                {
                    _isNewDrop = false;
                }

                if (swap_idx >= 0)
                {
                    list.RemoveAt(swap_idx);
                    list.Insert(swap_idx, new DataObject { timesList = (swap_idx+7)+":00", subjectsList = obj.subjectsList});
                }

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
                        if (item.ID == _candidate.ID)
                        {
                            item.NoOfClassesSet = item.NoOfClassesSet + 1;
                        }
                        newSubjects.Add(item);
                    }
                    _subjectsUI.ItemsSource = newSubjects;
                }
                

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
                var list = new ObservableCollection<DataObject>();
                list = (ObservableCollection<DataObject>)tableGrid.ItemsSource;

                int deleteIdx = tableGrid.SelectedIndex;

                
                var newSubjects = new ObservableCollection<Subject>();
                var candidate = list[deleteIdx];

                foreach (var item in _subjects)
                {
                    if (item.Name == candidate.subjectsList)
                    {
                        item.NoOfClassesSet = item.NoOfClassesSet - 1;
                    }
                    newSubjects.Add(item);
                }
                _subjectsUI.ItemsSource = newSubjects;

                string time = list.ElementAt(deleteIdx).timesList;
                
                list.RemoveAt(deleteIdx);
                list.Insert(deleteIdx, new DataObject { timesList = time , subjectsList = "" });
                this.tableGrid.ItemsSource = list;

                Console.WriteLine("deleted item at position " + deleteIdx);
                e.Handled = true;

            }
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
