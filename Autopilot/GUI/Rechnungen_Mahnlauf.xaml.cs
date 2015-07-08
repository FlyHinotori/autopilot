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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Entity;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Rechnungen_Mahnlauf.xaml
    /// </summary>
    public partial class Rechnungen_Mahnlauf : Page
    {
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        public string SQLCmd_txt;
        DataTable datatableSTAID33 = new DataTable("STAID33");
        DataTable datatableSTAID37 = new DataTable("STAID37");

        public Rechnungen_Mahnlauf()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Statistik();
        }

        public void SQLCmd()
        {
            SqlConnection conn = new SqlConnection(DBconnStrg);
            conn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = SQLCmd_txt;
            cmd.CommandType = CommandType.Text;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception err)
            {
                MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            conn.Close();
        }

        private void Statistik()
        {
            string SQLcmd00 = "SELECT count(auf_id) FROM auftrag, kunde WHERE auftrag.knd_id = kunde.knd_id AND kng_id != 1 AND auf_faellig_am < CONVERT(date,\'" + DateTime.Now + "\',103) AND (select (case when sum(buc_haben) is null then 0 else sum(buc_haben) end) - (case when sum(buc_soll) is null then 0 else sum(buc_soll) end) from buchung where buchung.auf_id = auftrag.auf_id) ";
            SqlConnection conn = new SqlConnection(DBconnStrg);
            
            //STA32 Überzahlung
            conn.Open();

            SqlCommand cmd32 = new SqlCommand();
            cmd32.Connection = conn;
            cmd32.CommandText = SQLcmd00 + " >= 0  AND auftrag.sta_id = 32 ";
            cmd32.CommandType = CommandType.Text;

            SqlDataReader dr32 = cmd32.ExecuteReader();
            if (dr32.Read())
            {
                lb_STA32uez.Content = "Status 32 Überzahlung: " + Convert.ToString(dr32.GetValue(0));
            }
            conn.Close();

            //STA32 Unterzahlung
            conn.Open();

            SqlCommand cmd32u = new SqlCommand();
            cmd32u.Connection = conn;
            cmd32u.CommandText = SQLcmd00 + " < 0  AND auftrag.sta_id = 32 ";
            cmd32u.CommandType = CommandType.Text;

            SqlDataReader dr32u = cmd32u.ExecuteReader();
            if (dr32u.Read())
            {
                lb_STA32uz.Content = "Status 32 Unterzahlung: " + Convert.ToString(dr32u.GetValue(0));
            }
            conn.Close();

            //STA33 Überzahlung
            conn.Open();

            SqlCommand cmd33 = new SqlCommand();
            cmd33.Connection = conn;
            cmd33.CommandText = SQLcmd00 + " >= 0  AND auftrag.sta_id = 33 ";
            cmd33.CommandType = CommandType.Text;

            SqlDataReader dr33 = cmd33.ExecuteReader();
            if (dr33.Read())
            {
                lb_STA33uez.Content = "Status 33 Überzahlung: " + Convert.ToString(dr33.GetValue(0));
            }
            conn.Close();

            //STA33 Unterzahlung
            conn.Open();

            SqlCommand cmd33u = new SqlCommand();
            cmd33u.Connection = conn;
            cmd33u.CommandText = SQLcmd00 + " < 0  AND auftrag.sta_id = 33 ";
            cmd33u.CommandType = CommandType.Text;

            SqlDataReader dr33u = cmd33u.ExecuteReader();
            if (dr33u.Read())
            {
                lb_STA33uz.Content = "Status 33 Unterzahlung: " + Convert.ToString(dr33u.GetValue(0));
            }
            conn.Close();
        }
        
        private void bt_ManhlaufDurchf_Click(object sender, RoutedEventArgs e)
        {
            string SQLcmd00 = "SELECT auf_id FROM auftrag, kunde WHERE auftrag.knd_id = kunde.knd_id AND kng_id != 1 AND auf_faellig_am < CONVERT(date,\'" + DateTime.Now + "\',103) AND (select (case when sum(buc_haben) is null then 0 else sum(buc_haben) end) - (case when sum(buc_soll) is null then 0 else sum(buc_soll) end) from buchung where buchung.auf_id = auftrag.auf_id) ";
            SqlConnection conn = new SqlConnection(DBconnStrg);
            
            //Rechnungen mit Saldo größer gleich NULL, fällig und STA_ID = 32/34/35/36/37 --> STA_ID = 33
            datatableSTAID33.Clear();

            conn.Open();

            string SQLcmd33 = SQLcmd00 + " >= 0  AND (auftrag.sta_id = 32 OR auftrag.sta_id = 34 OR auftrag.sta_id = 35 OR auftrag.sta_id = 36 OR auftrag.sta_id = 37)";
            SqlDataAdapter adapter33 = new SqlDataAdapter(SQLcmd33, conn);
            adapter33.Fill(datatableSTAID33);

            conn.Close();

            foreach (DataRow row in datatableSTAID33.Rows)
            {
                SQLCmd_txt = "UPDATE auftrag SET sta_id = 33 WHERE auf_id = " + row[0].ToString();
                SQLCmd();
            }

            //Rechnungen mit Saldo kleiner NULL, fällig und STA_ID = 36 --> STA_ID = 37
            datatableSTAID37.Clear();

            conn.Open();

            string SQLcmd37 = SQLcmd00 + " < 0  AND auftrag.sta_id = 36 ";
            SqlDataAdapter adapter37 = new SqlDataAdapter(SQLcmd37, conn);
            adapter37.Fill(datatableSTAID37);

            conn.Close();

            foreach (DataRow row in datatableSTAID37.Rows)
            {
                SQLCmd_txt = "UPDATE auftrag SET sta_id = 37, mst_id = 2 WHERE auf_id = " + row[0].ToString();
                SQLCmd();
                
                //Mahngebühr berechnen
                decimal mahngebuehr = 0;
                
                conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT (auf_preis + auf_zusatzkosten) * mst_zuschlag FROM auftrag, mahnstufe WHERE auftrag.mst_id = mahnstufe.mst_id AND auf_id = " + row[0].ToString();
                cmd.CommandType = CommandType.Text;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    mahngebuehr = Convert.ToDecimal(dr.GetValue(0));
                }
                conn.Close();
                
                SQLCmd_txt = "INSERT INTO buchung (auf_id,buc_datum,buc_soll,buc_text) VALUES (" + row[0].ToString() + ",CONVERT(date,\'" + DateTime.Now + "\',103)," + mahngebuehr.ToString().Replace(",",".") + ",\'Mahngebühr Stufe 2\')";
                SQLCmd();
            }

            MessageBox.Show("Mahnlauf durchgeführt!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }        
    }
}
