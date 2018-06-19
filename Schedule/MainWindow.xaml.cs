using Microsoft.Win32;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public class EntityData
    {
        public List<Subject> subjects;
        public List<Course> courses;
        public List<Software> software;
        public List<Classroom> classrooms;
    }

    public partial class MainWindow : Window
    {
        private ItemList itemList;
        private Table table;
        SearchWindow sw;
        FilterWindow fw;

        AddClassroomWindow aclassroomw;
        AddSubjectWindow asubjectw;
        AddCourseWindow acoursew;
        AddSoftwareWindow asoftwarew;
        
        private ObservableCollection<Subject> subjects;
        private ObservableCollection<Course> courses;
        private ObservableCollection<Software> software;
        private ObservableCollection<Classroom> classrooms;

        

        public static MainWindow _mainWindow;
        public static string _file;


        public ObservableCollection<Software> Softwares { get { return software; } set { software = value; } }
        public ObservableCollection<Course> Courses { get { return courses; } set { courses = value; } }
        public ObservableCollection<Subject> Subjects { get { return subjects; } set { subjects = value; } }
        public ObservableCollection<Classroom> Classrooms { get { return classrooms; } set { classrooms = value; } }



        public MainWindow()
        {
            _mainWindow = this;

            InitializeComponent();
            InitItems();
            InitTable();
            InitWindows();
            AddEventHandlers();
        }

        public void InitWindows()
        {
            aclassroomw = new AddClassroomWindow();
            aclassroomw.Hide();

            asubjectw = new AddSubjectWindow();
            asubjectw.Hide();

            acoursew = new AddCourseWindow();
            acoursew.Hide();

            asoftwarew = new AddSoftwareWindow();
            asoftwarew.Hide();
        }

        private void InitTable()
        {
            table = new Table();
            grid.Children.Add(table);
            Grid.SetColumn(table, 1);
            

        }

        //public void Save()
        //{
        //    using (FileStream stream = new FileStream(@"D:\Test.xaml", FileMode.Create))
        //    {
        //        XamlWriter.Save(this.Content, stream);
        //    }
        //}

        //public void Load()
        //{
        //    if (File.Exists(@"C:\Test.xaml"))
        //    {
        //        using (FileStream stream = new FileStream(@"D:\Test.xaml", FileMode.Open))
        //        {
        //            this.Content = XamlReader.Load(stream);
        //        }
        //    }
        //}

        private void InitItems()
        {
            itemList = new ItemList();
            Grid.SetColumn(itemList, 0);
            grid.Children.Add(itemList);


            subjects = new ObservableCollection<Subject>();
            courses = new ObservableCollection<Course>();
            software = new ObservableCollection<Software>();
            classrooms = new ObservableCollection<Classroom>();

            UseDummyData();

            itemList.Courses = courses;
            itemList.Software = software;
            itemList.Subjects = subjects;
            itemList.lv.ItemsSource = itemList.Subjects;
            itemList.Classrooms = classrooms;
            itemList.lv2.ItemsSource = itemList.Classrooms;
            itemList.lv3.ItemsSource = subjects;

        }

        private void UseDummyData()
        {


            courses.Add(new Course("Prvi", "SIIT", new DateTime(), "opis"));
            courses.Add(new Course("Drugi", "E2", new DateTime(), "opis"));
            courses.Add(new Course("Treci", "PSI", new DateTime(), "opis"));

            software.Add(new Software("ID1", "Photoshop", "Windows", "Adobe", "www.newst.com", 2017, 2015, "opis"));
            software.Add(new Software("ID2", "Ilustrator", "Windows", "Adobe", "www.newst.com", 2017, 2015, "opis"));
            software.Add(new Software("ID3", "Visual Studio", "Windows", "Adobe", "www.newst.com", 2017, 2015, "opis"));

            List<Software> l = new List<Software>();
            l.Add(new Software("ID1", "Photoshop", "Windows", "Adobe", "www.newst.com", 2017, 2015, "opis"));

            subjects.Add(new Subject { ID = "Prvi", Name = "Interakcija covek racunar", Course = new Course("Prvi", "SIIT", new DateTime(), "opis"), Description = "asd", ClassLength = 1, NoOfClasses = 3, GroupSize = 3, Projector = true, Board = true, SmartBoard = true, OS = "Windows", Software = new List<Software>() });
            subjects.Add(new Subject("Drugi", "Metodologije razvoja softvera", new Course("Prvi", "E2", new DateTime(), "opis"), "asd", 1, 3, 2, 0, true, true, true, "Windows", l));
            subjects.Add(new Subject("Treci", "Pisana i govorna komunikacija u tehnici", new Course("Prvi", "PSI", new DateTime(), "opis"), "asd", 1, 3, 3, 0, true, true, true, "Windows", new List<Software>()));


            classrooms.Add(new Classroom("IC1", "Internet centar ucionica 1", 40, true, true, false, "Windows", l));
            classrooms.Add(new Classroom("JUG-112", "Jugodrvo ucionica 112", 60, true, true, false, "Windows", l));
            classrooms.Add(new Classroom("RC-6", "Racunarski centar ucionica 6", 40, true, false, false, "Linux", l));
        }
        private void AddEventHandlers()
        {
            itemList.SearchHandler += SearchWin;
            itemList.FilterHandler += FilterWin;
            itemList.AddClassroom += Add_Classroom;
            itemList.AddSubject += Add_Subject;
            itemList.AddCourse += Add_Course;
            itemList.AddSoftware += Add_Software;
            Closing += (a, b) => Environment.Exit(0);
        }

        private void FilterWin()
        {
            if(itemList.getComboboxText() == "Courses")
            {
                return;
            }

            fw = new FilterWindow(itemList.getComboboxText());
            fw.FilterHandler += Filter;
            fw.Subjects = subjects;
            fw.Courses = courses;
            fw.Software = software;
            fw.SetItemsSource(itemList.getComboboxText());
            fw.ShowDialog();
        }

        private void FilterWin(object sender, EventArgs e)
        {
            FilterWin();
        }

        private void Filter(object sender, EventArgs e)
        {
            fw.Hide();

            string what = fw.filterCBox.Text;
            string param = fw.otherCBox.Text;

            ObservableCollection<NameClass> filterRes = new ObservableCollection<NameClass>();
            ObservableCollection<Classroom> cfilterRes = new ObservableCollection<Classroom>();

            if (itemList.getComboboxText() == "Subjects")
            {
                if (what == "Course")
                {
                    foreach (Subject s in subjects)
                    {
                        if (s.Course.Name == param)
                        {
                            filterRes.Add(s);
                        }
                    }
                }
                else if (what == "Software")
                {
                    foreach (Subject s in subjects)
                    {
                        foreach (Software soft in s.Software)
                        {
                            if (soft.Name == param)
                            {
                                filterRes.Add(s);
                                break;
                            }
                        }
                    }
                }
                else if (what == "Board")
                {
                    bool needs = false;
                    if (param == "Yes")
                    {
                        needs = true;
                    }

                    foreach (Subject s in subjects)
                    {
                        if (s.Board == needs)
                        {
                            filterRes.Add(s);
                        }
                    }
                }
                else if (what == "Smart board")
                {
                    bool needs = false;
                    if (param == "Yes")
                    {
                        needs = true;
                    }

                    foreach (Subject s in subjects)
                    {
                        if (s.SmartBoard == needs)
                        {
                            filterRes.Add(s);
                        }
                    }
                }
                else if (what == "Projector")
                {
                    bool needs = false;
                    if (param == "Yes")
                    {
                        needs = true;
                    }

                    foreach (Subject s in subjects)
                    {
                        if (s.Projector == needs)
                        {
                            filterRes.Add(s);
                        }
                    }
                }


                if (filterRes.Count == 0 && (what == "Software" || what == "Course"))
                {
                    itemList.lableText.Text = "No matches found for: " + param;
                }
                else if (filterRes.Count > 0 && (what == "Software" || what == "Course"))
                {
                    itemList.lableText.Text = "Filter parameter: " + param;
                }
                else if (filterRes.Count == 0)
                {
                    itemList.lableText.Text = "No subject that fits: " + what.ToLower() + ", " + param.ToLower();
                }
                else
                {
                    itemList.lableText.Text = "Filter parameter: " + what.ToLower() + ", " + param.ToLower();
                }


                itemList.lv.ItemsSource = filterRes;
                itemList.lv3.ItemsSource = filterRes;
                Grid.SetRow(itemList.lv, 3);
                Grid.SetRow(itemList.lv3, 3);

            }
            else if (itemList.getComboboxText() == "Classrooms")
            {
                if (what == "Board")
                {
                    bool needs = false;
                    if (param == "Yes")
                    {
                        needs = true;
                    }

                    foreach (Classroom c in classrooms)
                    {
                        if (c.Board == needs)
                        {
                            cfilterRes.Add(c);
                        }
                    }
                }
                else if (what == "Smart board")
                {
                    bool needs = false;
                    if (param == "Yes")
                    {
                        needs = true;
                    }

                    foreach (Classroom c in classrooms)
                    {
                        if (c.SmartBoard == needs)
                        {
                            cfilterRes.Add(c);
                        }
                    }
                }
                else if (what == "Projector")
                {
                    bool needs = false;
                    if (param == "Yes")
                    {
                        needs = true;
                    }

                    foreach (Classroom c in classrooms)
                    {
                        if (c.Projector == needs)
                        {
                            cfilterRes.Add(c);
                        }
                    }
                }

                if (cfilterRes.Count == 0)
                {
                    itemList.lableText.Text = "No classrooms that fit: " + what.ToLower() + ", " + param.ToLower();
                }
                else
                {
                    itemList.lableText.Text = "Filter parameter: " + what.ToLower() + ", " + param.ToLower();
                }

                itemList.lv.Visibility = Visibility.Hidden;
                itemList.lv2.Visibility = Visibility.Visible;

                itemList.lv2.ItemsSource = cfilterRes;
                Grid.SetRow(itemList.lv2, 3);
                Grid.SetRow(itemList.lv3, 3);
            }
            else if (itemList.getComboboxText() == "Software")
            {
                foreach (Software s in software)
                {
                    if (s.OS == param)
                    {
                        filterRes.Add(s);
                    }
                }

                if (filterRes.Count == 0)
                {
                    itemList.lableText.Text = "No software that requires " + param.ToLower();
                }
                else
                {
                    itemList.lableText.Text = "Filter parameter: " + what.ToLower();
                }

                itemList.lv.ItemsSource = filterRes;
                Grid.SetRow(itemList.lv, 3);
                Grid.SetRow(itemList.lv3, 3);
            }


            itemList.Search.Visibility = Visibility.Hidden;
            itemList.Filter.Visibility = Visibility.Hidden;
            itemList.label.Visibility = Visibility.Visible;
            itemList.xButton.Visibility = Visibility.Visible;


            fw.Close();
        }

        private void SearchWin()
        {
            sw = new SearchWindow(itemList.getComboboxText());
            sw.SearchHandler += Search;
            sw.ShowDialog();
        }

        private void SearchWin(object sender, EventArgs e)
        {
            SearchWin();
        }

        private void Search(object sender, EventArgs e)
        {
            sw.Hide();

            string what = sw.What;
            string param = sw.searchBox.Text;

            ObservableCollection<NameClass> searchRes = new ObservableCollection<NameClass>();
            ObservableCollection<Classroom> cSearchRes = new ObservableCollection<Classroom>();

            if (what == "Subjects")
            {
                foreach (Subject s in subjects)
                {
                    if ((s.Name.ToLower() + s.ID.ToLower()).Contains(param.ToLower()))
                    {
                        searchRes.Add(s);
                    }
                }
            }
            else if (what == "Courses")
            {
                foreach (Course c in courses)
                {
                    if ((c.Name.ToLower() + c.ID.ToLower()).Contains(param.ToLower()))
                    {
                        searchRes.Add(c);
                    }
                }
            }
            else if (what == "Software")
            {
                foreach (Software s in software)
                {
                    if ((s.Name.ToLower() + s.ID.ToLower()).Contains(param.ToLower()))
                    {
                        searchRes.Add(s);
                    }
                }
            }
            else if (what == "Classrooms")
            {
                foreach (Classroom c in classrooms)
                {
                    if (c.ID.ToLower().Contains(param.ToLower()))
                    {
                        cSearchRes.Add(c);
                    }
                }
            }

            itemList.Search.Visibility = Visibility.Hidden;
            itemList.Filter.Visibility = Visibility.Hidden;
            itemList.label.Visibility = Visibility.Visible;
            itemList.xButton.Visibility = Visibility.Visible;



            if (what == "Classrooms")
            {
                itemList.lv2.ItemsSource = cSearchRes;
                Grid.SetRow(itemList.lv2, 3);
                Grid.SetRow(itemList.lv3, 3);
                itemList.lv.Visibility = Visibility.Hidden;
                itemList.lv2.Visibility = Visibility.Visible;
                if (cSearchRes.Count == 0)
                {
                    itemList.lableText.Text = "No matches found for: " + param;
                }
                else
                {
                    itemList.lableText.Text = "Search parameter: " + param;
                }
            }
            else
            {
                itemList.lv.ItemsSource = searchRes;
                if (what == "Subjects")
                {
                    itemList.lv3.ItemsSource = searchRes;
                }
                Grid.SetRow(itemList.lv, 3);
                Grid.SetRow(itemList.lv3, 3);
                itemList.lv.Visibility = Visibility.Visible;
                itemList.lv2.Visibility = Visibility.Hidden;
                if (searchRes.Count == 0)
                {
                    itemList.lableText.Text = "No matches found for: " + param;
                }
                else
                {
                    itemList.lableText.Text = "Search parameter: " + param;
                }
            }


            sw.Close();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            itemList.ChangePreview(itemList.getComboboxText());
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Schedules (*.sch)|*.sch";
            if (saveFileDialog.ShowDialog() == true)
            {
                _file = saveFileDialog.FileName;
                _file = _file.Remove(_file.Length - 4, 4);
                Console.WriteLine(_file);
                Table.SaveSchedule();
                SaveEntities();
            }
        }

        private void Load_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Schedules (*.sch)|*.sch";
            if (openFileDialog.ShowDialog() == true)
            {
                _file = openFileDialog.FileName;
                _file = _file.Remove(_file.Length - 4, 4);
                Console.WriteLine(_file);
                Table.LoadSchedule();
                LoadEntities();
            }
        }

        private void SaveEntities()
        {
            XmlSerializer xs = new XmlSerializer(typeof(EntityData));
            TextWriter tw = new StreamWriter(_file + ".ent");

            EntityData ser_data = new EntityData();

            ser_data.classrooms = classrooms.ToList<Classroom>();
            ser_data.courses = courses.ToList<Course>();
            ser_data.subjects = subjects.ToList<Subject>();
            ser_data.software = software.ToList<Software>();



            xs.Serialize(tw, ser_data);
            Console.WriteLine("Saved entities!");
        }

        private void LoadEntities()
        {
            XmlSerializer xs = new XmlSerializer(typeof(EntityData));
            using (var sr = new StreamReader(_file + ".ent"))
            {
                EntityData ser_data = (EntityData)xs.Deserialize(sr);

                courses = new ObservableCollection<Course>();
                foreach (var course in ser_data.courses)
                {
                    courses.Add(course);
                }
                classrooms = new ObservableCollection<Classroom>();
                foreach (var classroom in ser_data.classrooms)
                {
                    classrooms.Add(classroom);
                }
                subjects = new ObservableCollection<Subject>();
                foreach (var subject in ser_data.subjects)
                {
                    subjects.Add(subject);
                }
                software = new ObservableCollection<Software>();
                foreach (var sw in ser_data.software)
                {
                    software.Add(sw);
                }

                // stajiceva magija
                itemList.Courses = courses;
                itemList.Software = software;
                itemList.Subjects = subjects;
                itemList.lv.ItemsSource = itemList.Subjects;
                itemList.Classrooms = classrooms;
                itemList.lv2.ItemsSource = itemList.Classrooms;
                itemList.lv3.ItemsSource = subjects;

                // moja magija

                Table._subjects = ItemList._itemList.Subjects;
                Table._subjectsUI = ItemList._itemList.lv3;
            }
            Console.WriteLine("Loaded entities!");
        }

        private void Add_Classroom(object sender, EventArgs e)
        {
            aclassroomw.FillDataGrid();
            aclassroomw.Show();
        }

        private void Add_Subject(object sender, EventArgs e)
        {
            asubjectw.Refill();
            asubjectw.Show();
        }

        private void Add_Course(object sender, EventArgs e)
        {
            acoursew.Show();
        }

        private void Add_Software(object sender, EventArgs e)
        {
            asoftwarew.Show();
        }

        public static void AddClassroom(Classroom c)
        {
            _mainWindow.classrooms.Add(c);
        }

        public static void AddCourse(Course c)
        {
            _mainWindow.courses.Add(c);
        }

        public static void AddSoftware(Software s)
        {
            _mainWindow.software.Add(s);
        }

        public static void AddSubject(Subject s)
        {
            _mainWindow.subjects.Add(s);
        }

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void Demo_Mode_Clicked(object sender, RoutedEventArgs e)
        {
            DemoShowcase.StartDemo();
        }

        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show("Unsaved changes will be lost. Proceed?", "Warning", btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:

                    _mainWindow.NewSchedule();
                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

        private void NewSchedule()
        {

            Subjects = new ObservableCollection<Subject>();
            Courses = new ObservableCollection<Course>();
            Softwares = new ObservableCollection<Software>();
            Classrooms = new ObservableCollection<Classroom>();

 


            itemList.Courses = courses;
            itemList.Software = software;
            itemList.Subjects = subjects;
            itemList.Classrooms = classrooms;

            itemList.lv.ItemsSource = itemList.Subjects;
            itemList.lv2.ItemsSource = itemList.Classrooms;
            itemList.lv3.ItemsSource = subjects;


            table.ResetTable();
        }
    }
}
