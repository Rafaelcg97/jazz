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
using System.Configuration;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Printing;
using System.Drawing.Printing;
using NHibernate.Impl;

namespace Production_control_1._0
{
    public partial class impresion : Page
    {
        #region clases_especiales
        private PrintDocument printDoc = new PrintDocument();
        #endregion

        #region datos_iniciales
        public impresion()
        {
            InitializeComponent();
            //agregar la lista de impresoras instaladas
            string impresora_instalada;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                impresora_instalada = PrinterSettings.InstalledPrinters[i];
                impresora.Items.Add(impresora_instalada);
                if (printDoc.PrinterSettings.IsDefaultPrinter)
                {
                    impresora.SelectedItem = printDoc.PrinterSettings.PrinterName;
                }
            }

            //agregar orientaciones de paginas
            orientacion_impresion.Items.Add("Horizontal");
            orientacion_impresion.Items.Add("Vertical");

            //agregar tamaños de paginas
            tamano_impresion.Items.Add("Tabloide");
            tamano_impresion.Items.Add("Carta");
            tamano_impresion.Items.Add("Oficio");

            //establecer orientacion predeterminada
            orientacion_impresion.SelectedItem = "Horizontal";

            //establecer tamaño predeterminado
            tamano_impresion.SelectedItem="Tabloide";

            //numero de copias
            copias.Text = "1";

            cargar_layout();
            cargar_image();

        }

        #endregion

        #region tamanos_de_letra_/_tipo_de_texto

        private void letra_pequena(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.9 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }

        private void letra_pequena_2(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.4 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }

        private void letra_pequena_3(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.7 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }

