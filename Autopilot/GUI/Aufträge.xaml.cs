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
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Aufträge.xaml
    /// </summary>
    public partial class Aufträge : Page
    {
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        DataTable TableAuftraege = new DataTable("Auftraege");
        int FAuftragsID = 0;

        public Aufträge()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TableAuftraege.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);

            //Aufträge laden
            string SQLcmd = "SELECT a.auf_id, a.knd_id, anr.anr_bez, k.knd_vorname, k.knd_name, t.ter_beginn, t.ter_ende, s.sta_bez, kg.kng_bez"
                + " FROM auftrag a LEFT JOIN kunde k ON (k.knd_id = a.knd_id) LEFT JOIN anrede anr ON (anr.anr_id = k.anr_id) LEFT JOIN kundengruppe kg ON (kg.kng_id = k.knd_id) " 
                + " LEFT JOIN status s ON (s.sta_id = a.sta_id) LEFT JOIN termin_auftrag ta ON (ta.auf_id = a.auf_id) LEFT JOIN termin t ON (t.ter_id = ta.ter_id) ORDER BY a.sta_id ASC";
            SqlCommand cmd = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(TableAuftraege);

            GridAuftraege.ItemsSource = TableAuftraege.DefaultView;
        }

        private void SetButtons(string Kundengruppe, string Auftragsstatus)
        { 
            BtnAngebotErstellen.IsEnabled = Auftragsstatus == "Aufnahme";
            BtnAuftragStornieren.IsEnabled = (Auftragsstatus == "Angebot") || Auftragsstatus == "Vertrag";
            BtnVertragErstellen.IsEnabled = Auftragsstatus == "Angebot";
            BtnVertragUnterschrieben.IsEnabled = Auftragsstatus == "Vertrag";
            BtnRechnungErstellen.IsEnabled = ((Auftragsstatus == "Durchführung") && (Kundengruppe == "PRE")) || 
                ((Auftragsstatus == "Beendet") && (Kundengruppe != "PRE"));
            BtnFlugdatenErfassen.IsEnabled = Auftragsstatus == "Durchführung";
            BtnFeedbackErfassen.IsEnabled = (Auftragsstatus != "Aufnahme") && (Auftragsstatus != "Angebot") &&
                (Auftragsstatus != "Vertrag");
        }

        private void GridAuftraege_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridAuftraege.SelectedCells.Count != 0 && GridAuftraege.ItemsSource != null && GridAuftraege.SelectedItem != null)
            {
                DataRowView row = GridAuftraege.SelectedItems as DataRowView;
                string Kundengruppe = ((DataRowView)GridAuftraege.SelectedItem).Row["kng_bez"].ToString();
                string Auftragsstatus = ((DataRowView)GridAuftraege.SelectedItem).Row["sta_bez"].ToString();
                SetButtons(Kundengruppe, Auftragsstatus);
                FAuftragsID = Convert.ToInt32(((DataRowView)GridAuftraege.SelectedItem).Row["auf_id"].ToString());
            }  
        }

        private void BtnAngebotErstellen_Click(object sender, RoutedEventArgs e)
        {
            CalculateCosts();
        }

        private void CalculateCosts()
        {
            double Fixkosten = GetFixkosten();
        }

        private double GetFixkosten()
        {
            double Fixkosten = 0;
            SqlConnection conn = new SqlConnection(DBconnStrg);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            //Get annual fix costs and flight duration (in days)
            cmd.CommandText = "SELECT CAST(ft.ftyp_fkosten_pa AS int) AS costperanno, CAST( (t.ter_ende - t.ter_beginn + 1) AS Int) AS flugzeit FROM termin_auftrag ta LEFT JOIN termin t ON (ta.ter_id = t.ter_id)"
                + " LEFT JOIN termin_flugzeug tf ON (tf.ter_id = t.ter_id) LEFT JOIN flugzeug f ON (f.flz_id = tf.flz_id) LEFT JOIN flugzeugtyp ft ON (ft.ftyp_id = f.ftyp_id)"
                + " WHERE ta.auf_id = " + FAuftragsID.ToString();
            cmd.CommandType = System.Data.CommandType.Text;

            conn.Open();

            SqlDataReader ResultSet = cmd.ExecuteReader();
            if (ResultSet.Read())
            {
                int CostPerAnno = (int)ResultSet["costperanno"];
                int DurationInDays = (int)ResultSet["flugzeit"];
                Fixkosten = (CostPerAnno / 365) * DurationInDays;
            }
            conn.Close();
            return Fixkosten;
        }
    }
}
