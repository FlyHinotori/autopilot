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
        DataTable datatableSTAID36 = new DataTable("STAID36");
        DataTable datatableSTAID35 = new DataTable("STAID35");
        DataTable datatableSTAID34 = new DataTable("STAID34");

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
                lb_STA32uez.Content = "nicht bezahlt/Guthaben: " + Convert.ToString(dr32.GetValue(0));
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
                lb_STA32uz.Content = "nicht bezahlt/Unterzahlung: " + Convert.ToString(dr32u.GetValue(0));
            }
            conn.Close();

            //STA33 Überzahlung
            conn.Open();

            SqlCommand cmd33 = new SqlCommand();
            cmd33.Connection = conn;
            cmd33.CommandText = SQLcmd00 + " > 0  AND auftrag.sta_id = 33 ";
            cmd33.CommandType = CommandType.Text;

            SqlDataReader dr33 = cmd33.ExecuteReader();
            if (dr33.Read())
            {
                lb_STA33uez.Content = "bezahlt/Guthaben: " + Convert.ToString(dr33.GetValue(0));
            }
            conn.Close();

            //STA33 Unterzahlung
            conn.Open();

            SqlCommand cmd33u = new SqlCommand();
            cmd33u.Connection = conn;
            cmd33u.CommandText = SQLcmd00 + " = 0  AND auftrag.sta_id = 33 ";
            cmd33u.CommandType = CommandType.Text;

            SqlDataReader dr33u = cmd33u.ExecuteReader();
            if (dr33u.Read())
            {
                lb_STA33uz.Content = "bezahlt: " + Convert.ToString(dr33u.GetValue(0));
            }
            conn.Close();

            //STA34 Überzahlung
            conn.Open();

            SqlCommand cmd34 = new SqlCommand();
            cmd34.Connection = conn;
            cmd34.CommandText = SQLcmd00 + " >= 0  AND auftrag.sta_id = 34 ";
            cmd34.CommandType = CommandType.Text;

            SqlDataReader dr34 = cmd34.ExecuteReader();
            if (dr34.Read())
            {
                lb_STA34uez.Content = "Erinnerung 1/Guthaben: " + Convert.ToString(dr34.GetValue(0));
            }
            conn.Close();

            //STA34 Unterzahlung
            conn.Open();

            SqlCommand cmd34u = new SqlCommand();
            cmd34u.Connection = conn;
            cmd34u.CommandText = SQLcmd00 + " < 0  AND auftrag.sta_id = 34 ";
            cmd34u.CommandType = CommandType.Text;

            SqlDataReader dr34u = cmd34u.ExecuteReader();
            if (dr34u.Read())
            {
                lb_STA34uz.Content = "Erinnerung 1/Unterzahlung: " + Convert.ToString(dr34u.GetValue(0));
            }
            conn.Close();

            //STA35 Überzahlung
            conn.Open();

            SqlCommand cmd35 = new SqlCommand();
            cmd35.Connection = conn;
            cmd35.CommandText = SQLcmd00 + " >= 0  AND auftrag.sta_id = 35 ";
            cmd35.CommandType = CommandType.Text;

            SqlDataReader dr35 = cmd35.ExecuteReader();
            if (dr35.Read())
            {
                lb_STA35uez.Content = "Erinnerung 2/Guthaben: " + Convert.ToString(dr35.GetValue(0));
            }
            conn.Close();

            //STA35 Unterzahlung
            conn.Open();

            SqlCommand cmd35u = new SqlCommand();
            cmd35u.Connection = conn;
            cmd35u.CommandText = SQLcmd00 + " < 0  AND auftrag.sta_id = 35 ";
            cmd35u.CommandType = CommandType.Text;

            SqlDataReader dr35u = cmd35u.ExecuteReader();
            if (dr35u.Read())
            {
                lb_STA35uz.Content = "Erinnerung 2/Unterzahlung: " + Convert.ToString(dr35u.GetValue(0));
            }
            conn.Close();

            //STA36 Überzahlung
            conn.Open();

            SqlCommand cmd36 = new SqlCommand();
            cmd36.Connection = conn;
            cmd36.CommandText = SQLcmd00 + " >= 0  AND auftrag.sta_id = 36 ";
            cmd36.CommandType = CommandType.Text;

            SqlDataReader dr36 = cmd36.ExecuteReader();
            if (dr36.Read())
            {
                lb_STA36uez.Content = "Mahnung 1/Guthaben: " + Convert.ToString(dr36.GetValue(0));
            }
            conn.Close();

            //STA36 Unterzahlung
            conn.Open();

            SqlCommand cmd36u = new SqlCommand();
            cmd36u.Connection = conn;
            cmd36u.CommandText = SQLcmd00 + " < 0  AND auftrag.sta_id = 36 ";
            cmd36u.CommandType = CommandType.Text;

            SqlDataReader dr36u = cmd36u.ExecuteReader();
            if (dr36u.Read())
            {
                lb_STA36uz.Content = "Mahnung 1/Unterzahlung: " + Convert.ToString(dr36u.GetValue(0));
            }
            conn.Close();

            //STA37 Überzahlung
            conn.Open();

            SqlCommand cmd37 = new SqlCommand();
            cmd37.Connection = conn;
            cmd37.CommandText = SQLcmd00 + " >= 0  AND auftrag.sta_id = 37 ";
            cmd37.CommandType = CommandType.Text;

            SqlDataReader dr37 = cmd37.ExecuteReader();
            if (dr37.Read())
            {
                lb_STA37uez.Content = "Mahnung 2/Guthaben: " + Convert.ToString(dr37.GetValue(0));
            }
            conn.Close();

            //STA37 Unterzahlung
            conn.Open();

            SqlCommand cmd37u = new SqlCommand();
            cmd37u.Connection = conn;
            cmd37u.CommandText = SQLcmd00 + " < 0  AND auftrag.sta_id = 37 ";
            cmd37u.CommandType = CommandType.Text;

            SqlDataReader dr37u = cmd37u.ExecuteReader();
            if (dr37u.Read())
            {
                lb_STA37uz.Content = "Mahnung 2/Unterzahlung: " + Convert.ToString(dr37u.GetValue(0));
            }
            conn.Close();

            //VIP Überzahlung
            conn.Open();

            SqlCommand cmdVIP = new SqlCommand();
            cmdVIP.Connection = conn;
            cmdVIP.CommandText = "SELECT count(auf_id) FROM auftrag, kunde WHERE auftrag.knd_id = kunde.knd_id AND kng_id = 1 AND auf_faellig_am < CONVERT(date,\'" + DateTime.Now + "\',103) AND (select (case when sum(buc_haben) is null then 0 else sum(buc_haben) end) - (case when sum(buc_soll) is null then 0 else sum(buc_soll) end) from buchung where buchung.auf_id = auftrag.auf_id) >= 0  AND (auftrag.sta_id = 32 OR auftrag.sta_id = 33 OR auftrag.sta_id = 34 OR auftrag.sta_id = 35 OR auftrag.sta_id = 36 OR auftrag.sta_id = 37)";
            cmdVIP.CommandType = CommandType.Text;

            SqlDataReader drVIP = cmdVIP.ExecuteReader();
            if (drVIP.Read())
            {
                lb_KNG1uez.Content = "VIP Guthaben: " + Convert.ToString(drVIP.GetValue(0));
            }
            conn.Close();

            //VIP Unterzahlung
            conn.Open();

            SqlCommand cmdVIPu = new SqlCommand();
            cmdVIPu.Connection = conn;
            cmdVIPu.CommandText = "SELECT count(auf_id) FROM auftrag, kunde WHERE auftrag.knd_id = kunde.knd_id AND kng_id = 1 AND auf_faellig_am < CONVERT(date,\'" + DateTime.Now + "\',103) AND (select (case when sum(buc_haben) is null then 0 else sum(buc_haben) end) - (case when sum(buc_soll) is null then 0 else sum(buc_soll) end) from buchung where buchung.auf_id = auftrag.auf_id) < 0  AND (auftrag.sta_id = 32 OR auftrag.sta_id = 33 OR auftrag.sta_id = 34 OR auftrag.sta_id = 35 OR auftrag.sta_id = 36 OR auftrag.sta_id = 37)";
            cmdVIPu.CommandType = CommandType.Text;

            SqlDataReader drVIPu = cmdVIPu.ExecuteReader();
            if (drVIPu.Read())
            {
                lb_KNG1uz.Content = "VIP Unterzahlung: " + Convert.ToString(drVIPu.GetValue(0));
            }
            conn.Close();
        }
        
        private void bt_ManhlaufDurchf_Click(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Soll jetzt ein Mahnlauf vorgenommen werden?", "Mahnlauf?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                string SQLcmd00 = "SELECT auf_id FROM auftrag, kunde WHERE auftrag.knd_id = kunde.knd_id AND kng_id != 1 AND (select (case when sum(buc_haben) is null then 0 else sum(buc_haben) end) - (case when sum(buc_soll) is null then 0 else sum(buc_soll) end) from buchung where buchung.auf_id = auftrag.auf_id) ";
                SqlConnection conn = new SqlConnection(DBconnStrg);

                //Rechnungen mit Saldo größer gleich NULL, fällig und STA_ID = 32/34/35/36/37 --> STA_ID = 33
                datatableSTAID33.Clear();

                conn.Open();

                string SQLcmd33 = SQLcmd00 + " >= 0 AND auf_faellig_am < CONVERT(date,\'" + DateTime.Now + "\',103) AND (auftrag.sta_id = 32 OR auftrag.sta_id = 34 OR auftrag.sta_id = 35 OR auftrag.sta_id = 36 OR auftrag.sta_id = 37)";
                SqlDataAdapter adapter33 = new SqlDataAdapter(SQLcmd33, conn);
                adapter33.Fill(datatableSTAID33);

                conn.Close();

                foreach (DataRow row in datatableSTAID33.Rows)
                {
                    SQLCmd_txt = "UPDATE auftrag SET sta_id = 33, mst_id = null WHERE auf_id = " + row[0].ToString();
                    SQLCmd();
                }

                //Rechnungen mit Saldo kleiner NULL, fällig incl. Mahntage(90d) und STA_ID = 36 --> STA_ID = 37
                datatableSTAID37.Clear();

                conn.Open();

                string SQLcmd37 = SQLcmd00 + " < 0 AND auf_faellig_am + 90 < CONVERT(date,\'" + DateTime.Now + "\',103) AND auftrag.sta_id = 36 ";
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
                    cmd.CommandText = "SELECT ((case when auf_preis is null then 0 else auf_preis end) + (case when auf_zusatzkosten is null then 0 else auf_zusatzkosten end)) * mst_zuschlag FROM auftrag, mahnstufe WHERE auftrag.mst_id = mahnstufe.mst_id AND auf_id = " + row[0].ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        mahngebuehr = Convert.ToDecimal(dr.GetValue(0));
                    }
                    conn.Close();

                    SQLCmd_txt = "INSERT INTO buchung (auf_id,buc_datum,buc_soll,buc_text) VALUES (" + row[0].ToString() + ",CONVERT(date,\'" + DateTime.Now + "\',103)," + mahngebuehr.ToString().Replace(",", ".") + ",\'Mahngebühr Stufe 2\')";
                    SQLCmd();
                }

                //Rechnungen mit Saldo kleiner NULL, fällig incl. Mahntage(60d) und STA_ID = 35 --> STA_ID = 36
                datatableSTAID36.Clear();

                conn.Open();

                string SQLcmd36 = SQLcmd00 + " < 0 AND auf_faellig_am + 60 < CONVERT(date,\'" + DateTime.Now + "\',103) AND auftrag.sta_id = 35 ";
                SqlDataAdapter adapter36 = new SqlDataAdapter(SQLcmd36, conn);
                adapter36.Fill(datatableSTAID36);

                conn.Close();

                foreach (DataRow row in datatableSTAID36.Rows)
                {
                    SQLCmd_txt = "UPDATE auftrag SET sta_id = 36, mst_id = 1 WHERE auf_id = " + row[0].ToString();
                    SQLCmd();

                    //Mahngebühr berechnen
                    decimal mahngebuehr = 0;

                    conn.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT ((case when auf_preis is null then 0 else auf_preis end) + (case when auf_zusatzkosten is null then 0 else auf_zusatzkosten end)) * mst_zuschlag FROM auftrag, mahnstufe WHERE auftrag.mst_id = mahnstufe.mst_id AND auf_id = " + row[0].ToString();
                    cmd.CommandType = CommandType.Text;

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        mahngebuehr = Convert.ToDecimal(dr.GetValue(0));
                    }
                    conn.Close();

                    SQLCmd_txt = "INSERT INTO buchung (auf_id,buc_datum,buc_soll,buc_text) VALUES (" + row[0].ToString() + ",CONVERT(date,\'" + DateTime.Now + "\',103)," + mahngebuehr.ToString().Replace(",", ".") + ",\'Mahngebühr Stufe 2\')";
                    SQLCmd();
                }

                //Rechnungen mit Saldo kleiner NULL, fällig incl. Mahntage(30d) und STA_ID = 34 --> STA_ID = 35
                datatableSTAID35.Clear();

                conn.Open();

                string SQLcmd35 = SQLcmd00 + " < 0 AND auf_faellig_am + 30 < CONVERT(date,\'" + DateTime.Now + "\',103) AND auftrag.sta_id = 34 ";
                SqlDataAdapter adapter35 = new SqlDataAdapter(SQLcmd35, conn);
                adapter35.Fill(datatableSTAID35);

                conn.Close();

                foreach (DataRow row in datatableSTAID35.Rows)
                {
                    SQLCmd_txt = "UPDATE auftrag SET sta_id = 35 WHERE auf_id = " + row[0].ToString();
                    SQLCmd();
                }

                //Rechnungen mit Saldo kleiner NULL, fällig und STA_ID = 33 --> STA_ID = 34
                datatableSTAID34.Clear();

                conn.Open();

                string SQLcmd34 = SQLcmd00 + " < 0 AND auf_faellig_am < CONVERT(date,\'" + DateTime.Now + "\',103) AND auftrag.sta_id = 33 ";
                SqlDataAdapter adapter34 = new SqlDataAdapter(SQLcmd35, conn);
                adapter34.Fill(datatableSTAID34);

                conn.Close();

                foreach (DataRow row in datatableSTAID34.Rows)
                {
                    SQLCmd_txt = "UPDATE auftrag SET sta_id = 34 WHERE auf_id = " + row[0].ToString();
                    SQLCmd();
                }

                MessageBox.Show("Mahnlauf durchgeführt!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                Statistik();
            }
        }        
    }
}
