using ClasesTexops;
using SQLConnection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace JazzCCO._0.pantallasMantenimiento
{
    public partial class calendarioProgramacion : Page
    {
        CalenderBackground background;
        List<frecuenciasManto> frecuencias = new List<frecuenciasManto>();
        List<EquipoManto> equipos = new List<EquipoManto>();
        List<ActividadesManto> actividades = new List<ActividadesManto>();
        List<DateTime> fechasVisibles = new List<DateTime>();

        public calendarioProgramacion()
        {
            InitializeComponent();
            cargarActividades();
            generarFechasVisibles();
            cargarFechas();
        }

        private void salir__Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        private void btnAgregarActividad_Click(object sender, RoutedEventArgs e)
        {
            if (calendarFecha.SelectedDate.HasValue)
            {
                if (frecuencias.Count == 0)
                {
                    using (SqlConnection cn = ConexionTexopsServer.Mantenimiento())
                    {
                        try
                        {
                            string sql = "SELECT idFrecuencia, nombreFrecuencia, equivalenteEnDias FROM frecuenciasManto";
                            SqlCommand cm = new SqlCommand(sql, cn);
                            SqlDataReader dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                frecuencias.Add(new frecuenciasManto(Convert.ToInt32(dr["idFrecuencia"]), dr["nombreFrecuencia"].ToString(), Convert.ToInt32(dr["equivalenteEnDias"])));
                            }
                            dr.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                        try
                        {
                            string sql = "SELECT idEquipoManto, NombreEquipo FROM equiposMantenimiento";
                            SqlCommand cm = new SqlCommand(sql, cn);
                            SqlDataReader dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                equipos.Add(new EquipoManto(Convert.ToInt32(dr["idEquipoManto"]), dr["NombreEquipo"].ToString()));
                            }
                            dr.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }

                    cmbFrecuencia.ItemsSource = frecuencias;
                    cmbMaquina.ItemsSource = equipos;
                }
                else
                {
                    cmbFrecuencia.SelectedIndex = -1;
                    dtpFecha.SelectedDate = calendarFecha.SelectedDate;
                    cmbMaquina.SelectedIndex = -1;
                    popUpAgregarActividad.IsOpen = true;
                }

                txtActividad.Text = "";
                txtComentario.Text = "";
            }
            else
            {
                MessageBox.Show("Seleccione una fecha");
            }

        }
        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            popUpAgregarActividad.IsOpen = false;
        }
        private void btnProgramar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbFrecuencia.SelectedIndex > -1 && cmbMaquina.SelectedIndex > -1 && !string.IsNullOrWhiteSpace(txtActividad.Text))
            {
                using (SqlConnection cn = ConexionTexopsServer.Mantenimiento())
                {
                    try
                    {
                        string sql = "INSERT INTO actividadesManto(nombreActividad, idFrecuencia, idEquipoManto, inicio, idStatusActividad, comentariosActividad) VALUES('" + txtActividad.Text.ToString() + "', '" + ((frecuenciasManto)cmbFrecuencia.SelectedItem).IdFrecuencia +"', '" + ((EquipoManto)cmbMaquina.SelectedItem).IdEquipoManto + "', '" + Convert.ToDateTime(dtpFecha.SelectedDate).ToString("yyyy-MM-dd") +"', '" + 1 +"', '" + txtComentario.Text +"')";
                        SqlCommand cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();
                        MessageBox.Show("Actividad Registrada");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione todos los datos");
            }


        }
        private void calendarFecha_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            cargarFechas();
        }
        private void cargarFechas()
        {
            background = new CalenderBackground(calendarFecha);



            background.AddOverlay("tjek", "tjek.png");
            background.AddOverlay("gray", "gray.png");
            background.AddOverlay("circle", "circle.png");
            background.AddOverlay("cross", "cross.png");

            foreach(DateTime item in fechasVisibles)
            {
                background.AddDate(new DateTime(item.Year, item.Month, item.Day), "cross");
            }




            background.grayoutweekends = "gray";

            calendarFecha.Background = background.GetBackground();

            // Update background when changing the displayed month
            calendarFecha.DisplayDateChanged += calendarFecha_DisplayDateChanged;

        }
        private List<ActividadesManto> cargarActividades()
        {
            using (SqlConnection cn = ConexionTexopsServer.Mantenimiento())
            {
                try
                {
                    string sql = "SELECT a.idActividadManto, a.nombreActividad, b.nombreFrecuencia, c.NombreEquipo, a.idStatusActividad, a.inicio FROM actividadesManto a LEFT JOIN frecuenciasManto b ON a.idFrecuencia=b.idFrecuencia LEFT JOIN equiposMantenimiento c ON a.idEquipoManto=c.idEquipoManto WHERE a.idStatusActividad=1";
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        actividades.Add(new ActividadesManto(Convert.ToInt32(dr["idActividadManto"]), dr["nombreActividad"].ToString(), dr["nombreFrecuencia"].ToString(), dr["nombreEquipo"].ToString(), Convert.ToInt32(dr["idStatusActividad"]), Convert.ToDateTime(dr["inicio"]).ToString("yyyy-MM-dd")));
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return actividades;
        }
        private void calendarFecha_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
          //  cargarActividades();
        }

        private void btnGuardar_GotFocus(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            lstActividadesDia.SelectedItem = button.DataContext;
        }



        private List<DateTime> generarFechasVisibles()
        {
            foreach(ActividadesManto item in actividades)
            {

                switch (item.NombreFrecuencia)
                {
                    case "Diariamente":

                        break;
                    case "Semanalmente":
                        int mes = calendarFecha.DisplayDate.Month;
                        int anio = calendarFecha.DisplayDate.Year;
                        DateTime fechaEvaluada = Convert.ToDateTime(item.Inicio);
                        DateTime fechaBase = calendarFecha.DisplayDate.Date.AddDays(15);

                        while (fechaEvaluada<= fechaBase)
                        {
                            fechasVisibles.Add(fechaEvaluada);
                            fechaEvaluada.AddDays(7);
                        }
                        break;
                }
            }
            return fechasVisibles;
        }


    }
}
