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
    /// Interaktionslogik für Rechnungen_Regulierung.xaml
    /// </summary>
    public partial class Rechnungen_Regulierung : Page
    {
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        public string SQLCmd_txt;
        DataTable datatableUebersicht = new DataTable("Uebersicht");
        public Int32 auf_id = 0;

        public Rechnungen_Regulierung()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            fuelleDataGridUebersicht();
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

        private void fuelleDataGridUebersicht()
        {
            datatableUebersicht.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);

            //Aufträge laden
            string SQLcmd = "SELECT auftrag.auf_id, status.sta_id, auftragsart.aart_id, kunde.knd_id, sta_bez, (select (case when sum(buc_haben) is null then 0 else sum(buc_haben) end) - (case when sum(buc_soll) is null then 0 else sum(buc_soll) end) from buchung where buchung.auf_id = auftrag.auf_id) as saldo, aart_bez, ter_beginn, ter_ende, knd_name + \', \' + knd_vorname as kunde_bez, (select flh_name + \'(\' + flh_stadt + \')\' as abflug from flughafen where flughafen.flh_id = auftrag.flh_id_beginn) as abflughafen, (select flh_name + '(' + flh_stadt + ')' as zielflug from flughafen where flughafen.flh_id = auftrag.flh_id_ende) as zielflughafen, auf_faellig_am FROM auftrag, status, auftragsart, kunde, termin_auftrag, termin WHERE auftrag.sta_id = status.sta_id AND auftrag.aart_id = auftragsart.aart_id AND auftrag.knd_id = kunde.knd_id AND auftrag.auf_id = termin_auftrag.auf_id AND termin_auftrag.ter_id = termin.ter_id AND auf_faellig_am < CONVERT(date,\'" + DateTime.Now + "\',103) AND (select (case when sum(buc_haben) is null then 0 else sum(buc_haben) end) - (case when sum(buc_soll) is null then 0 else sum(buc_soll) end) from buchung where buchung.auf_id = auftrag.auf_id) > 0 AND auftrag.sta_id = 33";
            SqlCommand cmd = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(datatableUebersicht);

            DataGridUebersicht.ItemsSource = datatableUebersicht.DefaultView;

            bt_Auszahlung.IsEnabled = false;
        }

        private void DataGridUebersicht_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridUebersicht.SelectedCells.Count != 0 && DataGridUebersicht.ItemsSource != null && DataGridUebersicht.SelectedItem != null)
            {
                DataRowView row = DataGridUebersicht.SelectedItems as DataRowView;
                auf_id = Convert.ToInt32(((DataRowView)DataGridUebersicht.SelectedItem).Row["auf_id"].ToString());

                bt_Auszahlung.IsEnabled = true;
            }
        }

        private void bt_Auszahlung_Click(object sender, RoutedEventArgs e)
        {
            string saldo = Convert.ToString(((DataRowView)DataGridUebersicht.SelectedItem).Row["saldo"].ToString());
            
            var res = MessageBox.Show("Soll die Auszahlung in Höhe von €" + saldo + " jetzt vorgenommen und verbucht werden?", "Auszahlung?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                SqlConnection conn = new SqlConnection(DBconnStrg);
                conn.Open();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = conn;
                cmd1.CommandText = "INSERT INTO buchung (auf_id, buc_datum, buc_soll, buc_text) VALUES (" + auf_id + ", CONVERT(date,\'" + DateTime.Now.ToShortDateString() + "\',103)," + saldo.Replace(",", ".") + ",\'Guthaben Auszahlung\')";
                cmd1.CommandType = CommandType.Text;

                try
                {
                    cmd1.ExecuteNonQuery();

                    MessageBox.Show("Auszahlung verbucht.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

                    fuelleDataGridUebersicht();
                }
                catch (System.Exception err)
                {
                    MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                conn.Close();
            }
        }
    }
}
