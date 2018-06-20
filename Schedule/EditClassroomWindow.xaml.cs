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
    /// Interaction logic for EditClassroomWindow.xaml
    /// </summary>
    public partial class EditClassroomWindow : Window
    {
        private Classroom c;

        public Classroom C { get { return c; } set { c = value; } }

        public static int index;

        public EditClassroomWindow(Classroom obj,int i)
        {
            this.c = obj;
            index = i;
            InitializeComponent();
            FillDataGridSoftwares();
        }

        public void FillDataGridSoftwares()
        {
            var listItem = new List<SoftwareTableItem>();

            foreach (Model.Software s in MainWindow._mainWindow.Softwares)
            {
                bool belong = false;
                foreach(Software it in this.c.Software)
                {
                    if(s.ID == it.ID)
                    {
                        belong = true;
                        break;
                    }
                }

                listItem.Add(new SoftwareTableItem() { ID = s.ID, Name = s.Name, Os = s.OS, Maker = s.Maker, Website = s.Website,MyBool = belong });

            }

            kolekcija.ItemsSource = listItem;
        }


        private void ResetWindow()
        {
            this.id.Text = "";
            this.seats.Text = "";
            this.desc.Text = "";

            this.projector.IsChecked = false;
            this.board.IsChecked = false;
            this.smart_board.IsChecked = false;
            this.os1.IsChecked = false;
            this.os2.IsChecked = false;
        }

        private void Edit_Classroom(object sender, RoutedEventArgs e)
        {
            c.ID = this.id.Text;

            int b = 0;

            foreach (Model.Classroom el in MainWindow._mainWindow.Classrooms)
            {
                if (el.ID.Equals(c.ID) && b != index)
                {
                    MessageBox.Show("id already exists !!!");
                    ResetWindow();
                    this.Hide();
                    return;
                }
                b++;
            }



            c.NoOfSeats = Int32.Parse(this.seats.Text);
            c.Description = this.desc.Text;

            c.Projector = this.projector.IsChecked == true;
            c.Board = this.board.IsChecked == true;
            c.SmartBoard = this.smart_board.IsChecked == true;

            if(this.os1.IsChecked == true)
            {
                c.System = "windows";
            }

            if (this.os2.IsChecked == true)
            {
                c.System = "linux";
            }

            if (this.os1.IsChecked == true && this.os2.IsChecked == true)
            {
                c.System = "Windows/Linux";
            }



            int brojac = 0;
            c.Software = new List<Software>();
            foreach (var item in kolekcija.ItemsSource)
            {

                SoftwareTableItem i = (SoftwareTableItem)item;

                if (i.MyBool == true)
                {
                    c.Software.Add(MainWindow._mainWindow.Softwares[brojac]);
                }
                brojac++;
            }

            MainWindow._mainWindow.Classrooms.RemoveAt(index);

            MainWindow._mainWindow.Classrooms.Insert(index, this.c);

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
