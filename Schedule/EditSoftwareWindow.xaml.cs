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
    /// Interaction logic for EditSoftwareWindow.xaml
    /// </summary>
    public partial class EditSoftwareWindow : Window
    {
        private Software s;

        public static int index;

        public Software S { get { return s; } set { s = value; } }

        public EditSoftwareWindow(Software obj,int i)
        {
            this.s = obj;
            index = i;
            InitializeComponent();
        }

        private void Edit_Software(object sender, RoutedEventArgs e)
        {
            this.s.ID = id.Text;
            this.s.Name = n.Text;
            this.s.Maker = mak.Text;
            this.s.Website = web.Text;
            this.s.Year = Int32.Parse(y.Text);
            this.s.Price = float.Parse(p.Text);
            this.s.Description = desc.Text;

            if (win.IsChecked == true){
                this.s.OS = "windows";
            }else if(lin.IsChecked == true){
                this.s.OS = "linux";
            }else{
                this.s.OS = "cross-platform";
            }

            MainWindow._mainWindow.Softwares.RemoveAt(index);

            MainWindow._mainWindow.Softwares.Insert(index, this.s);
            ResetWindow();
            this.Hide();
        }

        private void ResetWindow()
        {
            id.Text = "";
            n.Text = "";
            mak.Text = "";
            web.Text = "";
            y.Text = "";
            p.Text = "";
            desc.Text = "";
        }


        private void Cancel_click(object sender, RoutedEventArgs e)
        {
            ResetWindow();
            this.Hide();
        }

        private void YearValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^(19|20)[0-9][0-9]");
            e.Handled = regex.IsMatch(e.Text);
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
