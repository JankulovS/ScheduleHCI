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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Schedule
{
    /// <summary>
    /// Interaction logic for AddClassroomWindow.xaml
    /// </summary>
    public partial class AddClassroomWindow : Window
    {
        public AddClassroomWindow()
        {
            InitializeComponent();
        }

        public void Add_classrom_click(object sender, EventArgs e)
        {
            string _id = id.ToString();
            int ns = Int32.Parse(seats.ToString());
            string des = desc.ToString();

            bool proj = false;
            bool b = false;
            bool sb = false;
            string sys = "";


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


            if (os1.IsChecked == true)
            {
                sys = "windows";
            }

            if (os2.IsChecked == true)
            {
                sys = "linux";
            }

            if (os1.IsChecked == true && os2.IsChecked == true)
            {
                sys = "windows, linux";
            }


            //foreach (Model.Classroom el in MainWindow.classrooms)
            //{
            //    if (el.ID.Equals(_id))
            //    {
            //        MessageBox.Show("id already exists !!!");
            //        return;
            //    }
            //}


            //!!!
            Model.Classroom c = new Model.Classroom(_id, des, ns, proj, b, sb, sys, new List<Model.Software>());

            //MainWindow.classrooms.Add(c);
            this.Close();

        }


        public void Cancel_click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Add_software_Click(object sender, EventArgs e)
        {

            //ChooseSoftwareWindow w = new ChooseSoftwareWindow();
            //evo ne znam sta da radim majke mi

        }

        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            //Do whatever you want here..
        }
    }
}
