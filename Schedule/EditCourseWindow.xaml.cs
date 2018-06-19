using System;
using System.Collections.Generic;
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
using Schedule.Model;

namespace Schedule
{
    /// <summary>
    /// Interaction logic for EdidCourseWindow.xaml
    /// </summary>
    public partial class EditCourseWindow : Window
    {
        private Course c;

        public static int index;

        public Course C { get { return c; } set { c = value; } }


        public EditCourseWindow(Course obj,int i)
        {
            this.c = obj;
            index = i;
            InitializeComponent();
        }

        private void Edit_Course(object sender,RoutedEventArgs e)
        {
            this.c.ID = id.Text;
            this.c.Name = n.Text;
            this.c.Date = DateTime.Parse(d.Text);
            this.c.Description = desc.Text;

            MainWindow._mainWindow.Courses.RemoveAt(index);

            MainWindow._mainWindow.Courses.Insert(index,this.c);

            ResetWindow();
            this.Hide();
        }

        private void ResetWindow()
        {
            id.Text = "";
            n.Text = "";
            d.Text = "";
            desc.Text = "";
        }



        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            ResetWindow();
            this.Hide();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetWindow();
            this.Hide();
        }
    }
}
