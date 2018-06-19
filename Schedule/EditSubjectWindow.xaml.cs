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
using System.Text.RegularExpressions;

namespace Schedule
{
    /// <summary>
    /// Interaction logic for EditSubjectWindow.xaml
    /// </summary>
    public partial class EditSubjectWindow : Window
    {
        private Subject s;

        public static int index;

        public Subject S { get { return s; } set { s = value; } }

        public static List<String> smerovi;

        public static int indeks_smera;


        public EditSubjectWindow(Subject obj,int i)
        {
            smerovi = new List<string>();
            this.s = obj;
            index = i;
            InitializeComponent();
            FillDataGridSoftwares();
            FillComboBoxCourses();
        }

        public void Refill()
        {
            FillDataGridSoftwares();
            FillComboBoxCourses();
        }

        private void FillDataGridSoftwares()
        {
            var listItem = new List<SoftwareTableItem>();

            foreach (Model.Software s in MainWindow._mainWindow.Softwares)
            {
                bool belong = false;
                foreach (Software it in this.s.Software)
                {
                    if (s.ID == it.ID)
                    {
                        belong = true;
                        break;
                    }
                }

                listItem.Add(new SoftwareTableItem() { ID = s.ID, Name = s.Name, Os = s.OS, Maker = s.Maker, Website = s.Website, MyBool = belong });

            }

            soft.ItemsSource = listItem;
        }

        private void FillComboBoxCourses()
        {
            smerovi = new List<string>();

            int brojac = 0;
            foreach (Model.Course c in MainWindow._mainWindow.Courses)
            {
                smerovi.Add(c.Name);
                if(this.s.Course.Name == c.Name)
                {
                    smer.SelectedIndex = brojac;
                    indeks_smera = brojac;
                }
                brojac++;
            }

            smer.ItemsSource = smerovi;
            

        }




        private void ResetWindow()
        {
            this.id.Text = "";
            this.n.Text = "";
            this.n_students.Text = "";
            this.len.Text = "";
            this.n_terms.Text = "";
            this.desc.Text = "";

            this.projector.IsChecked = false;
            this.board.IsChecked = false;
            this.smart_board.IsChecked = false;
            

            this.win.IsChecked = true;


        }


        private void Edit_Subject(object sender, RoutedEventArgs e)
        {
            this.s.ID = this.id.Text.Trim();
            int b = 0;
            foreach (Model.Subject el in MainWindow._mainWindow.Subjects)
            {
                if (el.ID.Equals(this.s.ID) && b != index)
                {
                    MessageBox.Show("id already exists !!!");
                    ResetWindow();
                    this.Hide();
                    return;
                }
                b++;
            }







            this.s.Name = this.n.Text.Trim();
            this.s.GroupSize = Int32.Parse(this.n_students.Text);
            this.s.ClassLength = Int32.Parse(this.len.Text);
            this.s.NoOfClasses = Int32.Parse(this.n_terms.Text);
            this.s.Description = this.desc.Text;

            this.s.Projector = this.projector.IsChecked == true;
            this.s.Board = this.board.IsChecked == true;
            this.s.SmartBoard = this.smart_board.IsChecked == true;

            if(this.win.IsChecked == true)
            {
                this.s.OS = "windows";
            }else if(this.lin.IsChecked == true)
            {
                this.s.OS = "linux";
            }
            else
            {
                this.s.OS = "Windows/Linux";
            }

            int brojac = 0;
            s.Software = new List<Software>();
            foreach (var item in soft.ItemsSource)
            {
                SoftwareTableItem i = (SoftwareTableItem)item;

                if (i.MyBool == true)
                {
                    s.Software.Add(MainWindow._mainWindow.Softwares[brojac]);
                }
                brojac++;
            }
            indeks_smera = smer.SelectedIndex;
            
            this.s.Course = MainWindow._mainWindow.Courses[indeks_smera];

            MainWindow._mainWindow.Subjects.RemoveAt(index);

            MainWindow._mainWindow.Subjects.Insert(index, this.s);


            ResetWindow();
            this.Hide();

        }

        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            ResetWindow();
            this.Hide();
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
