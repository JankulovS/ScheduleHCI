﻿using Schedule.Model;
using System;
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

namespace Schedule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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

        private static MainWindow _mainWindow;

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

        private void InitItems()
        {
            itemList = new ItemList();
            Grid.SetColumn(itemList, 0);
            grid.Children.Add(itemList);


            subjects = new ObservableCollection<Subject>();
            courses = new ObservableCollection<Course>();
            software = new ObservableCollection<Software>();
            classrooms = new ObservableCollection<Classroom>();

            //Privremeni podaci---------------------------------------------------------


            courses.Add(new Course("Prvi", "SIIT", new DateTime(), "opis"));
            courses.Add(new Course("Drugi", "E2", new DateTime(), "opis"));
            courses.Add(new Course("Treci", "PSI", new DateTime(), "opis"));

            software.Add(new Software("ID1", "Photoshop", "Windows", "Adobe", "www.newst.com", 2017, 2015, "opis"));
            software.Add(new Software("ID2", "Ilustrator", "Windows", "Adobe", "www.newst.com", 2017, 2015, "opis"));
            software.Add(new Software("ID3", "Visual Studio", "Windows", "Adobe", "www.newst.com", 2017, 2015, "opis"));

            List<Software> l = new List<Software>();
            l.Add(new Software("ID1", "Photoshop", "Windows", "Adobe", "www.newst.com", 2017, 2015, "opis"));

            subjects.Add(new Subject { ID = "Prvi", Name = "Interakcija covek racunar", Course = new Course("Prvi", "SIIT", new DateTime(), "opis"), Description = "asd", ClassLength = 1, NoOfClasses = 3, GroupSize = 3, Projector = true, Board = true, SmartBoard = true, OS = "Windows", Software = new List<Software>() });
            subjects.Add(new Subject("Drugi", "Metodologije razvoja softvera", new Course("Prvi", "E2", new DateTime(), "opis"), "asd", 1, 3, 2, 0,true, true, true, "Windows", l));
            subjects.Add(new Subject("Treci", "Pisana i govorna komunikacija u tehnici", new Course("Prvi", "PSI", new DateTime(), "opis"), "asd", 1, 3, 3, 0, true, true, true, "Windows", new List<Software>()));
            

            classrooms.Add(new Classroom("IC1", "Internet centar ucionica 1", 40, true, true, false, "Windows", l));
            classrooms.Add(new Classroom("JUG-112", "Jugodrvo ucionica 112", 60, true, true, false, "Windows", l));
            classrooms.Add(new Classroom("RC-6", "Racunarski centar ucionica 6", 40, true, false, false, "Linux", l));
            //-------------------------------------------------------------------------------
            itemList.Courses = courses;
            itemList.Software = software;
            itemList.Subjects = subjects;
            itemList.lv.ItemsSource = itemList.Subjects;
            itemList.Classrooms = classrooms;
            itemList.lv2.ItemsSource = itemList.Classrooms;
            itemList.lv3.ItemsSource = subjects;

        }
        private void AddEventHandlers()
        {
            itemList.SearchHandler += SearchWin;
            itemList.FilterHandler += FilterWin;
        }

        private void FilterWin()
        {
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
                    if (s.Name.ToLower().Contains(param.ToLower()))
                    {
                        searchRes.Add(s);
                    }
                }
            }
            else if (what == "Courses")
            {
                foreach (Course c in courses)
                {
                    if (c.Name.ToLower().Contains(param.ToLower()))
                    {
                        searchRes.Add(c);
                    }
                }
            }
            else if (what == "Software")
            {
                foreach (Software s in software)
                {
                    if (s.Name.ToLower().Contains(param.ToLower()))
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
            Table.SaveSchedule();
        }

        private void Load_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Table.LoadSchedule();
        }

        private void add_new_classroom_clicked(object sender, RoutedEventArgs e)
        {
            aclassroomw.Show();
        }

        private void add_new_subject_clicked(object sender, RoutedEventArgs e)
        {
            asubjectw.Show();
        }

        private void add_new_course_clicked(object sender, RoutedEventArgs e)
        {
            acoursew.Show();
        }

        private void add_new_software_clicked(object sender, RoutedEventArgs e)
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
    }
}
