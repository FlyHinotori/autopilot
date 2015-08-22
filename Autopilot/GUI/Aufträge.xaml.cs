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

namespace Autopilot.GUI
{
    /// <summary>
    /// Interaktionslogik für Aufträge.xaml
    /// </summary>
    public partial class Aufträge : Page
    {
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        DataTable TableAuftraege = new DataTable("Auftraege");

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
            }  
        }
    }
}
