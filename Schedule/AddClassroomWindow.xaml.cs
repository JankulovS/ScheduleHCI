using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public void FillDataGrid()
        {
            var listItem = new List<SoftwareTableItem>();

            
            foreach (Model.Software s in MainWindow._mainWindow.Softwares)
            {
                listItem.Add(new SoftwareTableItem() { ID = s.ID, Name = s.Name, Os = s.OS, Maker = s.Maker, Website = s.Website });
                
            }

            //MessageBox.Show(listItem.Count.ToString());
            
            kolekcija.ItemsSource = listItem;

            
        }


        public void Add_classroom_click(object sender, EventArgs e)
        {
            if ((os1.IsChecked == false && os2.IsChecked == false) || desc.Text == "" || id.Text == "" || seats.Text == "")
            {
                MessageBox.Show("Mandatory fields are not filled.");
                return;
            }

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
                sys = "Windows/Linux";
            }


            foreach (Model.Classroom el in MainWindow._mainWindow.Classrooms)
            {
                if (el.ID.Equals(_id))
                {
                    MessageBox.Show("id already exists !!!");
                    //ResetWindow();
                    //this.Hide();
                    return;
                }
            }


            //!!!


            List<Model.Software> softveri = new List<Model.Software>();
            int brojac = 0;

            foreach (var item in kolekcija.ItemsSource)
            {

                SoftwareTableItem i = (SoftwareTableItem)item;

                if (i.MyBool == true)
                {

                    softveri.Add(MainWindow._mainWindow.Softwares[brojac]);
                }
                brojac++;
            }


            Model.Classroom c = new Model.Classroom(_id, des, ns, proj, b, sb, sys, softveri);

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


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetWindow();
            this.Hide();
        }
    }
}