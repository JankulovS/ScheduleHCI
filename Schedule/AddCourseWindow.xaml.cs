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
using static Schedule.Model.Course;

namespace Schedule
{
    /// <summary>
    /// Interaction logic for AddCourseWindow.xaml
    /// </summary>
    public partial class AddCourseWindow : Window
    {

        //public event EventHandler AddCourseHandler;


        public AddCourseWindow()
        {
            InitializeComponent();
        }


        public void Add_course(object sender, RoutedEventArgs e)
        {
            if (id.Text == "" || n.Text == "" || desc.Text == "" || d.Text == "")
            {
                MessageBox.Show("Mandatory fields are not filled.");
                return;
            }


            string _id = id.Text.ToString();
            string name = n.Text.ToString();
            DateTime date = DateTime.Parse( d.Text.ToString());
            string description = desc.Text.ToString();


            //foreach (Model.Course c in MainWindow.courses)
            //{
            //    if (c.ID.Equals(_id))
            //    {
            //        MessageBox.Show("id already exists !!!");
            //        return;
            //    }
            //}


            Model.Course course = new Model.Course(_id, name, date, description);

            MainWindow.AddCourse(course);
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

        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            ResetWindow();
            this.Hide();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            ResetWindow();
            e.Cancel = true;
            this.Hide();
            //Do whatever you want here..
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetWindow();
            this.Hide();
        }
    }
}
