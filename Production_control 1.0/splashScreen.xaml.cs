using System;
using System.Windows;
using System.Windows.Threading;

namespace JazzCCO._0
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
