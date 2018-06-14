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
            string _id = id.ToString();
            string name = n.ToString();
            string date = d.ToString();
            string description = desc.ToString();


            //foreach (Model.Course c in MainWindow.courses)
            //{
            //    if (c.ID.Equals(_id))
            //    {
            //        MessageBox.Show("id already exists !!!");
            //        return;
            //    }
            //}


            //Model.Course course = new Model.Course(_id, name, date, description);

            //MainWindow.courses.Add(course);

            this.Hide();


        }

        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            //Do whatever you want here..
        }
    }
}
