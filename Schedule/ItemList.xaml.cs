using Schedule.Model;
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
        private ObservableCollection<Course> courses;
        private ObservableCollection<Software> software;
        private ObservableCollection<Classroom> classrooms;

        internal ObservableCollection<Classroom> Classrooms { get => classrooms; set => classrooms = value; }
        internal ObservableCollection<Subject> Subjects { get => subjects; set => subjects = value; }
        internal ObservableCollection<Course> Courses { get => courses; set => courses = value; }
        internal ObservableCollection<Software> Software { get => software; set => software = value; }

        private int i;
        Point startPoint = new Point();

        public event EventHandler SearchHandler;
        public event EventHandler FilterHandler;
        
        public ItemList()
        {
            i = 1;
            InitializeComponent();
            i = 2;
            this.DataContext = this;
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

            string text = (e.AddedItems[0] as ComboBoxItem).Content as string;

            ChangePreview(text);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SearchHandler(this, EventArgs.Empty);
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
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
            Table.ChangeClassroomLabel(classroom.ID);
        }
    }
}
