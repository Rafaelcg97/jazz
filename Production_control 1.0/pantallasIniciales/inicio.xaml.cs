using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using LiveCharts;
using LiveCharts.Wpf;

namespace JazzCCO._0.pantallasIniciales
{
    public partial class inicio : UserControl
    {
        #region datosIniciales
        public inicio()
        {
            InitializeComponent();
            labelHora.Content = DateTime.Now.ToLongDateString();
            string nombre= System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            nombre = nombre.Substring(7,nombre.Length-7);
            labelUsuario.Content = nombre;
            ClrPcker_Background.SelectedColor = (Color)ColorConverter.ConvertFromString(ConfigurationManager.AppSettings["colorInicial"]);
            areaDeTrabajo.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ConfigurationManager.AppSettings["colorInicial"]);

            string curFile = ConfigurationManager.AppSettings["imagenUsuario"];
            if (File.Exists(curFile))
            {
                //agregar imagen de usuario
                try
                {
                    Uri fileUri = new Uri(ConfigurationManager.AppSettings["imagenUsuario"], UriKind.RelativeOrAbsolute);
                    imageUsuario.ImageSource = new BitmapImage(fileUri);
                }

                // si no encuentra la imagen se carga la imagen por defecto
                catch
                {
                }
            }
        }
        #endregion
        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "colorInicial") { node.Attributes[1].Value = ClrPcker_Background.SelectedColor.ToString(); }
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");
            areaDeTrabajo.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ConfigurationManager.AppSettings["colorInicial"]);
        }
        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.8 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void letraPequena(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.6 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void buttonUserImage_Click(object sender, RoutedEventArgs e)
        {
            string nombreAleatorio = "";

            //generar nombre aletorio con que se guardara la imagen
            for (int i = 1; i < 6; i++)
            {
                nombreAleatorio = Guid.NewGuid().ToString("n").Substring(0, 8);
            }

            //abrir explorador de archivos para buscar la imagen que usaremos, solo seran imagenes jpg o imagenes png
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "jpg|*.jpg|png|*.png";
            var result = fileDialog.ShowDialog();
            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    // si se encuentra una imagen y se selecciona esta se copia en la carpeta de imagenes publicas del equipo
                    var file = fileDialog.FileName;
                    string fileName = nombreAleatorio + file.ToString().Substring(file.ToString().Length - 4, 4);
                    string origen = file.ToString();
                    string destino = Environment.GetFolderPath(Environment.SpecialFolder.CommonPictures) + @"\jazzPicture\";

                    //si no existe la carpeta se crea
                    System.IO.Directory.CreateDirectory(destino);
                    //se copia la imagen
                    string destFile = System.IO.Path.Combine(destino, fileName);
                    System.IO.File.Copy(origen, destFile, true);

                    //se pone la imagen como foto de usuario
                    Uri fileUri = new Uri(destino + fileName, UriKind.RelativeOrAbsolute);
                    imageUsuario.ImageSource = new BitmapImage(fileUri);

                    //se deja guardada la ruta de la imagen para que se acceda a ella cada vez que se carga pantalla de inicio
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    foreach (XmlElement element in xmldoc.DocumentElement)
                    {
                        if (element.Name.Equals("appSettings"))
                        {
                            foreach (XmlNode node in element.ChildNodes)
                            {

                                if (node.Attributes[0].Value == "imagenUsuario") { node.Attributes[1].Value = destino + fileName; }
                            }
                        }
                    }
                    xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
                    ConfigurationManager.RefreshSection("appSettings");

                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    break;
            }
        }
    }
}
