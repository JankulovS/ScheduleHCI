using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for AddSubjectWindow.xaml
    /// </summary>
    public partial class AddSubjectWindow : Window
    {

        public static string ops;

        public virtual string Ops
        { get { return ops; } set { ops = value; } }

        public AddSubjectWindow()
        {
            InitializeComponent();
        }

        public void Add_subject_click(object sender, EventArgs e)
        {
            string _id = id.Text.ToString();
            string name = n.Text.ToString();
            int size_of_group = Int32.Parse(n_students.Text.ToString());
            int l = Int32.Parse(len.Text.ToString());
            int terms = Int32.Parse(n_terms.Text.ToString());
            string des = desc.Text.ToString();

            bool proj = false;
            bool b = false;
            bool sb = false;


            if (projector.IsChecked == true)
            {
                proj = true;
            }

            if (board.IsChecked == true)
            {
                b = true;
            }

            if (smart_board.IsChecked == true)
            {
                sb = true;
            }



            //foreach (Model.Subject el in MainWindow.subjects)
            //{
            //    if (el.ID.Equals(_id))
            //    {
            //        MessageBox.Show("id already exists !!!");
            //        return;
            //    }
            //}


            //!!!
            Model.Subject s = new Model.Subject(_id, name, new Model.Course(), des, size_of_group, l, terms, 0, proj, b, sb, ops);

            MainWindow.AddSubject(s);
            this.Hide();

        }


        public void Cancel_click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Radio_Click(object sender, RoutedEventArgs e)
        {
            if (win.IsChecked == true)
            {
                ops = "windows";
            }
            else if (lin.IsChecked == true)
            {
                ops = "linux";
            }
            else
            {
                ops = "others";
            }

        }


        public void Add_course_click(object sender, EventArgs e)
        {
            //AddCourseWindow w = new AddCourseWindow();
            //nisam pametan
        }

        public void Add_software_click(object sender, EventArgs e)
        {
            //AddSoftwareWindow w = new AddSoftwareWindow();
            // -||-
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            //Do whatever you want here..
        }

    }
}
