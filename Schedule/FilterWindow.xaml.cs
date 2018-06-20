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
using System.Windows.Shapes;

namespace Schedule
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public partial class FilterWindow : Window
    {
        private ObservableCollection<Subject> subjects;
        private ObservableCollection<Course> courses;
        private ObservableCollection<Software> software;
        internal ObservableCollection<Subject> Subjects { get => subjects; set => subjects = value; }
        internal ObservableCollection<Course> Courses { get => courses; set => courses = value; }
        internal ObservableCollection<Software> Software { get => software; set => software = value; }

        private int i;
        private string v;

        public event EventHandler FilterHandler;

        ObservableCollection<ComboBoxItem> l;
        public ObservableCollection<ComboBoxItem> list;

        public FilterWindow(string v)
        {
            i = 1;
            InitializeComponent();
            i = 2;
            this.v = v;
            InitParams();

        }

        private void InitParams()
        {

            filterLabel.Content = "                     Filter " + v.ToLower() + " by:";

            l = new ObservableCollection<ComboBoxItem>();
            l.Add(new ComboBoxItem { Content = "Yes" });
            l.Add(new ComboBoxItem { Content = "No" });

            list = new ObservableCollection<ComboBoxItem>();
            if (v == "Subjects")
            {
                list.Add(new ComboBoxItem { Content = "Course" });
                list.Add(new ComboBoxItem { Content = "Software" });
                list.Add(new ComboBoxItem { Content = "Projector" });
                list.Add(new ComboBoxItem { Content = "Board" });
                list.Add(new ComboBoxItem { Content = "Smart board" });
                otherLabel.Content = "                     Choose a " + filterCBox.Text.ToLower() + ":";
            }
            else if (v == "Classrooms")
            {
                list.Add(new ComboBoxItem { Content = "Projector" });
                list.Add(new ComboBoxItem { Content = "Board" });
                list.Add(new ComboBoxItem { Content = "Smart board" });
                otherLabel.Content = "                     Classroom has a " + filterCBox.Text.ToLower() + ":";
            }
            else if (v == "Software")
            {
                filterCBox.Visibility = Visibility.Hidden;
                filterLabel.Visibility = Visibility.Hidden;
                otherLabel.Content = "              Filter " + v.ToLower() + " by operating sistem:";
                Grid.SetRow(otherLabel, 2);
                Grid.SetRow(otherCBox, 3);

                list.Add(new ComboBoxItem { Content = "Windows" });
                list.Add(new ComboBoxItem { Content = "Linux" });
                list.Add(new ComboBoxItem { Content = "Windows/Linux" });

                otherCBox.ItemsSource = list;
                otherCBox.SelectedIndex = 0;
            }
            filterCBox.ItemsSource = list;
            filterCBox.SelectedIndex = 0;
        }
        internal void SetItemsSource(string v)
        {
            if (v == "Subjects")
            {
                otherCBox.ItemsSource = courses;
                otherCBox.SelectedIndex = 0;
            }
            else if (v == "Classrooms")
            {
                otherCBox.ItemsSource = l;
                otherCBox.SelectedIndex = 0;
            }
        }

        private void filterCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (i == 1)
            {
                return;
            }

            string text = (e.AddedItems[0] as ComboBoxItem).Content as string;

            if (v == "Subjects")
            {
                if (text == "Projector" || text == "Board" || text == "Smart board")
                {
                    otherLabel.Content = "                     Subject needs a " + text.ToLower() + ":";
                }
                else
                {
                    otherLabel.Content = "                     Choose a " + text.ToLower() + ":";
                }
            }
            else if (v == "Classrooms")
            {
                otherLabel.Content = "                     Classroom has a " + text.ToLower() + ":";
            }

            if (text == "Course")
            {
                otherCBox.ItemsSource = courses;
                otherCBox.SelectedIndex = 0;
            }
            else if (text == "Software")
            {
                otherCBox.ItemsSource = software;
                otherCBox.SelectedIndex = 0;
            }
            else if (text == "Projector" || text == "Board" || text == "Smart board")
            {
                otherCBox.ItemsSource = l;
                otherCBox.SelectedIndex = 0;
            }
        }

        private void filterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterHandler(this, EventArgs.Empty);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

    }
}
