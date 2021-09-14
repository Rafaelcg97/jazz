using java.nio.file;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace Production_control_1._0
{
    public partial class splashScreen : Window
    {
        DispatcherTimer dt = new DispatcherTimer();

        public splashScreen()
        {
            InitializeComponent();
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0,0,5);
            dt.Start();
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            MainWindow inicio = new MainWindow();
            inicio.Show();

            dt.Stop();
            this.Close();
        }
    }
}
