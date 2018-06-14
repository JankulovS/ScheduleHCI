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
            FillDataGrid();
        }

        private void FillDataGrid()
        {
            var listItem = new List<SoftwareTableItem>();

            foreach (Model.Software s in MainWindow._mainWindow.Softwares)
            {
                listItem.Add(new SoftwareTableItem() { ID = s.ID, Name = s.Name, Os = s.OS, Maker = s.Maker, Website = s.Website });

            }

            kolekcija.ItemsSource = listItem;
        }


        public void Add_classroom_click(object sender, EventArgs e)
        {
            string _id = id.Text.ToString();
            int ns = Int32.Parse(seats.Text.ToString());
            string des = desc.Text.ToString();

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

            MainWindow.AddClassroom(c);

            ResetWindow();
            this.Hide();

        }

        private void ResetWindow()
        {
            id.Text = "";
            seats.Text = "";
            desc.Text = "";
            projector.IsChecked = false;
            board.IsChecked = false;
            smart_board.IsChecked = false;
            os1.IsChecked = false;
            os2.IsChecked = false;
        }

        public void Cancel_click(object sender, EventArgs e)
        {
            ResetWindow();
            this.Hide();
        }

        public void Add_software_Click(object sender, EventArgs e)
        {

            //ChooseSoftwareWindow w = new ChooseSoftwareWindow();
            //evo ne znam sta da radim majke mi

        }

        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            ResetWindow();
            this.Hide();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            ResetWindow();
            e.Cancel = true;
            this.Hide();
            //Do whatever you want here..
        }

    }
}