        private void letra_list_box(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.2 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }

        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }
        #endregion

        #region control_general_Del_programa

        private void salir_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        #endregion

        #region formulario_impresion
        private void confirmar_impresion_Click(object sender, RoutedEventArgs e)
        {
            //varibales para hacer el switch de los tamaños de pagina y orientaciones de pagina
            string tamano_pagina=tamano_impresion.SelectedItem.ToString();
            string orientacion_pagina = tamano_impresion.SelectedItem.ToString();
            PageMediaSize tamano;
            PageOrientation orientacion;


            //configurar el valor del tamño de pagina
            switch (tamano_pagina)
                 {
                case "Carta":
                    tamano = new PageMediaSize(PageMediaSizeName.NorthAmericaLetter);
                    break;
                case "Oficio":
                    tamano = new PageMediaSize(PageMediaSizeName.NorthAmericaLegal);
                    break;
                case "Tabloide":
                    tamano = new PageMediaSize(PageMediaSizeName.NorthAmericaTabloid);
                    break;
                default:
                    tamano = new PageMediaSize(PageMediaSizeName.NorthAmericaTabloid);
                    break;
            };

            //configurar el valor de orientacion de pagina
            switch (orientacion_pagina)
            {
                case "Horizontal":
                   orientacion = PageOrientation.Landscape;
                    break;
                case "Vertical":
                    orientacion = PageOrientation.Portrait;
                    break;
                default:
                    orientacion= PageOrientation.Landscape;
                    break;
            };

            //establecer las propiedades de impresion

            try
            {
            PrintDialog dialog = new PrintDialog();
            dialog.PrintTicket.PageOrientation = orientacion;
            dialog.PrintTicket.PageBorderless = PageBorderless.None;
            dialog.PrintTicket.PageMediaSize = tamano;
            dialog.PrintTicket.CopyCount = Convert.ToInt32(copias.Text);
            dialog.PrintQueue = new PrintQueue(new PrintServer(), impresora.SelectedItem.ToString());
            dialog.PrintVisual(area_de_impresion, "LayOut");
            }
            catch
            {
                MessageBox.Show("No se reconoce la impresora o el número de copias es invalido");
            }
        }

        private void aumentar_copias_Click(object sender, RoutedEventArgs e)
        {
            copias.Text = (Convert.ToInt32(copias.Text) + 1).ToString();
        }

        private void disminuir_copias_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(copias.Text) > 1)
            {
                copias.Text = (Convert.ToInt32(copias.Text) - 1).ToString();
            }
        }

        #endregion

        #region calculos_generales

        private void cargar_layout()
        {
            #region maquinas
            mp1.Content = impresion_global.maquina_1;
            mp2.Content = impresion_global.maquina_2;
            mp3.Content = impresion_global.maquina_3;
            mp4.Content = impresion_global.maquina_4;
            mp5.Content = impresion_global.maquina_5;
            mp6.Content = impresion_global.maquina_6;
            mp7.Content = impresion_global.maquina_7;
            mp8.Content = impresion_global.maquina_8;
            mp9.Content = impresion_global.maquina_9;
            mp10.Content = impresion_global.maquina_10;
            mp11.Content = impresion_global.maquina_11;
            mp12.Content = impresion_global.maquina_12;
            mp13.Content = impresion_global.maquina_13;
            mp14.Content = impresion_global.maquina_14;
            mp15.Content = impresion_global.maquina_15;
            mp16.Content = impresion_global.maquina_16;
            mp17.Content = impresion_global.maquina_17;
            mp18.Content = impresion_global.maquina_18;
            mp19.Content = impresion_global.maquina_19;
            mp20.Content = impresion_global.maquina_20;
            mp21.Content = impresion_global.maquina_21;
            mp22.Content = impresion_global.maquina_22;
            mp23.Content = impresion_global.maquina_23;
            mp24.Content = impresion_global.maquina_24;
            mp25.Content = impresion_global.maquina_25;
            mp26.Content = impresion_global.maquina_26;
            mp27.Content = impresion_global.maquina_27;
            mp28.Content = impresion_global.maquina_28;
            mp29.Content = impresion_global.maquina_29;
            mp30.Content = impresion_global.maquina_30;
            mp31.Content = impresion_global.maquina_31;
            mp32.Content = impresion_global.maquina_32;
            mp33.Content = impresion_global.maquina_33;
            mp34.Content = impresion_global.maquina_34;
            mp35.Content = impresion_global.maquina_35;
            mp36.Content = impresion_global.maquina_36;
            mp37.Content = impresion_global.maquina_37;
            mp38.Content = impresion_global.maquina_38;
            mp39.Content = impresion_global.maquina_39;
            mp40.Content = impresion_global.maquina_40;
            mp41.Content = impresion_global.maquina_41;
            mp42.Content = impresion_global.maquina_42;
            mp43.Content = impresion_global.maquina_43;
            mp44.Content = impresion_global.maquina_44;
            mp45.Content = impresion_global.maquina_45;
            mp46.Content = impresion_global.maquina_46;
            mp47.Content = impresion_global.maquina_47;
            mp48.Content = impresion_global.maquina_48;
            mp49.Content = impresion_global.maquina_49;
            mp50.Content = impresion_global.maquina_50;
            mp51.Content = impresion_global.maquina_51;
            mp52.Content = impresion_global.maquina_52;
            mp53.Content = impresion_global.maquina_53;
            mp54.Content = impresion_global.maquina_54;
            mp55.Content = impresion_global.maquina_55;
            mp56.Content = impresion_global.maquina_56;
            mp57.Content = impresion_global.maquina_57;
            mp58.Content = impresion_global.maquina_58;
            mp59.Content = impresion_global.maquina_59;
            mp60.Content = impresion_global.maquina_60;
            mp61.Content = impresion_global.maquina_61;
            mp62.Content = impresion_global.maquina_62;
            mp63.Content = impresion_global.maquina_63;
            mp64.Content = impresion_global.maquina_64;
            mp65.Content = impresion_global.maquina_65;
            mp66.Content = impresion_global.maquina_66;
            #endregion

            #region operarios
            op1.ItemsSource = impresion_global.operaciones_1;
            op2.ItemsSource = impresion_global.operaciones_2;
            op3.ItemsSource = impresion_global.operaciones_3;
            op4.ItemsSource = impresion_global.operaciones_4;
            op5.ItemsSource = impresion_global.operaciones_5;
            op6.ItemsSource = impresion_global.operaciones_6;
            op7.ItemsSource = impresion_global.operaciones_7;
            op8.ItemsSource = impresion_global.operaciones_8;
            op9.ItemsSource = impresion_global.operaciones_9;
            op10.ItemsSource = impresion_global.operaciones_10;
            op11.ItemsSource = impresion_global.operaciones_11;
            op12.ItemsSource = impresion_global.operaciones_12;
            op13.ItemsSource = impresion_global.operaciones_13;
            op14.ItemsSource = impresion_global.operaciones_14;
            op15.ItemsSource = impresion_global.operaciones_15;
            op16.ItemsSource = impresion_global.operaciones_16;
            op17.ItemsSource = impresion_global.operaciones_17;
            op18.ItemsSource = impresion_global.operaciones_18;
            op19.ItemsSource = impresion_global.operaciones_19;
            op20.ItemsSource = impresion_global.operaciones_20;
            op21.ItemsSource = impresion_global.operaciones_21;
            op22.ItemsSource = impresion_global.operaciones_22;
            op23.ItemsSource = impresion_global.operaciones_23;
            op24.ItemsSource = impresion_global.operaciones_24;
            op25.ItemsSource = impresion_global.operaciones_25;
            op26.ItemsSource = impresion_global.operaciones_26;
            op27.ItemsSource = impresion_global.operaciones_27;
            op28.ItemsSource = impresion_global.operaciones_28;
            op29.ItemsSource = impresion_global.operaciones_29;
            op30.ItemsSource = impresion_global.operaciones_30;
            op31.ItemsSource = impresion_global.operaciones_31;
            op32.ItemsSource = impresion_global.operaciones_32;
            op33.ItemsSource = impresion_global.operaciones_33;
            op34.ItemsSource = impresion_global.operaciones_34;
            op35.ItemsSource = impresion_global.operaciones_35;
            op36.ItemsSource = impresion_global.operaciones_36;
            op37.ItemsSource = impresion_global.operaciones_37;
            op38.ItemsSource = impresion_global.operaciones_38;
            op39.ItemsSource = impresion_global.operaciones_39;
            op40.ItemsSource = impresion_global.operaciones_40;
            op41.ItemsSource = impresion_global.operaciones_41;
            op42.ItemsSource = impresion_global.operaciones_42;
            op43.ItemsSource = impresion_global.operaciones_43;
            op44.ItemsSource = impresion_global.operaciones_44;
            op45.ItemsSource = impresion_global.operaciones_45;
            op46.ItemsSource = impresion_global.operaciones_46;
            op47.ItemsSource = impresion_global.operaciones_47;
            op48.ItemsSource = impresion_global.operaciones_48;
            op49.ItemsSource = impresion_global.operaciones_49;
            op50.ItemsSource = impresion_global.operaciones_50;
            op51.ItemsSource = impresion_global.operaciones_51;
            op52.ItemsSource = impresion_global.operaciones_52;
            op53.ItemsSource = impresion_global.operaciones_53;
            op54.ItemsSource = impresion_global.operaciones_54;
            op55.ItemsSource = impresion_global.operaciones_55;
            op56.ItemsSource = impresion_global.operaciones_56;
            op57.ItemsSource = impresion_global.operaciones_57;
            op58.ItemsSource = impresion_global.operaciones_58;
            op59.ItemsSource = impresion_global.operaciones_59;
            op60.ItemsSource = impresion_global.operaciones_60;
            op61.ItemsSource = impresion_global.operaciones_61;
            op62.ItemsSource = impresion_global.operaciones_62;
            op63.ItemsSource = impresion_global.operaciones_63;
            op64.ItemsSource = impresion_global.operaciones_64;
            op65.ItemsSource = impresion_global.operaciones_65;
            op66.ItemsSource = impresion_global.operaciones_66;
            #endregion

            #region operario

            o1.Content = impresion_global.operario_1;
            o2.Content = impresion_global.operario_2;
            o3.Content = impresion_global.operario_3;
            o4.Content = impresion_global.operario_4;
            o5.Content = impresion_global.operario_5;
            o6.Content = impresion_global.operario_6;
            o7.Content = impresion_global.operario_7;
            o8.Content = impresion_global.operario_8;
            o9.Content = impresion_global.operario_9;
            o10.Content = impresion_global.operario_10;
            o11.Content = impresion_global.operario_11;
            o12.Content = impresion_global.operario_12;
            o13.Content = impresion_global.operario_13;
            o14.Content = impresion_global.operario_14;
            o15.Content = impresion_global.operario_15;
            o16.Content = impresion_global.operario_16;
            o17.Content = impresion_global.operario_17;
            o18.Content = impresion_global.operario_18;
            o19.Content = impresion_global.operario_19;
            o20.Content = impresion_global.operario_20;
            o21.Content = impresion_global.operario_21;
            o22.Content = impresion_global.operario_22;
            o23.Content = impresion_global.operario_23;
            o24.Content = impresion_global.operario_24;
            o25.Content = impresion_global.operario_25;
            o26.Content = impresion_global.operario_26;
            o27.Content = impresion_global.operario_27;
            o28.Content = impresion_global.operario_28;
            o29.Content = impresion_global.operario_29;
            o30.Content = impresion_global.operario_30;
            o31.Content = impresion_global.operario_31;
            o32.Content = impresion_global.operario_32;
            o33.Content = impresion_global.operario_33;
            o34.Content = impresion_global.operario_34;
            o35.Content = impresion_global.operario_35;
            o36.Content = impresion_global.operario_36;
            o37.Content = impresion_global.operario_37;
            o38.Content = impresion_global.operario_38;
            o39.Content = impresion_global.operario_39;
            o40.Content = impresion_global.operario_40;
            o41.Content = impresion_global.operario_41;
            o42.Content = impresion_global.operario_42;
            o43.Content = impresion_global.operario_43;
            o44.Content = impresion_global.operario_44;
            o45.Content = impresion_global.operario_45;
            o46.Content = impresion_global.operario_46;
            o47.Content = impresion_global.operario_47;
            o48.Content = impresion_global.operario_48;
            o49.Content = impresion_global.operario_49;
            o50.Content = impresion_global.operario_50;
            o51.Content = impresion_global.operario_51;
            o52.Content = impresion_global.operario_52;
            o53.Content = impresion_global.operario_53;
            o54.Content = impresion_global.operario_54;
            o55.Content = impresion_global.operario_55;
            o56.Content = impresion_global.operario_56;
            o57.Content = impresion_global.operario_57;
            o58.Content = impresion_global.operario_58;
            o59.Content = impresion_global.operario_59;
            o60.Content = impresion_global.operario_60;
            o61.Content = impresion_global.operario_61;
            o62.Content = impresion_global.operario_62;
            o63.Content = impresion_global.operario_63;
            o64.Content = impresion_global.operario_64;
            o65.Content = impresion_global.operario_65;
            o66.Content = impresion_global.operario_66;

            #endregion

            #region color_maquina

            mp1.Background = impresion_global.color_1;
            mp2.Background = impresion_global.color_2;
            mp3.Background = impresion_global.color_3;
            mp4.Background = impresion_global.color_4;
            mp5.Background = impresion_global.color_5;
            mp6.Background = impresion_global.color_6;
            mp7.Background = impresion_global.color_7;
            mp8.Background = impresion_global.color_8;
            mp9.Background = impresion_global.color_9;
            mp10.Background = impresion_global.color_10;
            mp11.Background = impresion_global.color_11;
            mp12.Background = impresion_global.color_12;
            mp13.Background = impresion_global.color_13;
            mp14.Background = impresion_global.color_14;
            mp15.Background = impresion_global.color_15;
            mp16.Background = impresion_global.color_16;
            mp17.Background = impresion_global.color_17;
            mp18.Background = impresion_global.color_18;
            mp19.Background = impresion_global.color_19;
            mp20.Background = impresion_global.color_20;
            mp21.Background = impresion_global.color_21;
            mp22.Background = impresion_global.color_22;
            mp23.Background = impresion_global.color_23;
            mp24.Background = impresion_global.color_24;
            mp25.Background = impresion_global.color_25;
            mp26.Background = impresion_global.color_26;
            mp27.Background = impresion_global.color_27;
            mp28.Background = impresion_global.color_28;
            mp29.Background = impresion_global.color_29;
            mp30.Background = impresion_global.color_30;
            mp31.Background = impresion_global.color_31;
            mp32.Background = impresion_global.color_32;
            mp33.Background = impresion_global.color_33;
            mp34.Background = impresion_global.color_34;
            mp35.Background = impresion_global.color_35;
            mp36.Background = impresion_global.color_36;
            mp37.Background = impresion_global.color_37;
            mp38.Background = impresion_global.color_38;
            mp39.Background = impresion_global.color_39;
            mp40.Background = impresion_global.color_40;
            mp41.Background = impresion_global.color_41;
            mp42.Background = impresion_global.color_42;
            mp43.Background = impresion_global.color_43;
            mp44.Background = impresion_global.color_44;
            mp45.Background = impresion_global.color_45;
            mp46.Background = impresion_global.color_46;
            mp47.Background = impresion_global.color_47;
            mp48.Background = impresion_global.color_48;
            mp49.Background = impresion_global.color_49;
            mp50.Background = impresion_global.color_50;
            mp51.Background = impresion_global.color_51;
            mp52.Background = impresion_global.color_52;
            mp53.Background = impresion_global.color_53;
            mp54.Background = impresion_global.color_54;
            mp55.Background = impresion_global.color_55;
            mp56.Background = impresion_global.color_56;
            mp57.Background = impresion_global.color_57;
            mp58.Background = impresion_global.color_58;
            mp59.Background = impresion_global.color_59;
            mp60.Background = impresion_global.color_60;
            mp61.Background = impresion_global.color_61;
            mp62.Background = impresion_global.color_62;
            mp63.Background = impresion_global.color_63;
            mp64.Background = impresion_global.color_64;
            mp65.Background = impresion_global.color_65;
            mp66.Background = impresion_global.color_66;

            #endregion

            #region color operaciones_

            op1.Background = impresion_global.color_1;
            op2.Background = impresion_global.color_2;
            op3.Background = impresion_global.color_3;
            op4.Background = impresion_global.color_4;
            op5.Background = impresion_global.color_5;
            op6.Background = impresion_global.color_6;
            op7.Background = impresion_global.color_7;
            op8.Background = impresion_global.color_8;
            op9.Background = impresion_global.color_9;
            op10.Background = impresion_global.color_10;
            op11.Background = impresion_global.color_11;
            op12.Background = impresion_global.color_12;
            op13.Background = impresion_global.color_13;
            op14.Background = impresion_global.color_14;
            op15.Background = impresion_global.color_15;
            op16.Background = impresion_global.color_16;
            op17.Background = impresion_global.color_17;
            op18.Background = impresion_global.color_18;
            op19.Background = impresion_global.color_19;
            op20.Background = impresion_global.color_20;
            op21.Background = impresion_global.color_21;
            op22.Background = impresion_global.color_22;
            op23.Background = impresion_global.color_23;
            op24.Background = impresion_global.color_24;
            op25.Background = impresion_global.color_25;
            op26.Background = impresion_global.color_26;
            op27.Background = impresion_global.color_27;
            op28.Background = impresion_global.color_28;
            op29.Background = impresion_global.color_29;
            op30.Background = impresion_global.color_30;
            op31.Background = impresion_global.color_31;
            op32.Background = impresion_global.color_32;
            op33.Background = impresion_global.color_33;
            op34.Background = impresion_global.color_34;
            op35.Background = impresion_global.color_35;
            op36.Background = impresion_global.color_36;
            op37.Background = impresion_global.color_37;
            op38.Background = impresion_global.color_38;
            op39.Background = impresion_global.color_39;
            op40.Background = impresion_global.color_40;
            op41.Background = impresion_global.color_41;
            op42.Background = impresion_global.color_42;
            op43.Background = impresion_global.color_43;
            op44.Background = impresion_global.color_44;
            op45.Background = impresion_global.color_45;
            op46.Background = impresion_global.color_46;
            op47.Background = impresion_global.color_47;
            op48.Background = impresion_global.color_48;
            op49.Background = impresion_global.color_49;
            op50.Background = impresion_global.color_50;
            op51.Background = impresion_global.color_51;
            op52.Background = impresion_global.color_52;
            op53.Background = impresion_global.color_53;
            op54.Background = impresion_global.color_54;
            op55.Background = impresion_global.color_55;
            op56.Background = impresion_global.color_56;
            op57.Background = impresion_global.color_57;
            op58.Background = impresion_global.color_58;
            op59.Background = impresion_global.color_59;
            op60.Background = impresion_global.color_60;
            op61.Background = impresion_global.color_61;
            op62.Background = impresion_global.color_62;
            op63.Background = impresion_global.color_63;
            op64.Background = impresion_global.color_64;
            op65.Background = impresion_global.color_65;
            op66.Background = impresion_global.color_66;
            #endregion

            #region color_operario

            o1.Background = impresion_global.color_1;
            o2.Background = impresion_global.color_2;
            o3.Background = impresion_global.color_3;
            o4.Background = impresion_global.color_4;
            o5.Background = impresion_global.color_5;
            o6.Background = impresion_global.color_6;
            o7.Background = impresion_global.color_7;
            o8.Background = impresion_global.color_8;
            o9.Background = impresion_global.color_9;
            o10.Background = impresion_global.color_10;
            o11.Background = impresion_global.color_11;
            o12.Background = impresion_global.color_12;
            o13.Background = impresion_global.color_13;
            o14.Background = impresion_global.color_14;
            o15.Background = impresion_global.color_15;
            o16.Background = impresion_global.color_16;
            o17.Background = impresion_global.color_17;
            o18.Background = impresion_global.color_18;
            o19.Background = impresion_global.color_19;
            o20.Background = impresion_global.color_20;
            o21.Background = impresion_global.color_21;
            o22.Background = impresion_global.color_22;
            o23.Background = impresion_global.color_23;
            o24.Background = impresion_global.color_24;
            o25.Background = impresion_global.color_25;
            o26.Background = impresion_global.color_26;
            o27.Background = impresion_global.color_27;
            o28.Background = impresion_global.color_28;
            o29.Background = impresion_global.color_29;
            o30.Background = impresion_global.color_30;
            o31.Background = impresion_global.color_31;
            o32.Background = impresion_global.color_32;
            o33.Background = impresion_global.color_33;
            o34.Background = impresion_global.color_34;
            o35.Background = impresion_global.color_35;
            o36.Background = impresion_global.color_36;
            o37.Background = impresion_global.color_37;
            o38.Background = impresion_global.color_38;
            o39.Background = impresion_global.color_39;
            o40.Background = impresion_global.color_40;
            o41.Background = impresion_global.color_41;
            o42.Background = impresion_global.color_42;
            o43.Background = impresion_global.color_43;
            o44.Background = impresion_global.color_44;
            o45.Background = impresion_global.color_45;
            o46.Background = impresion_global.color_46;
            o47.Background = impresion_global.color_47;
            o48.Background = impresion_global.color_48;
            o49.Background = impresion_global.color_49;
            o50.Background = impresion_global.color_50;
            o51.Background = impresion_global.color_51;
            o52.Background = impresion_global.color_52;
            o53.Background = impresion_global.color_53;
            o54.Background = impresion_global.color_54;
            o55.Background = impresion_global.color_55;
            o56.Background = impresion_global.color_56;
            o57.Background = impresion_global.color_57;
            o58.Background = impresion_global.color_58;
            o59.Background = impresion_global.color_59;
            o60.Background = impresion_global.color_60;
            o61.Background = impresion_global.color_61;
            o62.Background = impresion_global.color_62;
            o63.Background = impresion_global.color_63;
            o64.Background = impresion_global.color_64;
            o65.Background = impresion_global.color_65;
            o66.Background = impresion_global.color_66;


            #endregion

            #region general

            modulo.Content = impresion_global.modulo;
            estilo.Content = impresion_global.estilo;
            temporada.Content = impresion_global.temporada;
            sam.Content = impresion_global.sam;
            operarios.Content = impresion_global.operarios;
            subutilizado.Text = impresion_global.subutilizado;
            sobrecarga.Text = impresion_global.sobrecarga;
            lote.Content = impresion_global.lote;
            ingeniero.Content = impresion_global.ingeniero;

            #endregion

            #region consolidado
            resumen_maquinas.Items.Clear();
            resumen_maquinas.ItemsSource = impresion_global.consolidado_maquinas;

            #endregion
        }

        private void cargar_image()
        {
            // se carga la imagen desde archivo
            try
            {
                Uri fileUri = new Uri(ConfigurationManager.AppSettings["imagenes"] + temporada.Content.ToString()+ "/" + estilo.Content.ToString() + ".jpg");
                imagen.Source = new BitmapImage(fileUri);
            }

            // si no encuentra la imagen del estilo carga la imagen inicial
            catch
            {
                Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                imagen.Source = new BitmapImage(fileUri);
            }
        }

        #endregion


    }
}
