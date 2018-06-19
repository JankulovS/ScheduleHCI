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


        public EditClassroomWindow(Classroom obj)
        {
            this.c = obj;
            InitializeComponent();
            FillDataGridSoftwares();
        }

        private void FillDataGridSoftwares()
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
                c.System = "windows, linux";
            }



            int brojac = 0;
            foreach (var item in kolekcija.ItemsSource)
            {

                SoftwareTableItem i = (SoftwareTableItem)item;

                c.Software = new List<Software>();

                if (i.MyBool == true)
                {
                    c.Software.Add(MainWindow._mainWindow.Softwares[brojac]);
                }
                brojac++;
            }

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
    }
}
