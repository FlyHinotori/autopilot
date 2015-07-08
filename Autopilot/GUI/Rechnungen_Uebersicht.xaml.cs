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
    /// Interaktionslogik für Rechnungen_Uebersicht.xaml
    /// </summary>
    public partial class Rechnungen_Uebersicht : Page
    {
        AutopilotEntities content = new AutopilotEntities();
        string DBconnStrg = Properties.Settings.Default.AutopilotConnectionString;
        DataTable datatableUebersicht = new DataTable("Uebersicht");
        DataTable datatableKonto = new DataTable("Konto");
        Int32 auf_id;

        public Rechnungen_Uebersicht()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            fuelleDataGridUebersicht();
        }

        private void fuelleDataGridUebersicht()
        {
            datatableUebersicht.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);

            //Aufträge laden
            string SQLcmd = "SELECT auftrag.auf_id, status.sta_id, auftragsart.aart_id, kunde.knd_id, sta_bez, (select (case when sum(buc_haben) is null then 0 else sum(buc_haben) end) - (case when sum(buc_soll) is null then 0 else sum(buc_soll) end) from buchung where buchung.auf_id = auftrag.auf_id) as saldo, (select mst_reihenfolge from mahnstufe where mahnstufe.mst_id = auftrag.mst_id) as mahnstufe, aart_bez, ter_beginn, ter_ende, knd_name + \', \' + knd_vorname as kunde_bez, (select flh_name + \'(\' + flh_stadt + \')\' as abflug from flughafen where flughafen.flh_id = auftrag.flh_id_beginn) as abflughafen, (select flh_name + '(' + flh_stadt + ')' as zielflug from flughafen where flughafen.flh_id = auftrag.flh_id_ende) as zielflughafen FROM auftrag, status, auftragsart, kunde, termin_auftrag, termin WHERE auftrag.sta_id = status.sta_id AND auftrag.aart_id = auftragsart.aart_id AND auftrag.knd_id = kunde.knd_id AND auftrag.auf_id = termin_auftrag.auf_id AND termin_auftrag.ter_id = termin.ter_id";
            SqlCommand cmd = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(datatableUebersicht);

            DataGridUebersicht.ItemsSource = datatableUebersicht.DefaultView;
            ti_Details.IsEnabled = false;
            ti_Kontouebersicht.IsEnabled = false;
            ti_Buchen.IsEnabled = false;
        }

        private void tb_FilterUebersicht_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataGridUebersicht != null)
            {
                string suchbegriff = Convert.ToString(tb_FilterUebersicht.Text);

                if (suchbegriff != null && suchbegriff != "")
                {
                    var dv = datatableUebersicht.DefaultView;
                    dv.RowFilter = "auf_id = \'" + suchbegriff + "\' OR kunde_bez like \'%" + suchbegriff + "%\' OR abflughafen like \'%" + suchbegriff + "%\' OR zielflughafen like \'%" + suchbegriff + "%\'";
                }
                else
                {
                    var dv = datatableUebersicht.DefaultView;
                    dv.RowFilter = "kunde_bez like \'%\'";
                }
            }
        }

        private void DataGridUebersicht_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGridUebersicht.SelectedCells.Count != 0 && DataGridUebersicht.ItemsSource != null && DataGridUebersicht.SelectedItem != null)
            {
                DataRowView row = DataGridUebersicht.SelectedItems as DataRowView;
                auf_id = Convert.ToInt32(((DataRowView)DataGridUebersicht.SelectedItem).Row["auf_id"].ToString());
                fuelleDetails();
                fuelleDataGridKonto();
                ti_Details.IsEnabled = true;
                ti_Kontouebersicht.IsEnabled = true;
                ti_Buchen.IsEnabled = true;
            }  
        }

        private void fuelleDetails()
        {
            SqlConnection conn = new SqlConnection(DBconnStrg);
            conn.Open();
            
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT convert(decimal(8,2),auf_preis), convert(decimal(8,2),auf_zusatzkosten), CONVERT(char, auf_faellig_am, 104) FROM auftrag WHERE auf_id = " + auf_id;
            cmd.CommandType = CommandType.Text;

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                lb_Preis.Content = "Preis: €" + Convert.ToString(dr.GetValue(0));
                lb_Zusatzkosten.Content = "Zusatzkosten: €" + Convert.ToString(dr.GetValue(1));
                lb_faelligam.Content = "fällig am: " + Convert.ToString(dr.GetValue(2));
            }
            
            conn.Close();
        }

        private void fuelleDataGridKonto()
        {
            datatableKonto.Clear();
            SqlConnection conn = new SqlConnection(DBconnStrg);

            //Buchungen laden
            string SQLcmd = "SELECT buc_datum, buc_soll, buc_haben, buc_text FROM buchung WHERE auf_id = " + auf_id + " ORDER BY buc_datum";
            SqlCommand cmd = new SqlCommand(SQLcmd, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(datatableKonto);

            DataGridKonto.ItemsSource = datatableKonto.DefaultView;
        }

        private void bt_buchen_Click(object sender, RoutedEventArgs e)
        {
            string buc_soll = "null";
            string buc_haben = "null";
            string betragOK = "0";
            string betrag = "0";

            try
            {
                Convert.ToDecimal(tb_Betrag.Text);
                betrag = tb_Betrag.Text.Replace(".", "");
                betrag = betrag.Replace(",", ".");
                betragOK = "1";
            }
            catch (System.Exception err)
            {
                betragOK = "0";
                MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (betragOK == "1")
            {
                if (rb_haben.IsChecked == true)
                {
                    buc_haben = Convert.ToString(betrag);
                }
                else
                {
                    buc_soll = Convert.ToString(betrag);
                }

                var res = MessageBox.Show("Soll die Buchung jetzt vorgenommen werden?","Buchen?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    SqlConnection conn = new SqlConnection(DBconnStrg);
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = conn;
                    cmd1.CommandText = "INSERT INTO buchung (auf_id, buc_datum, buc_soll, buc_haben, buc_text) VALUES (" + auf_id + ", CONVERT(date,\'" + DateTime.Now.ToShortDateString() + "\',103)," + buc_soll + "," + buc_haben + ",\'" + Convert.ToString(tb_Buchungstext.Text) + "\')";
                    cmd1.CommandType = CommandType.Text;

                    try
                    {
                        cmd1.ExecuteNonQuery();

                        tb_Betrag.Clear();
                        tb_Buchungstext.Clear();
                        rb_haben.IsChecked = false;
                        rb_soll.IsChecked = false;

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

        private void tb_Betrag_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tb_Betrag.Text != "")
            {
                try
                {
                    Convert.ToDecimal(tb_Betrag.Text);
                }
                catch (System.Exception err)
                {
                    MessageBox.Show("Fehlermeldung: " + err.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void rb_haben_Click(object sender, RoutedEventArgs e)
        {
            if(rb_haben.IsChecked == true)
            {
                rb_soll.IsChecked = false;
            }
        }

        private void rb_soll_Click(object sender, RoutedEventArgs e)
        {
            if (rb_soll.IsChecked == true)
            {
                rb_haben.IsChecked = false;
            }
        }
    }
}
