using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SQLConnection
{
    public class ConexionTexopsServer
    {
        public static SqlConnection Kanban()
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["JAZZCCO._0.Properties.Settings.conexionSQLKanban"].ConnectionString);

            if(cn.State== ConnectionState.Open)
            {
                cn.Close();
            }
            else
            {
                cn.Open();
            }

            return cn;
        }

        public static SqlConnection Produccion()
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["JAZZCCO._0.Properties.Settings.conexionSQLProduccion"].ConnectionString);

            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            else
            {
                cn.Open();
            }

            return cn;
        }

        public static SqlConnection Mantenimiento()
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["JAZZCCO._0.Properties.Settings.conexionSQLMantenimiento"].ConnectionString);

            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            else
            {
                cn.Open();
            }

            return cn;
        }
        public static SqlConnection Ingenieria()
        {
            SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["JAZZCCO._0.Properties.Settings.conexionSQLIngenieria"].ConnectionString);

            if (cn.State == ConnectionState.Open)
            {
                cn.Close();
            }
            else
            {
                cn.Open();
            }

            return cn;
        }
    }
}
