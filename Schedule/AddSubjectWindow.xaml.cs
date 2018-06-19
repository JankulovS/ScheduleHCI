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
using System.Windows.Shapes;
using Schedule.Model;

namespace Schedule
{
    /// <summary>
    /// Interaction logic for AddSubjectWindow.xaml
    /// </summary>
    public partial class AddSubjectWindow : Window
    {

        public static string ops;

        public static List<String> smerovi;

        public virtual string Ops
        { get { return ops; } set { ops = value; } }

        public AddSubjectWindow()
        {
            smerovi = new List<string>();
            InitializeComponent();
            FillDataGridSoftwares();
            FillComboBoxCourses();

        }


        private void FillDataGridSoftwares()
        {
            var listItem = new List<SoftwareTableItem>();

            foreach (Model.Software s in MainWindow._mainWindow.Softwares)
            {
                listItem.Add(new SoftwareTableItem() { ID = s.ID, Name = s.Name, Os = s.OS, Maker = s.Maker, Website = s.Website });

            }

            soft.ItemsSource = listItem;
        }

        private void FillComboBoxCourses()
        {

            foreach (Model.Course c in MainWindow._mainWindow.Courses)
            {
                smerovi.Add(c.Name);
            }

            smer.ItemsSource = smerovi;


        }

        public void Add_subject_click(object sender, EventArgs e)
        {
            if (desc.Text == "" || id.Text == "" || n.Text == "" || n_students.Text == "" || n_terms.Text == "" || len.Text == "")
            {
                MessageBox.Show("Mandatory fields are not filled.");
                return;
            }


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

            List<Model.Software> softveri = new List<Model.Software>();
            int brojac = 0;

            foreach (var item in soft.ItemsSource)
            {

                SoftwareTableItem i = (SoftwareTableItem)item;

                if (i.MyBool == true)
                {

                    softveri.Add(MainWindow._mainWindow.Softwares[brojac]);
                }
                brojac++;
            }


            brojac = 0;
            Model.Course c = new Model.Course();
            foreach(string str in smerovi)
            {
                if(smer.ToString().Equals(smer))
                {
                    c = MainWindow._mainWindow.Courses[brojac];
                }
                brojac++;
            }


            Model.Subject s = new Model.Subject(_id, name, c, des, size_of_group, l, terms, 0, proj, b, sb, ops, softveri);

            MainWindow.AddSubject(s);
            ResetWindow();
            this.Hide();

        }

        private void ResetWindow()
        {
            id.Text = "";
            n.Text = "";
            n_students.Text = "";
            len.Text = "";
            n_terms.Text = "";
            desc.Text = "";

            projector.IsChecked = false;
            board.IsChecked = false;
            smart_board.IsChecked = false;
        }


        public void Cancel_click(object sender, EventArgs e)
        {
            ResetWindow();
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
