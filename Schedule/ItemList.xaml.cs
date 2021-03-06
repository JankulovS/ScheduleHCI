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
    /// Interaction logic for ItemList.xaml
    /// </summary>
    /// 



    public partial class ItemList : UserControl
    {
        private ObservableCollection<Subject> subjects;
        private ObservableCollection<Subject> allSubjects;
        private ObservableCollection<Course> courses;
        private ObservableCollection<Software> software;
        private ObservableCollection<Classroom> classrooms;

        internal ObservableCollection<Classroom> Classrooms { get => classrooms; set => classrooms = value; }
        internal ObservableCollection<Subject> Subjects { get => subjects; set => subjects = value; }
        internal ObservableCollection<Subject> AllSubjects { get => allSubjects; set => allSubjects = value; }
        internal ObservableCollection<Course> Courses { get => courses; set => courses = value; }
        internal ObservableCollection<Software> Software { get => software; set => software = value; }

        private int i;
        Point startPoint = new Point();

        public static ItemList _itemList;

        public event EventHandler SearchHandler;
        public event EventHandler FilterHandler;
        public event EventHandler AddClassroom;
        public event EventHandler AddSubject;
        public event EventHandler AddCourse;
        public event EventHandler AddSoftware;

        private string text = "Subjects";
        public Classroom selectedClassroom;


        public ItemList()
        {
            i = 1;
            InitializeComponent();
            i = 2;
            _itemList = this;
            this.DataContext = this;
            selectedClassroom = null;
        }

        // DRAG AND DROP
        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);

                // Find the data behind the ListViewItem
                Subject subject = null;
                try
                {
                    subject = (Subject)listView.ItemContainerGenerator.
                    ItemFromContainer(listViewItem);
                }
                catch (Exception)
                {
                    return;
                }

                // Initialize the drag & drop operation
                DataObject dragData = new DataObject("myFormat", subject);
                Table._candidate = subject;
                Table._subjects = Subjects;
                Table._subjectsUI = lv3;
                DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }


        // END DRAG AND DROP


        public string getComboboxText()
        {
            return Cbox.Text;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (i == 1)
            {
                return;
            }

            text = (e.AddedItems[0] as ComboBoxItem).Content as string;

            ChangePreview(text);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchHandler(this, EventArgs.Empty);
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if (getComboboxText() == "Courses")
            {
                return;
            }
            FilterHandler(this, EventArgs.Empty);
        }

        private void xButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePreview(getComboboxText());
        }


        public void ChangePreview(string text)
        {
            if (text == "Subjects")
            {
                lv.ItemsSource = Subjects;
                lv3.ItemsSource = Subjects;
                lv.Visibility = Visibility.Visible;
                lv2.Visibility = Visibility.Hidden;
                Filter.IsEnabled = true;

                lv3.Visibility = Visibility.Visible;
                Grid.SetColumnSpan(lv, 3);
            }
            else if (text == "Courses")
            {
                lv.ItemsSource = courses;
                lv.Visibility = Visibility.Visible;
                lv2.Visibility = Visibility.Hidden;
                Filter.IsEnabled = false;

                lv3.Visibility = Visibility.Hidden;
                Grid.SetColumnSpan(lv, 4);
            }
            else if (text == "Software")
            {
                lv.ItemsSource = software;
                lv.Visibility = Visibility.Visible;
                lv2.Visibility = Visibility.Hidden;
                Filter.IsEnabled = true;

                lv3.Visibility = Visibility.Hidden;
                Grid.SetColumnSpan(lv, 4);

            }
            else if (text == "Classrooms")
            {
                lv2.ItemsSource = classrooms;
                lv.Visibility = Visibility.Hidden;
                lv2.Visibility = Visibility.Visible;
                Filter.IsEnabled = true;

                lv3.Visibility = Visibility.Hidden;
                Grid.SetColumnSpan(lv, 4);
            }

            Search.Visibility = Visibility.Visible;
            Filter.Visibility = Visibility.Visible;
            label.Visibility = Visibility.Hidden;
            xButton.Visibility = Visibility.Hidden;

            Grid.SetRow(lv, 2);
            Grid.SetRow(lv2, 2);
            Grid.SetRow(lv3, 2);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView list = sender as ListView;
            Classroom classroom = list.SelectedItem as Classroom;
            //Console.WriteLine(classroom.ID);
            try
            {
                Table.ChangeClassroomLabel(classroom.ID);
                selectedClassroom = classroom;
                SetSubjects();

            }
            catch (Exception)
            {
                return;
            }
        }

        private void SetSubjects(object sender, EventArgs e)
        {
            SetSubjects();
        }

        public void SetSubjects()
        {
            if (selectedClassroom == null)
            {
                subjects.Clear();
                foreach (Subject s in allSubjects)
                {
                    subjects.Add(s);
                }
                return;
            }
            subjects.Clear();
            bool add = false;
            foreach (Subject s in allSubjects)
            {
                add = false;
                if (s.Board == true)
                {
                    if (!selectedClassroom.Board)
                    {
                        continue;
                    }
                }

                if (s.SmartBoard == true)
                {
                    if (!selectedClassroom.SmartBoard)
                    {
                        continue;
                    }
                }

                if (s.Projector == true)
                {
                    if (!selectedClassroom.Projector)
                    {
                        continue;
                    }
                }

                if (selectedClassroom.System.ToLower() != s.OS.ToLower() && selectedClassroom.System.ToLower() != "windows/linux")
                {
                    continue;
                }


                foreach (Software ss in s.Software)
                {
                    foreach (Software cs in selectedClassroom.Software)
                    {
                        add = false;
                        if (ss.ID.ToLower() == ss.ID.ToLower())
                        {
                            add = true;
                        }
                        if (!add)
                        {
                            break;
                        }
                    }
                    if (!add)
                    {
                        break;
                    }
                }
                if (s.Software.Count == 0)
                {
                    add = true;
                }
                if (!add)
                {
                    continue;
                }

                subjects.Add(s);

            }
            lv.ItemsSource = subjects;
        }

        private void Add_Classroom(object sender, RoutedEventArgs e)
        {
            AddClassroom(this, EventArgs.Empty);
        }

        private void Add_Subject(object sender, RoutedEventArgs e)
        {
            AddSubject(this, EventArgs.Empty);
        }

        private void Add_Course(object sender, RoutedEventArgs e)
        {
            AddCourse(this, EventArgs.Empty);
        }

        private void Add_Software(object sender, RoutedEventArgs e)
        {
            AddSoftware(this, EventArgs.Empty);
        }

        private void Delete()
        {
            if (text == "Subjects")
            {
                if (subjects.ElementAt(lv.SelectedIndex).NoOfClassesSet > 0)
                {
                    MessageBox.Show("The subject is in use.");
                    return;
                }
                //MainWindow._mainWindow.Subjects.RemoveAt(lv.SelectedIndex);
                subjects.RemoveAt(lv.SelectedIndex);
                lv3.ItemsSource = subjects;
            }
            else if (text == "Courses")
            {
                bool check = false;
                foreach(Subject s in subjects)
                {
                    foreach(Software ss in s.Software)
                    {
                        if(ss.ID == software.ElementAt(lv.SelectedIndex).ID)
                        {
                            check = true;
                            break;
                        }
                    }
                    if (check)
                    {
                        break;
                    }
                }
                if (check)
                {
                    MessageBox.Show("Software is in use.");
                    return;
                }
                MainWindow._mainWindow.Courses.RemoveAt(lv.SelectedIndex);

            }
            else if (text == "Software")
            {
                MainWindow._mainWindow.Softwares.RemoveAt(lv.SelectedIndex);

            }
            else if (text == "Classrooms")
            {
                MainWindow._mainWindow.Classrooms.RemoveAt(lv2.SelectedIndex);

            }
        }

        private void Delete(object sender, EventArgs e)
        {
            Delete();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

            if (text == "Courses")
            {
                Edit_course(lv.SelectedIndex);
            }else if(text == "Software")
            {
                Edit_Software(lv.SelectedIndex);
            }else if(text == "Classrooms")
            {
                Edit_Classroom(lv2.SelectedIndex);
            }else if(text == "Subjects")
            {
                Edit_Subject(lv.SelectedIndex);
            }

            
        }

        private void Edit_course(int index)
        {
            Course c = MainWindow._mainWindow.Courses[index];
            EditCourseWindow w = new EditCourseWindow(c, index);

            w.id.Text = c.ID;
            w.n.Text = c.Name;
            w.desc.Text = c.Description;
            w.d.Text = c.Date.ToString();



            w.Show();
        }

        private void Edit_Software(int index)
        {
            Software s = MainWindow._mainWindow.Softwares[index];
            EditSoftwareWindow w = new EditSoftwareWindow(s, index);

            w.id.Text = s.ID;
            w.n.Text = s.Name;
            w.mak.Text = s.Maker;
            w.web.Text = s.Website;
            w.y.Text = s.Year.ToString();
            w.p.Text = s.Price.ToString();
            w.desc.Text = s.Description;


            if (s.OS.ToLower() == "windows")
            {
                w.win.IsChecked = true;
            }
            else if (s.OS.ToLower() == "linux")
            {
                w.lin.IsChecked = true;
            }
            else
            {
                s.OS.ToLower();
                w.cp.IsChecked = true;
            }
            w.Show();
        }


        private void Edit_Classroom(int index)
        {
            Classroom c = MainWindow._mainWindow.Classrooms[index];
            EditClassroomWindow w = new EditClassroomWindow(c, index);

            w.id.Text = c.ID;
            w.seats.Text = c.NoOfSeats.ToString();
            w.desc.Text = c.Description;

            w.FillDataGridSoftwares();

            if (c.Projector) {
                w.projector.IsChecked = true;
            }

            if (c.SmartBoard)
            {
                w.smart_board.IsChecked = true;
            }

            if (c.Board)
            {
                w.board.IsChecked = true;
            }

            if(c.System.ToLower() == "windows")
            {
                w.os1.IsChecked = true;
            }

            if (c.System.ToLower() == "linux")
            {
                w.os2.IsChecked = true;
            }

            if (c.System.ToLower() == "windows/linux")
            {
                w.os1.IsChecked = true;
                w.os2.IsChecked = true;
            }

            w.Show();
            w.edit += SetSubjects;
        }

        private void Edit_Subject(int index)
        {
            Subject s = MainWindow._mainWindow.Subjects[index];

            EditSubjectWindow w = new EditSubjectWindow(s, index);

            w.id.Text = s.ID;
            w.n.Text = s.Name;
            w.n_students.Text = s.GroupSize.ToString();
            w.len.Text = s.ClassLength.ToString();
            w.n_terms.Text = s.NoOfClasses.ToString();
            w.desc.Text = s.Description;

            w.projector.IsChecked = s.Projector == true;
            w.board.IsChecked = s.Board == true;
            w.smart_board.IsChecked = s.SmartBoard == true;

            w.Refill();

            if(s.OS.ToLower() == "windows")
            {
                w.win.IsChecked = true;
            }else if(s.OS.ToLower() == "linux")
            {
                w.lin.IsChecked = true;
            }
            else if(s.OS.ToLower() == "windows/linux")
            {
                w.cp.IsChecked = true;
            }
            w.Show();

            w.edit += SetSubjects;
        }


    }
}
