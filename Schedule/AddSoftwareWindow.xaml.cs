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
using static Schedule.Model.Software;

namespace Schedule
{
    /// <summary>
    /// Interaction logic for AddSoftwareWindow.xaml
    /// </summary>
    public partial class AddSoftwareWindow : Window
    {

        public static string ops;

        public virtual string Ops
        { get { return ops; } set { ops = value; } }


        public AddSoftwareWindow()
        {
            InitializeComponent();
        }

        public void Add_software_click(object sender, EventArgs e)
        {
            string _id = id.Text.ToString();
            string name = n.Text.ToString();
            string maker = mak.Text.ToString();
            string website = web.Text.ToString();
            int year = Int32.Parse(y.Text.ToString());
            float price = float.Parse(p.Text.ToString());
            string des = desc.Text.ToString();

            //foreach (Model.Software el in MainWindow.software)
            //{
            //    if (el.ID.Equals(_id))
            //    {
            //        MessageBox.Show("id already exists !!!");
            //        return;
            //    }
            //}



            Model.Software s = new Model.Software(_id, name, ops, maker, website, year, price, des);

            MainWindow.AddSoftware(s);
            this.Hide();

        }


        public void Cancel_click(object sender, EventArgs e)
        {
            this.Hide();
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
                ops = "cross-platform";
            }

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            //Do whatever you want here..
        }
    }
}
